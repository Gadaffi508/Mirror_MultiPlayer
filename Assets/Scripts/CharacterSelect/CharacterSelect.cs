using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mirror
{
    public class CharacterSelect : NetworkBehaviour
    {
        public GameObject characterSelectDisplay = default;
        public Transform characterPreviewParent = default;
        public TMP_Text characterNameText = default;
        public float turnSpeed = 90f;
        public Character[] characters = default;

        private int _currentCharacterIndex = 0;
        private List<GameObject> _characterInstance = new List<GameObject>();

        public override void OnStartClient()
        {
            if (characterPreviewParent.childCount == 0)
            {
                foreach (var character in characters)
                {
                    GameObject characterInstance = Instantiate(character.characterPreviewPrefab, characterPreviewParent);
                
                    characterInstance.SetActive(false);
                
                    _characterInstance.Add(characterInstance);
                }
            }
            
            _characterInstance[_currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[_currentCharacterIndex].characterName;
            
            characterSelectDisplay.SetActive(true);
        }

        private void Update()
        {
            characterPreviewParent.RotateAround(
                characterPreviewParent.position,
                characterPreviewParent.up,
                turnSpeed * Time.deltaTime);
        }

        public void Select()
        {
            CmdSelect(_currentCharacterIndex);
            characterSelectDisplay.SetActive(false);
        }

        [Command(requiresAuthority = false)]
        public void CmdSelect(int characterIndex, NetworkConnectionToClient sender = null)
        {
            GameObject characterInstacnes = Instantiate(characters[characterIndex].GamePlayCharacterPrefab);
            NetworkServer.Spawn(characterInstacnes,sender);
        }

        public void Right()
        {
            _characterInstance[_currentCharacterIndex].SetActive(false);

            _currentCharacterIndex = (_currentCharacterIndex + 1) % _characterInstance.Count;
            
            _characterInstance[_currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[_currentCharacterIndex].characterName;
        }
        public void Left()
        {
            _characterInstance[_currentCharacterIndex].SetActive(false);

            _currentCharacterIndex--;
            if (_currentCharacterIndex < 0)
            {
                _currentCharacterIndex += _characterInstance.Count;
            }
            
            _characterInstance[_currentCharacterIndex].SetActive(true);
            characterNameText.text = characters[_currentCharacterIndex].characterName;
        }
        
    }
}
