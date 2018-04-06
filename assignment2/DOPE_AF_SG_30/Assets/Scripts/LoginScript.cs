using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class LoginScript : MonoBehaviour {

    
    //Login panel
    public GameObject loginPanel;

    public InputField usernameInputField;
    public InputField passwordInputField;

    public Button loginButton;
    public Button signupButton;
   // public Button resetButton;

    //Signup panel
    public GameObject signupPanel;

    public InputField emailInputFieldSignup;
    public InputField usernameInputFieldSignup;
    public InputField passwordInputFieldSignup;

    public Button backButtonSignup;
    public Button signupButtonSignup;

    //Firebase Authentification
    Firebase.Auth.FirebaseAuth auth;

    //Firebase Database
    DatabaseReference database;
    string URL = "https://socialgaming-b6d55.firebaseio.com/";

    //Datastructs for database
    struct user
    {
        public user(string email) { this.email = email; }
        public string email;
    }

    // Use this for initialization
    private void Awake()
    {
        signupPanel.SetActive(false);
    }
    
    void Start() {
        //Firebase Authentification
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //Menus
        //Login
        loginButton.onClick.AddListener(onLoginButtonClick);
        signupButton.onClick.AddListener(onSignupButtonClick);
        //resetButton.onClick.AddListener(onResetButtonClick);

        //Signup
        backButtonSignup.onClick.AddListener(onbackButtonSignupClick); ;
        signupButtonSignup.onClick.AddListener(onsignupButtonSignupClick); ;

        //Firebase Database
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(URL);

        // Get the root reference location of the database.
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update() {

    }

    //Login
    void onLoginButtonClick() { LoginUsername(usernameInputField.text, passwordInputField.text); }
    void onSignupButtonClick() { loginPanel.SetActive(false); signupPanel.SetActive(true); }
    //void onResetButtonClick() { }

    //Signup
    void onbackButtonSignupClick() { signupPanel.SetActive(false); loginPanel.SetActive(true); }

    void onsignupButtonSignupClick() { Signup(usernameInputFieldSignup.text, emailInputFieldSignup.text, passwordInputFieldSignup.text); }

    //Source (Signup & Login): https://xinyustudio.wordpress.com/2017/01/22/using-firebase-in-unity3d-tutorial-2-authorization-and-user-login-example-with-user-name-and-password/
    public void Signup(string username, string email, string password)
    {
        Debug.Log(username + " " + email + " " + password);
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            //Error handling
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    Debug.Log("Error1");//Firebase.Auth.UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            string json = JsonUtility.ToJson(new user(email));
            Debug.Log(username+" "+json);
            database.Child("users").Child(username).SetRawJsonValueAsync(json);

            Firebase.Auth.FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            GlobalVariables.username = username;
            
            Login(email, password);
        });
    }

    public void LoginUsername(string username,string password)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(username).Child("email").GetValueAsync().ContinueWith(task => {
        if (task.IsFaulted)
        {
              // Handle the error...
          }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                GlobalVariables.username = username;
                

                string email = snapshot.GetValue(true).ToString();
                Login(email, password);
          }
    });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                return;
            }

            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            SceneManager.LoadScene("MainMenu");
        });
    }
}
