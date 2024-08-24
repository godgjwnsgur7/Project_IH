using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingMenu : InitBase
{
    enum SoundBarId
    {
        masterSound = 0,
        bgmSound = 1,
        effectSound = 2,
    }

    [SerializeField] SoundSliderBar masterSoundBar;
    [SerializeField] SoundSliderBar bgmSoundBar;
    [SerializeField] SoundSliderBar effectSoundBar;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider effectSlider;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        masterSoundBar.SetInfo(OnSliderChanged, OnToggleChanged, 0);
        bgmSoundBar.SetInfo(OnSliderChanged, OnToggleChanged, 1);
        effectSoundBar.SetInfo(OnSliderChanged, OnToggleChanged, 2);

        //masterSlider.onValueChanged.AddListener(OnMasterValueChanged);
        //bgmSlider.onValueChanged.AddListener(OnBgmValueChanged);
        //effectSlider.onValueChanged.AddListener(OnEffectValueChanged);

        return true;
    }

    public void OnMasterValueChanged(float value)
    {
        Debug.Log($"마스터 바가 변경되었습니다. 값 : {value}");
    }
    public void OnBgmValueChanged(float value)
    {
        Debug.Log($"비지엠 바가 변경되었습니다. 값 : {value}");
    }

    public void OnEffectValueChanged(float value)
    {
        Debug.Log($"이펙트 바가 변경되었습니다. 값 : {value}");
    }

    public void OnSliderChanged(float value, int id)
    {
        if(id == 0)
        {
            Debug.Log($"마스터 바가 변경되었습니다. 값 : {value}");
        }
        else if (id == 1)
        {
            Debug.Log($"비지엠 바가 변경되었습니다. 값 : {value}");
        }
        else if(id == 2)
        {
            Debug.Log($"이펙트 바가 변경되었습니다. 값 : {value}");
        }

        // 세이브가 되기 전까지 변경 값이 생긴 것.
    }

    public void OnToggleChanged(bool value, int id)
    {
        if (id == 0)
        {
            // 마스터 토글 눌림
            masterSoundBar.ChagneToggle();

        }
        else if (id == 1)
        {
            bgmSoundBar.ChagneToggle();

        }
        else if (id == 2)
        {
            effectSoundBar.ChagneToggle();

        }

        // 세이브가 되기 전까지 변경 값이 생긴 것.
    }
}
