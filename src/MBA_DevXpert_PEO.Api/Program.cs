using MBA_DevXpert_PEO.Api.Configuration;
using MBA_DevXpert_PEO.Api.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = false;
});

// Application-specific registrations
builder.Services.RegisterApplicationServices();

builder.Services.RegisterInfraData(builder.Configuration);

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddApiConfig();
builder.Services.AddSwaggerConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerConfig(provider);
    await app.UseDatabaseSeeder();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }
