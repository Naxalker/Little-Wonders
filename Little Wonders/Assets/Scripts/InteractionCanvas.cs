using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InteractionCanvas : MonoBehaviour
{
    public static InteractionCanvas Instance;

    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject destroyPanel;
    [SerializeField] private List<Button> buildButtons;
    [SerializeField] private GridBehavior grid;
    [SerializeField] private Button nextButton;

    private Cell selectedCell;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HidePanels();
    }

    private void Update()
    {
        if (CellIsSelected() && Input.GetMouseButtonDown(1)) 
        {
            HidePanels();
        }
    }

    public void ProcessDestroyPanel(Cell _cell)
    {
        selectedCell = _cell;
        transform.position = selectedCell.transform.position;
        destroyPanel.SetActive(true);
    }

    public void ProcessBuildPanel(Cell _cell)
    {
        selectedCell = _cell;
        transform.position = selectedCell.transform.position;
        buildPanel.SetActive(true);
        nextButton.transform.localScale = Vector3.one;

        List<Cell> buildOptions = selectedCell.availableCells;
        for (int i = 0; i < buildOptions.Count; i++)
        {
            string descriptionText = "";
            if (buildOptions[i].GetComponent<ProducingCell>())
            {
                if (buildOptions[i].GetComponent<ProducingCell>().producedResources.keys.Count > 0)
                    descriptionText = "Производит    ";
                foreach (var key in buildOptions[i].GetComponent<ProducingCell>().producedResources.keys)
                {
                    descriptionText += Globals.Instance.IconDescription(Globals.Instance.resourceIndex.GetValue(key)) + " ";
                }
            }
            descriptionText += "Стоимость<br>  ";
            foreach (var key in buildOptions[i].GetComponent<Cell>().cellCost.keys)
            {
                descriptionText += buildOptions[i].GetComponent<Cell>().cellCost.GetValue(key) + "   " + 
                                   Globals.Instance.IconDescription(Globals.Instance.resourceIndex.GetValue(key)) + " ";
            }
            buildButtons[i].GetComponent<ButtonBehavior>().SetButton(buildOptions[i].GetComponent<SpriteRenderer>().sprite, 
                                                                     i < 3,
                                                                     descriptionText);
        }

        nextButton.interactable = buildOptions.Count > 3;
    }

    private void HidePanels()
    {
        // здесь изменил
        if (selectedCell)
        {
            selectedCell.GetHighlighter().SetActive(false);
            selectedCell = null;
        }
        
        buildPanel.SetActive(false);
        destroyPanel.SetActive(false);
        foreach (Button button in buildButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ChangeVariants()
    {
        List<Cell> buildOptions = selectedCell.availableCells;
        for (int i = 0; i < buildOptions.Count; i++)
        {
            buildButtons[i].gameObject.SetActive(!buildButtons[i].gameObject.activeSelf);
        }
        Vector3 flip = nextButton.transform.localScale;
        flip.x *= -1;
        nextButton.transform.localScale = flip;
    }

    public void Build(int index)
    {
        if (selectedCell == null) return;

        Cell newCell = selectedCell.availableCells[index];

        if (ResourceManager.Instance.EnoughResources(newCell.cellCost))
        {
            grid.ReplaceCell(selectedCell, newCell);
            if (selectedCell.gameObject.name.Contains("castle"))
                GameManager.Instance.AddCastleCellCount();
            HidePanels();
            DescriptionCanvas.Instance.HideCanvas();
            foreach(var key in newCell.cellCost.keys)
            {
                ResourceManager.Instance.AddResource(key, -newCell.cellCost.GetValue(key));
            }
            SFXManager.Instance.PlaySFXPitched(0);
        }
    }

    public void DestroyCell()
    {
        if (selectedCell == null) return;

        Cell newCell = selectedCell.originCell;

        if (newCell != null)
        {
            newCell.gameObject.SetActive(true);
            grid.ReplaceCell(selectedCell, newCell);
            HidePanels();
        }
    }

    public bool CellIsSelected() => selectedCell != null;
}
