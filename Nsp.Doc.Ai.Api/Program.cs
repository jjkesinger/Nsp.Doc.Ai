using Nsp.Doc.Ai.Domain;
using Nsp.Doc.Ai.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDomainServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/test", async (IServiceProvider sp, CancellationToken ct) =>
{
    Nsp.Doc.Ai.Domain.Model.Document[] docs = [new Nsp.Doc.Ai.Domain.Model.Document
    {
        Key = 1,
        Summary = "Test summary. The color is blue.",
        Title = "Testing 123"
    }];

    var doc = sp.GetRequiredService<DocumentStorage>();
    await doc.StoreDocuments("test", docs, ct);
})
.WithName("Test");

app.Run();
