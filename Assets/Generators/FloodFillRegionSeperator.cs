using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodFillRegionSeperator : GridGenerator
{
    public struct Region
    {
        public Grid grid;
        public int size;

        public Region(Grid grid, int size)
        {
            this.grid = grid;
            this.size = size;
        }
    }

    private Grid grid;
    

    private List<Region> regions = new List<Region>();
    //private List<Grid> regions = new List<Grid>();
    //private List<Grid> filteredRegions = new List<Grid>();

    public FloodFillRegionSeperator(Grid grid)
    {
        this.grid = grid;
    }

    public override Grid Generate(bool ignoreLayer = false)
    {
        return regions[0].grid;
    }

    public List<Region> Seperate(Grid grid)
    {
        this.grid = new Grid(grid);
        regions.Clear();

        Grid regionGrid = new Grid(this.grid.width, this.grid.height);
        List<Vector2Int> cellQueue = new List<Vector2Int>();
        
        for (int x = 0; x < this.grid.width; x++)
        {
            for (int y = 0; y < this.grid.height; y++)
            {
                if (this.grid.Get(x, y) != 0)
                {
                    int size = 0;

                    cellQueue.Add(new Vector2Int(x, y));
                    regionGrid.Set(x, y, 1);
                    this.grid.Set(x, y, 0);
                    size++;


                    while (cellQueue.Count > 0)
                    {
                        Vector2Int p = cellQueue[0];
                        cellQueue.RemoveAt(0);

                        for (int nx = p.x - 1; nx < p.x + 2; nx++)
                        {
                            for (int ny = p.y - 1; ny < p.y + 2; ny++)
                            {
                                if (nx >= 0 && ny >= 0 && nx < this.grid.width && ny < this.grid.height) // Prevent checking outside array length
                                {
                                    if (nx == p.x || ny == p.y) // Prevent diagonals
                                    {
                                        if (this.grid.Get(nx, ny) != 0)
                                        {
                                            cellQueue.Add(new Vector2Int(nx, ny));
                                            regionGrid.Set(nx, ny, 1);
                                            this.grid.Set(nx, ny, 0);
                                            size++;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    regions.Add(new Region(new Grid(regionGrid), size));
                    regionGrid.Populate(0); // Reset
                }
            }
        }

        return regions;
    }

    public List<Region> Seprate2(Grid grid)
    {

        return regions;
    }


    public List<Region> Sort()
    {
        //regions.Sort(delegate (Region a, Region b)
        //{
        //    if (a.grid.Count(1) == b.grid.Count(1)) return 0;
        //    else if (a.grid.Count(1) < b.grid.Count(1)) return 1;
        //    return -1;

        //    //if (a.size == b.size) return 0;
        //    //else if (a.size < b.size) return 1;
        //    //return -1;
        //});

        ////Debug.Log("Sorted: ");
        //foreach (Region region in regions) {
        //   // Debug.Log(region.size);
        //}

        return regions;
    }

    public List<Region> Filter(int minSize, int maxSize = -1)
    {
        List<Region> filteredRegions = new List<Region>();

        foreach (Region region in regions)
        {
            if ((region.size >= minSize || minSize == -1) && (region.size <= maxSize || maxSize == -1))
            {
                filteredRegions.Add(region);
            }
        }

        return regions = filteredRegions;
    }

    public List<Region> GetRegions()
    {
        return regions;
    }
}
//bool ClearCaves()
//{
//    List<Vector2Int> cave = new List<Vector2Int>();
//    List<Vector2Int> queue = new List<Vector2Int>();

//    int[] tmpGrid = grid;

//    for (int x = 0; x < gridWidth; x++)
//    {
//        for (int y = 0; y < gridHeight; y++)
//        {
//            if (tmpGrid[x + y * gridWidth] != 0)
//            {
//                queue.Add(new Vector2Int(x, y));
//                cave.Add(new Vector2Int(x, y));

//                tmpGrid[x + y * gridWidth] = 0;

//                while (queue.Count > 0)
//                {
//                    Vector2Int p = queue[0];

//                    for (int nx = p.x - 1; nx < p.x + 2; nx++)
//                    {
//                        for (int ny = p.y - 1; ny < p.y + 2; ny++)
//                        {
//                            if (nx >= 0 && ny >= 0 && nx < gridWidth && ny < gridHeight) // Prevent checking outside array length
//                            {
//                                if (nx == p.x || ny == p.y) // Prevent diagonals
//                                {
//                                    if (tmpGrid[nx + ny * gridWidth] != 0)
//                                    {
//                                        queue.Add(new Vector2Int(nx, ny));
//                                        cave.Add(new Vector2Int(nx, ny));
//                                        tmpGrid[nx + ny * gridWidth] = 0;
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    queue.RemoveAt(0);
//                }

//                if (cave.Count >= gridWidth * gridHeight * minSize)
//                {
//                    grid = new int[gridWidth * gridHeight];

//                    foreach (Vector2Int p in cave)
//                    {
//                        grid[p.x + p.y * gridWidth] = 1;
//                    }

//                    return true;
//                }

//                cave.Clear();
//            }
//        }
//    }

//    return false;
//}