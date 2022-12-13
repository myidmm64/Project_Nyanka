using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogOptions
{
    public string characterName;
    public int imageIndex;
    public string[] contexts;
    public DialogEventType eventType;
}

[System.Serializable]
public enum DialogEventType
{
    NONE,
    SHAKE,
}