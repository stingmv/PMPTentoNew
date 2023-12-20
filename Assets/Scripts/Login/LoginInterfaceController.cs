using UnityEngine;
using UnityEngine.Events;

namespace Login
{
    public class LoginInterfaceController : MonoBehaviour
    {
        [SerializeField] private Canvas _GUILogin;
        [SerializeField] private Canvas _GUIUsername;
        [SerializeField] private Canvas _GUIInstructor;

        [SerializeField] private ScriptableObjectUser _objectUser;
        [SerializeField] private ScriptableObjectInstructor _objectInstructor;
        [SerializeField] private UnityEvent _onFinishLoginConfiguration;
        
        // Start is called before the first frame update
        void Start()
        {
            if (PlayerPrefs.HasKey("userInfo"))
            {
                _objectUser.userInfo = JsonUtility.FromJson<UserInfo>(PlayerPrefs.GetString("userInfo"));
            }
            Debug.Log(_objectUser.userInfo.haveUser);
            Debug.Log(_objectUser.userInfo.user.userName);
            Debug.Log(_objectUser.userInfo.haveUsername);
            Debug.Log(_objectUser.userInfo.haveUser);
            ComprobeLogin();
        }

        public void ComprobeLogin()
        {
            if (_objectUser.userInfo.haveUser)
            {
                ComprobeUsername();
            }
            else
            {
                _GUILogin.gameObject.SetActive(true);
                _GUIUsername.gameObject.SetActive(false);
                _GUIInstructor.gameObject.SetActive(false);
                
            }
        }

        public void ComprobeUsername()
        {
            if (_objectUser.userInfo.haveUsername)
            {
                ComprobeInstructor();
            }
            else
            {
                _GUILogin.gameObject.SetActive(false);
                _GUIUsername.gameObject.SetActive(true);
                _GUIInstructor.gameObject.SetActive(false);
            }
        }

        public void ComprobeInstructor()
        {
            if (_objectUser.userInfo.haveInstructor)
            {
                _onFinishLoginConfiguration?.Invoke();
            }
            else
            {
                _GUILogin.gameObject.SetActive(false);
                _GUIUsername.gameObject.SetActive(false);
                _GUIInstructor.gameObject.SetActive(true);
            }
        }
    }
}
