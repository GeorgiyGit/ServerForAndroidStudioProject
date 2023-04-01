using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Core.Resources;

namespace Core.Services
{
	public class ImageService : IImageService
	{
		private readonly List<System.Drawing.Size> USER_AVATAR_RESOLUTIONS = new List<System.Drawing.Size>()
		{
			new System.Drawing.Size(24,24),
			new System.Drawing.Size(50,50),
			new System.Drawing.Size(100,100),
			new System.Drawing.Size(400,400)
		};

		private readonly List<System.Drawing.Size> IMAGE_RESOLUTIONS = new List<System.Drawing.Size>()
		{
			new System.Drawing.Size(1920, 1080),
			new System.Drawing.Size(1024, 768),
			new System.Drawing.Size(320, 240),
			new System.Drawing.Size(64, 64)
		};

		private readonly string DIRECTORY_NAME="Uploads";


		private IWebHostEnvironment environment;
		private readonly IRepository<Image> repository;

		public ImageService(IWebHostEnvironment environment,
							IRepository<Image> repository)
		{
			this.environment = environment;
			this.repository = repository;
		}


		public async Task DeleteImageFromDatabase(int imageId)
		{
			var image = await repository.FindAsync(imageId);

			if (image == null) throw new HttpException(ErrorMessages.ImageNotFound, HttpStatusCode.NotFound);

			image.IsDeleted = true;

			repository.Update(image);

			await repository.SaveChangesAsync();
		}

		public async Task RemoveImageFile(int imageId)
		{
			var image = await repository.FindAsync(imageId);

			if (image == null) throw new HttpException(ErrorMessages.ImageNotFound, HttpStatusCode.NotFound);

			await RemoveAllImageFiles(image.FullName, IMAGE_RESOLUTIONS);
		}

		private async Task RemoveAllImageFiles(string imageFullName, List<System.Drawing.Size> resolutions)
		{
			var contentPath = this.environment.ContentRootPath;

			var path = Path.Combine(contentPath, DIRECTORY_NAME);

			foreach (var size in resolutions)
			{
				var fileWithPath = path + $"\\{size.Width}_" + imageFullName;

				if (System.IO.File.Exists(fileWithPath))
				{
					System.IO.File.Delete(fileWithPath);
				}
			}

		}

		public async Task RemoveUserAvatar(int imageId)
		{
			var image = await repository.FindAsync(imageId);

			if (image == null) throw new HttpException(ErrorMessages.ImageNotFound, HttpStatusCode.NotFound);

			await RemoveAllImageFiles(image.FullName, USER_AVATAR_RESOLUTIONS);
		}

		private async Task<Image> SaveImageWithResolution(IFormFile imageFile, List<System.Drawing.Size> resolutions)
		{
			var contentPath = this.environment.ContentRootPath;

			var path = Path.Combine(contentPath, DIRECTORY_NAME);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			var ext = Path.GetExtension(imageFile.FileName);
			var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
			if (!allowedExtensions.Contains(ext))
			{
				throw new HttpException(ErrorMessages.ImageType, HttpStatusCode.BadRequest);
			}

			string uniqueString = Guid.NewGuid().ToString();
			var newFileName = uniqueString + ext;
			var fileWithPath = Path.Combine(path, newFileName);

			foreach (var size in resolutions)
			{
				SaveImageToFileWithResolution(imageFile, size, path + $"\\{size.Width}_" + newFileName);
			}

			var image = new Image
			{
				FullName = newFileName,
				Path = fileWithPath,
				Name = imageFile.FileName
			};

			return image;
		}
		public async Task<Image> SaveImage(IFormFile imageFile)
		{
			return await SaveImageWithResolution(imageFile, IMAGE_RESOLUTIONS);
		}

		public async Task<List<Image>> SaveImages(List<IFormFile> imageFiles)
		{
			if (imageFiles == null || imageFiles.Count()==0) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			List<Image> images = new List<Image>(imageFiles.Count());

			foreach(var imageFile in imageFiles)
			{
				images.Add(await SaveImage(imageFile));
			}

			return images;
		}

		public async Task SaveImageToDatabase(Image image)
		{
			await this.repository.AddAsync(image);
		}
		public async Task UpdateImageInDatabase(Image image)
		{
			this.repository.Update(image);
		}

		public async Task<Image> SaveUserAvatar(IFormFile avatarFile)
		{
			return await SaveImageWithResolution(avatarFile, USER_AVATAR_RESOLUTIONS);
		}

		private void SaveImageToFileWithResolution(IFormFile imageFile, System.Drawing.Size size, string path)
		{
			System.Drawing.Image image = System.Drawing.Image.FromStream(imageFile.OpenReadStream(), true, true);

			System.Drawing.Bitmap resizedImage = new System.Drawing.Bitmap(image, size);

			resizedImage.Save(path);
		}
	}
}
