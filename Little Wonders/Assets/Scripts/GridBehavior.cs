using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    public List<GameObject> cellPrefabs;

    private Grid grid;
    private int cellType;
    public Vector2Int size;
    public Cell[,] cells;
    public int initGeneratingCells;
    
    
    private void Start()
    {
        grid = GetComponent<Grid>();

        size = gridProperties.size;

        cells = new Cell[size.x, size.y];

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                GameObject spawnedTile = Instantiate(GetRandomCell(), position, Quaternion.identity, transform);
                spawnedTile.name = "Tile" + x + y;
                // bool isExploredCell = false; 
                bool isExploredCell = true; // ненадолго добавил
                if (x >= Mathf.FloorToInt((float)size.x / 2) - 1 && x <= Mathf.FloorToInt((float)size.x / 2) + 1 && 
                    y >= Mathf.FloorToInt((float)size.y / 2) - 1 && y <= Mathf.FloorToInt((float)size.y / 2) + 1) 
                    isExploredCell = true;
                spawnedTile.GetComponent<Cell>().Init(Math.Abs(x + y) % 2 == 1, isExploredCell, new Vector2Int(x, y), cellType);
                cells[x, y] = spawnedTile.GetComponent<Cell>();
            }
        }
    }

    private GameObject GetRandomCell()
    {
        cellType = UnityEngine.Random.Range(0, initGeneratingCells + 1);
        return cellPrefabs[cellType];
    }
}

[CreateAssetMenu(fileName = "GridProperties", menuName = "ScriptableObjects/GridProperties")]
public class GridProperties : ScriptableObject
{
    public Vector2Int size;
}
