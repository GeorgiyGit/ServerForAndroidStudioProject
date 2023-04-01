using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IImageService
	{
		public Task<Image> SaveImage(IFormFile imageFile);
		public Task<List<Image>> SaveImages(List<IFormFile> imageFile);
		public Task<Image> SaveUserAvatar(IFormFile avatarFile);
		
		public Task SaveImageToDatabase(Image image);
		public Task UpdateImageInDatabase(Image image);

		public Task DeleteImageFromDatabase(int imageId);
		public Task RemoveImageFile(int imageId);
		public Task RemoveUserAvatar(int imageId);
	}
}
