using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Component")]
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
    [Header("Text File")]
    public TextAsset textFile;
    public float textSpeed;
    public string str;
    [Header("Avatar")]
    public Sprite playerAvatar, targetAvatar;


    private List<string> textList = new List<string>();
    public int index;

    private bool isTextFinished;
    private bool canceledTyping;

    // Start is called before the first frame update

    void Awake()
    {
        GetTextFromFile(textFile);
    }
    private void OnEnable()
    {
        isTextFinished = true;
        StartCoroutine(SetTextUI());
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            PlayerMovement.EnableMovement();
            return;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (isTextFinished && !canceledTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!isTextFinished)
            {
                canceledTyping = !canceledTyping;
            }

        }
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
        while (!canceledTyping && letter < textList[index].Length - 1)
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
