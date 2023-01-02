using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Room : MonoBehaviour
{
    int width;
    int height;
    Grid grid;
    Grid tileGrid;
    public RuleTile tile;
    public RuleTile wall;
    Tilemap tileMap;

    [Header("Settings1")]
    public int gridWidth1;
    public int gridHeight1;
    public int initialChance1;
    public int simulationSteps1;
    public int deathLimit1;
    public int birthLimit1;

    [Header("Settings2")]
    public int gridWidth2;
    public int gridHeight2;
    public int initialChance2;
    public int simulationSteps2;
    public int deathLimit2;
    public int birthLimit2;




    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        Generate();
         DrawTexture();
        //DrawMap();


    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Generate();
            DrawTexture();
           // DrawMap();
        }
            
        
        
    }

    void Generate()
    {
        transform.localScale = new Vector3(Mathf.FloorToInt(transform.localScale.x), Mathf.FloorToInt(transform.localScale.y), Mathf.FloorToInt(transform.localScale.z));
        width = (int)transform.lossyScale.x * 50;
        height = (int)transform.lossyScale.z * 50;

        CellularAutomataGenerator.GenerationSettings settings1;
        settings1.gridWidth = gridWidth1;
        settings1.gridHeight = gridHeight1;
        settings1.initialChance = initialChance1;
        settings1.simulationSteps = simulationSteps1;
        settings1.deathLimit = deathLimit1;
        settings1.birthLimit = birthLimit1;

        CellularAutomataGenerator.GenerationSettings settings2;
        settings2.gridWidth = gridWidth2;
        settings2.gridHeight = gridHeight2;
        settings2.initialChance = initialChance2;
        settings2.simulationSteps = simulationSteps2;
        settings2.deathLimit = deathLimit2;
        settings2.birthLimit = birthLimit2;

        CellularAutomataGenerator generator = new CellularAutomataGenerator(settings1);

        generator.SetLayer(generator.Generate());
        generator.settings = settings2;
        grid = generator.Generate();

        FloodFillRegionSeperator regionSeperator = new FloodFillRegionSeperator(grid);
        regionSeperator.Seperate(grid);
        regionSeperator.Filter(10000);
        regionSeperator.Sort();

        if (regionSeperator.GetRegions().Count > 0)
        {
            grid = regionSeperator.GetRegions()[0].grid;
        }
        else
        {
            grid.Populate(0);
        }

        generator.SetGrid(grid);
        grid = generator.MarkEdges();

        DoorwayFinder doorwayFinder = new DoorwayFinder(grid);
        //grid = doorwayFinder.Generate();
    }

    void DrawTexture()
    {
        Texture2D texture = new Texture2D(grid.width, grid.height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (grid.Get(x, y) == 0)
                    texture.SetPixel(x, y, Color.black);
                else if (grid.Get(x, y) == 2)
                    texture.SetPixel(x, y, Color.yellow);
                else if (grid.Get(x, y) == 3)
                    texture.SetPixel(x, y, Color.green);
                else if (grid.Get(x, y) == 4)
                    texture.SetPixel(x, y, Color.blue);
                else
                    texture.SetPixel(x, y, Color.red);
            }
        }

        GetComponent<Renderer>().material.mainTexture = texture;
        texture.Apply();
    }

    void DrawMap()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (grid.Get(x, y) == 1 || NCount(x, y, 0) <= 4)
                    tileMap.SetTile(new Vector3Int(x, y, 1), tile);
                else
                    tileMap.SetTile(new Vector3Int(x, y, 0), wall);
            }
        }
    }

    int NCount(int x, int y, int index = 0)
    {
        int count = 0;

        for (int nx = x - 1; nx < x + 2; nx++)
        {
            for (int ny = y - 1; ny < y + 2; ny++)
            {
                if (nx >= 0 && ny >= 0 && nx < grid.width && ny < grid.height)
                {
                    if (grid.Get(nx, ny) == index && (nx != x || ny != y))
                        count++;
                }
            }
        }

        return count;
    }
}
