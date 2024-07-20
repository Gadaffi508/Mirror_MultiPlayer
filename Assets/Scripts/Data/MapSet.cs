using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Rounds
{
    [CreateAssetMenu(fileName = "New Map Set", menuName = "Rounds/ Map Set")]
    public class MapSet : ScriptableObject
    {
        [Scene] public List<string> maps = new List<string>();

        public IReadOnlyCollection<string> Maps => maps.AsReadOnly();
    }
}
