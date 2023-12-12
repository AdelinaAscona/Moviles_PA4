using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using TMPro;

public class Database : MonoBehaviour
{
    public TextMeshProUGUI email_text;
    public TextMeshProUGUI password_text;
    
    //public RegisterManager register;

    [SerializeField] private User user;
    private DatabaseReference dataReference;
    private string userID;

    private void Awake()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        //userID = user.codeID.ToString();

        dataReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
        //dataReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        
    }

    public void OnClick_UserData()
    {
      
        CreateUser(email_text.text, password_text.text);
    }

    public void CreateUser(string email, string password)
    {
        int codeID = UnityEngine.Random.Range(0, 99999);
        user = new User(email, password, codeID);

        string json = JsonUtility.ToJson(user);
        dataReference.Child("users").Child(userID).SetRawJsonValueAsync(json).ContinueWith(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Done Update");
                }
                else
                {
                    Debug.Log("Failed Attempt");
                }
            });
    }

    public void CreateUser() //Prueba
    {
        int codeID = UnityEngine.Random.Range(0, 99999);

        string json = JsonUtility.ToJson(user);
        dataReference.Child("users").Child(userID).SetRawJsonValueAsync(json).ContinueWith(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Done Update");
                }
                else
                {
                    Debug.Log("Failed Attempt");
                }
            });
    }

    private IEnumerator GetFirstName(Action<string> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child("firstName").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    private IEnumerator GetLastName(Action<string> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child("lastName").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    private IEnumerator GetCodeID(Action<int> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child(nameof(User.codeID)).GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            //(int) -> Casting
            //int.Parse -> Parsing
            //https://teamtreehouse.com/community/when-should-i-use-int-and-intparse-whats-the-difference
            onCallBack?.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    private IEnumerator GetEmail(Action<string> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child("email").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    private IEnumerator GetPassword(Action<string> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child("password").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    public IEnumerator AuthenticateUser(string email, string password, Action<bool> onLoginComplete)
    {
        var userEmailData = dataReference.Child("users").Child(userID).Child("email").GetValueAsync();
        yield return new WaitUntil(() => userEmailData.IsCompleted);

        var userPasswordData = dataReference.Child("users").Child(userID).Child("password").GetValueAsync();
        yield return new WaitUntil(() => userPasswordData.IsCompleted);

        bool loginSuccessful = false;

        if (userEmailData != null && userPasswordData != null)
        {
            DataSnapshot emailSnapshot = userEmailData.Result;
            DataSnapshot passwordSnapshot = userPasswordData.Result;

            if (emailSnapshot != null && passwordSnapshot != null)
            {
                string userEmail = emailSnapshot.Value.ToString();
                string storedPassword = passwordSnapshot.Value.ToString();

                if (userEmail == email && storedPassword == password)
                {
                    loginSuccessful = true;
                }
            }
        }

        onLoginComplete?.Invoke(loginSuccessful);
    }

    public void RegisterUserScore(int score)
    {
        DatabaseReference scoresReference = dataReference.Child("users").Child(userID).Child("scores");

        scoresReference.Push().SetValueAsync(score).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Score {score} registrado exitosamente");
            }
            else
            {
                Debug.Log($"Error al registrar el score {score}");
            }
        });
    }

    public void SetScoresData(string scores)
    {
        DatabaseReference scoresReference = dataReference.Child("users").Child(userID).Child("scores");

        scoresReference.SetValueAsync(scores).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Scores registrados exitosamente");
            }
            else
            {
                Debug.Log("Error al registrar los scores");
            }
        });
    }

    public void GetTopScores(Action<List<int>> onTopScoresLoaded)
    {
        DatabaseReference scoresReference = dataReference.Child("users").Child(userID).Child("scores");

        scoresReference.LimitToLast(10).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot != null && snapshot.HasChildren)
                {
                    List<int> topScores = new List<int>();

                    foreach (var childSnapshot in snapshot.Children)
                    {
                        int score = int.Parse(childSnapshot.Value.ToString());
                        topScores.Add(score);
                    }

                    onTopScoresLoaded?.Invoke(topScores);
                }
                else
                {
                    Debug.Log("No se encontraron puntajes");
                }
            }
            else
            {
                Debug.Log("Error al cargar los puntajes");
            }
        });
    }

    public void SetPiecesData(List<int> pieces)
    {
        DatabaseReference piecesReference = dataReference.Child("users").Child(userID).Child("pieces");

        piecesReference.SetValueAsync(pieces).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Piezas registradas exitosamente");
            }
            else
            {
                Debug.Log("Error al registrar las piezas");
            }
        });
    }

    public IEnumerator GetPiecesData(string userID, Action<List<int>> onComplete)
    {
        DatabaseReference piecesReference = dataReference.Child("users").Child(userID).Child("pieces");

        var task = piecesReference.GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Error obteniendo datos de piezas: " + task.Exception);
            yield break;
        }

        DataSnapshot snapshot = task.Result;

        List<int> piecesList = new List<int>();

        if (snapshot != null && snapshot.HasChildren)
        {
            foreach (var childSnapshot in snapshot.Children)
            {
                int pieceValue = int.Parse(childSnapshot.Value.ToString());
                piecesList.Add(pieceValue);
            }
        }

        onComplete?.Invoke(piecesList);
    }


    public void ResetPiecesData()
    {
        DatabaseReference piecesReference = dataReference.Child("users").Child(userID).Child("pieces");

        piecesReference.RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Datos de piezas reseteados exitosamente");
            }
            else
            {
                Debug.Log("Error al resetear los datos de piezas");
            }
        });
    }

    public IEnumerator GetLastThreeUsers(Action<List<User>> onLastThreeUsers)
    {
        var usersReference = dataReference.Child("users").OrderByKey().LimitToLast(3).GetValueAsync();
        yield return new WaitUntil(() => usersReference.IsCompleted);

        if (usersReference.Exception != null)
        {
            Debug.LogError("Error: " + usersReference.Exception);
            yield break;
        }

        DataSnapshot snapshot = usersReference.Result;

        if (snapshot != null && snapshot.HasChildren)
        {
            List<User> lastThreeUsers = new List<User>();

            foreach (var childSnapshot in snapshot.Children)
            {
                User user = JsonUtility.FromJson<User>(childSnapshot.GetRawJsonValue());
                lastThreeUsers.Add(user);
            }

            onLastThreeUsers?.Invoke(lastThreeUsers);
        }
        else
        {
            Debug.Log("No se encontraron usuarios");
        }
    }

    public void SetScoreData(int score)
    {
        DatabaseReference piecesReference = dataReference.Child("users").Child(userID).Child("score");

        piecesReference.SetValueAsync(score).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Envio registrado exitosamente");
            }
            else
            {
                Debug.Log("Error al enviar confirmación");
            }
        });
    }

    public IEnumerator GetScoreData(string userID, Action<int> onScoreDataLoaded)
    {
        DatabaseReference scoreReference = dataReference.Child("users").Child(userID).Child("score");

        var task = scoreReference.GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot != null && snapshot.Exists)
            {
                int score = int.Parse(snapshot.Value.ToString());
                onScoreDataLoaded?.Invoke(score);
            }
            else
            {
                onScoreDataLoaded?.Invoke(0);
            }
        }
        else
        {
            Debug.Log("Error al cargar el puntaje");
        }
    }


    public void GetUserInfo()
    {
        StartCoroutine(GetFirstName(PrintData));
        StartCoroutine(GetLastName(PrintData));
        StartCoroutine(GetCodeID(PrintData));
        StartCoroutine(GetEmail(PrintData));
        StartCoroutine(GetPassword(PrintData));
    }

    private void PrintData(string name)
    {
        Debug.Log(name);
    }

    private void PrintData(int code)
    {
        Debug.Log(code);
    }
}

[Serializable]
public struct User
{
    public string email;
    public string password;
    public int codeID;

    public User(string email, string password, int codeID)
    {
        this.codeID = codeID;
        this.email = email;
        this.password = password;
    }
}
