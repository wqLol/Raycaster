using System;

public static class World
{
    public static int mapWidth = 30; // Change the width of the map
    public static int mapHeight = 20; // Change the height of the map
    public static int[,] world;

    static World()
    {
        Random rand = new Random();
        world = World.RandomizeMap(rand);
    }

    public static int[,] RandomizeMap(Random rand)
    {
        int[,] map = new int[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Assign a random value (0 or 1) to each element
                map[x, y] = rand.Next(2);
            }
        }

        return map;
    }
}