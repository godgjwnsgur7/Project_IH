using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPrefsController : MonoBehaviour
{
    #region PlayerPrefs_SetGet
    // PlayerPrefs Set �迭 �Լ���. keyName_keyType�� �̸����� ������
    private static void Set_Float(float value, string keyName, string keyType)
        => PlayerPrefs.SetFloat($"{keyName}_{keyType}", value);
    private static void Set_String(string value, string keyName, string keyType)
        => PlayerPrefs.SetString($"{keyName}_{keyType}", value);
    private static void Set_Int(int value, string keyName, string keyType)
        => PlayerPrefs.SetInt($"{keyName}_{keyType}", value);

    // PlayerPrefs Get �迭 �Լ���. keyName_keyType�� �̸����� �ҷ���
    private static float Get_Float(string keyName, string keyType)
        => PlayerPrefs.GetFloat($"{keyName}_{keyType}");
    private static string Get_String(string keyName, string keyType)
        => PlayerPrefs.GetString($"{keyName}_{keyType}");
    private static int Get_Int(string keyName, string keyType)
        => PlayerPrefs.GetInt($"{keyName}_{keyType}");

    // PlayerPrefs Delect Key �Լ�. ����� keyName_keyType�� �̸����� ���� 
    private static void Delete_Key(string keyName, string keyType)
    {
        PlayerPrefs.DeleteKey($"{keyName}_{keyType}");
    }
    #endregion

    public static void DeleteDataAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static bool Save_VolumeData(VolumeData volumeData)
    {
        if (volumeData == null)
        {
            Debug.Log("soundDatas Null�Դϴ�.");
            return false;
        }

        Set_Float(volumeData.masterVolume, "Volume", "Master");
        Set_Float(volumeData.masterVolume, "Volume", "Bgm");
        Set_Float(volumeData.masterVolume, "Volume", "Sfx");
        Set_Int(volumeData.isBgmMute ? 1 : 0, "Mute", "Master");
        Set_Int(volumeData.isBgmMute ? 1 : 0, "Mute", "Bgm");
        Set_Int(volumeData.isSfxMute ? 1 : 0, "Mute", "Sfx");

        PlayerPrefs.Save();
        Managers.Sound.SetVolumeData(volumeData);
        return true;
    }

    public static VolumeData Load_VolumeData()
    {
        // ����, ����� �����Ͱ� ���ٸ� �⺻ ������ ���� �� ����
        if (!PlayerPrefs.HasKey("Volume_Master"))
        {
            VolumeData tempVolumeData = new VolumeData(1.0f, 0.5f, 1.0f, false, false, false); // �⺻ ��
            Save_VolumeData(tempVolumeData);
            return tempVolumeData;
        }

        float wholeVolume = Get_Float("Volume", "Master");
        float bgmVolume = Get_Float("Volume", "Bgm");
        float sfxVolume = Get_Float("Volume", "Sfx");

        bool isMasterMute = Get_Int("Mute", "Master") == 1;
        bool isBgmMute = Get_Int("Mute", "Bgm") == 1;
        bool isSfxMute = Get_Int("Mute", "Sfx") == 1;

        VolumeData volumeData = new VolumeData(wholeVolume, bgmVolume, sfxVolume, isMasterMute, isBgmMute, isSfxMute);

        return volumeData;
    }
}
