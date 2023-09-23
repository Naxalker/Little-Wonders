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
                GameObject spawnedCell = GetTerrain(x, y);

                bool isExploredCell = IsExplored(x, y); 
                
                spawnedCell.GetComponent<Cell>().Init(Math.Abs(x + y) % 2 == 1, isExploredCell, new Vector2Int(x, y), cellType);
                cells[x, y] = spawnedCell.GetComponent<Cell>();
            }
        }
    }

    private GameObject GetRandomCell()
    {
        cellType = UnityEngine.Random.Range(0, initGeneratingCells + 1);
        return cellPrefabs[cellType];
    }
    private GameObject GetRandomCell(float _PerlinNoise)
    {   
        float resultNoise = _PerlinNoise * initGeneratingCells;
        cellType = Mathf.RoundToInt(resultNoise);
        cellType = UnityEngine.Random.Range(0f,1f) > 0.5f ? 0 : cellType;
        return cellPrefabs[cellType];
    } 
    private GameObject GetTerrain(int x, int y)
    {   
        float sid = UnityEngine.Random.Range(0f, 9999999f);
        float zoom = 70f;
        float perlinNoise = Mathf.PerlinNoise((x + sid) / zoom, (y + sid) / zoom);
        Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
        GameObject spawnedCell = Instantiate(GetRandomCell(perlinNoise), position, Quaternion.identity, transform);
        spawnedCell.name = "Tile" + x + y;
        return spawnedCell;
    }
    private bool IsExplored(int x, int y)
    {
        if (x >= Mathf.FloorToInt((float)size.x / 2) - 1 && x <= Mathf.FloorToInt((float)size.x / 2) + 1 && 
            y >= Mathf.FloorToInt((float)size.y / 2) - 1 && y <= Mathf.FloorToInt((float)size.y / 2) + 1) 
            return true;

        return false;
    }
}

[CreateAssetMenu(fileName = "GridProperties", menuName = "ScriptableObjects/GridProperties")]
public class GridProperties : ScriptableObject
{
    public Vector2Int size;
}
