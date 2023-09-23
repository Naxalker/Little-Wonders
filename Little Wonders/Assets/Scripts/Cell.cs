using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Cost
{
    public ResourceType resourceType;
    public int value;
}

public class Cell : MonoBehaviour
{
    public List<Cost> costList;
    public string descriptionText;

    #region Properties
    public List<Cell> availableCells;
    public bool isExplored { get; private set; }
    public Vector2Int coordinates { get; private set; }
    public bool isOffset { get; private set; }
    #endregion

    #region Visuals
    [Header("Visuals")]
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color offsetColor = new Color(1f, 1f, 1f, .8f);
    [SerializeField] GameObject highlight;
    [SerializeField] private string animationName;
    #endregion

    #region Components
    [Header("Components")]
    [SerializeField] private GridProperties gridProperties;
    private GridBehavior grid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = GetComponentInParent<GridBehavior>();
        anim = GetComponent<Animator>();
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {

    }

    public void Init(bool _isOffset, bool _isExplored, Vector2Int _coords)
    {
        isOffset = _isOffset;
        coordinates = _coords;
        isExplored = _isExplored;

        if (!isExplored)
        {
            spriteRenderer.color = Color.black;
        }
        else
        {
            spriteRenderer.color = isOffset ? offsetColor : baseColor;
        }

        if (animationName != "") {
            anim.SetBool(animationName, true);
            anim.SetFloat("Offset", UnityEngine.Random.Range(0f, 1f));
        }
    }

    private void OnMouseDown()
    {
        if (BuildCanvas.Instance.CellIsSelected()) return;

        if (isExplored)
        {
            BuildCanvas.Instance.ProcessUpgradePanel(this);
        } else if (IsNextToNeighbor())
        {
            isExplored = true;
            spriteRenderer.color = isOffset ? offsetColor : baseColor;
        }
    }

    private void OnMouseEnter()
    {
        if (!BuildCanvas.Instance.CellIsSelected())
            highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (!BuildCanvas.Instance.CellIsSelected())
            highlight.SetActive(false);
    }

    private bool IsNextToNeighbor()
    {
        if (grid.cells[(int)Mathf.Clamp(coordinates.x - 1, 0, gridProperties.size.x), coordinates.y].isExplored) return true;
        if (grid.cells[(int)Mathf.Clamp(coordinates.x + 1, 0, gridProperties.size.x), coordinates.y].isExplored) return true;
        if (grid.cells[coordinates.x, (int)Mathf.Clamp(coordinates.y - 1, 0, gridProperties.size.y)].isExplored) return true;
        if (grid.cells[coordinates.x, (int)Mathf.Clamp(coordinates.y + 1, 0, gridProperties.size.y)].isExplored) return true;

        return false;
    }
}
