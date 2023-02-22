using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using Zimaoshan.Xin.Cache.Foundation;
using Zimaoshan.Xin.Cache.Foundation.DependencyInjection;
using Zimaoshan.Xin.Core.Application;
using Zimaoshan.Xin.Core.Application.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder.RegisterModule(new AutofacModule(Assembly.GetExecutingAssembly())));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHybridCache(builder.Configuration);

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

app.MapGet("/test", (ITest t, IServiceProvider provider) =>
{
    var other = provider.GetService<ITest>();
    return Results.Ok($"{t.GetNowTime()} | {other!.GetNowTime()}");
});

app.MapGet("/test2", ([KeyFilter("test2")] ITest2 t, IComponentContext context) =>
{
    var test2 = context.Resolve<ITest2>();
    return Results.Ok($"{t?.GetNowTime()} | {test2?.GetNowTime()}");
});

app.MapGet("/test3", ([KeyFilter("test3")] ITest2 t, IComponentContext context) =>
{
    var test3 = context.Resolve<ITest2>();
    return Results.Ok($"{t?.GetNowTime()} | {test3?.GetNowTime()}");
});

app.Run();
