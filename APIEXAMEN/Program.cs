using APISTEL.Models;
using Microsoft.EntityFrameworkCore;
using static APISTEL.Utils.Utils;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
// Cadena de conexion MySQL/Pomelo 01/03/2024
var ConnectionString = builder.Configuration.GetConnectionString("defaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200", "https://localhost:44325").AllowAnyHeader().AllowAnyMethod();
                          //Peticiones desde cualquier URL
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

// Add services to the container.
// Date Converted  04/03/2024
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
