using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// تفعيل حزمة CORS لضمان تواصل سلس مع التطبيق
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

// 🔑 [جدول الـ Xtream الأساسي] ضع روابطك وبيانات الاشتراك الحقيقية هنا
var activationCodes = new Dictionary<string, XtreamAccount>
{
    { 
        "PIKA-112233", 
        new XtreamAccount("http://kljx7.m4rv-el.space:2095", "user_5432157882", "password_77643245") 
    }
};

// الـ Endpoint المستقر لاستقبال طلبات التفعيل
app.MapGet("/api/activation/verify/{code}", (string code) =>
{
    if (string.IsNullOrWhiteSpace(code))
    {
        return Results.BadRequest(new { message = "الكود فارغ" });
    }

    if (activationCodes.TryGetValue(code.Trim(), out var account))
    {
        return Results.Ok(new 
        { 
            serverUrl = account.ServerUrl, 
            username = account.Username, 
            password = account.Password 
        });
    }

    return Results.NotFound(new { message = "الكود غير صحيح" });
});

app.Run();

// 🛠️ تعريف هيكل البيانات بشكل خارجي سليم لمنع أخطاء الـ Compilation
public record XtreamAccount(string ServerUrl, string Username, string Password);
