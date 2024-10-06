using UnityEngine;

public class Scene : MonoBehaviour
{
    [HideInInspector] public AudioManager AudioManager;

    [HideInInspector] public Player Player;
    [HideInInspector] public InputManager InputManager;

    [HideInInspector] public BotManager BotManager;

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

        BotManager = GetComponentInChildren<BotManager>();
        BotManager.Init(this);

        Player = GetComponentInChildren<Player>();
        Player.Init(this);
    }

}
