using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientTransform : NetworkTransform
{
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner; //�����ڴ� ������ �����ϰ�
    }


    protected override bool OnIsServerAuthoritative()
    {
        //���������� false�� ����
        return false;
    }


    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();

        //�̰� NetworkTransform�ȿ� �ִ� �Ŵ�. ��Ȯ���� NetworkBehaviour��� �� �ؿ�
        //��� NetworkObject�� ���� NetworkManager�� �������°� (�̰� ��¥�� ��Ʈ��ũ �̱�����)
        //�����θ� �о�� ���� �������� ��Ʈ��ũ �Ŵ��� �����ÿ� �ش� ������Ʈ�� ������ ��Ʈ��ũ �Ŵ����� �����ϵ��� ���� ����
        if (NetworkManager != null)
        {
            //Ŭ���̾�Ʈ�μ� ����Ǿ��ְų� �����μ� �������� �ϰ� �ִ� ���̶�� 
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if (CanCommitToTransform)
                {
                    //�׷� ������ �ڽ��� transform�� ����ð����� ����ȭ�� �õ��Ѵ�. 
                    //�ð��� �ָ� interpolate�� �����ϴ�.

                
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }

}
