using FluentValidation;
using Template.MinimalApi.NET8.Models.Dtos;

namespace Template.MinimalApi.NET8.Validations;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Username).NotNull().EmailAddress().MinimumLength(1).MaximumLength(100);
        RuleFor(x => x.Password).NotNull().MinimumLength(1).MaximumLength(100);
    }
}