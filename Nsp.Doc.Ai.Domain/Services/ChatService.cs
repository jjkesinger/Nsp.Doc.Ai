using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class ChatService(Kernel kernel)
    {
        public async Task<string> Test(string collectionName, string query, CancellationToken cancellationToken)
        {
            var conversationSummary = kernel.Plugins.First();
            FunctionResult summary = await kernel.InvokeAsync(conversationSummary["SearchTheDocuments"],
                new() { ["collectionName"] = collectionName, ["query"] = query }, cancellationToken);

            var results = summary.GetValue<string[]>();

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(@"
            You are a helpful assistant that only uses the chat history to answer the question. 
            If it is not in the chat history, answer 'No results found.'
            ");

            foreach (var result in results!)
            {
                chatHistory.AddAssistantMessage(result);
            }

            chatHistory.AddUserMessage(query);

            var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

            var chatMessage = await chatCompletion.GetChatMessageContentAsync(chatHistory, cancellationToken: cancellationToken);

            return chatMessage.Content!;
        }
    }
}