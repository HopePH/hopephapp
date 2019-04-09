using FluentValidation;
using Yol.Punla.AttributeBase;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels.Validators
{
    [ModuleIgnore]
    public class VerificationCodeValidator : AbstractValidator<ConfirmVerificationCodePageViewModel>
    {
        public readonly string CODEEQUALMSG = AppStrings.VerficationCodeError;

        public VerificationCodeValidator(string verificationCodeEntered)
        {
            RuleFor(x => x.VerificationCode)
                   .Equal(x => x.VerificationCodeEntered)
                   .WithMessage(CODEEQUALMSG);
        }
    }
}
