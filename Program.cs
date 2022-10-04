using DogInfoWebService.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер DI.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<IDogBreedsList, DogBreedsList>();
builder.Services.AddHttpClient<IDogDownloadImage, DogDownloadImage>();
builder.Services.AddScoped<IDogImageInfoDb, DogImageInfoDb>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Конфигурация Http pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
