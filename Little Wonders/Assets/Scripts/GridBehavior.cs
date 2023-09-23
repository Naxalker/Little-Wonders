using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UIElements;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    public List<GameObject> cellPrefabs;
    public List<GameObject> castlePrefabs;

    private Grid grid;
    private int cellType;
    public Vector2Int size;
    public Cell[,] cells;

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

                spawnedCell.GetComponent<Cell>().Init(Math.Abs(x + y) % 2 == 1, isExploredCell, new Vector2Int(x, y));
                cells[x, y] = spawnedCell.GetComponent<Cell>();
            }
        }

        SetPlaceForCastle();
    }

    private void SetPlaceForCastle()
    {
        int counter = 0;
        int randX = UnityEngine.Random.Range(0, size.x - 2);
        int randY = UnityEngine.Random.Range(0, size.y - 2);
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ReplaceCell(cells[randX + j, randY + i], castlePrefabs[counter++].GetComponent<Cell>());
            }
        }
    }

    public void ReplaceCell(Cell _originCell, Cell _newCell)
    {
        GameObject spawnedCell = Instantiate(_newCell.gameObject, _originCell.transform.position, Quaternion.identity, transform);
        spawnedCell.GetComponent<Cell>().Init(_originCell.isOffset, _originCell.isExplored, _originCell.coordinates);
        spawnedCell.name = _originCell.gameObject.name;
        Destroy(_originCell.gameObject);
    }

    private GameObject GetRandomCell(float _PerlinNoise)
    {
        float resultNoise = _PerlinNoise * (cellPrefabs.Count - 1);
        cellType = Mathf.RoundToInt(resultNoise);
        cellType = UnityEngine.Random.Range(0f,1f) > 0.2f ? 0 : cellType;
        return cellPrefabs[cellType];
    }

    private GameObject GetTerrain(int x, int y)
    {
        float sid = UnityEngine.Random.Range(0f, 9999999f);
        float zoom = 70f;
        float perlinNoise = Mathf.PerlinNoise((x + sid) / zoom, (y + sid) / zoom);
        Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
        for (int i = 0; i < size.x - 2; i++)
        {
            for(int j = 0; j < size.y - 2; j++)
            {
                
            }
        }
        GameObject spawnedCell = Instantiate(GetRandomCell(perlinNoise), position, Quaternion.identity, transform);
        spawnedCell.name = "Tile" + x + y;
        return spawnedCell;
    }

    private bool IsExplored(int x, int y)
    {
        if (x >= Mathf.FloorToInt((float)size.x / 2) - 1 && x <= Mathf.FloorToInt((float)size.x / 2) + 1 &&
            y >= Mathf.FloorToInt((float)size.y / 2) - 1 && y <= Mathf.FloorToInt((float)size.y / 2) + 1)
            return true;

        return true;
    }
}

[CreateAssetMenu(fileName = "GridProperties", menuName = "ScriptableObjects/GridProperties")]
public class GridProperties : ScriptableObject
{
    public Vector2Int size;
}
