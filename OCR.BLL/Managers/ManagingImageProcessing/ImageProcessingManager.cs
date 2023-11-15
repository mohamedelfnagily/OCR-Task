using AutoMapper;
using IronOcr;
using Microsoft.AspNetCore.Identity;
using OCR.BLL.DTOs;
using OCR.BLL.DTOs.LicensceDataDto;
using OCR.BLL.Managers.ManagingUsers;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Non_Generic.CarsDataRepo;
using OCR.ProcessingImages;
using RestSharp;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;

namespace OCR.BLL.Managers.ManagingImageProcessing
{
	public class ImageProcessingManager : IImageProcessingManager
	{
		public int counter { get; set; } = 0;
		private readonly ICarDataRepository _cardatarespository;
		private readonly IManageUser _usermanager;
		private IMapper _mapper;
		public ImageProcessingManager(ICarDataRepository _carDataRepo,IMapper mapper,IManageUser manageuser)
		{
			_cardatarespository = _carDataRepo;
			_mapper = mapper;
			_usermanager = manageuser;
		}

		public async Task<CarDataReadDto> CutImageToStripes(BackLicensceImageDto licensceImage)
		{
			if (await _cardatarespository.CheckForDuplicates(licensceImage.UserId))
			{
				return new CarDataReadDto() { ErrorMessage = "This User has already entered a license before." };
			}
			if (!await _usermanager.CheckUserExistance(licensceImage.UserId))
			{
				return new CarDataReadDto() { ErrorMessage = "User doesn't exist, Please check the user Id" };
			}
			try
			{
				if (licensceImage != null)
				{
					string DriverName = string.Empty;
					string LicensceStartDate = string.Empty;
					string LicensceEndDate = string.Empty;
					int RectangleYPosition = 0;
					using (var memoryStream = new MemoryStream())
					{
						await licensceImage.CarBackLicenseImage.CopyToAsync(memoryStream);
						using (var LicImg = System.Drawing.Image.FromStream(memoryStream))
						{
							Bitmap LicensceImageFromUser = new Bitmap(LicImg);
							new ImageProcessing(LicensceImageFromUser)
							  .Grayscale()
							  .DetectEdges()
							  .Dilate()
							  .Binarize()
							  .HorizontalSmear()
							  .GetBlobs()
							  .ForEach((rectangle) =>
							  {

								  if (rectangle.Size.Height < 100 && rectangle.Size.Height > 20)
								  {

									  File.AppendAllText("Boundries.txt", rectangle.Y.ToString() + Environment.NewLine);
									  System.Drawing.Image img = LicImg;
									  int NewWidth = img.Width;
									  int NewHeight = rectangle.Size.Height;
									  //if (rectangle.Y != 0)
									  //{
									  // rectangle.Y = rectangle.Y - 5;
									  //}
									  rectangle.X = 0;
									  //if (rectangle.Y < img.Height - NewHeight)
									  //{
									  rectangle.Size = new System.Drawing.Size(NewWidth, NewHeight);

									  //}
									  var x = CropImage(img, rectangle);
									  var myIgmg = new Bitmap(x);
									  string ImagePath = @"D:\M.Elfnagily\OCR\OCR.API\LicensceImages\test" + counter + ".png";
									  if ((rectangle.Y - RectangleYPosition) > 10)
									  {
										  myIgmg.Save(ImagePath);
										  counter++;
									  }
									  RectangleYPosition = rectangle.Y;

									  //SearchForSpecificText(x, ref DriverName, ref LicensceStartDate, ref LicensceEndDate);
								  }

							  });
						}

						//ImageContainingTextCheck();
					}

					CarData myCarData = _mapper.Map<CarData>(licensceImage);
					myCarData.DriverFullName = DriverName;
					myCarData.LicensceEndDate = LicensceEndDate;
					myCarData.LicensceStartDate = LicensceStartDate;
					await _cardatarespository.AddAsync(myCarData);
					_cardatarespository.Save();
					CarDataReadDto RetrievedCarData = _mapper.Map<CarDataReadDto>(myCarData);
					return RetrievedCarData;
				}
				else
				{
					return new CarDataReadDto() { ErrorMessage = "Errors Occured" };
				}
			}
			catch (Exception)
			{

				return new CarDataReadDto() { ErrorMessage = "Errors Occured" };
			}
		}

		public System.Drawing.Image CropImage(System.Drawing.Image licensceImage, System.Drawing.Rectangle cropArea)
		{
			Bitmap bmpImage = new Bitmap(licensceImage);
			return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
		}
		public void SearchForSpecificText(System.Drawing.Image img, ref string DriverName,ref string LicensceStartDate, ref string LicensceEndDate)
		{
			//string DriverName = string.Empty;
			//string LicensceStartDate = string.Empty;
			//string LicensceEndDate = string.Empty;
			var client = new RestClient("https://api.apilayer.com/image_to_text/upload");
			var request = new RestRequest("https://api.apilayer.com/image_to_text/upload" ,Method.Post);
			request.AddHeader("apikey", "hiibTUQGrv7ebUYO7f9tTs0e3WmbxG9q");
			MemoryStream ms = new MemoryStream();
			img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			request.AddParameter("text/plain", ms.ToArray(), ParameterType.RequestBody);
			var response = client.Post(request);
			var content = response.Content; 

			dynamic json = JsonObject.Parse(content);
			string ExtractedText = string.Empty;
			try
			{
				 ExtractedText = string.Join(' ', json["annotations"]);
			}
			catch (Exception)
			{

				ExtractedText=string.Empty;
			}
			if(ExtractedText!=string.Empty )
			{
				if (ExtractedText.Contains("رخصه") || ExtractedText.Contains("تسيير") || ExtractedText.Contains("ملاكي"))
				{
					//Console.WriteLine(ExtractedText);
					Bitmap myImg = new Bitmap(img);
					myImg.Save(@"D:\M.Elfnagily\OCR\OCR.API\ImagesWithText\PassengerName.png");
					var Text = ExtractedText.Split(" ").ToList();
					if (ExtractedText.Contains("رخصه"))
					{
						Text.Remove("رخصة");
					}
					if (ExtractedText.Contains("تسيير"))
					{
						Text.Remove("تسيير");
					}
					if (ExtractedText.Contains("ملاكي"))
					{
						Text.Remove("ملاكي");
					}
					DriverName = Text[0] + " " + Text[1] + " " + Text[2] + " " + Text[3];
					Console.WriteLine(DriverName);
				}
				if (ExtractedText.Contains("تاريخ") || ExtractedText.Contains("التحرير") && !ExtractedText.Contains("الترخيص"))
				{
					Bitmap myImg = new Bitmap(img);
					myImg.Save(@"D:\M.Elfnagily\OCR\OCR.API\ImagesWithText\StartData.png");
					LicensceStartDate = toEnglishNumber(ExtractedText);
					Console.WriteLine(LicensceStartDate);

				}
				if (ExtractedText.Contains("نهاية") || ExtractedText.Contains("الترخيص") && !ExtractedText.Contains("التحرير"))
				{
					Bitmap myImg = new Bitmap(img);
					myImg.Save(@"D:\M.Elfnagily\OCR\OCR.API\ImagesWithText\EndData.png");
					LicensceEndDate = toEnglishNumber(ExtractedText);
					Console.WriteLine(LicensceEndDate);

				}
			}


		}
		private string toEnglishNumber(string input)
		{
			string EnglishNumbers = "";

			for (int i = 0; i < input.Length; i++)
			{
				if (Char.IsDigit(input[i]))
				{
					EnglishNumbers += char.GetNumericValue(input, i);
				}
				else if (input[i]=='-')
				{
					EnglishNumbers += '/';
				}
				else
				{
					EnglishNumbers += "";
				}
			}
			return EnglishNumbers;
		}
		public void ImageContainingTextCheck()
		{
			string[] files = System.IO.Directory.GetFiles(@"D:\M.Elfnagily\OCR\OCR.API\LicensceImages", "*.png");
			foreach (string file in files)
			{
				Pix myPix = Pix.LoadFromFile(file);
				TesseractEngine engine = new TesseractEngine(AppDomain.CurrentDomain.BaseDirectory + @"./tessdata", "Arabic", EngineMode.Default);
				Page page = engine.Process(myPix, PageSegMode.SparseText);
				string _result = page.GetText();
				if (_result!=string.Empty)
				{
					Bitmap ImageContainingText = new Bitmap(file);
					string ImageName = Path.GetFileName(file);
					string NewPath = @"D:\M.Elfnagily\OCR\OCR.API\ImagesWithText\LicensceImage" + ImageName;
					ImageContainingText.Save(NewPath);
				}
			}

			#region IRON OCR
			//IronOcr.License.LicenseKey = "IRONOCR.MELFNAGILY96.599-8601A9F524-HCJG5KBE6O7KR-63E3CKIHCLKR-WUMPG2XGZKIB-X22Z72XZ7GP6-XKZEQP5C6NVN-PMOKTY-TE7KDFE5MVGJEA-DEPLOYMENT.TRIAL-7ZJN3P.TRIAL.EXPIRES.09.APR.2023";
			//string[] files = System.IO.Directory.GetFiles(@"D:\M.Elfnagily\OCR\OCR.API\LicensceImages", "*.png");
			//var HalfOfFiles = Math.Ceiling((decimal)(files.Length / 2))-1;
			//Console.WriteLine(@"D:\M.Elfnagily\OCR\OCR.API\LicensceImages\test" + HalfOfFiles + ".png");
			//Console.WriteLine(@"D:\M.Elfnagily\OCR\OCR.API\LicensceImages\test" + (HalfOfFiles-1) + ".png");

			//foreach (string file in files)
			//{
			//	var ocr = new IronTesseract();
			//	System.Drawing.Image img = System.Drawing.Image.FromFile(file);
			//	var myIgmg = ResizeImage(img, 1250, 300);
			//	ocr.Language = OcrLanguage.Arabic;
			//	using (var input = new OcrInput(myIgmg))
			//	{
			//		input.DeNoise();
			//		input.Invert();
			//		var result = ocr.Read(input).Text;

			//		if (result.Any(char.IsDigit))
			//		{
			//			Bitmap ImageContainingText = new Bitmap(file);
			//			string ImageName = Path.GetFileName(file);
			//			string NewPath = @"D:\M.Elfnagily\OCR\OCR.API\ImagesWithText\LicensceImage" + ImageName;
			//			ImageContainingText.Save(NewPath);
			//		}
			//		//counter++;
			//	}
			//} 
			#endregion

		}

		public Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
		{
			var destRect = new System.Drawing.Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
