using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    [SerializeField] GridProperties gridProperties;
    [SerializeField] private GameObject cellPrefab;

    private Grid grid;
    private Vector2Int size;

    private void Start()
    {
        grid = GetComponent<Grid>();

        size = gridProperties.size;

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = -size.x / 2; x < Mathf.RoundToInt((float)size.x / 2); x++)
        {
            for(int y = -size.y / 2; y < Mathf.RoundToInt((float)size.y / 2); y++)
            {
                Vector3 position = grid.GetCellCenterWorld(new Vector3Int(x, y));
                GameObject spawnedTile = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                spawnedTile.name = "Tile" + x + y;
                spawnedTile.GetComponent<Cell>().Init(Math.Abs(x + y) % 2 == 1);
            }
        }
    }
}

[CreateAssetMenu(fileName = "GridProperties", menuName = "ScriptableObjects/GridProperties")]
public class GridProperties : ScriptableObject
{
    public Vector2Int size;
}
