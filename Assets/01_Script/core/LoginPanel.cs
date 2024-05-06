
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_InputField _inputName;
    [SerializeField] private Button _btnLogin;
    [SerializeField] GameObject obj;

    private void Awake()
    {
        _btnLogin.interactable = false;
        _btnLogin.onClick.AddListener(HandleLoginBtnClick);
        _inputName.onValueChanged.AddListener(ValidateUserName);
    }

    private void ValidateUserName(string name)
    {
        //����� ��ȣ ���� �̽������� ���� ����
        Regex regex = new Regex(@"^[a-zA-Z0-9]{2,12}$");
        bool success = regex.IsMatch(name);

        _btnLogin.interactable = success;
    }


    private void HandleLoginBtnClick()
    {
        ClientSingleton.Instance.GameManager.SetPlayerName(_inputName.text);

        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        //_canvasGroup.enabled = false;
        obj.SetActive(true);
    }
}

