using ePatient.Api.Common;
using ePatient.Application.Common;
using ePatient.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseServices();
builder.Services.AddValidationServices();
builder.Services.AddMediatRServices();
builder.Services.AddEndpoints();
builder.Services.AddOpenApiServices();

var app = builder.Build();

app.UseOpenApiConfiguration();

app.PopulateDatabase();

app.UseHttpsRedirection();

app.MapEndpoints();
app.Run();


