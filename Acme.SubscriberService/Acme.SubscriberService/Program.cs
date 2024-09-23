using Acme.SubscriberService.Application.FileImport;
using Acme.SubscriberService.AutoMapper;
using Acme.SubscriberService.Data;
using Acme.SubscriberService.Exceptions;
using Acme.SubscriberService.Interfaces;
using Acme.SubscriberService.Jobs;
using Acme.SubscriberService.Notifier;
using Acme.SubscriberService.Repositories;
using Acme.SubscriberService.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add services to the container.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationPipeline<,>));
});

builder.Services.AddQuartz(options =>
{
    var jobKey = JobKey.Create(nameof(NotificationJob));
    options
    .AddJob<NotificationJob>(jobKey)
    .AddTrigger(trigger =>
        trigger
            .ForJob(jobKey)
            .WithSimpleSchedule(schedule =>
                schedule.WithIntervalInMinutes(2).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddValidatorsFromAssemblyContaining<FileImportCommandValidator>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionNotifier, MultiChannelNotifier>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173");
        builder.WithMethods("GET", "POST");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
