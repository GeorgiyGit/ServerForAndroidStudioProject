using Core.DTOs.UserDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService accountSevice;
		public AccountController(IAccountService accountSevice)
		{
			this.accountSevice = accountSevice;
		}

		[HttpPost]
		[Route("log-in")]
		public async Task<IActionResult> LogIn([FromBody] UserLogInDTO model)
		{
			return Ok(await accountSevice.LogIn(model));
		}

		[HttpPost]
		[Route("sign-up")]
		public async Task<IActionResult> SignUp([FromForm] UserSignUpDTO model)
		{
			await accountSevice.SignUp(model);
			return Ok();
		}
	}
}
