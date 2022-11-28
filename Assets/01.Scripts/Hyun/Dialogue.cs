using UnityEngine;

[System.Serializable]
public struct TalkData
{
    public string name;
    public string[] contexts;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private string eventName;
    [SerializeField]
    private TalkData[] talkDatas;

    public TalkData[] GetObjectDialogue()
    {
        return DialogueParse.GetDialogue(eventName);
    }
}