using BillingAPI.ExternalServices;
using BillingAPI.Helper;
using BillingAPI.Settings;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configure Stripe settings for environment variables
var stripeSettings = new StripeSetting();
stripeSettings.ConfigureStripeSettings();
builder.Services.AddSingleton<StripeSetting>();

// Register Stripe service
builder.Services.AddSingleton<IStripeService, StripeService>();
builder.Services.AddSingleton<BillingHelper>();


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new
                {
                    Name = e.Key,
                    Message = e.Value?.Errors.First().ErrorMessage
                }).ToArray();

            return new BadRequestObjectResult(errors);
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
