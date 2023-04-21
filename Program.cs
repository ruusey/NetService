using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetService.Repo;
using NetService.Service;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);
var logNames = new Collection<String>();

logNames.Add("Application");
logNames.Add("Security");
builder.Services.AddDbContext<EventLogRepo>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddControllers();

//builder.Services.AddSingleton<IEventReaderService>(x =>
//    ActivatorUtilities.CreateInstance<EventReaderService>(x, parameters: new object[] { logNames }));

builder.Services.AddScoped<IEventReaderService, EventReaderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//var sp = builder.Services.BuildServiceProvider();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var serviceProvider = app.Services.CreateScope().ServiceProvider;
var hostingEnv = serviceProvider.GetService<IEventReaderService>();
hostingEnv.beginCollection();


app.Run();

