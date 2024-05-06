using System;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections;
using System.Text;
using Unity.Services.Authentication;

public class HostGameManager : IDisposable
{
    //����Ƽ ������ ������ �� Ŭ���̾�Ʈ�� ���� ������ �Ҵ����ִ� ����
    private Allocation _allocation;
    private string _joinCode;
    private string _lobbyId;
    private const int _maxConnections = 20;


    private NetworkServer _networkServer;

    private void MakeNetworkServer()
    {
        _networkServer = new NetworkServer(NetworkManager.Singleton);
    }

    public async Task StartHostAsync()
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(_maxConnections);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        try
        {
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            Debug.Log(_joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }


        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        var relayServerData = new RelayServerData(_allocation, "dtls"); //udp���� ������ ���� ����
        transport.SetRelayServerData(relayServerData);


        string playerName = ClientSingleton.Instance.GameManager.PlayerName;
        //���⼭ �κ������� �޾ƿ´�.
        try
        {
            //�κ� ����� ���� �ɼǵ��� �ִ´�.
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false; //�κ� �ɼ��� ���� �־���� �Ѵ�. ���� �̰� true�� �ϸ� �����ڵ�θ� ���� ����

            // �ش� �κ� �ɼǿ� Join�ڵ带 �־��ش�. (Ŀ���ҵ����͸� �̷������� �ִ´�)
            // Visbilty Member�� �ش� �κ��� ����� �����Ӱ� ���� �� �ִٴ� ��.
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member, value:_joinCode)
                }
            };
            //�κ� �̸��� �ɼ��� �־��ֵ��� �Ǿ� ����.
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync($"{playerName}'s Lobby", _maxConnections, lobbyOptions);

            //�κ�� ��������� Ȱ���� ������ �ı��ǵ��ϵǾ� �ִ�. ���� �����ð��������� ping�� ������ �Ѵ�.
            _lobbyId = lobby.Id;
            HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15)); //15�ʸ��� ��
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("���� ����" + e);
            return;
        }

        MakeNetworkServer();

        UserData userData = new UserData()
        {
            username = playerName,
            userAuthId = AuthenticationService.Instance.PlayerId
        };
        string json = JsonUtility.ToJson(userData);
        byte[] payload = Encoding.UTF8.GetBytes(json);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payload;
        Debug.LogError("��������2222");

        if (NetworkManager.Singleton.StartHost())
        {
            Debug.LogError("��������3333");
            NetworkManager.Singleton.SceneManager.LoadScene(SceneNames.ReadyLobbyScene, LoadSceneMode.Single);
        }

    }
    private IEnumerator HeartBeatLobby(float waitTimeSec)
    {
        var timer = new WaitForSecondsRealtime(waitTimeSec);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId); //�κ�� �� ������
            yield return timer;
        }
    }

    //��� ����
    public void Dispose()
    {
        Shutdown();
    }

    public async void Shutdown()
    {
        //��Ʈ��Ʈ �ڷ�ƾ ���ش�.
        HostSingleton.Instance.StopAllCoroutines();

        if (!string.IsNullOrEmpty(_lobbyId))
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(_lobbyId); //���ö� �����
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        _lobbyId = string.Empty;
        _networkServer?.Dispose();
    }



}
