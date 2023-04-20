using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetService.Repo;
using NetService.Service;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);
var logNames = new Collection<String>();

logNames.Add("Application");
logNames.Add("Security");
builder.Services.AddSingleton<IEventReaderService>(x =>
    ActivatorUtilities.CreateInstance<EventReaderService>(x, logNames));
builder.Services.AddControllers();
builder.Services.AddDbContext<TodoRepo>(opt =>
    opt.UseInMemoryDatabase("TodoList"));



//builder.Services.AddScoped<IEventReaderService, EventReaderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();