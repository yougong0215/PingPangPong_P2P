using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class UserData
{
    public string username;
    public string userAuthId;
}


public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;

    private Dictionary<ulong, string> _clientIdToAuthDictionary = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> _authIdToUserDataDictionary = new Dictionary<string, UserData>();

    public NetworkServer(NetworkManager networkManager)
    {
        _networkManager = networkManager; //ĳ��
        _networkManager.ConnectionApprovalCallback += ApprovalCheck; //���ο�û��

        _networkManager.OnServerStarted += HandleServerStarted;
    }

    //Ŭ���̾�Ʈ���� ������ ������ �� ������Ѽ� �������� ������ �����ϵ��� �Ѵ�. �̶� ��û�� ������ �Ѿ�´�.
    private void HandleApprovalCheck(
        NetworkManager.ConnectionApprovalRequest req,
        NetworkManager.ConnectionApprovalResponse res)
    {
        string json = Encoding.UTF8.GetString(req.Payload);
        UserData data = JsonUtility.FromJson<UserData>(json);

        Debug.Log(data.username);

        res.CreatePlayerObject = false; //�÷��̾� ������Ʈ �ڵ����� ���ֱ�.
        res.Approved = true; //���� ó�� �Ϸ�� ������ ���� ��

    }

    private void HandleServerStarted()
    {
        _networkManager.OnClientDisconnectCallback += HandleClientDisconnect;

    }

    private void HandleClientDisconnect(ulong clientID)
    {
        //Ŭ���̾�Ʈ ���������� ��ųʸ������� ����.
        if (_clientIdToAuthDictionary.TryGetValue(clientID, out string authID))
        {
            _clientIdToAuthDictionary.Remove(clientID);
            _authIdToUserDataDictionary.Remove(authID);
        }
    }

    public void Dispose()
    {
        if (_networkManager == null) return;
        _networkManager.ConnectionApprovalCallback -= HandleApprovalCheck;
        _networkManager.OnServerStarted -= HandleServerStarted;

        _networkManager.OnClientDisconnectCallback -= HandleClientDisconnect;

        if (_networkManager.IsListening)
        {
            _networkManager.Shutdown();
        }
    }

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest req,
        NetworkManager.ConnectionApprovalResponse res)
    {
        string json = Encoding.UTF8.GetString(req.Payload);
        UserData data = JsonUtility.FromJson<UserData>(json);

        Debug.Log(data.username);

        res.CreatePlayerObject = false; //�÷��̾� ������Ʈ �ڵ����� ���ֱ�.
        res.Approved = true; //���� ó�� �Ϸ�� ������ ���� ��
    }



}


