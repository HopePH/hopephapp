using FluentValidation;
using Yol.Punla.AttributeBase;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels.Validators
{
    [ModuleIgnore]
    public class EmailVerificationPageValidator : AbstractValidator<EmailVerificationPageViewModel>
    {
        public readonly string EMAILADDMSG = AppStrings.EnterAValidEmailText;
        public readonly string CODEEQUALMSG = AppStrings.VerficationCodeError;
        public readonly string DUPEMAIL = AppStrings.EmailExists;

        public EmailVerificationPageValidator(string verificationCode, bool isVerification, string emailDuplicate)
        {
            if(!isVerification)
                RuleFor(x => x.EmailAddress)
                    .NotEmpty()
                    .WithMessage(EMAILADDMSG)
                    .Matches(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}(\.[A-Za-z]{2})?$")
                    .WithMessage(EMAILADDMSG)
                    .NotEqual(emailDuplicate)
                    .WithMessage(DUPEMAIL);
            //else
            //    RuleFor(x => x.ConfirmVerificationCode)
            //       .Equal(verificationCode)
            //       .WithMessage(CODEEQUALMSG);
        }
    }
}
