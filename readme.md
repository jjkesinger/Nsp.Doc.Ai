# Summary
<iframe width="560" height="315" src="https://www.youtube.com/embed/cRMBgOTsgN0?si=nDKBxN8eTH2KpGcd" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>



# Solution Setup Guide

This solution targets **.NET 9** and integrates with Azure OpenAI for advanced language and embedding capabilities. Follow the steps below to set up and run the solution.

---

## Prerequisites

1. **.NET 9 SDK**
   - Download and install from: [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **Azure Subscription**
   - Sign up or log in at: [https://portal.azure.com](https://portal.azure.com)

3. **Azure OpenAI Resource**
   - Create an Azure OpenAI resource in the Azure Portal.

4. **Deploy Models**
   - In your Azure OpenAI resource, deploy the following models:
     - `text-embedding-3-large`
     - `gpt-4o`
   - Note the deployment names you assign (you will need them for configuration).

5. **API Keys and Endpoints**
   - Obtain your Azure OpenAI API key and endpoint from the Azure Portal.

6. **Qdrant Vector Storage**
   - Set up a Qdrant instance for vector storage. You can run Qdrant locally using Docker or use a managed Qdrant Cloud service.
   - **To run Qdrant locally with Docker:**

   - docker run -p 6333:6333 -p 6334:6334 qdrant/qdrant
  - For more information or advanced configuration, see the [Qdrant documentation](https://qdrant.tech/documentation/).
   - Note the Qdrant endpoint (e.g., `http://localhost:6333`) for configuration.

---

## Configuration

1. **Set Environment Variables**

   Configure the following environment variables with your Azure OpenAI and Qdrant details:

   - `AzureOpenAiEndpoint` – Your Azure OpenAI endpoint URL.
   - `AzureOpenAiKey` – Your Azure OpenAI API key.
   - `QdrantHost` – The URL of your Qdrant instance (e.g., `http://localhost:6333`).
   - `QdrantKey` - Your Qdrant API Key 

   Example (Windows Command Prompt):

    set AzureOpenAiEndpoint=https://<your-resource-name>.openai.azure.com/ 
    set AzureOpenAiKey=<your-api-key> 
    set QdrantHost=http://localhost:6333
    set QdrantKey=<your-qdrant-api-key>
1. 
    Or add these to your `appsettings.json` or user secrets as appropriate for your project.

---

## Build and Run

1. **Restore Dependencies**
    dotnet restore

2. **Build the Solution**
    dotnet build

3. **Run the Application**
    dotnet run --project <YourProjectName>

    Replace `<YourProjectName>` with the actual project folder or `.csproj` file name.

---

## Notes

- Ensure your Azure OpenAI deployments are active and you have sufficient quota.
- Ensure your Qdrant instance is running and accessible at the configured endpoint.
- If you encounter authentication or deployment errors, double-check your environment variables and Azure resource configuration.
- For more information on Azure OpenAI, see the [official documentation](https://learn.microsoft.com/azure/ai-services/openai/).
- For more information on Qdrant, see the [Qdrant documentation](https://qdrant.tech/documentation/).

---

## Troubleshooting

- **Model Not Found:** Verify the deployment names match those configured in Azure.
- **Authentication Errors:** Ensure your API key and endpoint are correct and not expired.
- **Qdrant Connection Issues:** Ensure Qdrant is running and the endpoint is correct.
- **.NET Version Issues:** Confirm you are using .NET 9 SDK.

---

## License

This project is licensed under the terms specified in the repository.
