using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [Space]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown screenDropdown;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider brightnessSlider;

    List<string> qualityOptions = new List<string>();
    List<string> resolutionOptions = new List<string>();
    List<string> screenOptions = new List<string>();

    [Header("Audio")]
    [Space]
    [SerializeField] AudioMixer audioMixer;

    [Space]
    [SerializeField] float defaultMasterVolume = .5f;
    [SerializeField] float defaultMusicVolume = .5f;
    [SerializeField] float defaultSFXVolume = .5f;
    [SerializeField] float defaultBrightnessVolume = 1f;

    float maVolume = 0;
    float muVolume = 0;
    float sfxVolume = 0;
    float bValue = 0;

    [Header("Screen")]
    [Space]
    [SerializeField] Vector2[] supportedRes;
    [SerializeField] Volume volume;

    LiftGammaGain liftGammaGain;

    private void Awake()
    {
        maVolume = CheckFloatKey("maVolume", defaultMasterVolume);
        muVolume = CheckFloatKey("muVolume", defaultMusicVolume);
        sfxVolume = CheckFloatKey("sfxVolume", defaultSFXVolume);
        bValue = CheckFloatKey("bVolume", defaultBrightnessVolume);
    }

    private void Start()
    {
        //ResetDropdown(qualityOptions, qualityDropdown, 1);
        ResetDropdown(resolutionOptions, resolutionDropdown, 2);
        ResetDropdown(screenOptions, screenDropdown, 3);

        UpdateSliders();
    }

    float CheckFloatKey(string key, float defaultVaule)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return defaultVaule;
        }
    }

    void UpdateSliders()
    {
        SetMasterVolume(maVolume);
        masterSlider.value = maVolume;
        SetMusicVolume(muVolume);
        musicSlider.value = muVolume;
        SetSFXVolume(sfxVolume);
        sfxSlider.value = sfxVolume;
        SetBrightness(bValue);
        brightnessSlider.value = bValue;
    }

    void ResetDropdown(List<string> sList, TMP_Dropdown dp, int i)
    {
        switch (i)
        {
            case 1:

                foreach (string s in QualitySettings.names)
                {
                    sList.Add(s);
                }

                dp.ClearOptions();
                dp.AddOptions(sList);

                break;
            case 2:

                foreach (Vector2 sr in supportedRes)
                {
                    sList.Add(sr.x + " x " + sr.y);
                }

                dp.ClearOptions();
                dp.AddOptions(sList);

                break;
            case 3:

                FullScreenMode[] screenMode = { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed };

                foreach (FullScreenMode y in screenMode)
                {
                    sList.Add(y.ToString());
                }

                dp.ClearOptions();
                dp.AddOptions(sList);

                break;
        }
    }

    public void SetResolutionDropdown(int res)
    {
        Screen.SetResolution(Mathf.RoundToInt(supportedRes[res].x), Mathf.RoundToInt(supportedRes[res].x), false);
    }

    public void SetScreenDropdown(int type)
    {
        switch (type)
        {
            case 0:

                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

                break;

            case 1:

                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

                break;
            case 2:

                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;

                break;
            case 3:

                Screen.fullScreenMode = FullScreenMode.Windowed;

                break;
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("maVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("muVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetBrightness(float value)
    {
        volume.profile.TryGet(out liftGammaGain);

        liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, value));

        PlayerPrefs.SetFloat("bVolume", value);
    }
}
