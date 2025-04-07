using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] Wellivator wellivatorPrefab;
    [SerializeField] GameObject itemStandPrefab;
    [SerializeField] GemStone gemStonePrefab;
    [SerializeField] float cellSize;

    public Wellivator Wellivator { get; private set; }
    LevelSO level;
    Cell[,] map;

    public void GenerateLevel(LevelSO level)
    {
        this.level = level;

        map = new Cell[level.size, level.size];

        for (int x = 0; x < level.size; x++)
            for (int y = 0; y < level.size; y++)
            {
                map[x, y] = new Cell() { x = x, y = y, isOuterCell = (x == 0 || y == 0 || x == level.size - 1 || y == level.size - 1) };
            }

        Cell startingCell = GetCellAt(level.size / 2, 1);
        Cell currentCell = startingCell;
        Cell gemstoneCell = null;
        Cell secondCell = null;

        startingCell.isStartingCell = true;
        startingCell.floor = true;

        float distanceFromStart = Random.Range(level.minDistanceFromStart, level.maxDistanceFromStart);

        int iterations = 0;
        int currentBias = 0;
        float directionalBias = -1f;

        List<Cell> emptyCells = new List<Cell>();

        while (iterations++ < level.maxIterations)
        {
            if (gemstoneCell == null)
            {
                MoveSnake1(ref currentCell, ref currentBias);

                emptyCells.Add(currentCell);

                switch (Random.Range(0, 1f))
                {
                    case <= 0.5f: currentBias = 0; break;
                    case <= 0.75f: currentBias = 1; break;
                    case <= 1f: currentBias = 2; break;
                }

                secondCell ??= currentCell;

                if (Vector2.Distance(new Vector2(startingCell.x, startingCell.y), new Vector2(currentCell.x, currentCell.y)) >= distanceFromStart)
                {
                    gemstoneCell = currentCell;
                    Instantiate(itemStandPrefab, new Vector3(currentCell.x * cellSize, 0, currentCell.y * cellSize), Quaternion.identity);
                    Instantiate(gemStonePrefab, new Vector3(currentCell.x * cellSize, 1f, currentCell.y * cellSize), Quaternion.identity);
                    //currentCell = startingCell;
                    currentCell = emptyCells[Random.Range(0, emptyCells.Count)];
                }
            }
            else
            {
                MoveSnake2(ref currentCell, ref directionalBias, gemstoneCell);

                directionalBias += 0.2f;

                if (Vector2.Distance(new Vector2(currentCell.x, currentCell.y), new Vector2(gemstoneCell.x, gemstoneCell.y)) <= 1)
                    break;
            }
        }

        if (gemstoneCell == null)
        {
            print("Ran out of time I guess");
            gemstoneCell = currentCell;
            Instantiate(itemStandPrefab, new Vector3(currentCell.x * cellSize, 0, currentCell.y * cellSize), Quaternion.identity);
            Instantiate(gemStonePrefab, new Vector3(currentCell.x * cellSize, 1.5f, currentCell.y * cellSize), Quaternion.identity);
        }

        FillLevel();

        float angle = C.AngleFromPosition(new Vector3(startingCell.x, startingCell.y), new Vector3(secondCell.x, secondCell.y));

        if (angle == 90)
            angle += 180;

        //print($"{secondCell.x} {secondCell.y} {startingCell.x} {startingCell.y} {angle}");
        Wellivator = Instantiate(wellivatorPrefab, new Vector3(startingCell.x * cellSize, 0, startingCell.y * cellSize), Quaternion.Euler(0, angle, 0));
    }

    void MoveSnake1(ref Cell currentCell, ref int currentBias)
    {
        Cell[] neighbors = GetNeighbors(currentCell);
        float[] weightedNeighbors = new float[neighbors.Length];
        float totalWeight = 0;

        for (int i = 0; i < neighbors.Length; i++)
        {
            Cell c = neighbors[i];

            if (c == null || c.isOuterCell || c.isStartingCell)
            {
                weightedNeighbors[i] = -1;
                continue;
            }

            int neighborCount = 4 - GetFloorNeighborCount(c);
            float weight = (neighborCount * 100) + 1;

            if (i == currentBias)
                weight *= 3;
            //if (i == 0)
            //    weight *= 2;
            //else if (i == 1 || i == 2)
            //    weight *= 1.25f;

            weightedNeighbors[i] = weight;
            totalWeight += weight;
        }

        float rand = Random.value * totalWeight;
        float accumulated = 0;

        for (int k = 0; k < weightedNeighbors.Length; k++)
        {
            float weight = weightedNeighbors[k];

            if (weight == -1)
                continue;

            accumulated += weight;

            if (rand <= accumulated)
            {
                currentCell = neighbors[k];
                break;
            }
        }

        currentCell.floor = true;
    }

    void MoveSnake2(ref Cell currentCell, ref float directionalBias, Cell gemstoneCell)
    {
        Cell[] neighbors = GetNeighbors(currentCell);
        float[] weightedNeighbors = new float[neighbors.Length];
        float totalWeight = 0;

        Vector2 directionToGem = (new Vector2(currentCell.x, currentCell.y) - new Vector2(gemstoneCell.x, gemstoneCell.y)).normalized;

        for (int i = 0; i < neighbors.Length; i++)
        {
            Cell c = neighbors[i];

            if (c == null || c.isOuterCell || c.isStartingCell)
            {
                weightedNeighbors[i] = -1;
                continue;
            }

            int neighborCount = 4 - GetFloorNeighborCount(c);
            //float weight = (neighborCount * 2) + 1;
            Vector2 dir = (new Vector2(c.x, c.y) - new Vector2(currentCell.x, currentCell.y)).normalized;

            // I can make it effect it more and more as we go on so that it isn't just a straight line to the target.
            float weight = 1 + (-Vector2.Dot(directionToGem, dir) * directionalBias);
            //print($"{weight} {directionToGem} {dir} {Vector2.Dot(directionToGem, dir)}");
            //if (i == 0)
            //    weight *= 2;
            //else if (i == 1 || i == 2)
            //    weight *= 1.25f;

            weightedNeighbors[i] = weight;
            totalWeight += weight;
        }

        float rand = Random.value * totalWeight;
        float accumulated = 0;

        for (int k = 0; k < weightedNeighbors.Length; k++)
        {
            float weight = weightedNeighbors[k];

            if (weight == -1)
                continue;

            accumulated += weight;

            if (rand <= accumulated)
            {
                currentCell = neighbors[k];
                break;
            }
        }

        currentCell.debug = true;
        currentCell.floor = true;
    }

    Cell[] GetNeighbors(Cell cell)
    {
        return new Cell[4]
        {
            GetCellAt(cell.x, cell.y + 1),GetCellAt(cell.x + 1, cell.y),
            GetCellAt(cell.x - 1, cell.y),GetCellAt(cell.x, cell.y - 1)
        };
    }

    int GetFloorNeighborCount(Cell cell)
    {
        int count = 0;
        Cell[] neighbors = GetNeighbors(cell);

        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] != null && neighbors[i].floor == false)
                count++;
        }

        return count;
    }

    void FillLevel()
    {
        int[,] filled = new int[level.size, level.size];

        for (int x = 0; x < level.size; x++)
        {
            for (int y = 0; y < level.size; y++)
            {
                Cell currentCell = GetCellAt(x, y);

                if (currentCell.floor)
                {
                    if (currentCell.isStartingCell)
                    {
                        Instantiate(level.wellFloorPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity, transform);
                        Instantiate(level.wellFloorPrefab, new Vector3(x * cellSize, 1.5f, y * cellSize), Quaternion.Euler(180, 0, 0), transform);
                    }
                    else
                    {
                        Instantiate(level.floorPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity, transform);
                        Instantiate(level.ceilingPrefab, new Vector3(x * cellSize, 1.5f, y * cellSize), Quaternion.Euler(180, 0, 0), transform);
                    }


                    Cell upCell = GetCellAt(x, y + 1);
                    Cell downCell = GetCellAt(x, y - 1);
                    Cell leftCell = GetCellAt(x - 1, y);
                    Cell rightCell = GetCellAt(x + 1, y);

                    TrySpawnWallOnCell(upCell, currentCell);
                    TrySpawnWallOnCell(downCell, currentCell);
                    TrySpawnWallOnCell(leftCell, currentCell);
                    TrySpawnWallOnCell(rightCell, currentCell);

                    if (currentCell.debug)
                        Instantiate(level.torchPrefab, new Vector3(currentCell.x * cellSize, 2.5f, currentCell.y * cellSize), Quaternion.identity);
                }
            }
        }

        void TrySpawnWallOnCell(Cell cell, Cell rootCell)
        {
            if (cell != null && cell.wall == false && cell.floor == false)
            {
                Instantiate(level.wallPrefab, new Vector3(cell.x * cellSize, 1, cell.y * cellSize), Quaternion.identity, transform);
                cell.wall = true;

                if (Random.Range(0, 1f) < 0.1f)
                {
                    // Some torches still get spawned inside walls.
                    Vector3 direction = new Vector3(rootCell.x, rootCell.y) - new Vector3(cell.x, cell.y);
                    Instantiate(level.torchPrefab, new Vector3(cell.x * cellSize, 1f, cell.y * cellSize) + (direction.normalized * (cellSize / 1.9f)), Quaternion.Euler(0, C.AngleFromDirection(direction) - 90, 0));
                }
            }
        }
    }

    Cell GetCellAt(int x, int y)
    {
        if (IsInBounds(x, y))
            return map[x, y];

        return null;
    }

    class Cell
    {
        public int x, y;
        public bool wall;
        public bool floor;
        public bool isOuterCell;
        public bool isStartingCell;
        public bool debug;
    }

    bool IsInBounds(int x, int y) => x >= 0 && x <= level.size - 1 && y >= 0 && y <= level.size - 1;
}