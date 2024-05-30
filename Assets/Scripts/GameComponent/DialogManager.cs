using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogManager : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject inputPanel;
    public GameObject textPanel;
    public TMP_InputField inputField;
    public TMP_Text displayText;

    public float textSpeed;

    private List<string> textList = new List<string>();
    public int index;

    private bool isTextFinished;
    private bool canceledTyping;

    private NPC npc;

    async void Awake()
    {
        npc = NPCAttachment.selectedNPC;
        if (npc != null)
        {
            NPCAttachment nPCAttachment = FindObjectOfType<NPCAttachment>();
            if (nPCAttachment != null)
            {
                string quest = nPCAttachment.quest;
                await AIManager.SetupMessage(npc.Npc_id, quest);
            }
        }
        else
        {
            Debug.Log("NPC is null");
        }
    }

    private void Start()
    {
        textPanel.SetActive(false);
    }

    void Update()
    {
        if (textPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) && index == textList.Count)
            {
                textPanel.SetActive(false);
                index = 0;
                PlayerMovement.EnableMovement();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Return))
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
        else if (inputPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (inputField.text.Length > 0)
                {
                    OnSubmitAsync();
                }
            }
        }
    }

    async void GetText(string text)
    {
        textList.Clear();
        index = 0;

        string response = await AIManager.StartDialog(npc.Npc_id, text);

        string[] separatorsForCut = { "-----" };

        // ʹ�÷ָ����ָ��ı�
        var splitText = response.Split(separatorsForCut, System.StringSplitOptions.None);

        var result = splitText[0];
        result = result.Trim();

        var lineData = result.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }

        // �ڻ�ȡ�ı��󣬿�ʼ������ʾ�ı�
        await DisplayTextAsync();
    }

    async Task DisplayTextAsync()
    {
        while (index < textList.Count)
        {
            isTextFinished = false;
            displayText.text = "";

            int letter = 0;
            while (!canceledTyping && letter < textList[index].Length)
            {
                displayText.text += textList[index][letter];
                letter++;
                await Task.Delay(System.TimeSpan.FromSeconds(textSpeed));
            }

            // ��ʾ�������ı���ȴ�һ��ʱ��
            await Task.Delay(System.TimeSpan.FromSeconds(textSpeed));

            index++;
            isTextFinished = true;
        }

        // �ȴ��û����»س���
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            await Task.Delay(1);
        }

        // �л����״̬
        inputPanel.SetActive(true);
        textPanel.SetActive(false);
        displayText.text = "";
    }

    IEnumerator SetTextUI()
    {
        isTextFinished = false;
        displayText.text = "";

        int letter = 0;
        while (!canceledTyping && letter < textList[index].Length - 1)
        {
            displayText.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        displayText.text = textList[index];
        canceledTyping = false;
        isTextFinished = true;
        index++;
    }

    // ���û����»س���ʱ����
    public async void OnSubmitAsync()
    {
        // ��ȡ�û�������ı�
        string userInput = inputField.text;

        // ���� GetText() ��������ʹ�� await
        GetText(userInput);

        // �л����״̬
        inputPanel.SetActive(false);
        textPanel.SetActive(true);

        // ��������ı���
        inputField.text = "";
    }

    // ���û�����رհ�ťʱ����
    public void OnClose()
    {
        // �л����״̬
        inputPanel.SetActive(false);
        textPanel.SetActive(false);
    }
}
