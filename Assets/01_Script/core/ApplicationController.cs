using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{

    [SerializeField] private ClientSingleton _clientPrefab;
    [SerializeField] private HostSingleton _hostPrefab;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //���� ��������Ʈ ������ ����� ���� ����
        LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async void LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            //do somethin in later
        }
        else
        {
            HostSingleton hostSingleton = Instantiate(_hostPrefab); //�����ٲٸ� �ȵ�
            hostSingleton.CreateHost();

            ClientSingleton clientSingleton = Instantiate(_clientPrefab);
            await clientSingleton.CreateClient();


            ClientSingleton.Instance.GameManager.GotoMenu();
            bool authenticated = await clientSingleton.CreateClient();

            if (authenticated)
            {
                //���� �̰��� ���� �ε��κ��� ���� �Ѵ�.
                Debug.Log("Load");
                ClientSingleton.Instance.GameManager.GotoMenu();
            }
            else
            {
                Debug.LogError("UGS Service login failed");


            }
        }
    }
}
