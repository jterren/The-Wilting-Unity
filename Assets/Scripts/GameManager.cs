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
            if (instance == null)
            {
                Instantiate(Resources.Load<GameManager>("GameManager"));
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
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            SaveSystem.Resume(); //Will result in error can't convert game objects to binary
        }
        if (Input.GetKey(KeyCode.M))
        {
            SaveSystem.Save(); //Will result in error can't convert game objects to binary
        }
    }

    public PlayerStats Player { get; set; }

    public SceneData Scene { get; set; }

    public SceneLoader SceneLoader { get; set; }
}
