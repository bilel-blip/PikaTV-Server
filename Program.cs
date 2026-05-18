// 🔑 جدول التفعيل المطور لدعم بيانات الـ Xtream Codes
var activationCodes = new Dictionary<string, object>
{
    { 
        "PIKA-112233", new {
            serverUrl = "http://your-iptv-server.com:8080", // رابط سيرفر الـ Xtream
            username = "user_bilal",                         // اسم المستخدم
            password = "password_pika"                       // كلمة المرور
        }
    }
};

// الـ Endpoint المطور
app.MapGet("/api/activation/verify/{code}", (string code) =>
{
    if (string.IsNullOrWhiteSpace(code)) return Results.BadRequest(new { message = "الكود فارغ" });

    if (activationCodes.TryGetValue(code.Trim(), out var xtreamData))
    {
        return Results.Ok(xtreamData); // إرسال البيانات الثلاثية للتطبيق
    }

    return Results.NotFound(new { message = "الكود غير صحيح" });
});
