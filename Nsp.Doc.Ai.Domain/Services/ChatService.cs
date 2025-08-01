using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class ChatService(Kernel kernel)
    {
        public async Task<string> Ask(string query, CancellationToken cancellationToken)
        {
            var chatHistory = new ChatHistory($"Given this query: {query}, answer the question based on the documents in the library. List the title of the document.");

            var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();
            return (await chatCompletion.GetChatMessageContentAsync(chatHistory,
                new PromptExecutionSettings() { FunctionChoiceBehavior = FunctionChoiceBehavior.Required() },
                kernel, cancellationToken)).Content!;
        }
    }
}