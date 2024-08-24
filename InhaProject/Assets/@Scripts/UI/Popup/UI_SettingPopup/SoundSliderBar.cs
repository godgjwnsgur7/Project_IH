using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderBar : InitBase
{
    Action<float, int> onSliderChange;
    Action<bool, int> onToggleChange;
    Slider slider;
    [SerializeField] Toggle toggle;
    int id;

    bool _isToggle;
    public bool IsToggle 
    {
        get { return _isToggle; }
        private set
        {
            _isToggle = value;
            toggle.isOn = _isToggle;
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        slider = this.transform.GetComponent<Slider>();
        toggle = Util.FindChild<Toggle>(this.gameObject);
        slider.onValueChanged.AddListener(OnSliderChanged);
        toggle.onValueChanged.AddListener(OnToggleChanged);
        IsToggle = false; 

        return true;
    }

    public void SetInfo(Action<float, int> onSliderChange, Action<bool, int> onToggleChange,  int id)
    {
        this.onSliderChange = onSliderChange;
        this.onToggleChange = onToggleChange;
        this.id = id;
        
        // 자신의 데이터를 받아와서 적용
        // 뮤트 상태, 볼륨 상태를 받아서 세팅해줌
    }

    public void SetToggle(bool isOn)
    {
        IsToggle = isOn;
    }

    public void ChagneToggle()
    {
        IsToggle = !IsToggle;
    }

    private void OnSliderChanged(float value)
    {
        onSliderChange?.Invoke(value, id);
    }

    private void OnToggleChanged(bool value)
    {
        onToggleChange?.Invoke(value, id);
    }
}
