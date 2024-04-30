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


public class SceneNames
{
    public const string MainScene = "Main";
    public const string MenuScene = "Menu";
    public const string NetBootstrap = "NetBootstrap";
    public const string GameScene = "Game";
}


public class ClientGameManager
{
    private JoinAllocation _allocation;

    public async Task<bool> InitAsync()
    {
        //플레이어 인증 부분 들어갈 예정.
        await UnityServices.InitializeAsync();

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

            NetworkManager.Singleton.StartClient();


        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

    }

}

