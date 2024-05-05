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
    public List<TextAsset> backgroundTextFile;
    public float textSpeed;
    public string str;
    [Header("Avatar")]
    public Sprite playerAvatar, targetAvatar;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    private List<string>textList = new List<string>();
    public int index;

    private bool isTextFinished;
    private bool canceledTyping;

    // Start is called before the first frame update

    void Awake()
    {
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation(GetTextFromFile(backgroundTextFile[0]));
        
        okButton.onClick.AddListener(() => GetResponse());
    }
    private void OnEnable()
    {
        StartCoroutine(SetTextUI());
        isTextFinished = true;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return) && index == textList.Count) 
        {
            gameObject.SetActive(false);
            index = 0;
            PlayerMovement.EnableMovement();
            return;
        }
        if (Input.GetKeyUp(KeyCode.Return)) 
        {
            if(isTextFinished && !canceledTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!isTextFinished) 
            {
                canceledTyping = !canceledTyping;                
            } 
            
        }
    }

    private void StartConversation(string str)
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, str)
        };

        /*inputField.text = "";
        string startString = "Hello!";
        textField.text = startString;
        Debug.Log(startString);*/

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
        Debug.Log(string.Format("{0}: {1}", userMessage.Role, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        //textField.text = string.Format("You: {0}", userMessage.Content);

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
        Debug.Log(string.Format("{0}; {1}", responseMessage.Role, responseMessage.Content));

        //
        messages.Add(responseMessage);

        //
        textList.Clear();
        index = 0;

        var lineData = string.Format("You: {0}.Alan: {1}", userMessage.Content, responseMessage.Content).Split('.');

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

    IEnumerator SetTextUI()
    {
        isTextFinished = false;
        textField.text = "";

        int letter = 0;
        while(!canceledTyping && letter < textList[index].Length - 1) 
        {
            textField.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textField.text = textList[index];
        canceledTyping = false;
        isTextFinished = true;
        index++;
    }
}
