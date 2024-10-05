using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Input.IBotActions, Input.IFlowActions
{
    public event Action<Vector2> MoveEvent;
    public event Action SelectEvent;
    public event Action PauseEvent;
    private Input _input;

    public void Init()
    {
    }

    private void Start()
    {
        TimeKeeper.Instance.PauseEvent += TimeKeeper_PauseEvent;
    }

    private void TimeKeeper_PauseEvent(bool isPaused)
    {
        if (isPaused)
        {
            _input.Bot.Disable();
            //UnlockCursor();
        }
        else
        {
            _input.Bot.Enable();
            //LockCursor();
        }
    }

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new Input();

            _input.Bot.AddCallbacks(this);
            _input.Flow.AddCallbacks(this);
        }
        _input.Bot.Enable();
        _input.Flow.Enable();
        //LockCursor();
    }

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void DisableAllInput()
    {
       // _input.Bot.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SelectEvent?.Invoke();
        }
    }
}
