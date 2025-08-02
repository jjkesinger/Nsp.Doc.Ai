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

app.Run();
