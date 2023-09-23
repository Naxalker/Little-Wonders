using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] GameObject highlight;
    [SerializeField] GridBehavior grid;

    private SpriteRenderer spriteRenderer;
    private Vector2Int coordinates;
    private bool isOffset;

    [HideInInspector] public bool isExplored = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = GetComponentInParent<GridBehavior>();
    }

    public void Init(bool _isOffset, bool _isExplored, Vector2Int _coords)
    {
        isOffset = _isOffset;
        coordinates = _coords;
        isExplored = _isExplored;

        if (!isExplored)
        {
            spriteRenderer.color = new Color(0f,0f,0f);
        }
        else
        {
            spriteRenderer.color = isOffset ? offsetColor : baseColor;
        }
    }

    private void OnMouseDown()
    {
        if (IsNextToNeighbour())
        {
            isExplored = true;
            spriteRenderer.color = isOffset ? offsetColor : baseColor;
        }
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    private bool IsNextToNeighbour()
    {
        if (grid.cells[(int)Mathf.Clamp(coordinates.x - 1, 0, grid.size.x), coordinates.y].isExplored) return true;
        if (grid.cells[(int)Mathf.Clamp(coordinates.x + 1, 0, grid.size.x), coordinates.y].isExplored) return true;
        if (grid.cells[coordinates.x, (int)Mathf.Clamp(coordinates.y - 1, 0, grid.size.y)].isExplored) return true;
        if (grid.cells[coordinates.x, (int)Mathf.Clamp(coordinates.y + 1, 0, grid.size.y)].isExplored) return true;

        return false;
    }
}
