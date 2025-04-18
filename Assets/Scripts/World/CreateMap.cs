using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateMap : MonoBehaviour
{
    private List<Func<Action>> algos = new();
    public List<GameObject> groundTiles;
    public List<GameObject> wallTiles;
    public GameObject groundParent;
    public GameObject wallParent;
    public GameObject spawnerParent;
    public int length;
    [SerializeField]
    private int borderWidth = 2;
    private Vector2 nextPos;
    private List<Maze> mazes = new();
    private Vector2 currentPos;
    private Maze curMaze;
    List<Vector2> directions = new()
        {
            new(0, 2), new(0, -2), new(2, 0), new(-2, 0)
        };

    private GameObject player;
    public List<GameObject> finishObjects;
    public List<GameObject> spawners;
    [SerializeField]
    private List<string> wallConflicts = new() { "Player", "Obstacle" };
    private LayerMask obstacleLayer;
    void Awake()
    {
        player = Tools.FindGameObjectByName("Player");
        algos.Add(() => HuntAndKill);
        nextPos = new(-borderWidth, -borderWidth);
        obstacleLayer = LayerMask.GetMask("Obstacle");
    }

    void Start()
    {
        if (GameManager.Instance.WorldSpace.world.gameObjects.Count == 0)
        {
            GenerateMazeBorders();
            CreateMazes();
            Tools.FinishLoading();
        }
    }
    private void GenerateMazeBorders()
    {

        for (int y = -borderWidth; y < 0; y++) //Generate x borders
        {
            CreateHorizontalWall(-borderWidth, y, length + borderWidth, false);
            CreateHorizontalWall(-borderWidth, length - y, length + borderWidth, false);
        }
        for (int x = -borderWidth; x < 0; x++) //Genereate y borders
        {
            CreateVerticalWall(x, -borderWidth, length + borderWidth, false);
            CreateVerticalWall(length - x, -borderWidth, length + borderWidth, false);
        }
    }

    private void CreateMazes()
    {
        int half = length / 2;
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                mazes.Add(new Maze(new Vector2(x * half, y * half), half, algos[UnityEngine.Random.Range(0, algos.Count)], finishObjects[UnityEngine.Random.Range(0, finishObjects.Count)]));
            }
        }
        FillMazes();
    }

    private void FillMazes()
    {
        foreach (Maze current in mazes)
        {
            curMaze = current;
            CreateHorizontalWall(current.start.x, (int)current.start.y + current.length, (int)current.start.x + current.length, true);
            CreateVerticalWall((int)current.start.x + current.length, current.start.y, (int)current.start.y + current.length, true);
            FillRegionWithWalls();
            if (current.start == Vector2.zero) curMaze.algorithm().Invoke();
        }
    }

    private void CreateHorizontalWall(float startX, float y, int length, bool permanant)
    {
        GameObject temp;

        for (float x = startX; x < length; x++)
        {
            Vector2 pos = new(x, y);
            if (!Tools.CollidingSpawnByLayer(pos, wallConflicts))
            {
                GameObject prefab = wallTiles[UnityEngine.Random.Range(0, wallTiles.Count)];
                temp = Instantiate(prefab, pos, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));
                temp.transform.parent = wallParent.transform;
                Tools.CaptureObject(temp, prefab.name);
                if (permanant) curMaze.borders.Add(temp.transform.position);
            }
        }
    }
    private void CreateVerticalWall(float x, float startY, int length, bool permanant)
    {
        GameObject temp;
        for (float y = startY; y <= length; y++)
        {
            Vector2 pos = new Vector2(x, y);
            if (!Tools.CollidingSpawnByLayer(pos, wallConflicts))
            {
                GameObject prefab = wallTiles[UnityEngine.Random.Range(0, wallTiles.Count)];
                temp = Instantiate(prefab, pos, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));
                temp.transform.parent = wallParent.transform;
                Tools.CaptureObject(temp, prefab.name);
                if (permanant) curMaze.borders.Add(temp.transform.position);
            }
        }
    }

    private void HuntAndKill()
    {
        if (Tools.IsObjectInMaze(player.transform.position, curMaze))
        {
            currentPos = player.transform.position;
        }
        else
        {
            currentPos = curMaze.start;
        }

        while (true)
        {
            if (HasUnvisitedNeighbors(currentPos, out Vector2 next))
            {
                Vector2 wall = (currentPos + next) / 2;
                curMaze.visited.Add(wall);
                curMaze.visited.Add(next);
                curMaze.unVisited.Remove(wall);
                curMaze.unVisited.Remove(next);
                ReplaceWithPathTile(wall);
                ReplaceWithPathTile(next);
                currentPos = next;
            }
            else if (!FindNextStart() || curMaze.unVisited.Count == 0)
            {
                if (curMaze.finish == Vector2.zero)
                {
                    curMaze.finish = curMaze.visited.ElementAt(UnityEngine.Random.Range(0, curMaze.visited.Count));
                    GameObject prefab = finishObjects[UnityEngine.Random.Range(0, finishObjects.Count)];
                    GameObject temp = Instantiate(prefab, curMaze.finish, Quaternion.identity);
                    temp.transform.parent = transform;
                    Tools.CaptureObject(temp, prefab.name);
                }
                break;
            }
        }
    }

    private void ReplaceWithPathTile(Vector2 pos)
    {
        try
        {
            Collider2D obstacle = Physics2D.OverlapPoint(pos, obstacleLayer);
            if (obstacle != null && obstacle.CompareTag("Obstacle") && !curMaze.borders.Contains(obstacle.transform.position))
            {
                int index = GameManager.Instance.WorldSpace.world.gameObjects.First(x => x.addressableKey == obstacle.name.Replace("(Clone)", "")).instances.FindIndex(go =>
                    Mathf.Approximately(go.x, obstacle.transform.position.x) &&
                    Mathf.Approximately(go.y, obstacle.transform.position.y));
                if (index != -1)
                {
                    GameManager.Instance.WorldSpace.world.gameObjects.First(x => x.addressableKey == obstacle.name.Replace("(Clone)", "")).instances.RemoveAt(index);
                    GameObject prefab = groundTiles[UnityEngine.Random.Range(0, groundTiles.Count)];
                    GameObject temp = Instantiate(prefab, obstacle.transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));
                    temp.transform.parent = groundParent.transform;
                    Tools.CaptureObject(temp, prefab.name);
                    Destroy(obstacle.gameObject);
                }

                if (UnityEngine.Random.Range(0, 100) % 10 == 0)
                {
                    GameObject prefab = spawners[UnityEngine.Random.Range(0, spawners.Count)];
                    GameObject temp = Instantiate(prefab, pos, Quaternion.identity);
                    temp.transform.parent = spawnerParent.transform;
                    Tools.CaptureObject(temp, prefab.name);
                }
            }
        }
        catch (Exception err)
        {
            Debug.LogError(err);
        }

    }

    private void FillRegionWithWalls()
    {
        float xLength = curMaze.start.x + curMaze.length;
        float yLength = curMaze.start.y + curMaze.length;
        nextPos = curMaze.start;

        do
        {
            if (Physics2D.OverlapCircle(nextPos, 0.25f) == null && !curMaze.borders.Contains(nextPos)) // Adjust radius as needed
            {
                GameObject prefab = wallTiles[UnityEngine.Random.Range(0, wallTiles.Count)];
                GameObject temp = Instantiate(prefab, nextPos, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));
                temp.transform.parent = wallParent.transform;
                Tools.CaptureObject(temp, prefab.name);
                curMaze.unVisited.Add(nextPos);
            }
            nextPos = nextPos.x >= xLength - 1 ? new Vector2(curMaze.start.x, nextPos.y + 1) : new Vector2(nextPos.x + 1, nextPos.y);
        } while (nextPos.y < yLength);
    }

    private bool HasVisitedNeighbor(Vector2 pos, out Vector2 neighbor)
    {
        directions.Shuffle();

        foreach (var dir in directions)
        {
            Vector2 adjacent = pos + dir;
            if (curMaze.visited.Contains(adjacent) && !curMaze.borders.Contains(adjacent))
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
                if (curMaze.unVisited.Contains(adjacent) && !curMaze.visited.Contains(adjacent) && !curMaze.borders.Contains(adjacent))
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
            Debug.LogError(err);
            next = Vector2.zero;
            return false;
        }

    }

    private bool FindNextStart()
    {
        float minX = curMaze.start.x;
        float minY = curMaze.start.y;
        float maxX = curMaze.start.x + length;
        float maxY = curMaze.start.y + length;
        for (float y = minY; y < maxY; y += 2)
        {
            for (float x = minX; x < maxX; x += 2)
            {
                Vector2 pos = new(x, y);
                if (!curMaze.visited.Contains(pos) && HasVisitedNeighbor(pos, out Vector2 neighbor))
                {
                    Vector2 wall = (pos + neighbor) / 2;
                    curMaze.visited.Add(wall);
                    curMaze.visited.Add(pos);
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

[Serializable]
public struct Maze
{
    public Vector2 start;
    public int length;

    public Func<Action> algorithm;

    public HashSet<Vector2> visited;
    public HashSet<Vector2> unVisited;
    public HashSet<Vector2> borders;
    public Vector2 finish;
    public GameObject finishObj;
    public Maze(Vector2 pos, int len, Func<Action> algo, GameObject obj)
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
}