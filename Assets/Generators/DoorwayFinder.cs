using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayFinder : GridGenerator
{
    Grid grid;
    const int chance = 1;
    List<Passage> passages = new List<Passage>();

    public DoorwayFinder(Grid grid)
    {
        this.grid = grid;
    }

    public struct Passage
    {
        public Vector2Int a;
        public Vector2Int b;
        public float distance;

        public Passage(Vector2Int a, Vector2Int b)
        {
            this.distance = Vector2Int.Distance(a, b);
            this.a = a;
            this.b = b;
        }
    }
    public override Grid Generate(bool ignoreLayer = false)
    {
        List<Vector2Int> doorPositions = new List<Vector2Int>();

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (grid.Get(x, y) == 2 && Random.Range(0, 101) > 100 - chance)
                {
                    doorPositions.Add(new Vector2Int(x, y));
                   
                }
            }
        }

        foreach (Vector2Int a in doorPositions)
        {
            foreach (Vector2Int b in doorPositions)
            {
                if (!a.Equals(b)) 
                {
                    passages.Add(new Passage(a, b));
                }
            }

        }

        Sort();

        Passage passage = passages[0];
        grid.Set(passage.a.x, passage.a.y, 3);
        grid.Set(passage.b.x, passage.b.y, 4);

      
        return grid;
    }

    

    public List<Passage> Sort()
    {
        passages.Sort(delegate (Passage a, Passage b)
        {
            if (a.distance == b.distance) return 0;
            else if (a.distance < b.distance) return 1;
            return -1;
        });

        return passages;
    }
}
