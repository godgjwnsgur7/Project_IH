using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI_GameSettingPopup
public class UI_SettingPopup : InitBase // UI_BasePopup -> ��ӹ����� �������� ������ ��
{
    [SerializeField] SoundSettingMenu soundSettingMenu;
    [SerializeField] DisplaySettingMenu displaySettingMenu;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }


}
