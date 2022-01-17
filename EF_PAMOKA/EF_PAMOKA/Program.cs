using EF_PAMOKA.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<PavyzdinisDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PavyzdinisDbContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true));

using (var scope = app.Services.CreateScope()) { var services = scope.ServiceProvider; var context = services.GetRequiredService<PavyzdinisDbContext>(); context.Database.Migrate(); }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
