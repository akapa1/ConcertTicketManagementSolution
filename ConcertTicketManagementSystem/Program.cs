using ConcertTicketManagementSystem.Interfaces;
using ConcertTicketManagementSystem.Repositories;
using ConcertTicketManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITicketRepository, TicketRepository>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();