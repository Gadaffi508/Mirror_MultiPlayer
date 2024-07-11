using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    public class PlayerNameInput : MonoBehaviour
    {
        public static string name;

        public string m_name;

        [SerializeField] private InputField nameInput;

        public void SaveName()
        {
            name = nameInput.text;
            m_name = name;
        }
    }
}
