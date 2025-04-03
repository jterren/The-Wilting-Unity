using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Runtime.Serialization.Formatters;

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
    public static GameObject FindGameObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }

        return null; ;
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

    public static List<GameObject> GetAllChildren(Transform parent)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child != parent) // Exclude parent itself
            {
                children.Add(child.gameObject);
            }
        }

        return children;
    }

    public static GameObject GetChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }
    public static Vector3 RandomizeChildPosition(float minX, float minY, float maxX, float maxY)
    {
        return new Vector3(
            UnityEngine.Random.Range(minX, maxX - 1),
            UnityEngine.Random.Range(minY, maxY - 1),
            0
        );
    }

    /*This functions goal is to create an abstract spacer (circle) around a parent and then create a random spawn location 
       outside the spacer out to the provided radius away.
    */
    // public static Vector3 RandomizeChildWithRadius(Transform parent, float radius)
    // {
    //     float spacer = GetMaxObjRadius(parent);
    //     radius += spacer;
    //     float randomDistance = Random.Range(spacer, radius);
    //     float angle = Random.Range(0f, Mathf.PI * 2);
    //     float xOffset = Mathf.Cos(angle) * randomDistance;
    //     float yOffset = Mathf.Sin(angle) * randomDistance;
    //     Vector3 spawn = parent.position + new Vector3(xOffset, yOffset, 0);
    //     Bounds bounds = GameManager.Instance.WorldSpace.worldBounds;
    //     spawn.x = Mathf.Clamp(spawn.x, bounds.min.x + 10, bounds.max.x - 10);
    //     spawn.y = Mathf.Clamp(spawn.y, bounds.min.y + 10, bounds.max.y - 10);

    //     return spawn;
    // }

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

    public static bool IsObjectInMaze(Vector2 objectPos, Maze reg)
    {
        return objectPos.x >= reg.start.x && objectPos.x < (reg.start.x + reg.length) &&
               objectPos.y >= reg.start.y && objectPos.y < (reg.start.y + reg.length);
    }

    public static void CompleteGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static List<GameObject> GetAllObjectsInScene()
    {
        List<GameObject> objectsInScene = new();

        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            objectsInScene.Add(go);
        }

        return objectsInScene;
    }

    public static async Task<Dictionary<string, GameObject>> LoadPrefabsAsync(List<string> prefabNames)
    {
        var initHandle = Addressables.InitializeAsync();
        await initHandle.Task;
        Dictionary<string, GameObject> loadedPrefabs = new();

        foreach (var prefabName in prefabNames)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(prefabName);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedPrefabs[prefabName] = handle.Result;
            }
            else
            {
                Debug.LogWarning($"Failed to load prefab: {prefabName}");
            }
        }

        return loadedPrefabs;
    }

    public static bool CollidingSpawnByTag(Vector2 pos, List<string> conflictingTag)
    {
        foreach (var tag in conflictingTag)
        {
            if (Physics2D.OverlapPoint(pos).CompareTag(tag)) return true;
        }
        return false;
    }

    public static bool CollidingSpawnByLayer(Vector2 pos, List<string> conflictingLayers)
    {
        Collider2D collider = Physics2D.OverlapPoint(pos);
        if (collider == null) return false;

        int colliderLayer = collider.gameObject.layer;

        foreach (var layerName in conflictingLayers)
        {
            int targetLayer = LayerMask.NameToLayer(layerName);
            if (colliderLayer == targetLayer)
            {
                return true;
            }
        }

        return false;
    }

    public static void FinishLoading()
    {
        GameObject player = FindGameObjectByName("Player");
        GameObject loading = FindGameObjectByName("Loading");
        GameObject background = FindGameObjectByName("MazeBackground");
        FindInactiveGameObjectsByTag("UI").ForEach(obj => { if (!obj.activeInHierarchy) obj.SetActive(true); });
        if (loading != null && loading.activeInHierarchy) loading.SetActive(false);
        if (background != null && background.activeInHierarchy) background.SetActive(false);
        if (player != null && !player.activeInHierarchy) player.SetActive(true);
    }
}
