using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;

    [SerializeField] private LobbyUI _lobbyUIPrefab;
    [SerializeField] private float _spacing = 30f;

    private List<LobbyUI> _lobbyList;

    private void Awake()
    {
        _lobbyList = new List<LobbyUI>();
        StartCoroutine(Refresh());
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    CreateLobbyUI();
        //}

    }
    public void CreateLobbyUI(Lobby lobby)
    {
        LobbyUI ui = Instantiate(_lobbyUIPrefab, _scrollRect.content);

        ui.SetRoomTemplate(lobby);

        _lobbyList.Add(ui);
        float offset = _spacing;

        for (int i = 0; i < _lobbyList.Count; i++)
        {
            _lobbyList[i].Rect.anchoredPosition = new Vector2(0, -offset);
            offset += _lobbyList[i].Rect.sizeDelta.y + _spacing;
        }

        Vector2 contentSize = _scrollRect.content.sizeDelta;
        contentSize.y = offset;
        _scrollRect.content.sizeDelta = contentSize;
    }

    IEnumerator Refresh()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(5f);
            RefreshList();
        }
    }


    public async void RefreshList()
    {
        try
        {
            //�κ� �����ϱ� ���� ���� ��ü
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25; //���������̼��� ���� ���������� � �ɼ�
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field:QueryFilter.FieldOptions.AvailableSlots ,
                    op: QueryFilter.OpOptions.GT,
                    value:"0"), //�����ִ� ĭ�� 0ĭ �ʰ��ΰ͵鸸
                new QueryFilter(
                    field:QueryFilter.FieldOptions.IsLocked ,
                    op: QueryFilter.OpOptions.EQ,
                    value:"0"),  //���� 0�̸� ���̵��� ���� �ֵ鸸
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            //�κ� ����
            ClearLobbies();

            //�ٽ� �������ִ� ���� ���⿡
            foreach (Lobby lobby in lobbies.Results)
            {
                CreateLobbyUI(lobby);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    //���� �ִ� �κ� �����
    private void ClearLobbies()
    {
        foreach (LobbyUI ui in _lobbyList)
        {
            Destroy(ui.gameObject);
        }

        _lobbyList.Clear();
    }

}
