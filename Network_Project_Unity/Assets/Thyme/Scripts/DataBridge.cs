using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;





public class DataBridge : MonoBehaviour
{
   public Text usernameTextInput, passwordInput;
   private Player data;

   private string DATA_URL = "https://networkproject-58995-default-rtdb.asia-southeast1.firebasedatabase.app/";

   void Start()
   {
       
   }
}
