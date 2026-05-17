using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// إعداد المنفذ (Port) ليتوافق مع السيرفر السحابي
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

var app = builder.Build();

// قاعدة بيانات تجريبية للأكواد والروابط الخاصة بالمشتركين
var userSubscriptions = new Dictionary<string, string>
{
    { "PIKA-112233", "http://server.com:8080/live/user1/pass1/1.m3u8" },
    { "PIKA-998877", "http://server.com:8080/live/user2/pass2/2.m3u8" }
};

// الرابط الذي سيتصل به تطبيق الكمبيوتر للتأكد من الكود
app.MapGet("/api/activation/verify/{code}", (string code) =>
{
    if (userSubscriptions.TryGetValue(code, out var url))
    {
        return Results.Ok(new { success = true, url = url, message = "Welcome to PikaTV" });
    }
    return Results.Json(new { success = false, message = "كود التفعيل غير صحيح!" }, statusCode: 401);
});

// صفحة ترحيبية أساسية للتأكد من أن السيرفر يعمل
app.MapGet("/", () => "PikaTV Server is Running!");

app.Run();