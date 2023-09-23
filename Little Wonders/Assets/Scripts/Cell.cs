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
    private Animator anim;
    private bool isOffset;
    private int type;

    [HideInInspector] public bool isExplored = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = GetComponentInParent<GridBehavior>();
        anim = GetComponent<Animator>();
    }

    public void Init(bool _isOffset, bool _isExplored, Vector2Int _coords, int _type)
    {
        isOffset = _isOffset;
        coordinates = _coords;
        isExplored = _isExplored;
        type = _type;

        if (!isExplored)
        {
            spriteRenderer.color = new Color(0f,0f,0f);
        }
        else
        {
            spriteRenderer.color = isOffset ? offsetColor : baseColor;
        }

        List<int> animatedCells = new List<int>(){ 0, 1, 2, 3, 6, 8, 10, 11, 12, 13, 14, 15 };
        if (animatedCells.Contains(type)) {
            anim.SetBool(grid.cellPrefabs[type].name, true);
            anim.SetFloat("Offset", UnityEngine.Random.Range(0f, 1f));
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
