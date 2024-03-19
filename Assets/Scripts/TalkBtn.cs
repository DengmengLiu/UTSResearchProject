using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBtn : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Button.SetActive(true);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            talkUI.SetActive(true);
        }
    }
}
