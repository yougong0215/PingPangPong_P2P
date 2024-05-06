using System;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using Unity.Services.Authentication;


public class SceneNames
{
    public const string MainScene = "Main";
    public const string MenuScene = "Menu";
    public const string NetBootstrap = "NetBootstrap";
    public const string ReadyLobbyScene = "ReadyLobby";
    public const string GameScene = "Game";
}


public class ClientGameManager : IDisposable
{
    private JoinAllocation _allocation;

    private string _playerName;
    public string PlayerName => _playerName;

    private NetworkClient _networkClient;

    public async Task<bool> InitAsync()
    {
        //�÷��̾� ���� �κ� �� ����.
        await UnityServices.InitializeAsync();

        _networkClient = new NetworkClient(NetworkManager.Singleton);

        AuthState authState = await UGSAuthWrapper.DoAuth();

        if (authState == AuthState.Authenticated)
        {
            return true;
        }
        return false;

    }

    public void GotoMenu()
    {
        SceneManager.LoadScene(SceneNames.MenuScene);
    }

    public async Task StartClientWithJoinCode(string joinCode)
    {
        try
        {
            _allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            var relayServerData = new RelayServerData(_allocation, "dtls");
            transport.SetRelayServerData(relayServerData);

            UserData userData = new UserData()
            {
                username = _playerName,
                userAuthId = AuthenticationService.Instance.PlayerId
            };

            string json = JsonUtility.ToJson(userData);
            byte[] payload = Encoding.UTF8.GetBytes(json);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payload;

            NetworkManager.Singleton.StartClient();

        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

    }



    public void SetPlayerName(string playerName)
    {
        _playerName = playerName;
    }


    public void Dispose()
    {
        _networkClient?.Dispose();
    }

    public void Disconnect()
    {
        _networkClient.Disconnect();
    }

}

