using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public struct Region
    {
        public Vector2 start;
        public int length;

        public Func<Action> algorithm;

        public HashSet<Vector2> visited;
        public HashSet<Vector2> unVisited;
        public HashSet<Vector2> borders;
        public Vector2 finish;
        public GameObject finishObj;

        public Region(Vector2 pos, int len, Func<Action> algo, GameObject obj)
        {
            start = pos;
            length = len;
            algorithm = algo;
            visited = new();
            unVisited = new();
            borders = new();
            finish = Vector2.zero;
            finishObj = obj;
        }

        public override string ToString()
        {
            return $"start postion: {start}, length: {length}";
        }

    }
    private List<Func<Action>> algos = new();
    public List<GameObject> groundTiles;
    public List<GameObject> wallTiles;
    public GameObject groundParent;
    public GameObject wallParent;
    public int length;
    [SerializeField]
    private int borderWidth;
    private Vector2 nextPos;
    private List<Region> regions = new();
    private Vector2 currentPos;
    private Region curRegion;
    List<Vector2> directions = new()
        {
            new(0, 2), new(0, -2), new(2, 0), new(-2, 0)
        };

    private GameObject player;
    public List<GameObject> finishObjects;
    public List<GameObject> spawners;
    public List<GameObject> enemyType;

    void Awake()
    {
        borderWidth = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        algos.Add(() => HuntAndKill);
        nextPos = new(0, 0);
        FillGround();
        GenerateMazeBorders();
        CreateRegions();
    }


    private void FillGround()
    {
        do
        {
            Instantiate(groundTiles[UnityEngine.Random.Range(0, groundTiles.Count)], nextPos, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90)).transform.parent = groundParent.transform;
            nextPos = nextPos.x >= length - 1 ? new Vector2(0, nextPos.y + 1) : new Vector2(nextPos.x + 1, nextPos.y);
        } while (nextPos.y < length);
    }

    private void GenerateMazeBorders()
    {

        for (int y = -borderWidth; y <= 0; y++) //Generate x borders
        {
            CreateXWall(-borderWidth, y, length + borderWidth, false);
            CreateXWall(-borderWidth, length - y, length + borderWidth, false);
        }
        for (int x = -borderWidth; x <= 0; x++) //Genereate y borders
        {
            CreateYWall(x, -borderWidth, length + borderWidth, false);
            CreateYWall(length - x, -borderWidth, length + borderWidth, false);
        }
    }

    private void CreateRegions()
    {
        int half = length / 2;
        for (int y = 0; y < 1; y++)
        {
            for (int x = 0; x < 1; x++)
            {
                regions.Add(new Region(new Vector2(x * half, y * half), half, algos[UnityEngine.Random.Range(0, algos.Count)], finishObjects[x]));
            }
        }
        FillRegions();
    }

    private void FillRegions()
    {
        foreach (Region current in regions)
        {
            curRegion = current;
            CreateXWall(current.start.x, current.start.y, (int)current.start.x + current.length, true);
            CreateYWall(current.start.x, current.start.y, (int)current.start.y + current.length, true);
            CreateXWall(current.start.x, (int)current.start.y + current.length, (int)current.start.x + current.length, true);
            CreateYWall((int)current.start.x + current.length, current.start.y, (int)current.start.y + current.length, true);
            curRegion.algorithm().Invoke();
        }
    }

    private void CreateXWall(float startX, float y, int length, bool permanant)
    {
        GameObject temp;
        for (float x = startX + 1; x < length; x++)
        {
            temp = Instantiate(wallTiles[UnityEngine.Random.Range(0, wallTiles.Count)], new Vector2(x, y), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));
            temp.transform.parent = wallParent.transform;
            if (permanant) curRegion.borders.Add(temp.transform.position);
        }
    }
    private void CreateYWall(float x, float startY, int length, bool permanant)
    {
        GameObject temp;
        for (float y = startY; y <= length; y++)
        {
            temp = Instantiate(wallTiles[UnityEngine.Random.Range(0, wallTiles.Count)], new Vector2(x, y), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));
            temp.transform.parent = wallParent.transform;
            if (permanant) curRegion.borders.Add(temp.transform.position);
        }
    }

    private void HuntAndKill()
    {
        Debug.Log($"Hunt and Kill in: {curRegion.start}");
        FillRegionWithWalls();
        currentPos = Tools.IsObjectInRegion(player.transform.position, curRegion) ? player.transform.position : curRegion.start;

        while (true)
        {
            if (HasUnvisitedNeighbors(currentPos, out Vector2 next))
            {
                Vector2 wall = (currentPos + next) / 2;
                curRegion.visited.Add(wall);
                curRegion.visited.Add(next);
                curRegion.unVisited.Remove(wall);
                curRegion.unVisited.Remove(next);
                ReplaceWithPathTile(wall);
                ReplaceWithPathTile(next);
                currentPos = next;
            }
            else if (!FindNextStart() || curRegion.unVisited.Count == 0)
            {
                break;
            }
        }
    }

    private void ReplaceWithPathTile(Vector2 pos)
    {
        Collider2D obstacle = Physics2D.OverlapPoint(pos);
        if (obstacle != null && !obstacle.CompareTag("Player") && !curRegion.borders.Contains(obstacle.transform.position))
        {
            Destroy(obstacle.gameObject);
            if (curRegion.finish == Vector2.zero && UnityEngine.Random.Range(0, 100) == 69)
            {
                curRegion.finish = pos;
                Instantiate(finishObjects[UnityEngine.Random.Range(0, finishObjects.Count)], pos, Quaternion.identity);
            }

            if (UnityEngine.Random.Range(0, 100) % 10 == 0)
            {
                Transform temp = Instantiate(spawners[UnityEngine.Random.Range(0, spawners.Count)], pos, Quaternion.identity).transform;
                CreateEnemies(temp);
            }
        }
    }

    void CreateEnemies(Transform targetSpawn)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(enemyType[0], targetSpawn.position += new Vector3((float)0.01 * i, 0, 0), Quaternion.identity);
            temp.SetActive(false);
            temp.transform.parent = targetSpawn;
            temp.GetComponent<EnemyData>().x = gameObject;
            temp.GetComponent<EnemyData>().Player = player.transform;
        }
    }

    private void FillRegionWithWalls()
    {
        float xLength = curRegion.start.x + curRegion.length;
        float yLength = curRegion.start.y + curRegion.length;
        nextPos = curRegion.start;

        do
        {
            if (Physics2D.OverlapCircle(nextPos, 0.5f) == null && !curRegion.borders.Contains(nextPos)) // Adjust radius as needed
            {
                Instantiate(wallTiles[UnityEngine.Random.Range(0, wallTiles.Count)], nextPos, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90)).transform.parent = wallParent.transform;
                curRegion.unVisited.Add(nextPos);
            }
            nextPos = nextPos.x >= xLength - 1 ? new Vector2(curRegion.start.x, nextPos.y + 1) : new Vector2(nextPos.x + 1, nextPos.y);
        } while (nextPos.y < yLength);
    }

    private bool HasVisitedNeighbor(Vector2 pos, out Vector2 neighbor)
    {
        directions.Shuffle();

        foreach (var dir in directions)
        {
            Vector2 adjacent = pos + dir;
            if (curRegion.visited.Contains(adjacent) && !curRegion.borders.Contains(adjacent))
            {
                neighbor = adjacent;
                return true;
            }
        }
        neighbor = Vector2.zero;
        return false;
    }

    private bool HasUnvisitedNeighbors(Vector2 pos, out Vector2 next)
    {
        try
        {
            directions.Shuffle();
            Vector2 adjacent;

            foreach (Vector2 dir in directions)
            {
                adjacent = pos + dir;
                if (curRegion.unVisited.Contains(adjacent) && !curRegion.visited.Contains(adjacent) && !curRegion.borders.Contains(adjacent))
                {
                    next = adjacent;
                    return true;
                }
            }
            next = Vector2.zero;
            return false;
        }
        catch (Exception err)
        {
            Debug.Log(err);
            next = Vector2.zero;
            return false;
        }

    }

    private bool FindNextStart()
    {
        float minX = curRegion.start.x;
        float minY = curRegion.start.y;
        float maxX = curRegion.start.x + length;
        float maxY = curRegion.start.y + length;
        for (float y = minY + 1; y < maxY - 1; y += 2)
        {
            for (float x = minX + 1; x < maxX - 1; x += 2)
            {
                Vector2 pos = new(x, y);
                if (!curRegion.visited.Contains(pos) && HasVisitedNeighbor(pos, out Vector2 neighbor))
                {
                    Vector2 wall = (pos + neighbor) / 2;
                    curRegion.visited.Add(wall);
                    curRegion.visited.Add(pos);
                    ReplaceWithPathTile(wall);
                    ReplaceWithPathTile(pos);
                    currentPos = pos;
                    return true;
                }
            }
        }
        return false;
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}

/*
using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public List<GameObject> pathTiles;
    public List<GameObject> obstacleTiles;

    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
    private Vector2 currentPos;
    private HashSet<Vector2> visited = new();
    private Vector2 nextPos;

    void Awake()
    {
        nextPos = new(minX, minY);
        FillWorld();
        CarveMaze();
    }


    private void CarveMaze()
    {
        currentPos = new Vector2(minX + 1, minY + 1);
        visited.Add(currentPos);
        ReplaceWithPathTile(currentPos);
        HuntAndKill();
    }

    private void HuntAndKill()
    {
        while (true)
        {
            if (HasUnvisitedNeighbors(currentPos, out Vector2 next))
            {
                Vector2 wall = (currentPos + next) / 2;
                visited.Add(wall);
                visited.Add(next);
                ReplaceWithPathTile(wall);
                ReplaceWithPathTile(next);
                currentPos = next;
            }
            else if (!FindNextStart())
            {
                break;
            }
        }
    }

    private bool FindNextStart()
    {
        for (int y = minY + 1; y < maxY - 1; y += 2)
        {
            for (int x = minX + 1; x < maxX - 1; x += 2)
            {
                Vector2 pos = new(x, y);
                if (!visited.Contains(pos) && HasVisitedNeighbor(pos, out Vector2 neighbor))
                {
                    Vector2 wall = (pos + neighbor) / 2;
                    visited.Add(wall);
                    visited.Add(pos);
                    ReplaceWithPathTile(wall);
                    ReplaceWithPathTile(pos);
                    currentPos = pos;
                    return true;
                }
            }
        }
        return false;
    }

    private bool HasUnvisitedNeighbors(Vector2 pos, out Vector2 next)
    {
        List<Vector2> directions = new()
        {
            new Vector2(0, 2), new Vector2(0, -2), new(2, 0), new Vector2(-2, 0)
        };
        directions.Shuffle();

        foreach (Vector2 dir in directions)
        {
            Vector2 neighbor = pos + dir;
            if (IsInBounds(neighbor) && !visited.Contains(neighbor))
            {
                next = neighbor;
                return true;
            }
        }
        next = Vector2.zero;
        return false;
    }

    private bool HasVisitedNeighbor(Vector2 pos, out Vector2 neighbor)
    {
        List<Vector2> directions = new()
        {
            new Vector2(0, 2), new Vector2(0, -2), new Vector2(2, 0), new Vector2(-2, 0)
        };
        directions.Shuffle();

        foreach (var dir in directions)
        {
            Vector2 adjacent = pos + dir;
            if (visited.Contains(adjacent))
            {
                neighbor = adjacent;
                return true;
            }
        }
        neighbor = Vector2.zero;
        return false;
    }

    private void ReplaceWithPathTile(Vector2 pos)
    {
        if (!IsWithinInnerBounds(pos)) return;
        Collider2D obstacle = Physics2D.OverlapPoint(pos);
        if (obstacle != null) Destroy(obstacle.gameObject);
        Instantiate(pathTiles[UnityEngine.Random.Range(0, pathTiles.Count)], new Vector2(pos.x, pos.y), Quaternion.identity).transform.parent = transform;
    }

    private bool IsInBounds(Vector2 pos)
    {
        return pos.x > minX && pos.x < maxX - 1 && pos.y > minY && pos.y < maxY - 1;
    }

    private bool IsWithinInnerBounds(Vector2 pos)
    {
        return pos.x > minX && pos.x < maxX - 1 && pos.y > minY && pos.y < maxY - 1;
    }

    private void FillWorld()
    {
        do
        {
            Instantiate(obstacleTiles[UnityEngine.Random.Range(0, obstacleTiles.Count)], nextPos, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90)).transform.parent = transform;
            nextPos = nextPos.x >= maxX - 1 ? new Vector2(0, nextPos.y + 1) : new Vector2(nextPos.x + 1, nextPos.y);
        } while (nextPos.y < maxY);
    }


}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
*/