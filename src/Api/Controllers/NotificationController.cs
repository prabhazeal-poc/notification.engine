using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Contracts;
using Notification.Application.Models;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController(
        INotificationService service,
        IValidator<EmailNotificationRequest> emailValidator,
        IValidator<SmsNotificationRequest> smsValidator,
        IAppLogger<NotificationController> logger) : ControllerBase
    {
        private readonly INotificationService _service = service;
        private readonly IValidator<EmailNotificationRequest> _emailValidator = emailValidator;
        private readonly IValidator<SmsNotificationRequest> _smsValidator = smsValidator;
        private readonly IAppLogger<NotificationController> _logger = logger;

        /// <summary>
        /// Sends an email notification.
        /// </summary>
        /// <param name="request">The email notification request.</param>
        /// <returns>Accepted if sent, BadRequest if invalid.</returns>
        [HttpPost("email")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> SendEmail([FromBody] EmailNotificationRequest request)
        {
            var validationResult = await _emailValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Email notification validation failed: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                await _service.EmailNotificationAsync(request);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email notification. NotificationId: {NotificationId}", request.NotificationId);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
        }

        /// <summary>
        /// Sends an SMS notification.
        /// </summary>
        /// <param name="request">The SMS notification request.</param>
        /// <returns>Accepted if sent, BadRequest if invalid.</returns>
        [HttpPost("sms")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> SendSms([FromBody] SmsNotificationRequest request)
        {
            var validationResult = await _smsValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("SMS notification validation failed: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                await _service.SmsNotificationAsync(request);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS notification. NotificationId: {NotificationId}", request.NotificationId);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
        }
    }
}
