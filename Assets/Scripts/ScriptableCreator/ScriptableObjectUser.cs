using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public string id;
    public int idProveedor;
    public int cursos;
    public int idAlumno;
    public int idContacto;
    public string clave;
    public string userName;
    public string token;
    public bool tipoCarrera;
    public string correoEnc;
    public string telEnc;
    public string userAgent;
    public string userIp;
    public Excepcion excepcion;
}
[Serializable]
public class Excepcion
{
    public bool excepcionGenerada;
    public string descripcionGeneral;
    // public string error;
}

[Serializable]
public class UserInfo
{
    public User user;
    public bool haveUser;
    public bool haveUsername;
    public string username;
    public bool haveInstructor;
    public int idInstructor;
    public int totalExperience;
    public int totalCoins;
}
[CreateAssetMenu(fileName = "User Data", menuName = "User data")]
public class ScriptableObjectUser : ScriptableObject
{
    public UserInfo userInfo;

}
