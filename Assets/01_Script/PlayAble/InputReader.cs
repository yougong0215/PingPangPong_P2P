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
            _controlAction.Player.SetCallbacks(this); //�÷��̾� ��ǲ�� �߻��ϸ� �� �ν��Ͻ��� �������ְ�
        }

        _controlAction.Player.Enable(); //�÷��̾� �Է� Ȱ��ȭ
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>(); //�̺κ��� ������ �ֶ�.
        MovementEvent?.Invoke(value);
    }

}
