using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public struct FixedString : INetworkSerializable
{
    public FixedString128Bytes name;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref name);
    }
}

public class PlayerReadyUI : NetworkBehaviour
{
    Button _btn = null;
    TextMeshProUGUI _tmp;
    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<FixedString128Bytes> = new NetworkVariable<FixedString128Bytes>("대기중", )
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _btn = GetComponent<Button>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();

        _btn.interactable = true;
        _tmp.text = "대기중";

        if (IsHost)
        {
            _btn.GetComponent<RectTransform>().localPosition = new Vector3(-400,0,0);

        }
        else
        {
            _btn.GetComponent<RectTransform>().localPosition = new Vector3(400, 0, 0);
        }

        _btn.onClick.AddListener(IsReadyMethod);
    }

    void IsReadyMethod()
    {
        IsReady.Value = true;


        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(ReadyDispose);
    }

    void ReadyDispose()
    {
        IsReady.Value = false;


        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(IsReadyMethod);
    }
    public override void OnNetworkDespawn()
    {
        _btn = GetComponent<Button>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();

        _btn.interactable = true;
        _tmp.text = "대기중";
    }

}
