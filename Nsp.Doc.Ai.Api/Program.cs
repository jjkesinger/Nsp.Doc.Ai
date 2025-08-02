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

app.MapGet("/ask", async (string query, ChatService cs, CancellationToken ct) =>
{
    return await cs.Ask(query, ct);
});

app.MapPost("/upload", async ([FromForm]IFormFileCollection files, DocumentStorage doc, DocumentReader s, CancellationToken ct) =>
{
    foreach (var file in files)
    {
        if (file.Length > 0)
        {
            var fileName = file.FileName;
            var fileType = file.ContentType;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, ct);

            await doc.StoreDocuments(await s.ReadDocuments([(fileName, fileType, stream.ToArray())], ct), ct);
        }
    }

    return Results.Ok("Files uploaded successfully.");
}).DisableAntiforgery();

app.MapGet("/seed", async (DocumentStorage doc, DocumentReader s, CancellationToken ct) =>
{
    var list = new List<(string FileName, string FileType, byte[] Contents)>
    {
        ("test.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("This ladder is silver.")),
        ("example.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The sky is blue.")),
        ("sample.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The desk is brown.")),
        ("document.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The car is red.")),
        ("file.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The book is on the table."))
    };

    await doc.StoreDocuments(await s.ReadDocuments([.. list], ct), ct);
    return Results.Ok("Documents created successfully.");
});

app.Run();
