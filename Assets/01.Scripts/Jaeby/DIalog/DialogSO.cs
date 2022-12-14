using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialogEvent
{
    public string eventName;
    public DialogOptions[] dialogs;
}

[System.Serializable]
public struct DialogOptions
{
    public Vector2 position;
    public string characterName;
    public int imageIndex;
    public string[] contexts;
    public DialogEventType eventType;
    public UnityEvent Callback;
}

[System.Serializable]
public enum DialogEventType
{
    NONE,
    SHAKE,
}