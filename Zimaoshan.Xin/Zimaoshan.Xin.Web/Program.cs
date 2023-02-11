using Microsoft.AspNetCore.Mvc;
using Zimaoshan.Xin.Cache.Foundation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHybridCache();

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

app.MapGet("/", () => "Hello World!");

app.MapPost("/cache/{key}", (string key, ICache cache, [FromBody] string data) =>
{
    cache.Set(key, data);
    return Results.Ok("success");
});

app.MapGet("/cache/{key}", (string key, ICache cache) =>
{
    var obj = cache.Get<string>(key);
    return obj == null ? Results.NotFound() : Results.Ok(obj);
});

app.MapDelete("/cache/{key}", (string key, ICache cache) =>
{
    cache.Remove(key);
    return Results.Ok("success");
});

app.Run();
