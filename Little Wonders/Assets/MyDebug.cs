using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug : MonoBehaviour
{
    private GridBehavior grid;

    private bool reveal = true;

    private void Awake()
    {
        grid = FindObjectOfType<GridBehavior>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            RevealAllCells();
        if (Input.GetKeyDown(KeyCode.V))
            GiveRes();
    }

    private void GiveRes()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            ResourceManager.Instance.AddResource(type, 999);
    }

    private void RevealAllCells()
    {
        foreach(var cell in grid.cells)
        {
            if (reveal)
                cell.RevealCell();
            else
            {
                cell.HideCell();
                grid.cells[0, 0].RevealCell();
            }
        }
        reveal = !reveal;
    }
}
