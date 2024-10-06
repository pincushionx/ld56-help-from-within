using UnityEngine;

public class Scene : MonoBehaviour
{
    [HideInInspector] public AudioManager AudioManager;

    [HideInInspector] public Player Player;
    [HideInInspector] public InputManager InputManager;

    [HideInInspector] public BotManager BotManager;

    [HideInInspector] public TumorManager TumorManager;

    [HideInInspector] public PauseMenu PauseMenu;

    //public static Scene Instance;

    private void Awake()
    {
        //if (Instance != null)
        //{
        //    Debug.LogError("A second scene has awakened.");
        //}
        //else
        //{
        //    Instance = this;
        //}

        

        AudioManager = GetComponentInChildren<AudioManager>();
        AudioManager.Init();

        InputManager = GetComponentInChildren<InputManager>();
        InputManager.Init();

        TumorManager = GetComponentInChildren<TumorManager>();
        TumorManager.Init(this);

        BotManager = GetComponentInChildren<BotManager>();
        BotManager.Init(this);

        Player = GetComponentInChildren<Player>();
        Player.Init(this);


        TumorManager.CureEvent += TumorManager_CureEvent;


        PauseMenu = GetComponentInChildren<PauseMenu>(true);
        TimeKeeper.Instance.PauseEvent += TimeKeeper_PauseEvent;
        PauseMenu.gameObject.SetActive(false);


    }

    private void TimeKeeper_PauseEvent(bool isPaused)
    {
        PauseMenu.gameObject.SetActive(isPaused);
    }

    private void TumorManager_CureEvent()
    {
        if (TumorManager.NumTumorsRemaining() == 0)
        {
            Debug.Log("Level complete");

            if (GameManager.Instance.HasNextLevel())
            {
                GameManager.LevelData level = GameManager.Instance.GetNextLevel();
                GameManager.Instance.ShowLevel(level.SceneId);
            }
            else
            {
                // Game over
                Debug.Log("Done all levels");
                GameManager.Instance.ShowStartScreen();
            }

        }
    }
}
