using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    //public SceneController scene;
    private Database database;

    public TextMeshProUGUI email;
    public TextMeshProUGUI password;

    private void Start()
    {
        database = GetComponent<Database>();
    }

    public void OnButtonLogin()
    {
        StartCoroutine(database.AuthenticateUser(email.text, password.text, OnLoginComplete));
    }

    private void OnLoginComplete(bool loginComplete)
    {
        if (loginComplete)
        {
            //scene.SceneChange("Gameplay");
            Debug.Log("Inicio de sesión exitoso");
        }
        else
        {
            Debug.Log("Inicio de sesión fallido. Email o contraseña incorrectos.");
        }
    }
}
