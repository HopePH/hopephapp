using FluentValidation;
using Yol.Punla.AttributeBase;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels
{
    [DefaultModule]
    public class PostFeedDetailsPageValidator : AbstractValidator<PostFeedDetailPageViewModel>
    {
        public readonly string emptyCommentError = AppStrings.EmptyCommentErrorMessage;

        public PostFeedDetailsPageValidator()
        {
            RuleFor(x => x.CommentText)
                .NotEmpty()
                    .WithMessage(emptyCommentError);
        }
    }
}
