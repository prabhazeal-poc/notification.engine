using FluentValidation;
using Notification.Application.Models;


namespace Notification.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="SmsNotificationRequest"/>. Ensures required fields are present and valid,
    /// including NotificationId, CorrelationId, Recipients, Channel, and Message.
    /// </summary>
    public class SmsNotificationRequestValidator : AbstractValidator<SmsNotificationRequest>
    {
        public SmsNotificationRequestValidator()
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
                .IsInEnum().WithMessage("Channel is required.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.");
        }
    }
}
