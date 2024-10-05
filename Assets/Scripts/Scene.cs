using UnityEngine;

public class Scene : MonoBehaviour
{
    [HideInInspector] public AudioManager AudioManager;

    [HideInInspector] public Player Player;
    [HideInInspector] public InputManager InputManager;


    void Awake()
    {
        AudioManager = GetComponentInChildren<AudioManager>();
        AudioManager.Init();

        InputManager = GetComponentInChildren<InputManager>();
        InputManager.Init();

        Player = GetComponentInChildren<Player>();
        Player.Init(this);
    }

}
