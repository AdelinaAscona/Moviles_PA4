using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Authentification : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI email_R;
    //[SerializeField] TextMeshProUGUI password_R;

    //[SerializeField] TextMeshProUGUI email_L;
    //[SerializeField] TextMeshProUGUI password_L;

    //[Header("Bool Actions")]
    //[SerializeField] private bool signUp = false;
    //[SerializeField] private bool signIn = false;

    //private FirebaseAuth _authReference;

    //public UnityEvent OnLogInSuccesful = new UnityEvent();

    //private void Awake()
    //{
    //    _authReference = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
    //}

    //private void Start()
    //{
    //    if (signUp)
    //    {
    //        Debug.Log("Start Register");
    //        StartCoroutine(RegisterUser(email_R.text, password_R.text));
    //    }

    //    if (signIn)
    //    {
    //        Debug.Log("Start Login");
    //        StartCoroutine(SignInWithEmail(email_L.text, password_L.text));
    //    }
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        LogOut();
    //    }
    //}


    //public IEnumerator RegisterUser(string email, string password)
    //{
    //    Debug.Log("Registering");
    //    var registerTask = _authReference.CreateUserWithEmailAndPasswordAsync(email, password);
    //    yield return new WaitUntil(() => registerTask.IsCompleted);

    //    if (registerTask.Exception != null)
    //    {
    //        Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
    //    }
    //    else
    //    {
    //        Debug.Log($"Succesfully registered user {registerTask.Result.User.Email}");
    //    }
    //}

    //public IEnumerator SignInWithEmail(string email, string password)
    //{
    //    Debug.Log("Loggin In");

    //    var loginTask = _authReference.SignInWithEmailAndPasswordAsync(email, password);
    //    yield return new WaitUntil(() => loginTask.IsCompleted);

    //    if (loginTask.Exception != null)
    //    {
    //        Debug.LogWarning($"Login failed with {loginTask.Exception}");
    //    }
    //    else
    //    {
    //        Debug.Log($"Login succeeded with {loginTask.Result.User.Email}");
    //        OnLogInSuccesful?.Invoke();
    //    }
    //}

    //private void LogOut()
    //{
    //    FirebaseAuth.DefaultInstance.SignOut();
    //}

    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    // Login Variables
    [Space]
    [Header("Login")]
    public TextMeshProUGUI emailLoginField;
    public TextMeshProUGUI passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TextMeshProUGUI nameRegisterField;
    public TextMeshProUGUI emailRegisterField;
    public TextMeshProUGUI passwordRegisterField;
    public TextMeshProUGUI confirmPasswordRegisterField;


    //Database

    DatabaseReference reference;


    string userId;
    private void Start()
    {


        StartCoroutine(CheckAndFixDependenciesAsync());

        //AQUIII DATABASE NAME
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //Obtiene el UID del usuario actualmente autenticado
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;



    }


    //desde aqui todo como antes
    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);


        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();

            yield return new WaitForEndOfFrame();
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }


    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;  

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }



    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));

    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {

            user = loginTask.Result.User;

            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

            SceneManager.LoadScene("SampleScene"); // OJITO


        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }

                Debug.Log(failedMessage);
            }
            else
            {
                // Obtén el usuario autenticado del resultado de la autenticación
                Firebase.Auth.AuthResult authResult = registerTask.Result;
                user = authResult.User;


                //UserProfile userProfile = new UserProfile { DisplayName = name, PhotoUrl = new Uri(defaultProfileImage) };  //AQUIIIIIIIIIIIII :)   

                //var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                //yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                //if (updateProfileTask.Exception != null)
                //{
                //    // Delete the user if user update failed
                //    user.DeleteAsync();

                //    Debug.LogError(updateProfileTask.Exception);

                //    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                //    AuthError authError = (AuthError)firebaseException.ErrorCode;


                //    string failedMessage = "Profile update Failed! Becuase ";
                //    switch (authError)
                //    {
                //        case AuthError.InvalidEmail:
                //            failedMessage += "Email is invalid";
                //            break;
                //        case AuthError.WrongPassword:
                //            failedMessage += "Wrong Password";
                //            break;
                //        case AuthError.MissingEmail:
                //            failedMessage += "Email is missing";
                //            break;
                //        case AuthError.MissingPassword:
                //            failedMessage += "Password is missing";
                //            break;
                //        default:
                //            failedMessage = "Profile update Failed";
                //            break;
                //    }

                ////    Debug.Log(failedMessage);
                //}
                //else
                //{
                    Debug.Log("Registration Sucessful Welcome " + user.DisplayName);

                //}
            }
        }
    }

   
}
