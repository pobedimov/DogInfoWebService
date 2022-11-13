using DogInfoWebService.Services;
using DogInfoWebService.Settings;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// �������� ��������� ��� ������ ���������� ��������.
builder.Services.Configure<AppImageSettings>(builder.Configuration.GetSection("AppImageSettings"));

// ���������� �������� � ��������� DI.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<IDogBreedsList, DogBreedsList>();
builder.Services.AddHttpClient<IDogDownloadImage, DogDownloadImage>();
builder.Services.AddScoped<IDogImageInfoDb, DogImageInfoDb>();

// �������� ������������������ �������� � ��������� ����������. 
builder.WebHost.UseDefaultServiceProvider((context, options) =>
{
    var isDevelopment = context.HostingEnvironment.IsDevelopment();

    options.ValidateScopes = isDevelopment;
    options.ValidateOnBuild = isDevelopment;
});

// ���������� Swagger.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DogInfo API",
        Description = "ASP.NET Core Web API ��� ��������� ������ ����� � �������� �����������"
    });

    // ��������� � ������������ ������������ �� ����.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();


// ������������ Http pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
