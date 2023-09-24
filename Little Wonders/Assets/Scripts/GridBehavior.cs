using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    public List<GameObject> cellPrefabs;
    public List<GameObject> castlePrefabs;
    public List<GameObject> startCellsPrefabs;

    private Grid grid;
    public Vector2Int size;
    public Cell[,] cells;

    [Header("Random Attributes")]
    private float sid;
    [Range(0, 1)] public float freq, waterFreq, cowFreq, rockFreq, oilFreq;

    private void Start()
    {
        grid = GetComponent<Grid>();

        cells = new Cell[size.x, size.y];

        GenerateGrid();
        
        // startTime = Time.time;
    }

    // private void Update()
    // {
    //     if (startTime + 0.3f <= Time.time) {
    //         foreach (Cell cell in cells)
    //             Destroy(cell.gameObject);
            
    //         GenerateGrid();

    //         startTime = Time.time;
    //     }
    // }

    private void GenerateGrid()
    {
        for (int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                GameObject spawnedCell = GetTerrain(x, y);

                bool isExploredCell = IsExplored(x, y);

                spawnedCell.GetComponent<Cell>().Init(Math.Abs(x + y) % 2 == 1, isExploredCell, new Vector2Int(x, y), null);
                cells[x, y] = spawnedCell.GetComponent<Cell>();
            }
        }

        SetStartCells();
        SetPlaceForCastle();
    }

    private void SetStartCells()
    {
        for (int x = Mathf.FloorToInt((float)size.x / 2) - 1; x <= Mathf.FloorToInt((float)size.x / 2) + 1; x++)
        {
            int randY = UnityEngine.Random.Range(Mathf.FloorToInt((float)size.y / 2) - 1, Mathf.FloorToInt((float)size.y / 2) + 1);
            ReplaceCell(cells[x, randY], startCellsPrefabs[0].GetComponent<Cell>());
            startCellsPrefabs.RemoveAt(0);
        }
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
                cells[randX + j, randY + i].gameObject.name = "castle";
            }
        }
    }

    public void ReplaceCell(Cell _originCell, Cell _newCell)
    {
        GameObject spawnedCellObj = Instantiate(_newCell.gameObject, _originCell.transform.position, Quaternion.identity, transform);
        spawnedCellObj.name = _originCell.gameObject.name;

        Cell spawnedCell = spawnedCellObj.GetComponent<Cell>();
        spawnedCell.Init(_originCell.isOffset, _originCell.isExplored, _originCell.coordinates, _originCell);

        cells[spawnedCell.coordinates.x, spawnedCell.coordinates.y] = spawnedCell;

        _originCell.gameObject.SetActive(false);
    }

    private float Noise(int x, int y)
    {
        return Mathf.PerlinNoise((x + sid) * freq, (y + sid) * freq);
    }
    private int Biome(float e)
    {
        if (e < waterFreq) return 0;
        else if (e < cowFreq) return 2;
        else if (e < rockFreq) return 3;
        else if (e < oilFreq) return 4;
        
        return 1;
    }
    private GameObject GetRandomCell(int x, int y)
    {
        float noise = Noise(x, y) + 0.5f * Noise(x, y) + 0.25f * Noise(x, y);
        return cellPrefabs[Biome(noise)];
    }
    private GameObject GetTerrain(int x, int y)
    {
        sid = UnityEngine.Random.Range(0f, 9999999f);
        Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
        GameObject spawnedCell = Instantiate(GetRandomCell(x, y), position, Quaternion.identity, transform);
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
