using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataUserAll", menuName = "ScriptableObjects/DataUserAll")]
public class DataUserAll : ScriptableObject
{
    [Serializable]
    public class DataUsers
    {
        public int id;
        public string userName;
        public int totalExperience;
        public string email;
        public string password;
    }

    public List<DataUsers> Users;

    private void OnEnable()
    {
        Users.Clear();
    }

    private void Reset()
    {
        Users.Clear();
    }
}
