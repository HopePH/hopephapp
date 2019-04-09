using FluentValidation;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.ViewModels
{
    //change this to like EventValidator : AbstractValidator<Event> in SA
    [DefaultModule]
    public class AccountRegistrationPageValidator : AbstractValidator<AccountRegistrationPageViewModel>
    {
        private const string EMAILADDMSG = "Please enter a valid email address.";
        private const string ALIASEMPTY = "Please enter your alias.";
        private const string MOBILENOEMPTY = "Please enter your mobile no.";
        private const string DEFAULTPICCHANGE = "Please change your 'Photo'.";

        public AccountRegistrationPageValidator()
        {
            //RuleFor(x => x.Cu.EmailAddress)
            //   .Matches(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}(\.[A-Za-z]{2})?$")
            //   .WithMessage(EMAILADDMSG);

            //RuleFor(x => x.AliasName)
            //  .NotEmpty().WithMessage(ALIASEMPTY);

            //RuleFor(x => x.MobilePhoneNo)
            // .NotEmpty().WithMessage(MOBILENOEMPTY);

            //RuleFor(x => x.Picture).NotEqual(AppImages.PandaAvatar)
            //    .WithMessage(DEFAULTPICCHANGE);
        }
    }
}
