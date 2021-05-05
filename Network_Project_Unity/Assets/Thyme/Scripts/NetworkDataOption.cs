using System;
using UnityEngine;
using System.Collections.Generic;

namespace NetworkGame
{
    public class NetworkDataOption
    {
        [Serializable]
        public class ReplicateObject
        {
            public string objectID;
            public string ownerID;
            public string prefName;
            public Vector3 position;
        }

        [Serializable]
        public class ReplicateObjectList
        {
            public List<ReplicateObject> replicateObjectList = new List<ReplicateObject>();
        }

        [Serializable]
        public class EventCallBack
        {
            public string eventName;
            public string data;
        }

        [Serializable]
        public class EventSendCreateRoom : EventCallBack
        {
            public Room.RoomOption roomOption;
        }

    }

    public class Room
    {
        [Serializable]
        public class RoomOption
        {
            public string roomName;

        }
        public RoomOption roomOption;

    }
}

