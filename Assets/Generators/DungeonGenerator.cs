using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(UnityEngine.Grid))]
[RequireComponent(typeof(TilemapRenderer))]
public class DungeonGenerator : MonoBehaviour
{
    Grid grid;

    Tilemap groundLayer;
    Tilemap wallLayer;

    public RuleTile wallTile;
    public RuleTile groundTile;

    public int width;
    public int height;

    [Header("Layer 1 (Structure)")]
    public int gridWidth1 = 120;
    public int gridHeight1 = 120;
    public int initialChance1 = 52;
    public int simulationSteps1 = 9;
    public int deathLimit1 = 4;
    public int birthLimit1 = 4;

    [Header("Layer 2")]
    public int gridWidth2 = 400;
    public int gridHeight2 = 400;
    public int initialChance2 = 55;
    public int simulationSteps2 = 11;
    public int deathLimit2 = 4;
    public int birthLimit2 = 4;

    void Start()
    {
        groundLayer = Instantiate(new GameObject().AddComponent<Tilemap>().gameObject.AddComponent<TilemapRenderer>().gameObject).GetComponent<Tilemap>();
        wallLayer = Instantiate(new GameObject().AddComponent<Tilemap>().gameObject.AddComponent<TilemapRenderer>().gameObject).GetComponent<Tilemap>();
        groundLayer.transform.SetParent(transform);
        wallLayer.transform.SetParent(transform);


        Generate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Generate();
        }
    }

    void Generate()
    {
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

        //DoorwayFinder doorwayFinder = new DoorwayFinder(grid);
        //grid = doorwayFinder.Generate();

        grid.CropAroundIndex(1);

        DrawTexture();
        //Draw();
    }

    private void Draw()
    {

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (grid.Get(x, y) == 1)
                    groundLayer.SetTile(new Vector3Int(x, y, 0), groundTile);
                else
                    wallLayer.SetTile(new Vector3Int(x, y, 0), wallTile);
            }
        }
    }

    private void DrawTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (grid.Get(x, y) == 0)
                    texture.SetPixel(x, y, Color.black);
            }
        }

        GetComponent<Renderer>().material.mainTexture = texture;
        texture.Apply();
    }
}   
