using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildCanvas : MonoBehaviour
{
    public static BuildCanvas Instance;
    private Cell selectedCell;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private List<Button> buildButtons;
    [SerializeField] private GridBehavior grid;
    [SerializeField] private Button nextButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (CellIsSelected() && Input.GetMouseButtonDown(1)) 
        {
            HideUpgradePanel();
        }
    }

    public void ProcessUpgradePanel(Cell _cell)
    {
        selectedCell = _cell;
        transform.position = selectedCell.transform.position;
        buildPanel.SetActive(true);
        nextButton.transform.localScale = Vector3.one;

        List<Cell> buildOptions = selectedCell.availableCells;
        for (int i = 0; i < buildOptions.Count; i++)
        {
            buildButtons[i].image.sprite = buildOptions[i].GetComponent<SpriteRenderer>().sprite;
            buildButtons[i].gameObject.SetActive(i < 3);
        }

        nextButton.interactable = buildOptions.Count > 3;
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
        grid.ReplaceCell(selectedCell, newCell);
        selectedCell = null;
        HideUpgradePanel();
    }

    private void HideUpgradePanel()
    {
        selectedCell = null;
        buildPanel.SetActive(false);
        foreach(Button button in buildButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public bool CellIsSelected() => selectedCell != null;
}
