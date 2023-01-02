using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int[] arr { get; private set; }
    public int width { get; private set; }
    public int height { get; private set; }

    public Grid(int width, int height, int value = 0)
    {
        this.width = width;
        this.height = height;
        this.arr = new int[this.width * this.height];

        if (value != 0)
            Populate(value);
    }

    public Grid(Grid copy)
    {
        this.width = copy.width;
        this.height = copy.height;
        this.arr = new int[this.width * this.height];

        for (int i = 0; i < this.width * this.height; i++)
        {
            this.arr[i] = copy.arr[i];
        }
    }

    public int Get(int x, int y)
    {
        return arr[x + y * width];
    }

    public void Set(int x, int y, int value)
    {
        if (IsValidPosition(x, y))
            arr[x + y * width] = value;
    }

    public bool IsValid()
    {
        return width > 0 && height > 0;
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height; // take a look at this
    }

    public void Populate(int value)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            arr[i] = value;
        }
    }

    public int Count(int index)
    {
        int count = 0;

        for (int i = 0; i < width * height; i++)
        {
            if (this.arr[i] == index)
                count++;
        }

        return count;
    }

    public Grid Clone()
    {
        Grid clone = new Grid(width, height);

        for (int i = 0; i < this.width * this.height; i++)
        {
            clone.arr[i] = arr[i];
        }

        return clone;
    }

    public void Resize(int width, int height)
    {
        if (this.width == width && this.height != height) return;

        Grid clone = Clone();
        float xRatio = (float)clone.width / (float)width;
        float yRatio = (float)clone.height / (float)height;

        // Resize
        arr = new int[width * height];
        this.width = width;
        this.height = height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int value = clone.Get(Mathf.FloorToInt(x * xRatio), Mathf.FloorToInt(y * yRatio));
                Set(x, y, value);
            }
        }
    }

    public void Crop(int minX, int minY, int maxX, int maxY)
    {
        Grid clone = Clone();

        //Resize
        width = maxX - minX;
        height = maxY - minY;
        arr = new int[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Set(x, y, clone.Get(minX + x, minY + y));
            }
        }
    }

    public void CropAroundIndex(int index, int margin = 1)
    {
        int minX = 0;
        int minY = height;
        int maxX = width - 1;
        int maxY = height - 1;
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Get(x, y) != index) continue;

                minX = (x > minX) ? x : minX;
                minY = (y > minY) ? y : minY;
                maxX = (x < maxX) ? x : maxX;
                maxY = (y < maxY) ? y : maxY;
            }
        }

        Crop(minX, minY, maxX, maxY);
    }
}
