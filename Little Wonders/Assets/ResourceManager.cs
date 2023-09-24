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
    [SerializeField] GameObject number;

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
        int targetIndex = resources.FindIndex(resource => resource.resourceType == _resourceType);
        Resource res = resources[targetIndex];
        res.resourceValue += _resourceValue;
        res.resourceText.text = res.resourceValue.ToString();
        resources[targetIndex] = res;

        string numberSign = _resourceValue < 0 ? "" : "+";
        number.GetComponent<TextMeshProUGUI>().text = numberSign + _resourceValue.ToString();
        Instantiate(number, 
                    new Vector3(UnityEngine.Random.Range(res.resourceText.transform.position.x - 75f * GetComponentInParent<Canvas>().scaleFactor, res.resourceText.transform.position.x + 25f * GetComponentInParent<Canvas>().scaleFactor),
                                UnityEngine.Random.Range(res.resourceText.transform.position.y - 85f * GetComponentInParent<Canvas>().scaleFactor, res.resourceText.transform.position.y - 60f * GetComponentInParent<Canvas>().scaleFactor), .5f) ,
                    Quaternion.identity, GetComponentInParent<Canvas>().transform);
    }

    public bool EnoughResources(Dictionary<ResourceType, int> costs)
    {
        for (int i = 0; i < costs.values.Count; i++)
        {
            if (costs.values[i] > resources.FirstOrDefault(x => x.resourceType == costs.keys[i]).resourceValue) 
                return false;
        }

        return true;
    }
}
