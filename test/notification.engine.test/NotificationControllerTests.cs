using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Notification.Api.Controllers;
using Notification.Application.Contracts;
using Notification.Application.Models;
using Notification.Domain.Enums;

public class NotificationControllerTests
{
    private readonly Mock<INotificationService> _serviceMock = new();
    private readonly Mock<IValidator<EmailNotificationRequest>> _emailValidatorMock = new();
    private readonly Mock<IValidator<SmsNotificationRequest>> _smsValidatorMock = new();
    private readonly Mock<IAppLogger<NotificationController>> _loggerMock = new();

    private NotificationController CreateController() =>
        new(_serviceMock.Object, _emailValidatorMock.Object, _smsValidatorMock.Object, _loggerMock.Object);

    [Fact]
    public async Task SendEmail_ReturnsAccepted_WhenRequestIsValid()
    {
        // Arrange
        var request = new EmailNotificationRequest
        {
            NotificationId = "id1",
            CorrelationId = "corr1",
            Recipients = new() { "user@example.com" },
            Subject = "Test",
            Content = "Hello",
            Priority = Priority.Normal,
            MergeTags = new()
        };

        _emailValidatorMock
            .Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        _serviceMock
            .Setup(s => s.EmailNotificationAsync(request))
            .Returns(Task.CompletedTask);

        var controller = CreateController();

        // Act
        var result = await controller.SendEmail(request);

        // Assert
        result.Should().BeOfType<AcceptedResult>();
    }

    [Fact]
    public async Task SendEmail_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new EmailNotificationRequest
        {
            NotificationId = "",
            CorrelationId = "",
            Recipients = new(),
            Subject = "",
            Content = "",
            Priority = Priority.Normal,
            MergeTags = new()
        };

        var failures = new[]
        {
            new ValidationFailure("NotificationId", "NotificationId is required.")
        };

        _emailValidatorMock
            .Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult(failures));

        var controller = CreateController();

        // Act
        var result = await controller.SendEmail(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}