
using Catalyst;
using Customers_support_chat_bot;
using System.Reflection.Metadata;

class Program
{
    static void Main(string[] args)
    {
        string apiKey = "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";
        var chatGPTClient = new ChatGPTClient(apiKey);

        Console.WriteLine("Welcome to the customers support! Type 'exit' to quit.");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Set text color to green
            Console.Write("You: ");
            Console.ResetColor();
            string input = Console.ReadLine() ?? string.Empty;

            if (input.ToLower() == "exit")
                break;

            string response = chatGPTClient.SendMessage(input);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Chatbot: ");
            Console.ResetColor();
            Console.WriteLine(response);
        }
    }
}