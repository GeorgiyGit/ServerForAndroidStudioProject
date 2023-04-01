using Core.DTOs.UserDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IAccountService accountSevice;
		private readonly IUserService userService;
		public UsersController(IAccountService accountSevice, IUserService userService)
		{
			this.accountSevice = accountSevice;
			this.userService = userService;
		}

		[HttpPost]
		[Route("delete/{id}")]
		public async Task<IActionResult> DeleteUser([FromRoute] string id)
		{
			await userService.DeleteUser(id);
			return Ok();
		}

		[HttpPost]
		[Route("update")]
		public async Task<IActionResult> SignUp([FromForm] UserEditDTO model)
		{
			await userService.UpdateUser(model);
			return Ok();
		}

		[HttpGet]
		[Route("users-seeder")]
		//Generate 5 users without images and get it. Only for testing
		public async Task<IActionResult> UsersSeeder()
		{
			List<UserSignUpDTO> newUsers = new List<UserSignUpDTO>()
			{
				new UserSignUpDTO()
				{
					Username="Roma",
					Email="roma@gmail.com",
					Password="Roma1234$"
				},
				new UserSignUpDTO()
				{
					Username="Anna",
					Email="anna@gmail.com",
					Password="Anna1234$"
				},
				new UserSignUpDTO()
				{
					Username="Andrey",
					Email="andrey@gmail.com",
					Password="Andrey1234$"
				},
				new UserSignUpDTO()
				{
					Username="Bread",
					Email="bread@gmail.com",
					Password="Bread1234$"
				},
				new UserSignUpDTO()
				{
					Username="Holo",
					Email="holo@gmail.com",
					Password="r$paQ3207"
				}
			};
			for(int i = 0; i < 5; i++)
			{
				await accountSevice.SignUp(newUsers[i]);
			}
			return Ok(newUsers);
		}
	}
}
