using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BallMove : NetworkBehaviour
{
    //[Header("참조 변수들")]
    [Header("셋팅값들")]
    [SerializeField] private float _projectileSpeed;

    [Header("Expose_Value")]
    [SerializeField] Vector2 _dir;
    BoxCollider2D _boxcols;
    Rigidbody2D _rigid;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _boxcols = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();

        transform.position = Vector3.zero;
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        _dir = new Vector3(x, y, 0);

        _rigid.velocity = _dir.normalized * 3f;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsOwner) return;

       

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dir.x *= -1;
            if (collision.gameObject.TryGetComponent<PlayerBinder>(out PlayerBinder bind))
            {
                if(bind.GetTeam() == Team.RED) 
                    CollisionBall(bind);
                else 
                    CollisionBall(bind);
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _dir.y *= -1;
            StartCoroutine(hitBallCo());
        }
    }

    void CollisionBall(PlayerBinder bind)
    {
        StartCoroutine(hitBallCo());
    }

    IEnumerator hitBallCo()
    {
        _rigid.velocity = _dir.normalized * 3f; // 나중에 Refresh 해줘야됨
        _boxcols.isTrigger = true;
        yield return null;
        _boxcols.isTrigger = false;
    }

}
