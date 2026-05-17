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
    { "131189", "http://v3tv.live:80/get.php?username=drmtv30_311086&password=2IAIkYmK&type=m3u_plus

" },
    { "1010", "http://96122-low.tx-4kott.com:80/get.php?username=411ae93831&password=3ff865fe8e20&type=m3u_plus

" }
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
