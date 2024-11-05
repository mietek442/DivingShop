using Api.Features.Common.Services.Storage;
using Api.Infrastructure.DbContext;
using Api.Infrastructure.Storage;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IApplicationContext, ApplicationContext>();

// dodanie mediatora (Adding mediator)
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());


});
// Dodaj us³ugi do kontenera (Add services to the container)

builder.Services.AddControllers();
// Dowiedz siê wiêcej o konfiguracji Swagger/OpenAPI na https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
option.SupportNonNullableReferenceTypes();
option.SwaggerDoc("v1", new OpenApiInfo { Title = "Diving Shop Api", Version = "v1" });


    //zezwolenie na annotations czyli SwaggerOperation gdzie ustawiamy np. Tags (grupujemy endpointy po tym)
    option.EnableAnnotations();


});
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// blob service dla Azure (blob service for Azure)
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));


var app = builder.Build();


// Konfiguracja potoku ¿¹dañ HTTP (Configure the HTTP request pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
