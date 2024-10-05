using System;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    public event Action<bool> PauseEvent;

    public static TimeKeeper Instance {get; private set; }

    public InputManager InputManager;

    public float PlayTimeElapsed { get; private set; }
    public bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A second timekeeper has awakened.");
        }
        else
        {
            Instance = this;
        }

        InputManager.PauseEvent += InputManager_PauseEvent;
    }

    private void InputManager_PauseEvent()
    {
        if (IsPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPaused)
        {
            PlayTimeElapsed += Time.deltaTime;
        }
    }

    public void Pause()
    {
        IsPaused = true;
        PauseEvent?.Invoke(IsPaused);
    }
    public void Unpause()
    {
        IsPaused = false;
        PauseEvent?.Invoke(IsPaused);
    }

    public float PlayTimeSince(float since) {
        return PlayTimeElapsed - since;
    }
}
