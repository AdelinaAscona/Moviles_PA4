using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;

public class Database : MonoBehaviour
{
    //public RegisterManager register;

    [SerializeField]
    private User user;
    private DatabaseReference dataReference;
    private string userID;

    private void Awake()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
    }

    void Start()
    {
        dataReference = FirebaseDatabase.DefaultInstance.RootReference;
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
