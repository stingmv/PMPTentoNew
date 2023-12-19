using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Login
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField] private InputBase _emailInput;
        [SerializeField] private InputBase _passwordInput;
        
        [SerializeField] private UnityEvent _onSuccessLogin;
        [SerializeField] private UnityEvent _onMissingFields;
        [SerializeField] private UnityEvent _onFailedLogin;


        public bool ComprobeMissFields()
        {
            var emptyEmail = _emailInput.HaveError;
            var emptyPassword = _passwordInput.HaveError;
            return emptyEmail || emptyPassword;
        }
        
        public bool ComprobeUser()
        {
            //Logica para buscar usuario
            var email = _emailInput.InputField.text;
            var password = _passwordInput.InputField.text;
            // var data = _dataUser.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            // Debug.Log(data.Id);
            // if (data.Id > 0)
            // {
            //     _selectionUser.userSelectionIndex = data.Id;
            //     Debug.Log("no nulo");
            //     return true;
            // }
            // _selectionUser.userSelectionIndex = 0;
            // Debug.Log("nulo");
            return false;

        }
        
        public void Login()
        {
            StopAllCoroutines();
            StartCoroutine(ILogin());

        }

        IEnumerator ILogin()
        {
            if (ComprobeMissFields())
            {
                _onMissingFields?.Invoke();
                yield break;
            }
            yield return new WaitForSeconds(1);
#if UNITY_EDITOR
            
            _onSuccessLogin?.Invoke();
            yield break;
#endif
            if (ComprobeMissFields())
            {
                _onMissingFields?.Invoke();
                yield break;
            }

            if (ComprobeUser())
            {
                _onSuccessLogin?.Invoke();
            }
            else
            {
                _onFailedLogin?.Invoke();
            }
            yield return null;
            
        }
    }
}
