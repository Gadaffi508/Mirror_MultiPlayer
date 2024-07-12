using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class InputManager : MonoBehaviour
    {
        private static readonly IDictionary<string, int> mapStates = new Dictionary<string, int>();
        
        // Controlls

        public static void Add(string mapName)
        {
            mapStates.TryGetValue(mapName, out int value);
            mapStates[mapName] = value + 1;

            UpdateMapState(mapName);
        }

        public static void Remove(string mapName)
        {
            mapStates.TryGetValue(mapName, out int value);
            mapStates[mapName] = Mathf.Max(value -1 , 0);
            
            UpdateMapState(mapName);
        }

        static void UpdateMapState(string mapName)
        {
            int value = mapStates[mapName];
        }
    }
}
