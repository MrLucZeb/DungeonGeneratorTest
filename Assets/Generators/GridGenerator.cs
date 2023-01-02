using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridGenerator
{
    protected Grid grid;
    protected Grid layer;

    public abstract Grid Generate(bool ignoreLayer = false);

    public virtual void SetGrid(Grid grid) 
    {
        this.grid = grid;
    }

    public virtual Grid GetLastGrid()
    {
        return grid;
    }
}
