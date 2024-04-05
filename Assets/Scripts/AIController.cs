using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OpenAI_API;
using OpenAI_API.Chat;
using System;
using OpenAI_API.Models;

public class AIController : MonoBehaviour
{
    [Header("UI Component")]
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
    [Header("Text File")]
    public TextAsset backgroundTextFile;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    private List<string>textList = new List<string>();
    public int index;

    // Start is called before the first frame update

    void Awake()
    {
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation(GetTextFromFile(backgroundTextFile));
        
        okButton.onClick.AddListener(() => GetResponse());
    }
    private void OnEnable()
    {
        textField.text = textList[index];
        index++;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.N) && index == textList.Count) 
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }
        if (Input.GetKeyUp(KeyCode.N)) 
        {
            textField.text = textList[index];
            index++;
        }
    }

    private void StartConversation(string str)
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, str)
        };

        inputField.text = "";
        string startString = "Hello!";
        textField.text = startString;
        Debug.Log(startString);

    }

    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable button
        okButton.enabled = false;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            userMessage.Content = userMessage.Content.Substring(0, 100);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        textField.text = string.Format("You: {0}", userMessage.Content);

        //Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 100,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}; {1}", responseMessage.rawRole, responseMessage.Content));

        //
        messages.Add(responseMessage);

        //
        textList.Clear();
        index = 0;

        var lineData = string.Format("You: {0}\nAlan: {1}", userMessage.Content, responseMessage.Content).Split('.');

        foreach ( var line in lineData )
        {
            textList.Add(line + '.');
        }
        
        //
        okButton.enabled = true;

    }

    //
    string GetTextFromFile(TextAsset file)
    {
        if (file != null)
        {
            string fileContent = file.text;
            return fileContent;
        }
        else
        {
            Debug.LogError("Please provide a valid TextAsset.");
            return null;
        }
    }
}
