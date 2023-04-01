using AutoMapper;
using Core.DTOs.UserDTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Help_elements;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class AccountService:IAccountService
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper mapper;
		private readonly IConfiguration configuration;
		private readonly IImageService imageService;
		public AccountService(UserManager<User> userManager,
							  SignInManager<User> signInManager,
							  RoleManager<IdentityRole> _roleManager,
							  IMapper mapper,
							  IConfiguration configuration,
							  IImageService imageService)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.mapper = mapper;
			this._roleManager = _roleManager;
			this.configuration = configuration;
			this.imageService = imageService;
		}

		public async Task<TokenDTO> LogIn(UserLogInDTO model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user != null && user.IsDeleted==false &&await userManager.CheckPasswordAsync(user, model.Password))
			{
				var userRoles = await userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				//var token = GetToken(authClaims);

				return new TokenDTO()
				{
					Token = await GenerateTokenAsync(user)
				};
			}
			throw new HttpException(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);
		}


		public async Task SignUp(UserSignUpDTO model)
		{
			var userExists = await userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
				throw new HttpException(ErrorMessages.UserExists, HttpStatusCode.NotFound);

			User user = new()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.Username
			};

			if (model.Avatar != null)
			{
				var avatar = await imageService.SaveUserAvatar(model.Avatar);

				if (avatar != null)
				{
					user.Avatar = avatar;
					avatar.User = user;

					await imageService.SaveImageToDatabase(avatar);
				}
			}

			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				throw new HttpException(ErrorMessages.UserCreationFailed, HttpStatusCode.InternalServerError);

			if (!await _roleManager.RoleExistsAsync(UserRoles.User))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

			if (await _roleManager.RoleExistsAsync(UserRoles.User))
			{
				await userManager.AddToRoleAsync(user, UserRoles.User);
			}
		}

		/*
		public async Task RegisterAdmin(UserSignUpDTO model)
		{
			var userExists = await userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
				throw new HttpException(ErrorMessages.UserExists, HttpStatusCode.NotFound);

			User user = new()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName
			};
			if (model.Avatar != null) user.Avatar = mapper.Map<Image>(model.Avatar);

			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				throw new HttpException(ErrorMessages.UserCreationFailed, HttpStatusCode.InternalServerError);

			if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
			if (!await _roleManager.RoleExistsAsync(UserRoles.User))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

			if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await userManager.AddToRoleAsync(user, UserRoles.Admin);
			}
			if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await userManager.AddToRoleAsync(user, UserRoles.User);
			}
		}*/

		private async Task<string> GenerateTokenAsync(User user)
		{

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var roles = await userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			// generate token
			var tokenHandler = new JwtSecurityTokenHandler();

			var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
			var key = Encoding.ASCII.GetBytes(jwtOptions.Key);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Issuer = jwtOptions.Issuer,
				Expires = DateTime.UtcNow.AddHours(jwtOptions.Lifetime), // TODO: not working - fix
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
