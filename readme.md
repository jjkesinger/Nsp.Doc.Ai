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

---

## Configuration

1. **Set Environment Variables**

   Configure the following environment variables with your Azure OpenAI details:

   - `AZURE_OPENAI_ENDPOINT` – Your Azure OpenAI endpoint URL.
   - `AZURE_OPENAI_KEY` – Your Azure OpenAI API key.
   - `AZURE_OPENAI_EMBEDDING_DEPLOYMENT` – The deployment name for `text-embedding-3-large`.
   - `AZURE_OPENAI_CHAT_DEPLOYMENT` – The deployment name for `gpt-4o`.

   Example (Windows Command Prompt):

    set AZURE_OPENAI_ENDPOINT=https://<your-resource-name>.openai.azure.com/ 
    set AZURE_OPENAI_KEY=<your-api-key> 
    set AZURE_OPENAI_EMBEDDING_DEPLOYMENT=text-embedding-3-large 
    set AZURE_OPENAI_CHAT_DEPLOYMENT=gpt-4o

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
- If you encounter authentication or deployment errors, double-check your environment variables and Azure resource configuration.
- For more information on Azure OpenAI, see the [official documentation](https://learn.microsoft.com/azure/ai-services/openai/).

---

## Troubleshooting

- **Model Not Found:** Verify the deployment names match those configured in Azure.
- **Authentication Errors:** Ensure your API key and endpoint are correct and not expired.
- **.NET Version Issues:** Confirm you are using .NET 9 SDK.

---

## License

This project is licensed under the terms specified in the repository.
