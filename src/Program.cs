using Azure.Messaging.ServiceBus;
using FluentValidation;
using Notification.Api.Middleware;
using Notification.Application.Contracts;
using Notification.Application.Models;
using Notification.Application.Validators;
using Notification.Infrastructure.Logging;
using Notification.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config["ServiceBus:ConnectionString"] ?? throw new ArgumentNullException("ServiceBus:ConnectionString");
    return new ServiceBusClient(connectionString);
});

// Register ServiceBusSettings from configuration
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddTransient<IValidator<EmailNotificationRequest>, EmailNotificationRequestValidator>();
builder.Services.AddTransient<IValidator<SmsNotificationRequest>, SmsNotificationRequestValidator>();

builder.Services.AddControllers();

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

// Register the logger abstraction
builder.Services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

builder.Services.AddScoped<IAuditLogger, AuditLogger>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) //for testing
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuditMiddleware();

app.MapControllers();

app.Run();