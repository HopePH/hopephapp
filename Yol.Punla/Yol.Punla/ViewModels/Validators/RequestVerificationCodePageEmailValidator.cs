using FluentValidation;
using Yol.Punla.AttributeBase;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels.Validators
{
    [ModuleIgnore]
    public class RequestVerificationCodePageEmailValidator : AbstractValidator<RequestSigninVerificationCodePageViewModel>
    {
        private const string FAKEEMAIL = "Ret45ujhh@gboy.com";
        private const string NOTEXISTSEMAIL = "Your email does not exists yet. Please signup instead";
        private readonly string EMAILADDMSG = AppStrings.EnterAValidEmailText;
        private readonly string CODEEQUALMSG = AppStrings.VerficationCodeError;

        public RequestVerificationCodePageEmailValidator(string emailString)
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage(EMAILADDMSG)
                .Matches(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}(\.[A-Za-z]{2})?$")
                .WithMessage(EMAILADDMSG)
                .NotEqual(FAKEEMAIL)
                .WithMessage(NOTEXISTSEMAIL);
        }
    }
}
