using Microsoft.EntityFrameworkCore;
using EmotionsApp.Domain.Interfaces;
using EmotionsApp.Infrastructure;
using EmotionsApp.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddTransient<IEmotionNoteService, EmotionNoteService>();
builder.Services.AddTransient<IEmotionNoteRepository, EmotionNoteRepository>();
builder.Services.AddTransient<IEmotionService, EmotionService>();
builder.Services.AddTransient<IEmotionRepository, EmotionRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPsychologistService, PsychologistService>();
builder.Services.AddTransient<IPsychologistRepository, PsychologistRepository>();
builder.Services.AddDbContext<EmotionsAppContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
