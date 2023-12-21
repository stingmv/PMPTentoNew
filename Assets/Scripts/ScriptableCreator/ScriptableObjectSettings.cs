using System;
using UnityEngine;

namespace ScriptableCreator
{
    [CreateAssetMenu(fileName = "Game setting ", menuName = "Game setting")]
    public class ScriptableObjectSettings : ScriptableObject
    {
        [Serializable]
        public struct SettingData
        {
            public float musicVolume;
            public float soundEffectVolume;
            public bool haveNotification;
        }

        public SettingData settingData;

    }
}
