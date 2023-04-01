using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
	public class UserEditDTO
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public IFormFile? Avatar { get; set; }
		public bool IsOldAvatar { get; set; }
	}
}
