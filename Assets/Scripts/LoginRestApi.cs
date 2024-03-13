using System;
using System.Collections;
using System.IO;
using System.Text;
using Login;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoginRestApi : MonoBehaviour
{
    [SerializeField] private string url = "https://api-portalweb.bsginstitute.com/api/AspNetUser/authenticate";
    [SerializeField] private string urlToMoreDetail = "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ObtenerCaracteristicasGamificacion";
    [SerializeField] private string urlToCredentials = "https://api-portalweb.bsginstitute.com/api/CredencialPortalPmp";

    [SerializeField] private ScriptableObjectUser _objectUser;
    [SerializeField] private LoginController _loginController;
    // [SerializeField] private string username = "pruebaxjrivera@bsginstitute.net";
    // [SerializeField] private string password = "BSgrupo123";

    public bool _finishRequest;
    public bool _haveError;
    public IEnumerator PostLoginRoutine(string username, string password)
    {
        _finishRequest = _haveError = false;
       using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
       {
            DataLogin dataLogin = new DataLogin() { username = username, password = password};
            
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            
            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);

                _objectUser.userInfo.haveUser = false;
                GameEvents.ErrorLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
            }
            else if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                    if (_objectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _objectUser.userInfo.haveUser = false;
                        GameEvents.FailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                        // _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                    }
                    else
                    {
                        StartCoroutine(GetGamificationData(_objectUser.userInfo.user.idAlumno));
                        _objectUser.userInfo.haveUser = true;
                        // GameEvents.SuccessfulLogin?.Invoke(_objectUser.userInfo.user);
                        
                        GameEvents.GetUserExam?.Invoke(_objectUser.userInfo.user.userName);
                        GameEvents.GetNameExam?.Invoke(_objectUser.userInfo.user.idAlumno.ToString());
                        // _loginController._onSuccessLogin?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text);
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onErrorInLogin?.Invoke("Se tuvo un error interno, vuelva a intentarlo mas tarde");
                }
                
            }
            // PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
            _finishRequest = true;
       }
    }

    public IEnumerator GetGamificationData(int userId)
    {
        _finishRequest = _haveError = false;
       using (UnityWebRequest request = new UnityWebRequest(urlToMoreDetail + "/" + userId, "GET"))
       {
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            
            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);

                _objectUser.userInfo.haveUser = false;
                GameEvents.ErrorLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
            }
            else if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    var detail = JsonUtility.FromJson<UserDetail>(request.downloadHandler.text);
                    _objectUser.userInfo.user.detail = detail;
                    if (_objectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _objectUser.userInfo.haveUser = false;
                        GameEvents.FailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                        // _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                    }
                    else
                    {
                        if (_objectUser.userInfo.user.detail.usernameG != "UserName")
                        {
                            _objectUser.userInfo.haveUser = true;
                            GameEvents.NewUsername?.Invoke(_objectUser.userInfo.user.detail.usernameG );

                        }
                        else
                        {
                            _objectUser.userInfo.haveUsername = false;
                        }

                        if (_objectUser.userInfo.user.detail.idCaracteristicaGamificacion != 0)
                        {
                            Debug.Log("idcaracteristicaGamificacion 1");
                            GameEvents.NewInstuctorId?.Invoke(_objectUser.userInfo.user.detail.instructorID);

                        }
                        GameEvents.SuccessfulLogin?.Invoke(_objectUser.userInfo.user);
                        
                        // GameEvents.GetUserExam?.Invoke(_objectUser.userInfo.user.userName);
                        // GameEvents.GetNameExam?.Invoke(_objectUser.userInfo.user.idAlumno.ToString());
                        // _loginController._onSuccessLogin?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text);
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onErrorInLogin?.Invoke("Se tuvo un error interno, vuelva a intentarlo mas tarde");
                }
                
            }
            // PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
            _finishRequest = true;
       }
    }

    public IEnumerator GetAvatar(string username, string password)
    {
        Debug.Log("-> Enviando para obtener avatar");

       using (UnityWebRequest request = new UnityWebRequest(urlToCredentials, "POST"))
       {
           DataLoginToCredentials dataLogin = new DataLoginToCredentials() { username = username, clave = password};
            
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            Debug.Log("-> Enviando para obtener avatar");

            yield return request.SendWebRequest();
            
            if (request.responseCode >= 400)
            {
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log("->"+request.error);
            }
            else
            {
                try
                {
                    var credential = JsonUtility.FromJson<ResponseToAvatar>(request.downloadHandler.text);
                    Debug.Log("->"+request.downloadHandler.text);
                    if (credential.contieneAvatar)
                    {
                        _objectUser.userInfo.haveAvatar = true;
                        _objectUser.userInfo.urlAvatar = GenerateUrlToAvatar(credential.avatar);
                        StartCoroutine(downloadSVG(_objectUser.userInfo.urlAvatar));
                    }
                    else
                    {
                        _objectUser.userInfo.haveAvatar = false;
                        
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("_>"+request.downloadHandler.text);
                    _objectUser.userInfo.haveAvatar = false;
                }
                
            }
       }
    }
    IEnumerator downloadSVG(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
     
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error while Receiving: " + www.error);
        }
        else
        {
            //Convert byte[] data of svg into string
            string bitString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            var tessOptions = new VectorUtils.TessellationOptions()
            {
                StepDistance = 100.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f
            };
         
            // Dynamically import the SVG data, and tessellate the resulting vector scene.
            var sceneInfo = SVGParser.ImportSVG(new StringReader(bitString));
            var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);
         
            // Build a sprite with the tessellated geometry
            Sprite sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
            _objectUser.userInfo.spriteAvatar = sprite;
            Debug.Log("creado: " + _objectUser.userInfo.urlAvatar);
            Debug.Log("creado: " + _objectUser.userInfo.spriteAvatar);
            Debug.Log("creado: " + _objectUser.userInfo.haveAvatar);
            Debug.Log("creado");
            GameEvents.SuccessGetAvatar?.Invoke();

        }    
    }
    private string GenerateUrlToAvatar(AvatarInfo avatarInfo)
    {
        string url = $"https://avataaars.io/?avatarStyle=Circle";
        url += $"&topType={avatarInfo.top}";
        url += $"&accessoriesType={avatarInfo.accessories}";
        url += $"&hairColor={avatarInfo.hair_Color}";
        url += $"&facialHairType={avatarInfo.facial_Hair}";
        url += $"&facialHairColor={avatarInfo.facial_Hair_Color}";
        url += $"&clotheType={avatarInfo.clothes}";
        url += $"&clotheColor={avatarInfo.clothes_Color}";
        url += $"&eyeType={avatarInfo.eyes}";
        url += $"&eyebrowType={avatarInfo.eyesbrow}";
        url += $"&mouthType={avatarInfo.mouth}";
        url += $"&skinColor={avatarInfo.skin}";
        return url;
    }
    public void PostLogin(string username, string password)
    {
        StartCoroutine(PostLoginRoutine(username, password));
        StartCoroutine(GetAvatar(username, password));
    }
    [ContextMenu(nameof(PostLogin))]
    public void PostLogin()
    {
        StartCoroutine(PostLoginRoutine("pruebaxjrivera@bsginstitute.net", "BSgrupo123"));
    }
}

[Serializable]
public class DataLogin
{
    public string username;
    public string password;
}
[Serializable]
public class DataLoginToCredentials
{
    public string username;
    public string clave;
}
[Serializable]
public class ResponseToAvatar
{
    public AvatarInfo avatar;
    public bool contieneAvatar;
}
[Serializable]
public class AvatarInfo
{
    public string top;
    public string accessories;
    public string hair_Color;
    public string facial_Hair;
    public string facial_Hair_Color;
    public string clothes;
    public string eyes;
    public string eyesbrow;
    public string mouth;
    public string skin;
    public string clothes_Color;
}

[Serializable]
public class DataGetUserGami
{
    public int IdAlumno;
}