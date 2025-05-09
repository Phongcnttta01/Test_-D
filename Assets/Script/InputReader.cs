
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader", fileName = "InputReader")]

public class InputReader : ScriptableObject , PlayerInput.IPlayerActions
{
     public PlayerInput input;

     public UnityAction<bool> Jump;
    
    public Vector2 Direction => input.Player.Move.ReadValue<Vector2>();

    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInput();
            input.Player.SetCallbacks(this);
        }
    }

    public void Enable()
    {
        input.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump?.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Jump?.Invoke(false);
                break;
        }
    }
}
