using UnityEngine;
using System.Collections.Generic;

public class Tools
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
    public static Vector3 RandomizeChildWithRadius(Transform parent, float spacer, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float randomDistance = Random.Range(spacer, radius);

        float xOffset = Mathf.Cos(angle) * randomDistance;
        float yOffset = Mathf.Sin(angle) * randomDistance;

        return parent.position + new Vector3(xOffset, yOffset, 0);
    }
}
