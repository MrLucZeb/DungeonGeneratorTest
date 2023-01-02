using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomataGenerator : GridGenerator
{
    public const int WallCell = 0;
    public const int GroundCell = 1;
    public const int EdgeCell = 2;

    [System.Serializable]
    public struct GenerationSettings
    {
        public int gridWidth;
        public int gridHeight;
        public int initialChance;
        public int simulationSteps;
        public int deathLimit;
        public int birthLimit;

        public void SetDefaultValues()
        {
            gridWidth = 25;
            gridHeight = 25;
            initialChance = 50;
            simulationSteps = 9;
            deathLimit = 4;
            birthLimit = 4;
        }
    }

    public GenerationSettings settings;

    public CellularAutomataGenerator(GenerationSettings generationSettings)
    {
        settings = generationSettings;
    }

    public CellularAutomataGenerator()
    {
        settings.SetDefaultValues();
    }

    public override Grid Generate(bool ignoreLayer = false)
    {
        grid = new Grid(settings.gridWidth, settings.gridHeight);


        if (hasLayer() && (layer.width != grid.width || layer.height != grid.height))
        {
            layer.Resize(grid.width, grid.height);
        }
            
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (IsLayerWall(x, y)) continue;

                if (Random.Range(0, 101) > settings.initialChance)
                    grid.Set(x, y, WallCell);
                else
                    grid.Set(x, y, GroundCell);
            }
        }

        for (int i = 0; i < settings.simulationSteps; i++)
        {
            Simulate();
        }

        return grid;
    }

    public Grid MarkEdges()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                
                if (grid.Get(x, y) == WallCell && NCount(x, y) > 0)
                {
                    grid.Set(x, y, EdgeCell);
                }
                  
            }
        }

        return grid;
    }

    /// <summary> Perform a simulation step on the grid </summary>
    void Simulate()
    {
        Grid simGrid = new Grid(grid.width, grid.height);

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (IsLayerWall(x, y)) continue;

                int neighbours = NCount(x, y);

                if (grid.Get(x, y) == WallCell)
                {
                    if (neighbours > settings.birthLimit)
                        simGrid.Set(x, y, GroundCell);
                    else
                        simGrid.Set(x, y, WallCell);
                }
                else
                {
                    if (neighbours < settings.deathLimit)
                          simGrid.Set(x, y, WallCell);
                    else
                        simGrid.Set(x, y, GroundCell);
                }
            }
        }

        grid = simGrid;
    }

    /// <summary> Return the amount of living neighbour cells </summary>
    int NCount(int x, int y, int index = GroundCell) 
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

    bool IsLayerWall(int x, int y)
    {
        return (hasLayer() && layer.Get(x, y) == WallCell) ||
               (x < 1 || y < 1 || x > grid.width - 2 || y > grid.height - 2); // Prevent edge tiles;
    }

    public bool hasLayer()
    {
        return layer != null && layer.IsValid();
    }

    public void ClearLayer()
    {
        layer.Populate(0);
    }

    public void SetLayer(Grid layer)
    {
        this.layer = layer;
    }
} 