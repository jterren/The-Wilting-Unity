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
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {

    }

    public PlayerStats Player { get; set; }

    public Rounds Rounds { get; set; }

    public int RoundCounter { get; set; } = 1;

    public SceneData Scene { get; set; }

    public SceneLoader SceneLoader { get; set; }

    public string SelectedSave { get; set; }
}
