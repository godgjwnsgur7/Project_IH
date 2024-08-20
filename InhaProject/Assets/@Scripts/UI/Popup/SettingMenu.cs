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

    GameObject settingMenu;
    GameObject displayMenu;

    void Start()
    {
        //ResolutionDropdown = TMP_Dropdown.FindObjectOfType<TMP_Dropdown>();

        settingMenu = GameObject.Find("SoundSetting");
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
        Debug.Log("디스플레이윈도우버튼클릭");
        displayMenu.SetActive(true);
        settingMenu.SetActive(false);
    }


    public void OnClickSoundWindow(bool SoundWindow)
    {
        Debug.Log("사운드윈도우버튼클릭");
        displayMenu.SetActive(true);
        settingMenu.SetActive(true);
    }

    public AudioMixer audioMixer;
    public void OnClickSetMaster(float Volume)
    {
        audioMixer.SetFloat("Master", Volume);
        audioMixer.SetFloat("BGM", Volume);
        audioMixer.SetFloat("Effect", Volume);
    }
    public void OnClickSetBGM(float Volume)
    {
        audioMixer.SetFloat("BGM", Volume);
    }
    public void OnClickSetEffect(float Volume)
    {
        audioMixer.SetFloat("Effect", Volume);
    }









}
