using System;
using System.Collections;
using System.Linq;
using ScriptableCreator;
using TMPro;
using Unity.VectorGraphics;
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

        [SerializeField] private SVGImage _mainMenuAvatar;
        [SerializeField] private SVGImage _storeAvatar;
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
            if (_user.userInfo.haveAvatar)
            {
                _mainMenuAvatar.sprite = _user.userInfo.spriteAvatar;
                _storeAvatar.sprite = _user.userInfo.spriteAvatar;    
            }
            else
            {
                _mainMenuAvatar.enabled = false;
                _storeAvatar.enabled = false;   
            }
        }

        private void OnEnable()
        {
            GameEvents.NewInstuctorId += GameEvents_InstructorChanged;
            GameEvents.CoinsChanged += GameEvents_CoinsChanged;
            GameEvents.ExperienceChanged += GameEvents_ExperienceChanged;
            GameEvents.UsernameSelected	 += GameEvents_UsernameSelected;
        }

        private void GameEvents_UsernameSelected()
        {
            _username.text = _user.userInfo.user.detail.usernameG;
        }

        private void OnDisable()
        {
            GameEvents.NewInstuctorId -= GameEvents_InstructorChanged;
            GameEvents.CoinsChanged -= GameEvents_CoinsChanged;
            GameEvents.ExperienceChanged -= GameEvents_ExperienceChanged;
            GameEvents.UsernameSelected	 -= GameEvents_UsernameSelected;
        }

        private void GameEvents_ExperienceChanged()
        {
            _totalExpirience.text = _user.userInfo.user.detail.totalExperience.ToString();
        }

        private void GameEvents_CoinsChanged()
        {
            _totalCoins.text = _user.userInfo.user.detail.totalCoins.ToString();
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
            StopAllCoroutines();
            StartCoroutine(IChangeInstructor(indexInstructor));
        }

        IEnumerator IChangeInstructor(int indexInstructor)
        {
            Debug.Log(_instructorInstantiated.name);
            Destroy(_instructorInstantiated);
            while (_instructorInstantiated)
            {
                yield return null;
                Debug.Log("Waiting");
            }
            _instructorInstantiated =Instantiate(_objectInstructor.instructors.FirstOrDefault(x => x.id == indexInstructor)!.prefab,
                _pointToInstantiate.position, _pointToInstantiate.rotation);
            _instructorInstantiated.layer = 0;
        }

        public void SetUserProperties()
        {
            _userInputField.text = _user.userInfo.user.detail.usernameG;
            _username.text = _user.userInfo.user.detail.usernameG;
            _totalCoins.text = _user.userInfo.user.detail.totalCoins.ToString();
            _totalExpirience.text = _user.userInfo.user.detail.totalExperience.ToString();
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
            // _user.userInfo.username = _userInputField.text;
            // _objectSettings.settingData.musicVolume = _music.value;
            // _objectSettings.settingData.soundEffectVolume = _soundEffects.value;
            _objectSettings.settingData.haveNotification = _notificationToggle.ToggleNotification.isOn;
            // SaveUserInformation();
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

            // _notificationTextUsername.Raise(_user.userInfo.username );
        }

        public void SaveSettingInformation()
        {
            PlayerPrefs.SetString("MusicVolume", JsonUtility.ToJson(_objectSettings.settingData));
            PlayerPrefs.SetString("SounEffectVolume", JsonUtility.ToJson(_objectSettings.settingData));
PlayerPrefs.Save();
        }
    }
}
