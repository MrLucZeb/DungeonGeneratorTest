using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapLoader : MonoBehaviour
{
    public struct WallTiles
    {
        public TileBase up;
        public TileBase up1;
        public TileBase up2;
        public TileBase u3p;
        public TileBase up4;
        public TileBase up33;
        public TileBase up5;
    };

    public WallTiles wall;
    Tilemap d;
    public TileBase til;
    
    //public MapLoader(Grid grid)
    //{
    //    this.grid = grid;
    //}

    //public override Grid Generate(bool ignoreLayer = false)
    //{
    //    Grid indexGrid = new Grid(grid.width, grid.height);
    //    for (int x = 0; x < grid.width; x++)
    //    {
    //        for (int y = 0; y < grid.height; y++)
    //        {
    //            indexGrid.Set(x, y, ComputeIndex(x, y));
    //        }
    //    }

    //    TileBase d;
    //    d
    //    return indexGrid;
    //}

    //public int ComputeIndex(int x, int y)
    //{
    //    int index = 0;
    //    int i = 0;

    //    for (int cx = x - 1; cx < x + 2; cx++) {
    //        for (int cy = y - 1; cy < y + 2; cy++)
    //        {
    //            if (grid.IsValidPosition(cx, cy) && grid.Get(cx, cy) == 1)
    //            {
    //                //Debug.Log("doid");
    //                index |= 1 << i;
    //            }


    //            i++;
    //        }
    //    }
  
    //    return index;
    //}
}
