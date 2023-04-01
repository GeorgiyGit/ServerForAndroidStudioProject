using Core.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IUserService
	{
		public Task UpdateUser(UserEditDTO userDTO);
		public Task DeleteUser(string userId);
		public Task<string> GetCurrentUserId();
	}
}
