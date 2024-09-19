using System;
using Unity.Collections;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    [SerializeField] private Grid gird;
    [SerializeField] private Material fogMaterial;
    [SerializeField] int size = 100;
    [SerializeField] int cellSize = 10;
    public int CellSize => cellSize;
    private bool[,] fogMatrix; //All value is False
    private Texture2D fogAlphaTexture;
    private Vector2Int currentCell = Vector2Int.zero;

    private void Start()
    {
        fogMatrix = new bool[size, size];
        GenerateTexture();
    }

    private void GenerateTexture()
    {
        fogAlphaTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);

        fogAlphaTexture.filterMode = FilterMode.Point;       // No smoothing, pixel-perfect
        fogAlphaTexture.wrapMode = TextureWrapMode.Clamp;    // No texture wrapping

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                fogAlphaTexture.SetPixel(row, col, fogMatrix[row, col] ? Color.clear : Color.white);
            }
        }
        fogAlphaTexture.Apply();

        fogMaterial.SetTexture("_DepthTexture", fogAlphaTexture);
    }

    private void UpdateTexture(Vector2Int cell, Vector2Int size)
    {
        //New for multiple cell
        bool hasChanged = false;
        int fromX = cell.x - size.x, toX = cell.x + size.x;
        fromX = Math.Max(fromX, 0);
        toX = Math.Min(toX, this.size - 1);

        int fromY = cell.y - size.y, toY = cell.y + size.y;
        fromY = Math.Max(fromY, 0);
        toY = Math.Min(toY, this.size - 1);
        //Loop check and change Texture Cell
        for (int x = fromX; x <= toX; x++)
        {
            for (int y = fromY; y <= toY; y++)
            {       
                if (fogMatrix[x, y] == false)
                {
                    //Update Value for matrix and texture
                    fogMatrix[x, y] = true;

                    fogAlphaTexture.SetPixel(x, y, Color.clear);
                    //Spawn Effect
                    hasChanged = true;
                }

            }
        }

        // Apply the changes to the texture
        if (hasChanged) fogAlphaTexture.Apply();
    }

    public void UpdateFog(Vector3 position, Vector2Int size)
    {
        position.y = 0;
        Vector3Int cell3d = gird.WorldToCell(position); //Have X and Z value 
        Vector2Int newCell = new Vector2Int(cell3d.x, cell3d.z);
        if (currentCell == newCell) return;
        currentCell = newCell;
        UpdateTexture(currentCell, size);
    }

    public void UpdateFog(Vector3 position, int size) => UpdateFog(position, new Vector2Int(size, size));

    public void SetSelected(Vector3 postion, int size)
    {
        postion.y = 0;
        Vector3Int cell3d = gird.WorldToCell(postion); //Have X and Z value 
        Vector2Int newCell = new Vector2Int(cell3d.x, cell3d.z);

        //Set by chunk
        Vector2Int chunkNumberPos = newCell / size * size;
        Vector2 selectCenterChunk = new Vector2(chunkNumberPos.x + size / 2, chunkNumberPos.y + size / 2);

        // fogMaterial.SetInteger("_ISSELECTCLOUD", 1);
        fogMaterial.SetVector("_Postion", selectCenterChunk);
        fogMaterial.SetVector("_SelectSize", new Vector2(size, size));
    }
    public void UnSelect()
    {
        fogMaterial.SetVector("_Postion", Vector2.zero);
        fogMaterial.SetVector("_SelectSize", Vector2.zero);
    }

    public bool IsDiscovered(Vector3 worldPosition)
    {
        Vector3Int cell3d = gird.WorldToCell(worldPosition);
        return fogMatrix[cell3d.x, cell3d.z];
    }

    public bool Discoverable(Vector3 postion, int size)
    {
        Vector3Int cell3d = gird.WorldToCell(postion);
        Vector2Int newCell = new Vector2Int(cell3d.x, cell3d.z);
        Vector2Int chunkNumberPos = newCell / size * size;

        Vector2Int centerPos = new Vector2Int(chunkNumberPos.x + size / 2, chunkNumberPos.y + size / 2);

        int topX = centerPos.x + size / 2, topY = centerPos.y + size / 2;
        topX = Math.Min(topX, this.size - 1);
        topY = Math.Min(topY, this.size - 1);
        int bottomX = centerPos.x - size / 2, bottomY = centerPos.y - size / 2;
        bottomX = Math.Max(bottomX, 0);
        bottomY = Math.Max(bottomY, 0);

        for (int x = bottomX; x <= topX; x++)
        {
            if (fogMatrix[x, bottomY] == true) return true;
            if (fogMatrix[x, topY] == true) return true;
        }

        for (int y = bottomY; y <= topY; y++)
        {
            if (fogMatrix[bottomX, y] == true) return true;
            if (fogMatrix[topX, y] == true) return true;
        }

        return false;
    }

    private void OnApplicationQuit()
    {
        Destroy(fogAlphaTexture);
    }
}
