using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class settingsManager : MonoBehaviour
{

    [SerializeField]
    Slider sensitivitySlider, fieldOfViewSlider, masterVolumeSlider, effectVolumeSlider, musicVolumeSlider;
    [SerializeField]
    InputField sensitivityField, fieldOfViewField, masterVolumeField, effectVolumeField, musicVolumeField;
    [SerializeField]
    Toggle showTimerToggle;
    [SerializeField]
    Image fadePanel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateEffectVolumeFields(PlayerPrefs.GetFloat("effectVolume", 1.0f));
        UpdateMasterVolumeFields(PlayerPrefs.GetFloat("masterVolume", 1.0f));
        UpdateMusicVolumeFields(PlayerPrefs.GetFloat("musicVolume", 1.0f));
        UpdateSensitivityFields(PlayerPrefs.GetFloat("sensitivity", 1.0f));
        UpdateFieldOfViewFields(PlayerPrefs.GetFloat("fieldOfView", 80.0f));
        UpdateShowTimerField(PlayerPrefs.GetInt("showTimer", 1) == 1);
        Tween t = fadePanel.DOFade(0.0f, 0.5f).SetEase(Ease.InOutCubic);
        DOTween.Play(t);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        DOTween.Play(DOTween.Sequence().Join(fadePanel.DOFade(1.0f, 0.5f).SetEase(Ease.InOutCubic)).AppendCallback(() => { SceneManager.LoadScene("MainMenu"); }));
    }

    public void SetSensitivity(float _sensitivity)
    {
        float sensitivity = Mathf.Clamp(_sensitivity, sensitivitySlider.minValue, sensitivitySlider.maxValue);
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
        UpdateSensitivityFields(sensitivity);
    }

    public void SetSensitivity(string _sensitivity)
    {
        SetSensitivity(float.Parse(_sensitivity));
    }

    void UpdateSensitivityFields(float _sensitivity)
    {
        sensitivityField.text = _sensitivity.ToString();
        sensitivitySlider.value = _sensitivity;
    }

    public void SetShowTimer(bool _showTimer)
    {
        PlayerPrefs.SetInt("showTimer", _showTimer ? 1 : 0);
    }

    void UpdateShowTimerField(bool _showTimer)
    {
        showTimerToggle.isOn = _showTimer;
    }

    public void SetFieldOfView(float _fieldOfView)
    {
        float fieldOfView = Mathf.Clamp(_fieldOfView, fieldOfViewSlider.minValue, fieldOfViewSlider.maxValue);
        PlayerPrefs.SetFloat("fieldOfView", fieldOfView);
        UpdateFieldOfViewFields(fieldOfView);
    }

    public void SetFieldOfView(string _fieldOfView)
    {
        SetFieldOfView(float.Parse(_fieldOfView));
    }

    void UpdateFieldOfViewFields(float _fieldOfView)
    {
        fieldOfViewField.text = _fieldOfView.ToString();
        fieldOfViewSlider.value = _fieldOfView;
    }

    public void SetMasterVolume(float _masterVolume)
    {
        float masterVolume = Mathf.Clamp(_masterVolume, masterVolumeSlider.minValue, masterVolumeSlider.maxValue);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        UpdateMasterVolumeFields(masterVolume);
    }

    public void SetMasterVolume(string _masterVolume)
    {
        SetMasterVolume(float.Parse(_masterVolume));
    }

    void UpdateMasterVolumeFields(float _masterVolume)
    {
        masterVolumeField.text = _masterVolume.ToString();
        masterVolumeSlider.value = _masterVolume;
    }

    public void SetEffectVolume(float _effectVolume)
    {
        float effectVolume = Mathf.Clamp(_effectVolume, effectVolumeSlider.minValue, effectVolumeSlider.maxValue);
        PlayerPrefs.SetFloat("effectVolume", effectVolume);
        UpdateEffectVolumeFields(effectVolume);
    }

    public void SetEffectVolume(string _effectVolume)
    {
        SetEffectVolume(float.Parse(_effectVolume));
    }

    void UpdateEffectVolumeFields(float _effectVolume)
    {
        effectVolumeField.text = _effectVolume.ToString();
        effectVolumeSlider.value = _effectVolume;
    }

    public void SetMusicVolume(float _musicVolume)
    {
        float musicVolume = Mathf.Clamp(_musicVolume, musicVolumeSlider.minValue, musicVolumeSlider.maxValue);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        UpdateMusicVolumeFields(musicVolume);
    }

    public void SetMusicVolume(string _musicVolume)
    {
        SetMusicVolume(float.Parse(_musicVolume));
    }

    void UpdateMusicVolumeFields(float _musicVolume)
    {
        musicVolumeField.text = _musicVolume.ToString();
        musicVolumeSlider.value = _musicVolume;
    }

}
