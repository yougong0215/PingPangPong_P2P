using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IPConnectUI : MonoBehaviour
{
    [SerializeField] private Button _hostBtn, _clientBtn;
    [SerializeField] private TMP_InputField _ipText, _portText;
    /*
    private void Awake()
    {
        _hostBtn.onClick.AddListener(HandleHostBtnClick);
        _clientBtn.onClick.AddListener(HandleClientBtnClick);


        _ipText.text = FindIPAddress();
        _portText.text = "7777";

        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }

    private string FindIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                //Debug.Log(ip);
                return ip.ToString();
            }
        }
        return null;
    }

    private void HandleHostBtnClick()
    {
        if (!SetUpNetworkPassport()) return;
        if (NetworkManager.Singleton.StartHost())
        {
            NetworkManager.Singleton.SceneManager.LoadScene(SceneNames.GameScene, LoadSceneMode.Single);
        }
        else
        {
            //유니티 네트워크 매니저 셧다운.
            NetworkManager.Singleton.Shutdown();
        }

    }

    private void HandleClientBtnClick()
    {
        if (!SetUpNetworkPassport()) return;
        if (!NetworkManager.Singleton.StartClient())
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    //입력한 IP와 포트가 올바른지 검사후 올바르면 패트포트 생성
    private bool SetUpNetworkPassport()
    {
        var ip = _ipText.text;
        var port = _portText.text;

        var portRegex = new Regex(@"[0-9]{3,5}");
        var ipRegex = new Regex(@"^[0-9\.]+$");

        var portMatch = portRegex.Match(port);
        var ipMatch = ipRegex.Match(ip);

        if (!portMatch.Success || !ipMatch.Success)
        {
            Debug.LogError("올바르지 못한 아이피 또는 포트번호입니다.");
            return false;
        }

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ip,
            (ushort)int.Parse(port)
        );
        return true;
    }


    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }

    private void HandleClientDisconnected(ulong clientID)
    {
        Debug.Log(clientID + ", 에러 발생");
    }
    */
}
