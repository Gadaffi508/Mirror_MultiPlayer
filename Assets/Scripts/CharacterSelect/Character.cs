using UnityEngine;

namespace Mirror
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character Selection/Character")]
    public class Character : ScriptableObject
    {
        public string characterName = default;
        public GameObject characterPreviewPrefab = default;
        public GameObject gamePlayCharacterPrefab = default;

        public string CharacterName => characterName;
        public GameObject CharacterPreviewPrefab => characterPreviewPrefab;
        public GameObject GamePlayCharacterPrefab => gamePlayCharacterPrefab;
    }
}
