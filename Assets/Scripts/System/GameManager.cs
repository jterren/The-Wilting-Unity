using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return null;
            }
#endif
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (Tools.FindGameObjectByName("GameManager")) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {
    }

    public PlayerStats Player { get; set; }

    public SceneData Scene { get; set; }

    public SceneLoader SceneLoader { get; set; }

    public string SelectedSave { get; set; }

    public List<Maze> Mazes { get; set; }

    public WorldSpace WorldSpace { get; set; }
}
