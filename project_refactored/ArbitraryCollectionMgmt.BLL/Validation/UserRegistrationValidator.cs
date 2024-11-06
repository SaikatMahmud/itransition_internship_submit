using ArbitraryCollectionMgmt.BLL.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.Validation
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationVM>
    {
        public UserRegistrationValidator() {

            RuleFor(u => u.Name).NotEmpty();
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");

        }
    }
}
