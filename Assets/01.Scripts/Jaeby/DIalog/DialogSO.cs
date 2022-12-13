using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogSO
{
    public string name;
    public int imageIndex;
    public DialogEvent[] dialogEvent;
}

[System.Serializable]
public struct DialogEvent
{
    public string[] contexts;
    public DialogEventType eventType;
}

[System.Serializable]
public enum DialogEventType
{
    NONE,
    SHAKE,
}