using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public const string SFX_VOLUME = "SFXVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string MASTER_VOLUME = "MasterVolume";

    [SerializeField] private AudioSettingsSO _audioSettings;
    [SerializeField] private IncorrectQuestionsSO _incorrectQuestions;
    [SerializeField] private AudioSource _sFXAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    public AudioSettingsSO AudioSettings => _audioSettings;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {

        AudioEvents.SFXVolumeChanged += AudioEvents_OnSFXVolumeChanged;
        AudioEvents.MusicVolumeChanged += AudioEvents_OnMusicVolumeChanged;
        AudioEvents.MasterVolumeChanged += AudioEvents_OnMasterVolumeChanged;
    }

    private void OnDisable()
    {
        AudioEvents.SFXVolumeChanged -= AudioEvents_OnSFXVolumeChanged;
        AudioEvents.MusicVolumeChanged -= AudioEvents_OnMusicVolumeChanged;
        AudioEvents.MasterVolumeChanged -= AudioEvents_OnMasterVolumeChanged;
    }

    private void AudioEvents_OnMasterVolumeChanged(float volume)
    {
        float decibelVolume = ConvertLinearToDecibel(volume);
        _audioSettings.AudioMixer.SetFloat(MASTER_VOLUME, decibelVolume);
    }

    private void AudioEvents_OnSFXVolumeChanged(float volume)
    {
        float decibelVolume = ConvertLinearToDecibel(volume);
        _audioSettings.AudioMixer.SetFloat(SFX_VOLUME, decibelVolume);
    }

    void Initialize()
    {
        if (PlayerPrefs.HasKey("SounEffectVolume"))
        {
            AudioEvents_OnSFXVolumeChanged(PlayerPrefs.GetFloat("SounEffectVolume"));
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            AudioEvents_OnMusicVolumeChanged(PlayerPrefs.GetFloat("MusicVolume"));
        }
        
    }

    void AudioEvents_OnMusicVolumeChanged(float volume)
    {
        float decibelVolume = ConvertLinearToDecibel(volume);
        _audioSettings.AudioMixer.SetFloat(MUSIC_VOLUME, decibelVolume);
    }

    public void PlaySFXAtPoint(AudioClip audioClip, Vector3 position, float delay = 0, bool loop = false)
    {
        _sFXAudioSource.Stop();
        StartCoroutine(PlaySFXAtPointDelayed(audioClip, position, delay, loop));
    }
    public void PlayMusic(AudioClip audioClip, Vector3 position, float delay = 0, bool loop = false)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.PlayOneShot(audioClip);
        // StartCoroutine(PlaySFXAtPointDelayed(audioClip, position, delay, loop));
    }

    IEnumerator PlaySFXAtPointDelayed(AudioClip audioClip, Vector3 position, float delay, bool loop)
    {
        yield return new WaitForSeconds(delay);
        _sFXAudioSource.loop = loop;
        if (audioClip)
        {
            _sFXAudioSource.PlayOneShot(audioClip);
        }
        else
        {
            _sFXAudioSource.Stop();
        }
    }
    static float ConvertLinearToDecibel(float linearVolume)
    {
        return Mathf.Log10(Mathf.Max(0.0001f, linearVolume)) * 20.0f;
    }
    static float ConvertDecibelToLinear(float decibelVolume)
    {
        return Mathf.Pow(10, decibelVolume / 20f);
    }
}
