using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

var app = builder.Build();
app.Urls.Clear();
app.Urls.Add("http://localhost:5000");
app.UseStaticFiles();
app.UseRouting();
app.UseCors(policy =>
{
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowCredentials();
    policy.WithOrigins("http://localhost:4200");
});

app.MapMethods("/", new[] { HttpMethod.Get.Method, HttpMethod.Head.Method }, async (context) =>
{
    // always need an api key, even when requesting HEAD
    if (!context.Request.Headers.ContainsKey("X-API-Key"))
    {
        await Results.Unauthorized().ExecuteAsync(context);
    }

    var actionContext = context.RequestServices.GetService<IActionContextAccessor>();
    var env = context.RequestServices.GetService<IWebHostEnvironment>();
    var fi = env.WebRootFileProvider.GetFileInfo("files/sample.pdf");

    context.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition, Content-Range");
    var result = Results.File(fi.CreateReadStream(), contentType: "application/pdf", enableRangeProcessing: true, fileDownloadName: "sample.pdf");
    await result.ExecuteAsync(context);
});

app.Run();