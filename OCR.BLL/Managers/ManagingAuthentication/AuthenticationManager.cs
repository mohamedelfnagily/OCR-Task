using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OCR.BLL.AutoMapper;
using OCR.BLL.DTOs;
using OCR.BLL.Helpers.JWTDataExtractor;
using OCR.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingAuthentication
{
	public class AuthenticationManager:IAuthenticationManager
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;

		public AuthenticationManager(UserManager<User> userManager , IConfiguration config, IMapper mapper)
		{
			_userManager = userManager;
			_config = config;
			_mapper = mapper;
		}

		public async Task<UserReadDto> RegisterNewUser(UserAddDto model)
		{
			var Errors = string.Empty;
			var user = _mapper.Map<User>(model);
			if(user==null)
			{
				return null;
			}
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					Errors += $"{error.Description},";
				return new UserReadDto { ErrorMessage = Errors };
			}
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id),
			};
			await _userManager.AddClaimsAsync(user, claims);
			UserReadDto AddedUSer = _mapper.Map<UserReadDto>(user);
			return AddedUSer;
		}

		public async Task<TokenDto> SignInUser(UserSignInDto model)
		{
			TokenDto UsersToken = new TokenDto();
			IList<Claim> UsersClaims;
			var user = await _userManager.FindByEmailAsync(model.Email);
			if(user == null)
			{
				UsersToken.Message = "Invalid Email or password, Please try again later.";
				return UsersToken;
			}
			UsersClaims = await _userManager.GetClaimsAsync(user);
			if(!await _userManager.CheckPasswordAsync(user, model.Password))
			{
				UsersToken.Message = "Invalid Email or password, Please try again later.";
				return UsersToken;
			}
			if(user.LockoutEnabled)
			{
				UsersToken.Message = "Sorry user you are currently banned, please contact our IT Team for more info.";
				return UsersToken;
			}
			UsersToken.Token = TokenGenerator(UsersClaims, JWTData.getCredentials(_config));
			UsersToken.ExpiryDuration = JWTData.GetExpiryDuration(_config);
			UsersToken.Message = "Success";
			return UsersToken;
		}

		public string TokenGenerator(IEnumerable<Claim> claims, SigningCredentials credentials)
		{
			var jwt = new JwtSecurityToken(
						claims: claims,
						signingCredentials: credentials,
						expires: DateTime.Now.AddMinutes(15),
						notBefore: DateTime.Now
					);
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.WriteToken(jwt);
			return token;
		}
	}
}
