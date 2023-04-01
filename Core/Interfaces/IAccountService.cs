using Core.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IAccountService
	{
		public Task<TokenDTO> LogIn(UserLogInDTO model);
		public Task SignUp(UserSignUpDTO model);
	}
}
