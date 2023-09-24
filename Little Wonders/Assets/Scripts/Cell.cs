using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Dictionary<ResourceType, int> cellCost;
    [HideInInspector] public Cell originCell;

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
    [SerializeField] private GameObject highlight;
    [SerializeField] private string animationName;
    #endregion

    #region Components
    [Header("Components")]
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

    public void Init(bool _isOffset, bool _isExplored, Vector2Int _coords, Cell _originCell)
    {
        isOffset = _isOffset;
        coordinates = _coords;
        isExplored = _isExplored;
        originCell = _originCell;

        if (!isExplored)
        {
            // здесь изменил
            spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
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

    protected virtual void OnMouseDown()
    {
        if (InteractionCanvas.Instance.CellIsSelected() || !GameManager.Instance.canControl) return;

        if (!isExplored && IsNextToNeighbor())
        {
            if (ResourceManager.Instance.EnoughResources(Globals.Instance.exploreCost))
                RevealCell();
        } else
        {
            SFXManager.Instance.PlaySFXPitched(1);
        }
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.canControl) return;

        if (!InteractionCanvas.Instance.CellIsSelected())
        {
            highlight.SetActive(true);
            if (!isExplored)
            {
                string text = "Исследовать <br> ";
                foreach (var key in Globals.Instance.exploreCost.keys)
                {
                    text += Globals.Instance.exploreCost.GetValue(key) + "   " +
                            Globals.Instance.IconDescription(Globals.Instance.resourceIndex.GetValue(key)) + " ";
                }
                DescriptionCanvas.Instance.ShowCanvas(text);
            }
            else 
                DescriptionCanvas.Instance.HideCanvas();

        }
    }

    private void OnMouseExit()
    {
        if (!GameManager.Instance.canControl) return;

        if (!InteractionCanvas.Instance.CellIsSelected())
        {
            highlight.SetActive(false);
        }
    }

    protected bool IsNextToNeighbor()
    {
        if (grid.cells[(int)Mathf.Clamp(coordinates.x - 1, 0, grid.size.x - 1), coordinates.y].isExplored) return true;
        if (grid.cells[(int)Mathf.Clamp(coordinates.x + 1, 0, grid.size.x - 1), coordinates.y].isExplored) return true;
        if (grid.cells[coordinates.x, (int)Mathf.Clamp(coordinates.y - 1, 0, grid.size.y - 1)].isExplored) return true;
        if (grid.cells[coordinates.x, (int)Mathf.Clamp(coordinates.y + 1, 0, grid.size.y - 1)].isExplored) return true;

        return false;
    }

    // здесь изменил
    public GameObject GetHighlighter()
    {
        return highlight;
    }

    public void RevealCell()
    {
        isExplored = true;
        spriteRenderer.color = isOffset ? offsetColor : baseColor;
        foreach(var key in Globals.Instance.exploreCost.keys)
        {
            ResourceManager.Instance.AddResource(key, -Globals.Instance.exploreCost.GetValue(key));
        }
        SFXManager.Instance.PlaySFXPitched(1);
    }

    public void HideCell()
    {
        isExplored = false;
        spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
    }
}
