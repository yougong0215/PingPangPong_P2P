using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public enum Team
{
    RED = 0,
    BLUE,
}

public class PlayerBinder : NetworkBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] BallMove _balls;
    private Vector2 _prevMovementInput;
    [SerializeField] private float _movementSpeed = 5f;
    Team _myEnum;

    void Awake()
    {
        transform.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0) * 8 * Time.deltaTime;

        //_inputReader.
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;

        _inputReader.MovementEvent += HandleMovement;

        if (IsHost)
        {
            transform.position = new Vector3(-8, 0, 0);
            _myEnum = Team.RED;
        }
        else
        {
            transform.position = new Vector3(8, 0, 0);
            _myEnum = Team.BLUE;
        }

    }

    public Team GetTeam()
    {
        return _myEnum;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.MovementEvent -= HandleMovement;
    }


    private void HandleMovement(Vector2 movementInput)
    {
        _prevMovementInput = movementInput;
    }

    private void Update()
    {
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectileInstance = Instantiate(_balls.gameObject, Vector3.zero, Quaternion.identity);
            projectileInstance.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        transform.position += new Vector3(0,_prevMovementInput.y,0) * _movementSpeed *Time.deltaTime;
    }


}
