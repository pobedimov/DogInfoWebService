using DogInfoWebService.Services;

var builder = WebApplication.CreateBuilder(args);

// ���������� �������� � ��������� DI.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<IDogBreedsList, DogBreedsList>();
builder.Services.AddHttpClient<IDogDownloadImage, DogDownloadImage>();
builder.Services.AddScoped<IDogImageInfoDb, DogImageInfoDb>();

builder.Services.AddSwaggerGen();

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
