using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Production
{
    public ResourceType resourceType;
    public int value;
}

public class ProducingCell : Cell
{
    [SerializeField] List<Production> resources;
    [SerializeField] float cooldown;

    private float startTime;

    public override void Start()
    {
        base.Start();

        startTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (startTime + cooldown <= Time.time) 
        {
            foreach (Production res in resources)
            {
                ResourceManager.Instance.AddResource(res.resourceType, res.value);
            }
            startTime = Time.time;
        }
    }
}
