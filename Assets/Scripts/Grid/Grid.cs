using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class Grid
{
    readonly int width;
    readonly int height;
    readonly float cellSize;
    readonly int[,] gridArray;
    readonly TextMesh[,] textMeshArray;
    readonly Vector3 originPosition;

    internal Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];

        textMeshArray = new TextMesh[width, height];
        this.originPosition = originPosition;
    }

    public int Width => width;
    public int Height => height;

    internal void DrawGrid(Transform parentTransform = null)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                textMeshArray[x,y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), parentTransform, GetWorldPosition(x, y), 10, Color.cyan, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f); //  Horizontal
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);  //  Vertical
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
    }

    Vector3 GetWorldPosition(int x, int y)  //  Get Position from order of Grid
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    void GetXY(Vector3 worldPosition, out int x, out int y) //  Get order of Grid from Position
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    internal void SetValue(int x, int y, int value) //  value for grid
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return;    // invalid x, y

        gridArray[x, y] = value;
        textMeshArray[x, y].text = gridArray[x, y].ToString();
    }
    internal void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    internal int GetValue(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return -1;    // invalid x, y

        return gridArray[x, y];
    }

    internal int GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    class HeatMapVisual
    {
        Grid grid;

        public HeatMapVisual(Grid grid)
        {
            this.grid = grid;

            Vector3 vertices;
            Vector2 uv;
            int[] triangles;

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {

                }
            }
        }
    }
}
