using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// تفعيل الـ CORS لضمان استقبال الطلبات من التطبيق دون قيود
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

// 🔑 [جدول الـ Xtream الرئيسي] ضع هنا بيانات السيرفر الشغال والمضمون لديك
var activationCodes = new Dictionary<string, XtreamAccount>
{
    { 
        "112233", 
        new XtreamAccount("http//kljx7.m4rv-el.space:2095", "user_5432157882", "password_77643245") 
    }
};

// الـ Endpoint الخاص بالتحقق من الأكواد
app.MapGet("/api/activation/verify/{code}", (string code) =>
{
    if (string.IsNullOrWhiteSpace(code))
    {
        return Results.BadRequest(new { message = "الكود فارغ" });
    }

    if (activationCodes.TryGetValue(code.Trim(), out var account))
    {
        // إرسال البيانات بحروف صغيرة صريحة لتجنب أي تحويل تلقائي يعطل التطبيق
        return Results.Ok(new 
        { 
            serverurl = account.ServerUrl, 
            username = account.Username, 
            password = account.Password 
        });
    }

    return Results.NotFound(new { message = "الكود غير صحيح" });
});

app.Run();

// هيكل البيانات الثابت
public record XtreamAccount(string ServerUrl, string Username, string Password);
