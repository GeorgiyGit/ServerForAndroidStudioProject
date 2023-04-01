using Core.DTOs.UserDTOs;
using Core.Help_elements;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Validators.UserValidators
{
    internal class UserEditValidator : AbstractValidator<UserEditDTO>
    {
        public UserEditValidator()
        {
            RuleFor(e => e.Id)
                .NotNull()
                .NotEmpty();

			RuleFor(e => e.Username)
				.NotNull()
				.NotEmpty()
				.MinimumLength(StaticProperties.USER_NAME_MIN_LENGTH)
				.MaximumLength(StaticProperties.USER_NAME_MAX_LENGTH);

			RuleFor(e => e.Email)
				.NotNull()
				.NotEmpty()
				.MinimumLength(StaticProperties.EMAIL_MIN_LENGTH)
				.MaximumLength(StaticProperties.EMAIL_MAX_LENGTH)
				.EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);

			RuleFor(e => e.Password)
				.NotNull()
				.NotEmpty()
				.Must(x => HasValidPassword(x));
		}
		private bool HasValidPassword(string pw) => Regex.IsMatch(pw, @"^(?=.*\d)(?!.*\s)(?=.*[a-zA-Z]).{8,20}$");
	}
}
