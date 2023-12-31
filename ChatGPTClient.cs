﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Recognizers.Text.DateTime.English;
using Newtonsoft.Json;
using OpenAI_API;
using RestSharp;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customers_support_chat_bot.Exceptions;

namespace Customers_support_chat_bot
{
    internal class ChatGPTClient
    {
        /*
        private readonly string _apiKey;
        private readonly RestClient _client;

        public ChatGPTClient(string apiKey)
        {
            _apiKey = apiKey;
            _client = new RestClient("https://api.openai.com/v1/chat/completions");
        }

        public string SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return "Sorry, I didn't receive any input. Please try again!";
            }

            try
            {
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {_apiKey}");

                var requestBody = new
                {
                    prompt = message,
                    // 1 token is approximately 4 characters 
                    max_tokens = 100,
                    n = 1,
                    stop = (string?)null,
                    temperature = 0.7,
                    model = "gpt-3.5-turbo",
                };

                request.AddJsonBody(JsonConvert.SerializeObject(requestBody));

                string jsonRequest = JsonConvert.SerializeObject(request, Formatting.Indented);
                //File.WriteAllText("C:\\Users\\jmiku\\OneDrive\\Desktop\\ASOS\\requests tmp\\request.json", jsonRequest);

                var response = _client.Execute<RestClient>(request);

                string jsonresponse = JsonConvert.SerializeObject(response, Formatting.Indented);
                //File.WriteAllText("C:\\Users\\jmiku\\OneDrive\\Desktop\\ASOS\\responses tmp\\response.json", jsonresponse);

                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content ?? string.Empty);


                return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "Sorry, there was an error processing your request.";
            }*/


        
        private OpenAIAPI _api = new OpenAIAPI(new APIAuthentication(ConfigurationManager.AppSettings["ChatGPTKey"]));
        private OpenAI_API.Chat.Conversation _conversation { get;  }
        private ConversationHistory _conversationHistory { get; }
        public ChatGPTClient()
        {
            try
            {
            this._conversation = this._api.Chat.CreateConversation();
            this._conversationHistory = new ConversationHistory();
                this._conversation.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
            this._conversation.AppendSystemMessage("You are customers support assistant.");
            }catch (Exception e) 
            {
                throw new ChatGPTClientException("Exception thrown during ChatGPT client instantiation.", e);
            }
        }

        public async Task<String> Ask(String question)
        {
            String response = "";
            try
            {
                this._conversation.AppendUserInput(question);
                this._conversationHistory.AddUserMessage(question);
                response = await this._conversation.GetResponseFromChatbotAsync();
                this._conversationHistory.AddBotMessage(response);
            }catch (Exception e)
            {
                throw new ChatGPTClientException(e.Message, e);
            }

            return response;
        }
    }
}
