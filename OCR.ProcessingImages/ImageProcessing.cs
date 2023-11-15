using Accord.Imaging.Filters;
using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OCR.ProcessingImages
{
	public class ImageProcessing
	{
		private Bitmap _image;

		public ImageProcessing(Bitmap image)
		{
			_image = CopyImage(image);
		}

		public Bitmap CopyImage(Bitmap image)
		{
			var newImage = new Bitmap(image.Width, image.Height, image.PixelFormat);
			using (var g = Graphics.FromImage(newImage))
			{
				var rect = new Rectangle(0, 0, image.Width, image.Height);
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(image, rect, rect, GraphicsUnit.Pixel);
			}
			return newImage;
		}

		public ImageProcessing Preview(Action<Bitmap> onPreview)
		{
			onPreview(_image);
			return this;
		}

		public ImageProcessing Grayscale()
		{
			Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
			_image = filter.Apply(_image);
			return this;
		}

		public ImageProcessing Opening()
		{
			Opening filter = new Opening();
			_image = filter.Apply(_image);
			return this;
		}

		public ImageProcessing Binarize()
		{
			Threshold filter = new Threshold(100);
			filter.ApplyInPlace(_image);
			return this;
		}

		public ImageProcessing DetectEdges()
		{
			CannyEdgeDetector filter = new CannyEdgeDetector(20, 50, 50);
			filter.ApplyInPlace(_image);
			return this;
		}

		public ImageProcessing SobelEdge()
		{
			SobelEdgeDetector sobel = new SobelEdgeDetector();
			sobel.ApplyInPlace(_image);
			return this;
		}

		public ImageProcessing Dilate()
		{
			Dilation dilation = new Dilation();
			dilation.ApplyInPlace(_image);
			return this;
		}

		public ImageProcessing HorizontalSmear()
		{
			HorizontalRunLengthSmoothing hrls = new HorizontalRunLengthSmoothing(15);
			hrls.ApplyInPlace(_image);
			return this;
		}

		public IEnumerable<Rectangle> GetBlobs()
		{
			BlobCounter bc = new BlobCounter();
			bc.ProcessImage(_image);
			return bc.GetObjectsRectangles();
		}

		public ImageProcessing EqualizeHistogram()
		{
			HistogramEqualization filter = new HistogramEqualization();
			filter.ApplyInPlace(_image);
			return this;
		}
	}
}
