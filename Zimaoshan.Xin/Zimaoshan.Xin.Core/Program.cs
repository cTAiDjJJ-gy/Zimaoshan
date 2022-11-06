using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Zimaoshan.Xin.Cache.Foundation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
