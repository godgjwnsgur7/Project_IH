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
        Debug.Log($"������ �ٰ� ����Ǿ����ϴ�. �� : {value}");
    }
    public void OnBgmValueChanged(float value)
    {
        Debug.Log($"������ �ٰ� ����Ǿ����ϴ�. �� : {value}");
    }

    public void OnEffectValueChanged(float value)
    {
        Debug.Log($"����Ʈ �ٰ� ����Ǿ����ϴ�. �� : {value}");
    }

    public void OnSliderChanged(float value, int id)
    {
        if(id == 0)
        {
            Debug.Log($"������ �ٰ� ����Ǿ����ϴ�. �� : {value}");
        }
        else if (id == 1)
        {
            Debug.Log($"������ �ٰ� ����Ǿ����ϴ�. �� : {value}");
        }
        else if(id == 2)
        {
            Debug.Log($"����Ʈ �ٰ� ����Ǿ����ϴ�. �� : {value}");
        }

        // ���̺갡 �Ǳ� ������ ���� ���� ���� ��.
    }

    public void OnToggleChanged(bool value, int id)
    {
        if (id == 0)
        {
            // ������ ��� ����
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

        // ���̺갡 �Ǳ� ������ ���� ���� ���� ��.
    }
}
