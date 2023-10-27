
using Catalyst;
using Customers_support_chat_bot;
using System.Reflection.Metadata;

class Program
{
    static async Task Main(string[] args)
    {
        NLPModel model = new NLPModel();
        Console.WriteLine("Type something:\n");
        var userInput = Console.ReadLine();
        IDocument processedInput = await model.Process(userInput is null ? "" : userInput);
        Console.Write(processedInput.ToJson());
    }
}