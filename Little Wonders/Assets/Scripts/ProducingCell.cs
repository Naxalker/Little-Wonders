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

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
