# NSP.Doc.AI – Project Setup & Semantic Kernel Overview

[![Watch the video](https://img.youtube.com/vi/BgFDT4RdXqA/0.jpg)](https://www.youtube.com/watch?v=BgFDT4RdXqA)

---

## Project Overview

NSP.Doc.AI is an AI-powered document assistant leveraging Azure OpenAI for advanced language and embedding capabilities, and Qdrant for vector storage. The solution includes a web UI (Angular), a .NET 9 backend, and seamless integration with Azure OpenAI and Qdrant.

---

## Prerequisites

1. **.NET 9 SDK**  
   Download and install from: [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)  
   _If you’re on Ubuntu 24.04, download the tarball and extract it manually. Add it to your PATH._

2. **Azure Subscription**  
   Sign up or log in at: [https://portal.azure.com](https://portal.azure.com)

3. **Azure OpenAI Resource**  
   - Create an Azure OpenAI resource in the Azure Portal.
   - Deploy the following models:
     - `text-embedding-3-large`
     - `gpt-4o`
   - Note the deployment names for configuration.

4. **Qdrant Vector Storage**  
   - Run locally with Docker:  
     ```
     docker run -p 6333:6333 -p 6334:6334 qdrant/qdrant
     ```
   - Or use [Qdrant Cloud](https://qdrant.tech/documentation/).

---

## Configuration

Set the following environment variables (or add to `appsettings.json`):

- `AzureOpenAiEndpoint` – Your Azure OpenAI endpoint URL
- `AzureOpenAiKey` – Your Azure OpenAI API key
- `QdrantHost` – Qdrant endpoint (e.g., `http://localhost:6333`)
- `QdrantKey` – Qdrant API Key

Example (Linux):
```
export AzureOpenAiEndpoint=https://<your-resource>.openai.azure.com/
export AzureOpenAiKey=<your-api-key>
export QdrantHost=http://localhost:6333
export QdrantKey=<your-qdrant-api-key>
```

---

## Build & Run

1. Restore dependencies:  
   ```
   dotnet restore
   ```
2. Build the solution:  
   ```
   dotnet build
   ```
3. Run the backend:  
   ```
   dotnet run --project Nsp.Doc.Ai.Api
   ```
4. Start the Angular UI as per its documentation.

---

## How the App Works

- **User Interaction:**  
  The Angular UI (`app.ts`) lets users chat or upload files. Messages and files are sent to the backend API.

- **Backend Processing:**  
  The .NET backend uses **Semantic Kernel** to:
  - Generate embeddings for documents/queries using Azure OpenAI (`text-embedding-3-large`).
  - Store/retrieve vectors in Qdrant for semantic search.
  - Use `gpt-4o` for chat completions and responses.

- **Semantic Kernel Flow:**  
  1. **Embedding:**  
     User input or document is embedded via Azure OpenAI.
  2. **Vector Search:**  
     Embedding is sent to Qdrant to find relevant documents.
  3. **Completion:**  
     Context and user query are sent to `gpt-4o` for a natural language response.

---

## Example Angular UI Code

```typescript
public addChat(inputElement: HTMLInputElement): void {
  const message = inputElement.value.trim();
  if (message) {
    this.chatHistory.push(new ChatHistory(message, false));
    this.askAssistant(message);
  }
  inputElement.value = '';
}

private askAssistant(message: string): void {
  this.http.get<{ response: any }>('https://<api-url>/ask?query=' + encodeURIComponent(message))
    .subscribe({
      next: (res: any) => {
        this.chatHistory.push(new ChatHistory(res.message, true));
      },
      error: (err) => {
        this.chatHistory.push(new ChatHistory('Sorry, something went wrong.', true));
      }
    });
}
```

---

## What is Semantic Kernel?

- **Semantic Kernel** is a Microsoft SDK for integrating LLMs (like Azure OpenAI) with traditional programming.
- It orchestrates:
  - Prompt engineering
  - Embedding generation
  - Memory (vector search)
  - Chaining LLM calls with code

**In this project:**  
Semantic Kernel connects user input, document embeddings, Qdrant vector search, and LLM completions into a seamless workflow.

---

## Resources

- [Azure OpenAI Documentation](https://learn.microsoft.com/azure/ai-services/openai/)
- [Qdrant Documentation](https://qdrant.tech/documentation/)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel)

---

## License

This project is licensed under the terms specified in the repository.
