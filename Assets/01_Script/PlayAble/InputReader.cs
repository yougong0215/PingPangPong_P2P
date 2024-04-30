using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;
using static UnityEngine.InputSystem.DefaultInputActions;
[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MovementEvent;

    private PlayerInput _controlAction;
    private void OnEnable()
    {
        if (_controlAction == null)
        {
            _controlAction = new PlayerInput();
            _controlAction.Player.SetCallbacks(this); //플레이어 인풋이 발생하면 이 인스턴스를 연결해주고
        }

        _controlAction.Player.Enable(); //플레이어 입력 활성화
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>(); //이부분은 과제로 주라.
        MovementEvent?.Invoke(value);
    }

}
