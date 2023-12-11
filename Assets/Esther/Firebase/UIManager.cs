using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    //public static UIManager Instance;

    //[SerializeField]
    //private GameObject loginPanel;

    //[SerializeField]
    //private GameObject registrationPanel;

    //[SerializeField]
    //private GameObject gamePanel;

    //****************************************************************
    //[Space]
    //[Header("Profile Picture Update Data")]
    //public GameObject profileUpdatePanel;
    //public Image profileImage;
    //public InputField urlInputText;

    //[Space]
    //[Header("DataBase Update NewUserName")]
    //public InputField newUsernameInput;
    //public Button changeUsernameButton;

    //public Database updateUsernameScript;

    //private void Awake()
    //{
    //    CreateInstance();
    //}

    //private void Start()
    //{
    //    changeUsernameButton.onClick.AddListener(ChangeUsername);
    //}

    //private void CreateInstance()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //}

    //private void ClearUI()
    //{
    //    loginPanel.SetActive(false);
    //    registrationPanel.SetActive(false);
    //    //gamePanel.SetActive(false);
    //}

    //public void OpenLoginPanel()
    //{
       
    //    loginPanel.SetActive(true);
    //    registrationPanel.SetActive(false);
    //}

    //public void OpenRegistrationPanel()
    //{
    //    ClearUI();
    //    registrationPanel.SetActive(true);
    //    loginPanel.SetActive(false);
    //}

    //public void OpenGamePanel()
    //{
    //    ClearUI();
    //    gamePanel.SetActive(true);
    //}


    //*****************************************
    //public void OpenUpdateProfilePanel()
    //{

    //    profileUpdatePanel.SetActive(!profileUpdatePanel.activeSelf);
    //}

    //public void CloseAvatarsPanel()
    //{
    //    profileUpdatePanel.SetActive(true);
    //}

    //public void LoadProfileImage(string url)
    //{
    //    StartCoroutine((LoadProfileImageIE(url)));
    //}

    //public IEnumerator LoadProfileImageIE(string url)
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.LogError(www.error);
    //    }
    //    else
    //    {
    //        Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
    //        profileImage.sprite = sprite;
    //        profileUpdatePanel.SetActive(false);

    //    }
    //}

    //string url;

    //public void Button1()
    //{
    //    // Acción para el botón con identificador "boton1"
    //    url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQQuhSVd1TOQr6IVF-iKZqLIG7U8I_PlxSU-su1RrdW1_7_UolRG9sdr4vkqh1H7HQWm4o&usqp=CAU";
    //    GetURLtextProfile();
    //}
    //public void Button2()
    //{
    //    // Acción para el botón con identificador "boton2"

    //    url = "https://www.pngall.com/wp-content/uploads/12/Avatar-Profile-Vector-PNG-Photos.png";

    //    GetURLtextProfile();
    //}
    //public void Button3()
    //{
    //    // Acción para el botón con identificador "boton3"

    //    url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDKFmIAeGGdc5Sudv7nTDy6S1_fhcJ8lvlHw&usqp=CAU";

    //    GetURLtextProfile();
    //}
    //public string GetURLtextProfile()
    //{

    //    return url;
    //}

    //public void ChangeUsername()
    //{
    //    string newUsername = newUsernameInput.text;

    //    if (!string.IsNullOrEmpty(newUsername))
    //    {
    //        updateUsernameScript.AddUsername(newUsername);
          
    //    }
    //}
    //public  string GetNewUserName()
    //{
    //    return newUsernameInput.text;
    //}

}
