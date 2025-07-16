using Api;
using Api.Features.Common.Services.Storage;
using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using Api.Infrastructure.Storage;
using Azure.Storage.Blobs;
using Deepseek.AspClient.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .WithMethods("GET")
              .WithOrigins("http://localhost:8804")
              .AllowCredentials();
    });
});

builder.Services.AddApplication();
// to jest od dodania migracji do bazy(tak to działa że musi być asynchronicznie to 
Task.Run(async () => await builder.Services.AddInfrastructureAsync(builder.Configuration)).Wait();



builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



builder.Services.AddControllers().AddJsonOptions(option =>
{

    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}
);
builder.Services.AddSingleton(provider =>
                new DeepseekClient("sk-5c0412dca29e46c79bc242fd39b2711d"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SupportNonNullableReferenceTypes();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Diving Shop Api", Version = "v1" });


    option.EnableAnnotations();
});

/*builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});*/

//builder.Services.AddScoped<IApplicationContext, ApplicationContext>();


builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));

builder.Services.AddSingleton<IUrlHelpers, UrlHelpers>();

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.ApplyMigrationsAsync();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
