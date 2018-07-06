using FluentValidation;
using Yol.Punla.AttributeBase;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels.Validators
{
    [ModuleIgnore]
    public class RequestVerificationCodePageEmailValidator : AbstractValidator<RequestSigninVerificationCodePageViewModel>
    {
        public readonly string EMAILADDMSG = AppStrings.EnterAValidEmailText;
        public readonly string CODEEQUALMSG = AppStrings.VerficationCodeError;
        public readonly string NOTEXISTSEMAIL = "Your email does not exists yet. Please signup instead";

        public RequestVerificationCodePageEmailValidator(string emailString, string existingEmail)
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage(EMAILADDMSG)
                .Matches(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}(\.[A-Za-z]{2})?$")
                .WithMessage(EMAILADDMSG)
                .Equal(existingEmail)
                .WithMessage(NOTEXISTSEMAIL);
        }
    }
}
