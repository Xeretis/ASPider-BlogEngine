using System.Reflection;
using Application.MappingProfiles;
using Auth;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//ApiUserProfile is in a separate project -> different assembly
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(ApiUserProfile)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await SeedDatabase.Initialize(app.Services);

app.Run();