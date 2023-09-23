using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    [SerializeField] private List<GameObject> cellPrefabs;

    private Grid grid;
    private Vector2Int size;
    public Cell[,] cells { get; private set; }

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
                GameObject spawnedCell = Instantiate(GetRandomCell(), position, Quaternion.identity, transform);
                spawnedCell.name = "Tile" + x + y;
                bool isExploredCell = false;
                if (x >= Mathf.FloorToInt((float)size.x / 2) - 1 && x <= Mathf.FloorToInt((float)size.x / 2) + 1 && 
                    y >= Mathf.FloorToInt((float)size.y / 2) - 1 && y <= Mathf.FloorToInt((float)size.y / 2) + 1) 
                    isExploredCell = true;
                spawnedCell.GetComponent<Cell>().Init(Math.Abs(x + y) % 2 == 1, isExploredCell, new Vector2Int(x, y));
                cells[x, y] = spawnedCell.GetComponent<Cell>();
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

    private GameObject GetRandomCell()
    {
        return cellPrefabs[UnityEngine.Random.Range(0, cellPrefabs.Count)];
    }
}

[CreateAssetMenu(fileName = "GridProperties", menuName = "ScriptableObjects/GridProperties")]
public class GridProperties : ScriptableObject
{
    public Vector2Int size;
}
