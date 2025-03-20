using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Tools : MonoBehaviour
{
    public static List<GameObject> FindInactiveGameObjectsByTag(string tag)
    {
        List<GameObject> inactiveObjects = new List<GameObject>();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && !obj.activeInHierarchy)
            {
                inactiveObjects.Add(obj);
            }
        }

        return inactiveObjects;
    }

    public static List<GameObject> GetAllChildrenByTag(Transform parent, string tag)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag(tag) && child != parent) // Exclude parent itself
            {
                children.Add(child.gameObject);
            }
        }

        return children;
    }
    public static Vector3 RandomizeChildPosition(float minX, float minY, float maxX, float maxY)
    {
        return new Vector3(
            Random.Range(minX, maxX - 1),
            Random.Range(minY, maxY - 1),
            0
        );
    }

    /*This functions goal is to create an abstract spacer (circle) around a parent and then create a random spawn location 
       outside the spacer out to the provided radius away.
    */
    public static Vector3 RandomizeChildWithRadius(Transform parent, float radius)
    {
        float spacer = GetMaxObjRadius(parent);
        radius += spacer;
        float randomDistance = Random.Range(spacer, radius);
        float angle = Random.Range(0f, Mathf.PI * 2);
        float xOffset = Mathf.Cos(angle) * randomDistance;
        float yOffset = Mathf.Sin(angle) * randomDistance;
        Vector3 spawn = parent.position + new Vector3(xOffset, yOffset, 0);
        Bounds bounds = GameManager.Instance.WorldSpace.worldBounds;
        spawn.x = Mathf.Clamp(spawn.x, bounds.min.x + 10, bounds.max.x - 10);
        spawn.y = Mathf.Clamp(spawn.y, bounds.min.y + 10, bounds.max.y - 10);

        return spawn;
    }

    private static float GetMaxObjRadius(Transform obj)
    {
        Collider collider = obj.GetComponentInChildren<Collider>();
        if (collider != null)
        {
            return collider.bounds.extents.magnitude;
        }

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.extents.magnitude;
        }
        Debug.Log("Bounds not found.");
        return 10f;
    }

    public static bool IsObjectInRegion(Vector2 objectPos, CreateMap.Region reg)
    {
        return objectPos.x >= reg.start.x && objectPos.x < (reg.start.x + reg.length) &&
               objectPos.y >= reg.start.y && objectPos.y < (reg.start.y + reg.length);
    }

    public static void CompleteGame()
    {
        SceneManager.LoadScene(3);
    }
}
