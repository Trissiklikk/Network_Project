using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace NetworkGame
{ 
    public class WebSocketConnection : MonoBehaviour
    {
        public Room currentRoom;
        private WebSocket ws;

        private string tempMessageString;

        private NetworkDataOption.ReplicateObjectList replicateSend = new NetworkDataOption.ReplicateObjectList();

        public static WebSocketConnection instance;

        public void Awake()
        {
            instance = this;
        }

        public void Connect()
        {
            string url = $"ws://127.0.0.1:8080/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }

        private void OnDestroy()
        {
            if (ws != null)
                ws.Close();
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            tempMessageString = messageEventArgs.Data;
        }

        private IEnumerator UpdataReplicateObject()
        {
            float duration = 1.0f;

            WaitForSeconds waitForSec = new WaitForSeconds(duration);

            while(true)
            {
                string toJson = JsonUtility.ToJson(replicateSend);

                SendReplicateData(toJson);

                yield return waitForSec;
            }
        }

        public void SendReplicateData(string jsonStr)
        {
            NetworkDataOption.EventCallBack eventData = new NetworkDataOption.EventCallBack();
            eventData.eventName = "ReplicateData";
            eventData.data = jsonStr;

            string toJson = JsonUtility.ToJson(eventData);
        }

        private void Internal_CreateRoom(string data)
        {
            Room.RoomOption roomOption = JsonUtility.FromJson<Room.RoomOption>(data);

            if(roomOption != null && currentRoom == null)
            {
                currentRoom = new Room();
                currentRoom.roomOption = roomOption;

                StartCoroutine(UpdataReplicateObject());
            }
        }

        public void CreateRoom(Room.RoomOption roomOption)
        {
            NetworkDataOption.EventSendCreateRoom eventData = new NetworkDataOption.EventSendCreateRoom();


            eventData.eventName = "CreateRoom";
            eventData.roomOption = roomOption;

            string toJson = JsonUtility.ToJson(eventData);
            ws.Send(toJson);

        }
    }
}


