using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
	public class UserDTO
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public ImageDTO Avatar { get; set; }
	}
}
