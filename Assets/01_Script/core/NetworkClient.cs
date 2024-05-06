using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkClient : IDisposable
{
    private NetworkManager _networkManager;

    public NetworkClient(NetworkManager networkManager)
    {
        _networkManager = networkManager;
        _networkManager.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void HandleClientDisconnect(ulong clientID)
    {
        //������ ������ �ƴϰ� �ڱⰡ �����͵� �ƴ϶�� �� ����
        if (clientID != 0 && clientID != _networkManager.LocalClientId) return;

        Disconnect();
    }

    public void Disconnect()
    {
        //���� �̵������ְ� ���� ���ӵǾ� �ִٸ� ������ �����ش�.
        if (SceneManager.GetActiveScene().name != SceneNames.MenuScene)
        {
            SceneManager.LoadScene(SceneNames.MenuScene);
        }

        if (_networkManager.IsConnectedClient)
        {
            _networkManager.Shutdown();
        }
    }

    public void Dispose()
    {
        if (_networkManager != null)
        {
            _networkManager.OnClientDisconnectCallback -= HandleClientDisconnect;
        }
    }

}

