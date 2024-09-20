using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TextureCreate : MonoBehaviour
{
    [SerializeField] Material cloudMaterial;

    [SerializeField] GridManager gridManager;

    bool[,] fogMatrix; //All value is False
    Texture2D fogAlphaTexture;

    int size = 0;
    int offset = 0;

    private void Start()
    {
        size = gridManager.bottomLeftLocation.x * -2;
        offset = Mathf.Abs(gridManager.bottomLeftLocation.x);
        fogMatrix = new bool[size, size];

        GenerateTexture();
    }

    private void GenerateTexture()
    {
        Debug.Log("Size: " + (size));
        fogAlphaTexture = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,       // No smoothing, pixel-perfect
            wrapMode = TextureWrapMode.Clamp    // No texture wrapping
        };

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                fogAlphaTexture.SetPixel(row, col, fogMatrix[row, col] ? Color.clear : Color.white);
            }
        }
        fogAlphaTexture.Apply();


        cloudMaterial.SetTexture("_AlphaTexture", fogAlphaTexture);

        ImportTextureAsset(fogAlphaTexture);
    }

    void ImportTextureAsset(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();

        string filepath = Path.Combine(Application.dataPath, "Shaders/CreateTexture/texture1.png");

        File.WriteAllBytes(filepath, bytes);

        AssetDatabase.ImportAsset("Assets/Shaders/CreateTexture/texture1.png");
    }

    private void UpdateTexture(Vector2Int cell, Vector2Int size)
    {
        //New for multiple cell
        bool hasChanged = false;
        int fromX = cell.x - size.x, toX = cell.x + size.x;
        fromX = Mathf.Max(fromX, 0);
        toX = Mathf.Min(toX, this.size - 1);

        int fromY = cell.y - size.y, toY = cell.y + size.y;
        fromY = Mathf.Max(fromY, 0);
        toY = Mathf.Min(toY, this.size - 1);
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
    public void UpdateTexture(int x, int y)
    {
        Debug.Log(x + " " + y);
        fogAlphaTexture.SetPixel(x, y, Color.clear);
        fogAlphaTexture.Apply();
    }

    internal void OpenCloud(List<PathNod> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            UpdateTexture(path[i].x + offset, path[i].y + offset);
        }
    }
}
