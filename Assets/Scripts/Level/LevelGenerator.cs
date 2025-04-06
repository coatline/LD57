using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] PlayerController playerPrefab;
    [SerializeField] Wellivator wellivatorPrefab;
    [SerializeField] LevelSO firstLevel;
    [SerializeField] float cellSize;

    Vector2Int snakePosition;
    LevelSO level;
    Cell[,] map;

    private void Awake()
    {
        GenerateLevel(firstLevel);
    }

    public void GenerateLevel(LevelSO level)
    {
        this.level = level;

        map = new Cell[level.size, level.size];

        for (int x = 0; x < level.size; x++)
            for (int y = 0; y < level.size; y++)
            {
                map[x, y] = new Cell() { x = x, y = y };
            }

        Vector2Int startPosition = new Vector2Int(level.size / 2, 1);
        Vector2Int secondPosition = -Vector2Int.one;
        snakePosition = startPosition;
        int iterations = 0;

        while (iterations++ < level.maxIterations)
        {

            Vector2Int prevSnakePos = snakePosition;

            MoveSnake();

            if ((snakePosition.x > 1 && snakePosition.x < level.size - 1 && snakePosition.y > 1 && snakePosition.y < level.size - 1) == false)
            {
                snakePosition = startPosition;
                //snakePosition = prevSnakePos;
                //snakePosition = new Vector2Int(level.size / 2, level.size / 2);
            }
            else if (secondPosition == -Vector2Int.one)
                secondPosition = snakePosition;
        }

        FillLevel();

        Vector3 direction = new Vector3(secondPosition.x, secondPosition.y) - new Vector3(startPosition.x, startPosition.y);
        Wellivator wellivator = Instantiate(wellivatorPrefab, new Vector3(startPosition.x * cellSize, 0, startPosition.y * cellSize), Quaternion.Euler(0, C.AngleFromDirection(direction) + 180, 0));

        //PlayerController p = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //p.transform.GetChild(0).position = new Vector3(startPosition.x, 0, startPosition.y) * cellSize;
    }

    void MoveSnake()
    {
        Cell cell = GetCellAt(snakePosition.x, snakePosition.y);
        cell.floor = true;

        Cell[] neighbors = GetNeighbors(cell);
        float[] weightedNeighbors = new float[neighbors.Length];
        float totalWeight = 0;

        for (int i = 0; i < neighbors.Length; i++)
        {
            Cell c = neighbors[i];

            if (c == null)
            {
                weightedNeighbors[i] = -1;
                continue;
            }

            int neighborCount = 4 - GetFloorNeighborCount(c);
            float weight = (neighborCount * 2) + 1;

            if (i == 0)
                weight *= 2;
            else if (i == 1 || i == 2)
                weight *= 1.25f;

            weightedNeighbors[i] = weight;
            totalWeight += weight;
        }

        float rand = Random.value * totalWeight;
        float accumulated = 0;

        //print($"rand: {rand} {totalWeight} {weightedNeighbors[0]} {weightedNeighbors[1]} {weightedNeighbors[2]} {weightedNeighbors[3]}");

        for (int k = 0; k < weightedNeighbors.Length; k++)
        {
            float weight = weightedNeighbors[k];

            if (weight == -1)
                continue;

            accumulated += weight;

            if (rand <= accumulated)
            {
                snakePosition = new Vector2Int(neighbors[k].x, neighbors[k].y);
                break;
                //print($"Chose snakePos {k} {snakePosition}");
            }
        }
    }

    Cell[] GetNeighbors(Cell cell)
    {
        return new Cell[4]
        {
            GetCellAt(snakePosition.x, snakePosition.y + 1),GetCellAt(snakePosition.x, snakePosition.y - 1),
            GetCellAt(snakePosition.x-1, snakePosition.y),GetCellAt(snakePosition.x+1, snakePosition.y)
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
                    Instantiate(level.floorPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity, transform);
                    Instantiate(level.ceilingPrefab, new Vector3(x * cellSize, 2, y * cellSize), Quaternion.Euler(180, 0, 0), transform);

                    Cell upCell = GetCellAt(x, y + 1);
                    Cell downCell = GetCellAt(x, y - 1);
                    Cell leftCell = GetCellAt(x - 1, y);
                    Cell rightCell = GetCellAt(x + 1, y);

                    TrySpawnWallOnCell(upCell, currentCell);
                    TrySpawnWallOnCell(downCell, currentCell);
                    TrySpawnWallOnCell(leftCell, currentCell);
                    TrySpawnWallOnCell(rightCell, currentCell);
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
    }

    bool IsInBounds(int x, int y) => x >= 0 && x <= level.size - 1 && y >= 0 && y <= level.size - 1;
}