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
    private Vector2Int currentPos;
    private HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    private System.Random rng = new System.Random();
    private Vector2 nextPos;

    void Awake()
    {
        FillWorld();
        CarveMaze();
    }


    private void CarveMaze()
    {
        currentPos = new Vector2Int(minX + 1, minY + 1);
        visited.Add(currentPos);
        ReplaceWithPathTile(currentPos);
        HuntAndKill();
    }

    private void HuntAndKill()
    {
        while (true)
        {
            if (HasUnvisitedNeighbors(currentPos, out Vector2Int next))
            {
                Vector2Int wall = (currentPos + next) / 2;
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
                Vector2Int pos = new Vector2Int(x, y);
                if (!visited.Contains(pos) && HasVisitedNeighbor(pos, out Vector2Int neighbor))
                {
                    Vector2Int wall = (pos + neighbor) / 2;
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

    private bool HasUnvisitedNeighbors(Vector2Int pos, out Vector2Int next)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 2), new Vector2Int(0, -2), new Vector2Int(2, 0), new Vector2Int(-2, 0)
        };
        directions.Shuffle();

        foreach (var dir in directions)
        {
            Vector2Int neighbor = pos + dir;
            if (IsInBounds(neighbor) && !visited.Contains(neighbor))
            {
                next = neighbor;
                return true;
            }
        }
        next = Vector2Int.zero;
        return false;
    }

    private bool HasVisitedNeighbor(Vector2Int pos, out Vector2Int neighbor)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 2), new Vector2Int(0, -2), new Vector2Int(2, 0), new Vector2Int(-2, 0)
        };
        directions.Shuffle();

        foreach (var dir in directions)
        {
            Vector2Int adjacent = pos + dir;
            if (visited.Contains(adjacent))
            {
                neighbor = adjacent;
                return true;
            }
        }
        neighbor = Vector2Int.zero;
        return false;
    }

    private void ReplaceWithPathTile(Vector2Int pos)
    {
        if (!IsWithinInnerBounds(pos)) return;
        Collider2D obstacle = Physics2D.OverlapPoint(pos);
        if (obstacle != null) Destroy(obstacle.gameObject);
        Instantiate(pathTiles[Random.Range(0, pathTiles.Count)], new Vector2(pos.x, pos.y), Quaternion.identity);
    }

    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x > minX && pos.x < maxX - 1 && pos.y > minY && pos.y < maxY - 1;
    }

    private bool IsWithinInnerBounds(Vector2Int pos)
    {
        return pos.x > minX && pos.x < maxX - 1 && pos.y > minY && pos.y < maxY - 1;
    }

    private void FillWorld(int x, int y)
    {
        if (y >= maxY) return;
        if (x >= maxX)
        {
            FillWorld(minX, y + 1);
            return;
        }

        Instantiate(obstacleTiles[Random.Range(0, obstacleTiles.Count)], new Vector2(x, y), Quaternion.identity);
        FillWorld(x + 1, y);
    }

    private void FillWorld()
    {
        nextPos = new(minX, minY);
        do
        {
            Instantiate(obstacleTiles[UnityEngine.Random.Range(0, obstacleTiles.Count)], nextPos, Quaternion.identity);
            nextPos = nextPos.x >= maxX - 1 ? new Vector2(0, nextPos.y + 1) : new Vector2(nextPos.x + 1, nextPos.y);
        } while (nextPos.y < maxY);
    }


}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
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
TODO: Random pathing
Randoms:
   Scaled by difficulty:
    - Number of intersections
    - Number of dead ends per intersection
    - Step distance
        - This will be the tougher calculation due to crowding in smaller areas however guidelines are:
            - Loop
                - Determine direction first, scale immediate desire based on difficulty (higher difficulties yearn for misdirection unless)
                    - Check buffer reached for all directions
                        - Create list of available next directions
                    - If difficultyRandom, go towards goal.
                        - If creates long sightline or clearing, and max of either reached, attempt other benificial direction first or use unoccupied space with buffer wall to navigate around by creating a step in best available direction length of buffer+1. 
                        - Else choose from available beneficial directions.
                    - If misdirection 
                        - subtract Range(0,10)*difficulty.ceiling from difficulty
                    - Else
                        - If no beneficial routes 
                = Determine step next
                    - When x/y coordinates are at extreme high/low high chance of longer distances, 
                    - When approaching median values decrease max step distance by relative to the numeric distance to extreme 
                        - Scaled by difficulty and space.
                            - Space determined by distance in any direction to next path

To avoid generating clearings by accident
If cardinal neighbors in next tile are empty then diagnol neighbors must be obstacles otherwise avoid move, + intersections must remain 1 tile wide along paths.

I could create directional stamps that would be able to dynamically adjust sightline distances to avoid apparent consistencies. As difficult increases likelihood of stamps decreases, predictability decreases with difficulty growth?

Create resuasble function that scales randomness based on difficulty, to be used to govern rarity in random event triggers. heavier weight allows micromanagement if desired: Random (0,100) - weight > difficulty
*/