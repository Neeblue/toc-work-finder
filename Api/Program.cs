using Backend.Interfaces;
using Backend.Scraping;
using Backend.Services;
using Api.Controllers;
using Api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSingleton<ISelenium, Selenium>();
builder.Services.AddSingleton<ISelenium, MockSelenium>();
builder.Services.AddSingleton<IJobsController, JobsController>();
builder.Services.AddSingleton<IJobService, JobService>();
builder.Services.AddTransient<IJobFilterService, JobFilterService>();
builder.Services.AddTransient<INotifyService, EmailService>();
//builder.Services.AddSingleton<INotifyService, TextService>();
builder.Services.AddTransient<IAcceptJobService, AcceptJobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    // .WithOrigins("https://super-train-q5pvvg6qp5wc9wr7-5173.app.github.dev")
    // .withOrigins("http://localhost:5173")
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
