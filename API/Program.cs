using API.Extensions;
using API.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using Tickets.Infraestructure.Identity.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddIdentityInfraestructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //options.SwaggerEndpoint("/swagger/ticket/swagger.json", "Ticket Identity v1");
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
