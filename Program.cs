using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

var mimeMap = new FileExtensionContentTypeProvider();

var app = builder.Build();

app.MapGet("/{filename}", (string filename, HttpContext httpContext) =>
{
    var ip = httpContext.Connection.RemoteIpAddress?.MapToIPv4();
    if (ip == null || filename == null)
        return Results.NotFound();

    var target = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ip.ToString()), filename);
    
    if (!File.Exists(target))
        return Results.NotFound();

    var mimeType = mimeMap.TryGetContentType(target, out var mimeTypeTemp) ? mimeTypeTemp : "application/octet-stream";

    return Results.File(target, mimeType);
});

app.Run();