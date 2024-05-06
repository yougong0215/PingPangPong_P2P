using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UGSConnectUI : MonoBehaviour
{
    [SerializeField] private Button _relayHostBtn;
    [SerializeField] private Button _enterLobbyBtn;

    private void Awake()
    {
        _relayHostBtn.onClick.AddListener(HandleRelayHostClick);
    }

    private async void HandleRelayHostClick()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }
}

