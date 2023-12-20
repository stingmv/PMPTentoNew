using TMPro;
using UnityEditor;
using UnityEngine;

namespace Configuration
{
    public class UsernameSelector : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectUser _objectUser;
        [SerializeField] private TMP_InputField _inputUsername;


        public void SetUsername()
        {
            _objectUser.userInfo.username  = _inputUsername.text;
            _objectUser.userInfo.haveUsername = true;
            PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));

        }
    }
}
