using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class TestDatabase : MonoBehaviour
{

    public Text emailInput, passwordInput;
    public Text debugForUser;

    public void Login()
    {
        if (emailInput.text == "" || passwordInput.text == "")
        {
            Debug.Log("Pls enter your Email or Password to login");
            return;
        }

        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith((task =>
        {

            if (task.IsCanceled)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMsg((AuthError)e.ErrorCode);
                return;
            }

            else if (task.IsFaulted)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMsg((AuthError)e.ErrorCode);
                return;
            }

            else if (task.IsCompleted)
            {

                Debug.Log("Login Completed!");
            }

        }));

    }

    public void Register()
    {
        if (emailInput.text == "" || passwordInput.text == "")
        {
            Debug.Log("Pls enter your Email or Password to register");
            return;
        }
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith((task =>
        {

            if (task.IsCanceled)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMsg((AuthError)e.ErrorCode);
                return;
            }

            else if (task.IsFaulted)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMsg((AuthError)e.ErrorCode);
                return;
            }

            else if (task.IsCompleted)
            {

                Debug.Log("Register Completed!");
            }
        }));
    }

    public void Anonymous()
    {
        FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith((task =>
        {

            if (task.IsCanceled)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMsg((AuthError)e.ErrorCode);
                return;
            }

            else if (task.IsFaulted)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMsg((AuthError)e.ErrorCode);
                return;
            }

            else if (task.IsCompleted)
            {

                Debug.Log("Login with Anonymous Completed!");
            }
        }));
    }

    public void Logout()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }
    }

    void GetErrorMsg(AuthError errorCode)
    {
        string msg = "";
        msg = errorCode.ToString();
        debugForUser.text = msg;
        Debug.Log(msg);
        

    }


}
