using Microsoft.AspNetCore.Mvc;
using Nsp.Doc.Ai.Domain;
using Nsp.Doc.Ai.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
builder.Services.AddOpenApi();
builder.Services.AddDomainServices(builder.Configuration);
builder.Services.AddAntiforgery();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.MapOpenApi();
}

app.UseAntiforgery();
app.UseHttpsRedirection();

app.MapGet("/ask", async (string query, ChatService cs, CancellationToken ct) => {
    return Results.Json(new { message = await cs.Ask(query, ct) });
});

app.MapPost("/upload", async ([FromForm]IFormFileCollection files, DocumentStorage ds, DocumentReader dr, CancellationToken ct) =>
{
    var uploadCount = await ds.StoreDocuments(
        await dr.ReadDocuments(
        [.. await Task.WhenAll(files.Select(async file =>
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream, ct);
                return (file.FileName, file.ContentType, stream.ToArray());
            }))], ct), ct);

    return Results.Json(new { message = $"{uploadCount} File(s) uploaded." });
}).DisableAntiforgery();

app.Run();
