using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// تفعيل الـ CORS لتسهيل استقبال الطلبات من تطبيقك
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

// هيكل كائن مخصص لبيانات الـ Xtream لمنع أخطاء الـ Build
public record XtreamAccount(string ServerUrl, string Username, string Password);

// 🔑 جدول التفعيل الرئيسي بنظام Xtream Codes
var activationCodes = new Dictionary<string, XtreamAccount>
{
    { 
        "112233", 
        new XtreamAccount("http://v3tv.live:80", "user_drmtv30_311086", "password_2IAIkYmK&type") 
    }
    // يمكنك إضافة أكواد أخرى هنا بنفس الطريقة تماماً
};

// الـ Endpoint الخاص بالتحقق
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
