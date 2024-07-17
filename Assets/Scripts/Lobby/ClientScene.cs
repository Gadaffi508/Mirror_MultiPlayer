using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class ClientScene : NetworkBehaviour
    {
        public static List<GameObject> networkObjects;
        public static void RegisterPrefab(GameObject objects)
        {
            networkObjects.Add(objects);
        }
    }
}
