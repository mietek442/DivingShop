using Api.Features.Common.Services.Storage;
using Api.Features.Common.Services.UrlHelper;
using Api.Infrastructure.DbContext;
using Api.Infrastructure.Storage;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SupportNonNullableReferenceTypes();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Diving Shop Api", Version = "v1" });


    option.EnableAnnotations();
});

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IApplicationContext, ApplicationContext>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
