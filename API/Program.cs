using API.Extensions;
using API.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using Tickets.Aplication;
using Tickets.Domain.Settings;
using Tickets.Infraestructure.Identity.Extensions;
using Microsoft.Extensions.Configuration;
using Tickets.Infraestructure.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddIdentityInfraestructure(builder.Configuration);
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Global Exception Handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
