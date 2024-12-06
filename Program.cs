using LebaraSign.Services;
using LebaraSign.Services.Storage;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json.Linq;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetSection("AzureBlobStorage:ConnectionString").Value;
var containerName = builder.Configuration.GetSection("AzureBlobStorage:ContainerName").Value;

builder.Services.AddAzureClients(ClientBuilder => {
    ClientBuilder.AddTableServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString"]);
    ClientBuilder.AddBlobServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString"]);
});

builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//using LebaraSign.Services;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Newtonsoft.Json.Linq;
//using System.Configuration;
//using Microsoft.OpenApi.Models;
//using LebaraSign.Common;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();

//// Configure BlobService
//var connectionString = builder.Configuration.GetSection("AzureBlobStorage:ConnectionString").Value;
//var containerName = builder.Configuration.GetSection("AzureBlobStorage:ContainerName").Value;
//builder.Services.AddSingleton(new BlobService(connectionString, containerName));

//// Configure Swagger
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blob Storage API", Version = "v1" });
//    //c.OperationFilter<FileUploadOperation>();
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//// Enable Swagger
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blob Storage API V1");
//    //c.RoutePrefix = string.Empty;
//});

//app.Run();

