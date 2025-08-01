using Nsp.Doc.Ai.Domain;
using Nsp.Doc.Ai.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
builder.Services.AddOpenApi();
builder.Services.AddDomainServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/ask", async (string query, ChatService cs, CancellationToken ct) =>
{
    return await cs.Ask(query, ct);
});

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

    await doc.StoreDocuments(await s.ReadDocuments(list, ct), ct);
    return Results.Ok("Documents created successfully.");
});

app.Run();
