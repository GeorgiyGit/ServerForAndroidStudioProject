using Core.DTOs.ImageDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
	internal class ImageProfile : AutoMapper.Profile
	{
		public ImageProfile()
		{
			CreateMap<Image, ImageDTO>();
		}
	}
}
