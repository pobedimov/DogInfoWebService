using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;
using DogInfo.WebApi.Services;
using DogInfo.WebApi.Dto;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Добавление сервисов в контейнер DI.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<IDogBreedsList, DogBreedsList>();
builder.Services.AddHttpClient<IDogDownloadImage, DogDownloadImage>();
builder.Services.AddScoped<IDogImageInfoDb, DogImageInfoDb>();
builder.Services.Configure<ImageSettingsDto>(builder.Configuration.GetSection("ImageSettings"));



// Проверка зарегистрированных сервисов в окружении разработки. 
builder.WebHost.UseDefaultServiceProvider((context, options) =>
{
    var isDevelopment = context.HostingEnvironment.IsDevelopment();

    options.ValidateScopes = isDevelopment;
    options.ValidateOnBuild = isDevelopment;
});

// Добавление Swagger.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DogInfo WebAPI",
        Description = "ASP.NET Core Web API для получения породы собак и загрузки изображений"
    });

    // Включение в документацию комментариев из кода.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


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
