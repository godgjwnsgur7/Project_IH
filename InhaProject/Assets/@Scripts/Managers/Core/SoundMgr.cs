using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundData
{
    public float a, b, c;
}

public class SoundMgr
{
    // 플레이어플랩스 사용 예제 코드 입니다.

    SoundData soundData = null;

    public void Init()
    {
        soundData = LoadSoundData();

        if(soundData == null )
        {
            // 기본 값 설정
        }
    }

    private void SaveSoundData(SoundData soundData)
    {
        PlayerPrefs.SetFloat("SoundData_a", soundData.a);
        PlayerPrefs.SetFloat("SoundData_b", soundData.b);
        PlayerPrefs.SetFloat("SoundData_c", soundData.c);

        PlayerPrefs.Save();
    }

    private SoundData LoadSoundData()
    {
        if (PlayerPrefs.HasKey("SoundData_a") == false)
            return null;

        SoundData soundData = new SoundData();

        soundData.a = PlayerPrefs.GetFloat("SoundData_a", soundData.a);
        soundData.b = PlayerPrefs.GetFloat("SoundData_b", soundData.b);
        soundData.c = PlayerPrefs.GetFloat("SoundData_c", soundData.c);

        return soundData;
    }
}
