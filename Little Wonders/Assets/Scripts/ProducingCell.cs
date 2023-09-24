using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProducingCell : Cell
{
    public Dictionary<ResourceType, int> producedResources;

    [SerializeField] float cooldown;

    private float startTime;

    protected override void OnMouseDown()
    {
        if (InteractionCanvas.Instance.CellIsSelected() || !GameManager.Instance.canControl) return;

        if (isExplored)
        {
            SFXManager.Instance.PlaySFXPitched(1);
            InteractionCanvas.Instance.ProcessDestroyPanel(this);
        }
        else if (IsNextToNeighbor())
        {
            if (ResourceManager.Instance.EnoughResources(Globals.Instance.exploreCost))
                RevealCell();
        }
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (startTime + cooldown <= Time.time) 
        {
            foreach(ResourceType type in producedResources.keys)
            {
                ResourceManager.Instance.AddResource(type, producedResources.GetValue(type));
            }
            startTime = Time.time;
        }
    }
}
