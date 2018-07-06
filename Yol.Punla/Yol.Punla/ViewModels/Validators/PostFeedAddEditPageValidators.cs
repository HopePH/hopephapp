using FluentValidation;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels
{
    public class PostFeedAddEditPageValidators : AbstractValidator<PostFeedAddEditPageViewModel>
    {
        private readonly string emptyPostError = AppStrings.EmptyPostErrorMessage;

        public PostFeedAddEditPageValidators()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                    .WithMessage(emptyPostError);
        }
    }
}
