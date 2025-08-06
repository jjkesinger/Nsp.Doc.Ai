using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Nsp.Doc.Ai.Domain.Services
{
    public class ChatService(Kernel kernel)
    {
        public async Task<string> Ask(string query, CancellationToken cancellationToken)
        {
            return (await kernel.GetRequiredService<IChatCompletionService>().GetChatMessageContentAsync(
                @$"Given this query: {query}, answer the question based on the documents in the library to the best of your ability. 
                If the answer is in the documents, reference the document title. Otherwise, say the answer was not found in the documents.",
                new PromptExecutionSettings() { FunctionChoiceBehavior = FunctionChoiceBehavior.Required() },
                kernel, cancellationToken)).Content!;
        }
    }
}