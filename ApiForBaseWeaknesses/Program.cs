using ApiForBaseWeaknesses;
using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddScoped<Service>();
builder.Services.AddHttpClient<Vulnerability>();

// В Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });
//swagger
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


app.Run();