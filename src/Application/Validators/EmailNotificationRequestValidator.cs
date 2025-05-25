using FluentValidation;
using Notification.Application.Models;
using Notification.Domain.Enums;

namespace Notification.Application.Validators
{
    /// <summary>
    /// Validates the <see cref="EmailNotificationRequest"/> to ensure all required fields are present and valid.
    /// </summary>
    public class EmailNotificationRequestValidator : AbstractValidator<EmailNotificationRequest>
    {
        public EmailNotificationRequestValidator()
        {
            RuleFor(x => x.NotificationId)
                .NotEmpty().WithMessage("NotificationId is required.")
                 .Must(id => Guid.TryParse(id, out _)).WithMessage("NotificationId must be a valid GUID.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("CorrelationId must be a valid GUID.");

            RuleFor(x => x.Recipients)
                .NotNull().WithMessage("Recipients are required.")
                .Must(r => r.Count > 0).WithMessage("At least one recipient is required.");


            RuleFor(x => x.Channel)
                .NotEmpty().WithMessage("Channel is required.")
                .Must(channel =>
                {
                    return Enum.TryParse(typeof(NotificationChannel), channel, true, out _);
                })
                .WithMessage("Channel must be a valid value.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Priority is required and must be valid.");

            RuleForEach(x => x.MergeTags)
                .NotEmpty().WithMessage("Merge tag values cannot be empty.");
        }
    }
}
