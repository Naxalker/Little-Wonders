using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dictionary<TKey, TValue>
{
    public List<TKey> keys = new List<TKey>();
    public List<TValue> values = new List<TValue>();

    public void SetValue(TKey key, TValue value)
    {
        values[keys.IndexOf(key)] = value;
    }

    public TValue GetValue(TKey key)
    {
        return values[keys.IndexOf(key)];
    }
}

public class Globals : MonoBehaviour
{
    public static Globals Instance;
    public Dictionary<ResourceType, int> resourceIndex;
    public Dictionary<ResourceType, int> exploreCost;

    private void Awake()
    {
        Instance = this;
    }

    public string IconDescription(int spriteIndex) => $"<voffset=.4em><size=.25><sprite={spriteIndex}></size></voffset>";
}
