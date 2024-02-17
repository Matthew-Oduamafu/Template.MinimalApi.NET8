using FluentValidation;
using Template.MinimalApi.NET8.Models.Dtos;

namespace Template.MinimalApi.NET8.Validations;

public class AppUserDtoValidator : AbstractValidator<AppUserDto>
{
    public AppUserDtoValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email).NotNull().EmailAddress();
        RuleFor(x => x.Password)
            .NotNull()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$")
            .WithMessage(
                "Password must be at least 6 characters and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")
            .MinimumLength(6);
        RuleFor(x => x.FirstName).NotNull().MinimumLength(1).MaximumLength(50);
        RuleFor(x => x.LastName).NotNull().MinimumLength(1).MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotNull().MinimumLength(1).MaximumLength(15)
            .Matches(@"^(233(20|24|26|27|54|55|57|59)\d{7})")
            .WithMessage("Invalid Ghanaian phone number. It must be a MTN, Vodafone, or AirtelTigo number.");
    }
}