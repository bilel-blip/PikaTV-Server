using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// تفعيل ميزة الـ CORS لضمان استقبال الطلبات من التطبيق دون قيود أمنية
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

// 🔑 [جدول التفعيل الرئيسي]: يمكنك إضافة أو تعديل أي كود هنا بكل سهولة وسلاسة
var activationCodes = new Dictionary<string, string>
{
    { "131189", "http://v3tv.live:80/get.php?username=drmtv30_311086&password=2IAIkYmK&type=m3us" },
    { "2026", "http://96122-low.tx-4kott.com:80/get.php?username=411ae93831&password=3ff865fe8e20&type=m3u" }
    // لإضافة مستخدم أو كود جديد، فقط أضف سطر جديد هنا بنفس الطريقة تماماً
};

// 🌐 الـ Endpoint الخاص بالتحقق من الأكواد للتطبيق الخاص بك
app.MapGet("/api/activation/verify/{code}", (string code) =>
{
    if (string.IsNullOrWhiteSpace(code))
    {
        return Results.BadRequest(new { message = "كود التفعيل لا يمكن أن يكون فارغاً." });
    }

    // البحث في الجدول الذكي عن الكود المرسل
    if (activationCodes.TryGetValue(code.Trim(), out var m3uUrl))
    {
        return Results.Ok(new { m3uUrl = m3uUrl });
    }

    // إذا لم يتم العثور على الكود
    return Results.NotFound(new { message = "كود التفعيل غير صحيح أو منتهي الصلاحية." });
});

// تشغيل السيرفر تلقائياً على المنفذ المطلوب
app.Run();
