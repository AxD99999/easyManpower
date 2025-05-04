using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ILogin, LoginService>();
builder.Services.AddScoped<ISession, SessionService>();
builder.Services.AddScoped<IChangePassword, ChangePasswordService>();
builder.Services.AddScoped<IUpdateUser, UpdateUserService>();
builder.Services.AddScoped<ICountrySet, CountrySetService>();
builder.Services.AddScoped<IPostSet, PostSetService>();
builder.Services.AddScoped<IRefCountrySet, RefCountrySetService>();
builder.Services.AddScoped<IRefOutCountrySet, RefOutCountrySetService>();
builder.Services.AddScoped<ICompanySet, CompanySetService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS middleware
app.UseCors("AllowAllOrigins");

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