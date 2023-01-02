using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomataCaveGenerator
{
    public int gridWidth = 25;
    public int gridHeight = 25;
    public int simulationSteps = 11;
    public int deathLimit = 4;
    public int birthLimit = 4;
    bool clearInaccessibleCaves = true;
    [Range(0, 100)] public int initialChance = 50;
    [Range(0, 1)] public float minSize = 0.23f;

    protected int[] grid;

    protected int retries;
    protected int maxRetries = 5;

    public AutomataCaveGenerator(int width, int height)
    {
        Debug.Log("set height" + height);
        gridWidth = width;
        gridHeight = height;
    }

    public int[] Generate()
    {
        if (retries > maxRetries) 
        {
            Debug.LogError("Failed to generate cave, outputting last failed attempt");
            return grid;
        }

        PopulateGrid();

        for (int i = 0; i < simulationSteps; i++)
        {
            SimulationStep();
        }

        if (clearInaccessibleCaves)
        {
            if (!ClearCaves())
            {
                retries++;
                Generate();
            }
        }

        return grid;
    }

    void PopulateGrid()
    {
        grid = new int[gridWidth * gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (Random.Range(0, 101) > initialChance)
                    grid[x + y * gridWidth] = 0;
                else
                    grid[x + y * gridWidth] = 1;
            }
        }
    }

    void SimulationStep()
    {
        int[] newGrid = new int[gridWidth * gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int neighbours = NeighbourCount(x, y);

                if (grid[x + y * gridWidth] == 0)
                {
                    if (neighbours > birthLimit)
                        newGrid[x + y * gridWidth] = 1;
                    else
                        newGrid[x + y * gridWidth] = 0;
                }
                else
                {
                    if (neighbours < deathLimit)
                        newGrid[x + y * gridWidth] = 0;
                    else
                        newGrid[x + y * gridWidth] = 1;
                }
            }
        }

        grid = newGrid;
    }

    int NeighbourCount(int gX, int gY)
    {
        int count = 0;

        for (int x = gX - 1; x < gX + 2; x++)
        {
            for (int y = gY - 1; y < gY + 2; y++)
            {
                if (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight)
                {
                    if (grid[x + y * gridWidth] != 0 && (x != gX || y != gY))
                        count++;
                }
            }
        }

        return count;
    }

    bool ClearCaves()
    {
        List<Vector2Int> cave = new List<Vector2Int>();
        List<Vector2Int> queue = new List<Vector2Int>();

        int[] tmpGrid = grid;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (tmpGrid[x + y * gridWidth] != 0)
                {
                    queue.Add(new Vector2Int(x, y));
                    cave.Add(new Vector2Int(x, y));

                    tmpGrid[x + y * gridWidth] = 0;

                    while (queue.Count > 0)
                    {
                        Vector2Int p = queue[0];

                        for (int nx = p.x - 1; nx < p.x + 2; nx++)
                        {
                            for (int ny = p.y - 1; ny < p.y + 2; ny++)
                            {
                                if (nx >= 0 && ny >= 0 && nx < gridWidth && ny < gridHeight) // Prevent checking outside array length
                                {
                                    if (nx == p.x || ny == p.y) // Prevent diagonals
                                    {
                                        if (tmpGrid[nx + ny * gridWidth] != 0)
                                        {
                                            queue.Add(new Vector2Int(nx, ny));
                                            cave.Add(new Vector2Int(nx, ny));
                                            tmpGrid[nx + ny * gridWidth] = 0;
                                        }
                                    }
                                }
                            }
                        }

                        queue.RemoveAt(0);
                    }

                    if (cave.Count >= gridWidth * gridHeight * minSize)
                    {
                        grid = new int[gridWidth * gridHeight];

                        foreach (Vector2Int p in cave)
                        {
                            grid[p.x + p.y * gridWidth] = 1;
                        }

                        return true;
                    }

                    cave.Clear();
                }
            }
        }

        return false;
    }
}
