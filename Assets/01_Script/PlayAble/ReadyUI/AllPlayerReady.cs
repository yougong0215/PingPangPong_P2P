using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllPlayerReady : NetworkBehaviour
{
    PlayerReadyUI[] _playerReadyUI;

    private void Awake()
    {
        _playerReadyUI = GameObject.FindObjectsOfType<PlayerReadyUI>();
    }

    private void Update()
    {
        if (!IsHost)
            return;

        int h = 0;

        for(int i = 0; i < _playerReadyUI.Length; i++)
        {
            if (_playerReadyUI[i].IsReady.Value == false)
            {
                h++;    
            }
        }

        if(h == 2)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(SceneNames.GameScene, LoadSceneMode.Single);
        }
    }

}
