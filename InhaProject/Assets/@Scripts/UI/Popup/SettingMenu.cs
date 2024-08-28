using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class SettingMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    GameObject soundMenu;
    GameObject displayMenu;

    void Start()
    {
        soundMenu = GameObject.Find("SoundSetting");
        displayMenu = GameObject.Find("DisplaySetting");

        
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void OnClickSetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void OnClickSetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }
    public void OnClickSetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void OnClickDisplayWindow(bool DisplayWindow)
    {
        Debug.Log("디스플레이 윈도우");
        displayMenu.SetActive(true);
        soundMenu.SetActive(false);
    }


    public void OnClickSoundWindow(bool SoundWindow)
    {
        Debug.Log("사운드 윈도우");
        displayMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

 #region Sound Setting

    // Sound Control
    public AudioMixer audioMixer;
    public void OnClickSetMaster(float Volume)
    {
        audioMixer.SetFloat("Master", Volume);
    }
    public void OnClickSetBGM(float Volume)
    {
        audioMixer.SetFloat("BGM", Volume);
    }
    public void OnClickSetEffect(float Volume)
    {
        audioMixer.SetFloat("Effect", Volume);
    }

    // Sound Text
    public Slider masterSlider, bgmSlider, effectSlider;
    public TextMeshProUGUI mastersliderText, bgmsliderText, effectsliderText;
    float a = 1.25f;
    public void MasterSlider()
    {
        mastersliderText.text = Mathf.RoundToInt((masterSlider.value +80) * a).ToString();
    }
    public void BGMSlider()
    {
        bgmsliderText.text = Mathf.RoundToInt((bgmSlider.value + 80) * a).ToString();
    }
    public void EffectSlider()
    {
        effectsliderText.text = Mathf.RoundToInt((effectSlider.value + 80) * a).ToString();
    }

    // Sound Mute
    public void OnClickSetMasterMuteButton(float Volume, bool On)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Volume) * 0);
    }
    
    public void OnClickSetBGMMuteButton(float Volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(Volume) * 0);
    }

    public void OnClickSetEffectMuteButton(float Volume)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(Volume) * 0);
    }

}
#endregion