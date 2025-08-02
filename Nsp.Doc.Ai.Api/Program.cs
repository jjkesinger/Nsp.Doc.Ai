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

app.MapGet("/ask", async (string query, ChatService cs, CancellationToken ct) => await cs.Ask(query, ct));

app.MapPost("/upload", async ([FromForm]IFormFileCollection files, DocumentStorage doc, DocumentReader s, CancellationToken ct) =>
{
    var docFiles = (await Task.WhenAll(files.Select(async file =>
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, ct);
        return (file.FileName, file.ContentType, stream.ToArray());
    }))).ToList();

    var docs = await s.ReadDocuments(docFiles, ct);
    await doc.StoreDocuments(docs, ct);

    return Results.Ok($"{docs.Length} File(s) uploaded successfully.");
}).DisableAntiforgery();

app.Run();
