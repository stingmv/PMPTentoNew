using TMPro;
using UnityEditor;
using UnityEngine;

namespace Configuration
{
    public class UsernameSelector : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputUsername;


        public void SetUsername()
        {
            GameEvents.NewUsername?.Invoke(_inputUsername.text);
        }
    }
}
