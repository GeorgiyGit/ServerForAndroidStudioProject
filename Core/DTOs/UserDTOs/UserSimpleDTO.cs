using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
	public class UserSimpleDTO
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public ImageDTO Avatar { get; set; }
	}
}
