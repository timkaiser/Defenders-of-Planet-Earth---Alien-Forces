using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FriendlistScript : MonoBehaviour {

    public GameObject content;
    public Toggle[] friendstoggle;
    public Toggle tooglePrefab;

    public InputField addPlayerInput;
    public Button addPlayerButton;


    public static string[] friends = new string[0];
    //Firebase Database
    public static DatabaseReference database;
    static string URL = "https://socialgaming-b6d55.firebaseio.com/";
    
    // Use this for initialization
    void Start () {
        //Firebase Database
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(URL);

        // Get the root reference location of the database.
        database = FirebaseDatabase.DefaultInstance.RootReference;

        addPlayerButton.onClick.AddListener(onaddPlayerButtonClick);

        loadFriends();
    }

    public void onaddPlayerButtonClick(){ addFriend(addPlayerInput.text); Debug.Log(addPlayerInput.text); }



    public void addFriend(string name)
    {
        Debug.Log(GlobalVariables.username+" 1");
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(name).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("f1");

                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                Debug.Log("2");

                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                if( snapshot.Value != null)
                {
                    Debug.Log("3");

                    database.Child("Friendlist").Child(GlobalVariables.username).Child(name).SetValueAsync(System.DateTime.Now.ToUniversalTime().ToString());
                    loadFriends();
                }
            }
        });
    }

    public void loadFriends()
    {
        Debug.Log("4");

        FirebaseDatabase.DefaultInstance.GetReference("Friendlist").Child(GlobalVariables.username).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                friends = new string[snapshot.ChildrenCount];
                int i = 0;
                foreach (var childSnapshot in snapshot.Children)
                {
                    friends[i] = childSnapshot.Key;
                    i++;
                }

                updateFriendlist();
            }
        });
    }

    public void updateFriendlist() 
    {
        for (int i = 0; i < friendstoggle.Length; i++)
        {
            friendstoggle[i].transform.localPosition = new Vector3(-1000,-1000,-1000);
            Destroy(friendstoggle[i]);
        }

        friendstoggle = new Toggle[friends.Length];
        for(int i = 0; i< friendstoggle.Length; i++)
        {
            friendstoggle[i] = Instantiate(tooglePrefab, new Vector3(46, i * 43, 0), Quaternion.identity);
            friendstoggle[i].transform.SetParent(content.transform);
            friendstoggle[i].transform.localPosition = new Vector3(303, -33 - (i * 63), 0);
            friendstoggle[i].GetComponentInChildren<Text>().text = friends[i];
        }
    }
}
