using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Editor Settings")]
    public int gridWidth = 25;
    public int gridHeight = 25;
    public int simulationSteps = 9;
    public int nDeath = 4;
    public int nBirth = 4;
    [Range(0, 100)]
    public int chance = 25;
    [Range(0, 1)]
    public float minSize = 0.3f;

    protected int[,] grid;
    protected int gWidth = 0;
    protected int gHeight = 0;
    protected int simSteps;

    

    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale.x
        gridWidth = (int)(transform.localScale.x * 75 );
        gridHeight = (int)(transform.localScale.z * 75);

        GenerateGrid(gridWidth, gridHeight);
        ClearCaves();
        Debug.ClearDeveloperConsole();
        DrawGridOnTexture();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerateGrid(gridWidth, gridHeight);
            ClearCaves();
            Debug.ClearDeveloperConsole();
            DrawGridOnTexture();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SimulationStep();
            DrawGridOnTexture();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearCaves();
            DrawGridOnTexture();
        }
    }

    void DrawGridOnTexture()
    {
        Texture2D texture = new Texture2D(gWidth, gHeight);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < gWidth; x++)
        {
            for (int y = 0; y < gHeight; y++)
            {
                if (grid[x, y] == 0)
                    texture.SetPixel(x, y, Color.black);
                else
                    texture.SetPixel(x, y, Color.white);
            }
        }

        GetComponent<Renderer>().material.mainTexture = texture;
        texture.Apply();
    }

    struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    void ClearCaves()
    {
        List<Point> space = new List<Point>();
        List<Point> queue = new List<Point>();
        
        int[,] tmpGrid = grid;

        for (int x = 0; x < gWidth; x++)
        {
            for (int y = 0; y < gHeight; y++)
            {
                if (tmpGrid[x, y] != 0)
                {
                    queue.Add(new Point(x, y));
                    space.Add(new Point(x, y));
                    tmpGrid[x, y] = 0;

                    while (queue.Count > 0)
                    {
                        Point p = queue[0];
                        int nCount = 0;
                       
                        // Check neighbours
                        for (int nx = p.x - 1; nx < p.x + 2; nx++)
                        {
                            for (int ny = p.y - 1; ny < p.y + 2; ny++)
                            {
                                if (nx >= 0 && ny >= 0 && nx < gWidth && ny < gHeight) // Prevent checking outside array length
                                {
                                    if (nx == p.x || ny == p.y) // Prevent diagonals
                                    {
                                        if (tmpGrid[nx, ny] != 0)
                                        {
                                            queue.Add(new Point(nx, ny));
                                            space.Add(new Point(nx, ny));
                                            tmpGrid[nx, ny] = 0;
                                        }
                                    }
                                }
                            }
                        }

                        queue.RemoveAt(0);
                    }

                    if (space.Count >= gWidth * gHeight * minSize)
                    {
                        grid = new int[gWidth, gHeight];

                        foreach (Point p in space)
                        {
                            grid[p.x, p.y] = 1;
                        }

                        return;
                    }

                    space.Clear();
                }
            }
        }


    }

    void GenerateGrid(int width, int height)
    {
        gWidth = width;
        gHeight = height;
        simSteps = simulationSteps;

        PopulateGrid();

        for (int i = 0; i < simSteps; i++)
        {
            SimulationStep();
        }
    }

    void SimulationStep()
    {
        int[,] newGrid = new int[gWidth, gHeight];
       
        for (int x = 0; x < gWidth; x++)
        {
            for (int y = 0; y < gHeight; y++)
            {
                int neighbours = NeighbourCount(x, y);

                if (grid[x, y] == 0)
                {
                    if (neighbours > nBirth)
                        newGrid[x, y] = 1;
                    else
                        newGrid[x, y] = 0;
                } 
                else
                {
                    if (neighbours < nDeath)
                        newGrid[x, y] = 0;
                    else
                        newGrid[x, y] = 1;
                }
            }
        }

        grid = newGrid;
    }

    void PopulateGrid()
    {
        grid = new int[gWidth, gHeight];

        for (int x = 0; x < gWidth; x++)
        {
            for (int y = 0; y < gHeight; y++)
            {
                if (Random.Range(0, 101) > chance)
                    grid[x, y] = 0;
                else
                    grid[x, y] = 1;
            }
        }
    }

    int NeighbourCount(int gX, int gY)
    {
        int count = 0;

        for (int x = gX - 1; x < gX + 2; x++)
        {
            for (int y = gY - 1; y < gY + 2; y++)
            {
                if (x >= 0 && y >= 0 && x < gWidth && y < gHeight)
                {
                    if (grid[x, y] != 0 && (x != gX || y != gY))
                        count++;
                }
            }
        }

        return count;
    }
}
