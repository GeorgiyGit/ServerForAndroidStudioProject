using AutoMapper;
using Core.DTOs.UserDTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class UserService : IUserService
	{
		private IHttpContextAccessor httpContextAccessor;
		private readonly IMapper mapper;
		private readonly IImageService imageService;
		private readonly UserManager<User> userManager;

		public UserService(IHttpContextAccessor httpContextAccessor,
						   IMapper mapper,
						   IImageService imageService,
						   UserManager<User> userManager)
		{
			this.httpContextAccessor = httpContextAccessor;
			this.mapper = mapper;
			this.imageService = imageService;
			this.userManager = userManager;
		}
		public async Task DeleteUser(string userId)
		{
			User user = await userManager.FindByIdAsync(userId);

			if (user == null) throw new HttpException(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);

			user.IsDeleted = true;

			await userManager.UpdateAsync(user);
		}

		public async Task<string> GetCurrentUserId()
		{
			return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public async Task UpdateUser(UserEditDTO userDTO)
		{
			//if (repository.FindAsync(userDTO.Id) == null) throw new HttpException(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);

			User origUser = await userManager.FindByIdAsync(userDTO.Id);

			if (origUser == null || origUser.IsDeleted) throw new HttpException(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);
			
			var resetToken = await userManager.GeneratePasswordResetTokenAsync(origUser);
			await userManager.ResetPasswordAsync(origUser, resetToken, userDTO.Password);

			origUser = await userManager.FindByIdAsync(userDTO.Id);

			origUser.IsEdited = true;

			if (userDTO.Avatar != null && userDTO.IsOldAvatar == false)
			{
				var image = await imageService.SaveUserAvatar(userDTO.Avatar);

				if (origUser.Avatar == null)
				{
					origUser.Avatar = image;
					image.User = origUser;

					await imageService.SaveImageToDatabase(image);
				}
				else
				{
					origUser.Avatar.Path = image.Path;
					origUser.Avatar.FullName = image.FullName;
					origUser.Avatar.Name = image.Name;
					origUser.Avatar.IsEdited = true;


					await imageService.UpdateImageInDatabase(origUser.Avatar);
				}
			}

			origUser.Email = userDTO.Email;
			origUser.UserName = userDTO.Username;

			await userManager.UpdateAsync(origUser);
		}
	}
}
