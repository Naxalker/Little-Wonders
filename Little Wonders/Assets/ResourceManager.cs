using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Oil,
    Money,
    Food,
    Rock,
    Tools,
    Mood
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach(ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            resources.Add(type, 100);
        }
    }

    public void AddResource(ResourceType _resourceType, int _resourceValue)
    {
        resources[_resourceType] += _resourceValue;
    }
}
