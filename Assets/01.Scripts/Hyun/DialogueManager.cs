using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Dialogue[] dialogues;
    [SerializeField]
    private int cnt = 0;
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text context;
    [SerializeField]
    private float textDelay;
    private int lineCnt = 0;
    private int contextCnt = 0;
    private TalkData[] talkDatas;

    bool isDialogue = false;
    bool isType = false;

    private IEnumerator typeText;

    private void Awake()
    {
        dialogues = GameObject.Find("Dialogue").GetComponentsInChildren<Dialogue>();
        typeText = TypeWriter();
    }

    public void StartDialogue()
    {
        SettingUI();
        ShowDialogue(dialogues[cnt].GetObjectDialogue());
    }

    void Update()
    {
        if(isDialogue)
        {
            if(!isType)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    context.text = "";

                    if (++contextCnt < talkDatas[lineCnt].contexts.Length)
                    {
                        typeText = TypeWriter();
                        StartCoroutine(typeText);
                    }
                    else
                    {
                        contextCnt = 0;
                        if (++lineCnt < talkDatas.Length)
                        {
                            typeText = TypeWriter();
                            StartCoroutine(typeText);
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StopCoroutine(typeText);
                    context.text = talkDatas[lineCnt].contexts[contextCnt];
                    isType = false;
                }
            }
          
        }
    }


    private void SettingUI()
    {
        name.gameObject.SetActive(true);
        context.gameObject.SetActive(true);
    }

    public void EndDialogue()
    {
        StopCoroutine(typeText);
        isDialogue = false;
        contextCnt = 0;
        lineCnt = 0;
        talkDatas = null;
        name.gameObject.SetActive(false);
        context.gameObject.SetActive(false);
        cnt++;
    }

    public void ShowDialogue(TalkData[] talkData)
    {
        isDialogue = true;
        name.text = "";
        context.text = "";
        talkDatas = talkData;
        typeText = TypeWriter();
        StartCoroutine(typeText);
    }

    IEnumerator TypeWriter()
    {
        isType = true;
        name.text = talkDatas[lineCnt].name;
        for (int i=0;i< talkDatas[lineCnt].contexts[contextCnt].Length; i++)
        {
            context.text += talkDatas[lineCnt].contexts[contextCnt][i];
            yield return new WaitForSeconds(textDelay);
        }
        isType = false;
    }
}
