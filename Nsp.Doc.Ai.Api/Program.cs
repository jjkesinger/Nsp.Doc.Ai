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

app.MapGet("/test", async (string query, IServiceProvider sp, DocumentStorage doc, DocumentReader s, ChatService cs, CancellationToken ct) =>
{
    var collectionName = "test2";
    // var list = new List<(string FileName, string FileType, byte[] Contents)>
    // {
    //     ("test.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("This ladder is silver.")),
    //     ("example.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The sky is blue.")),
    //     ("sample.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The desk is brown.")),
    //     ("document.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The car is red.")),
    //     ("file.txt", "text/plain", System.Text.Encoding.UTF8.GetBytes("The book is on the table."))
    // };

    // var docs = await s.ReadDocuments(list, ct);
    // await doc.StoreDocuments(collectionName, docs, ct);

    return await cs.Test(collectionName, query, ct);
})
.WithName("Test");

app.Run();
