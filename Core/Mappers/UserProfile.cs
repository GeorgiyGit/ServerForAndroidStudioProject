using Core.DTOs.UserDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mappers
{
	internal class UserProfile : AutoMapper.Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserDTO>();

			CreateMap<UserSignUpDTO, User>();
			CreateMap<UserLogInDTO, User>();

			CreateMap<UserEditDTO, User>();
		}
	}
}
