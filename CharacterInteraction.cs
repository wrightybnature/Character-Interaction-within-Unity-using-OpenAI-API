using UnityEngine;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using TMPro;
// THIS IS MY SERIALIZABLE OBJECT DEFINITION AND FUNCTIONS FOR USING THE DATA TO CONNECT TO OPENAI API AND SEND/RECEIVE MESSAGES

[CreateAssetMenu(fileName = "NewCharacterInteraction", menuName = "Character Interaction")]
public class CharacterInteraction : ScriptableObject
{
    public string characterName;
    public string description; // Add a field to hold the character's description
    public string initialSystemMessage = "Hello there."; // Example initial message, gets replayed depending on character
    
    private OpenAIAPI api;
    private List<ChatMessage> messages; // List to hold message history for this character

    // Method to start a conversation
    public async void Interact(TextTyper textTyper, TMP_InputField inputField, Button okButton)
    {
        // Setup API and messages on first interaction
        if (api == null)
        {
            api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
            messages = new List<ChatMessage>
            {
                new ChatMessage(ChatMessageRole.System, description)
            };
        }

        // If there's input from the user, send it to OpenAI and get a response
        if (!string.IsNullOrEmpty(inputField.text))
        {
            ChatMessage userMessage = new ChatMessage
            {
                Role = ChatMessageRole.User,
                Content = inputField.text
            };
            messages.Add(userMessage); // Add message to list

            var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Temperature = 0.9,
                MaxTokens = 50,
                Messages = messages
            });

            ChatMessage responseMessage = new ChatMessage
            {
                Role = chatResult.Choices[0].Message.Role,
                Content = chatResult.Choices[0].Message.Content
            };
            messages.Add(responseMessage); // Add response to messages history

            // Update UI with both messages
            
            string combinedText = $"\nYou: {userMessage.Content}\n{characterName}: {responseMessage.Content}";
            textTyper.DisplayText(combinedText);
            inputField.text = ""; // Clear the input field
        }
        else
        {
            // Optionally handle empty input case, such as prompting the user for input
            textTyper.DisplayText(initialSystemMessage);
        }

        // Setup the button listener for the next message
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() => Interact(textTyper, inputField, okButton));
    }

    // Method to end the conversation and clear the message history
    public void EndInteraction(TMP_Text textField)
    {
        messages.Clear();
        textField.text = "";
    }
}
