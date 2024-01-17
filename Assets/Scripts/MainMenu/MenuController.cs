using System;
using System.Linq;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectUser _user;
        [SerializeField] private ScriptableObjectSettings _objectSettings ;
        [SerializeField] private ScriptableObjectInstructor _objectInstructor ;
        [SerializeField] private TextMeshProUGUI _username;
        [SerializeField] private TextMeshProUGUI _totalExpirience;
        [SerializeField] private TextMeshProUGUI _totalCoins;
        [SerializeField] private TMP_InputField _userInputField;
        // [SerializeField] private Slider _soundEffects;
        // [SerializeField] private Slider _music;
        [SerializeField] private NotificationToggle _notificationToggle;
        [SerializeField] private Transform _pointToInstantiate;
        [SerializeField] private UnityEvent _onSuccessSaveInformation;
        [Header("notificators")] [SerializeField]
        private ScriptableObjectNotificationText _notificationTextUsername;

        private GameObject _instructorInstantiated;
        private void Start()
        {
            // if (PlayerPrefs.HasKey("settingInfo"))
            // {
            //     _objectSettings.settingData = JsonUtility.FromJson<ScriptableObjectSettings.SettingData>(PlayerPrefs.GetString("settingInfo"));
            // }
            PathToInstantiateInstructor();
            SetUserProperties();
            SetConfigurationProperties();
        }

        private void OnEnable()
        {
            GameEvents.NewInstuctorId += GameEvents_InstructorChanged;
        }

        private void GameEvents_InstructorChanged(int obj)
        {
            ChangeInstructor();
        }

        public void PathToInstantiateInstructor()
        {
            var indexInstructor = _user.userInfo.idInstructor;
            _instructorInstantiated =Instantiate(_objectInstructor.instructors.FirstOrDefault(x => x.id == indexInstructor)!.prefab,
                _pointToInstantiate.position, _pointToInstantiate.rotation, _pointToInstantiate);
            _instructorInstantiated.layer = 0;
        }

        public void ChangeInstructor()
        {
            var indexInstructor = _user.userInfo.idInstructor;
            Destroy(_instructorInstantiated);
            _instructorInstantiated =Instantiate(_objectInstructor.instructors.FirstOrDefault(x => x.id == indexInstructor)!.prefab,
                _pointToInstantiate.position, _pointToInstantiate.rotation);
            _instructorInstantiated.layer = 0;

        }

        public void SetUserProperties()
        {
            _userInputField.text = _user.userInfo.username;
            _username.text = _user.userInfo.username;
            _totalCoins.text = _user.userInfo.totalCoins.ToString();
            _totalExpirience.text = _user.userInfo.totalExperience.ToString();
        }

        public void SetConfigurationProperties()
        {
            // _soundEffects.value = _objectSettings.settingData.soundEffectVolume;
            // _music.value = _objectSettings.settingData.musicVolume;
            _notificationToggle.ActiveNotification = _objectSettings.settingData.haveNotification;
            _notificationToggle.InitToggle();

        }

        public void SaveInformation()
        {
            _user.userInfo.username = _userInputField.text;
            // _objectSettings.settingData.musicVolume = _music.value;
            // _objectSettings.settingData.soundEffectVolume = _soundEffects.value;
            _objectSettings.settingData.haveNotification = _notificationToggle.ToggleNotification.isOn;
            SaveUserInformation();
            SaveSettingInformation();
            _onSuccessSaveInformation?.Invoke();
        }

        public void SaveInstructor()
        {
            
        }
        public void SaveUserInformation()
        {
            PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_user.userInfo));
            PlayerPrefs.Save();

            _notificationTextUsername.Raise(_user.userInfo.username );
        }

        public void SaveSettingInformation()
        {
            PlayerPrefs.SetString("MusicVolume", JsonUtility.ToJson(_objectSettings.settingData));
            PlayerPrefs.SetString("SounEffectVolume", JsonUtility.ToJson(_objectSettings.settingData));
PlayerPrefs.Save();
        }
    }
}
