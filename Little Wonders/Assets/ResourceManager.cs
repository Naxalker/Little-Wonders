using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum ResourceType
{
    Oil,
    Money,
    Food,
    Rock,
    Tools,
    Mood
}

[Serializable]
public struct Resource
{
    public ResourceType resourceType;
    public int resourceValue;
    public TextMeshProUGUI resourceText;
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    [SerializeField] private List<Resource> resources;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Resource res in resources)
        {
            res.resourceText.text = res.resourceValue.ToString();
        }
    }

    public void AddResource(ResourceType _resourceType, int _resourceValue)
    {
        Resource res = resources.FirstOrDefault(x => x.resourceType == _resourceType);
        res.resourceValue += _resourceValue;
        res.resourceText.text = res.resourceValue.ToString();
    }
}
