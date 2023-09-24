using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCell : Cell
{
    protected override void OnMouseDown()
    {
        if (InteractionCanvas.Instance.CellIsSelected() || !GameManager.Instance.canControl) return;

        if (isExplored)
        {
            InteractionCanvas.Instance.ProcessBuildPanel(this);
            SFXManager.Instance.PlaySFXPitched(1);
        }
        else if (IsNextToNeighbor())
        {
            if (ResourceManager.Instance.EnoughResources(Globals.Instance.exploreCost))
                RevealCell();
        }
    }
}
