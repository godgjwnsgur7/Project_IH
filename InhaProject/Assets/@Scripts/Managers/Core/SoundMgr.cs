using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ESoundType
{
    BGM = 0,
    SFX = 1,
    MASTER = 2,
}

/// <summary>
/// "Resources/Sounds/BGM/"
/// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
/// </summary>
[Serializable]
public enum EBgmSoundType
{
    None = 0,

    BossMap,
    NormalMap,

    Max,
}

/// <summary>
/// "Resources/Sounds/SFX/"
/// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
/// </summary>
[Serializable]
public enum ESfxSoundType
{
    None = 0,

    UI_Result_Defeat,
    UI_Result_Victory,

    Max,
}

[Serializable]
public class VolumeData
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
    public bool isMasterMute;
    public bool isBgmMute;
    public bool isSfxMute;

    public VolumeData(float masterVolume, float bgmVolume, float sfxVolume, bool isMasterMute, bool isBgmMute, bool isSfxMute)
    {
        this.masterVolume = masterVolume;
        this.bgmVolume = bgmVolume;
        this.sfxVolume = sfxVolume;
        this.isMasterMute = isMasterMute;
        this.isBgmMute = isBgmMute;
        this.isSfxMute = isSfxMute;
    }
}

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ESoundType.MASTER]; // BGM, SFX
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // 키 : 파일경로

    Coroutine fadeOutInBGMCoroutine;
    Coroutine bgmStopCoroutine;

    VolumeData volumeData = null;

    AudioListener audioListener = null;

    readonly float fadeSoundTime = 0.75f;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(ESoundType)); // BGM, SFX
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[(int)ESoundType.BGM].loop = true; // BGM은 반복 무한 재생

            volumeData = PlayerPrefsController.Load_VolumeData();
            audioSources[(int)ESoundType.BGM].mute = volumeData.isBgmMute;
            audioSources[(int)ESoundType.SFX].mute = volumeData.isSfxMute;
        }
    }

    public void Clear()
    {
        audioListener = null;
    }

    public void SaveVolumeData()
    {
        PlayerPrefsController.Save_VolumeData(volumeData);
    }

    public VolumeData LoadVolumeData()
    {
        volumeData = PlayerPrefsController.Load_VolumeData();
        return volumeData;
    }

    public void SetVolumeData(VolumeData _volumeData)
    {
        volumeData = _volumeData;
        audioSources[(int)ESoundType.BGM].volume = volumeData.masterVolume * volumeData.bgmVolume;
        audioSources[(int)ESoundType.SFX].volume = volumeData.masterVolume * volumeData.sfxVolume;
    }

    /// <summary>
    /// volumeValue은 0 ~ 1 사이의 값으로 세팅해야 함
    /// </summary>
    public void SetSoundVolumeData(ESoundType soundType, float volumeValue)
    {
        switch (soundType)
        {
            case ESoundType.MASTER:
                volumeData.masterVolume = volumeValue;
                audioSources[(int)ESoundType.BGM].volume = volumeData.masterVolume * volumeData.bgmVolume;
                audioSources[(int)ESoundType.SFX].volume = volumeData.masterVolume * volumeData.sfxVolume;
                break;
            case ESoundType.BGM:
                volumeData.bgmVolume = volumeValue;
                audioSources[(int)ESoundType.BGM].volume = volumeData.masterVolume * volumeData.bgmVolume;
                break;
            case ESoundType.SFX:
                volumeData.sfxVolume = volumeValue;
                audioSources[(int)ESoundType.SFX].volume = volumeData.masterVolume * volumeData.sfxVolume;
                break;
        }
    }

    public void SetSoundMuteData(ESoundType soundType, bool isMute)
    {
        switch (soundType)
        {
            case ESoundType.MASTER:
                volumeData.isMasterMute = isMute;
                audioSources[(int)ESoundType.BGM].mute = isMute;
                audioSources[(int)ESoundType.SFX].mute = isMute;
                break;
            case ESoundType.BGM:
                volumeData.isBgmMute = isMute;
                audioSources[(int)ESoundType.BGM].mute = isMute;
                break;
            case ESoundType.SFX:
                volumeData.isSfxMute = isMute;
                audioSources[(int)ESoundType.SFX].mute = isMute;
                break;
        }
    }

    public void PlayBgm(EBgmSoundType bgmType)
    {
        if (fadeOutInBGMCoroutine != null)
            CoroutineHelper.StopCoroutine(fadeOutInBGMCoroutine);

        // BGM 사운드 구현부는 코루틴 안에서 수행
        fadeOutInBGMCoroutine = CoroutineHelper.StartCoroutine(IFadeOutIn_BGM(bgmType));
    }

    public void StopBgm()
    {
        if (bgmStopCoroutine != null)
            CoroutineHelper.StopCoroutine(bgmStopCoroutine);

        bgmStopCoroutine = CoroutineHelper.StartCoroutine(IStop_BGM());
    }

    public void PlaySfx(ESfxSoundType sfxType)
    {
        float _currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        string path = $"{LoadPath.SOUND_SFX_PATH}/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return;

        audioSources[(int)ESoundType.SFX].volume = _currSfxVolume;
        audioSources[(int)ESoundType.SFX].PlayOneShot(audioClip);
    }

    private void SetBgmAudioSource(float currVolume)
    {
        audioSources[(int)ESoundType.BGM].volume = volumeData.masterVolume * currVolume;
    }

    private AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            audioClips.Add(path, audioClip);
        }

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Error! : {path}");
        }

        return audioClip;
    }

    private IEnumerator IStop_BGM()
    {
        yield return new WaitUntil(() => fadeOutInBGMCoroutine == null);

        float time = 0f;
        float currVolume = audioSources[(int)ESoundType.BGM].volume;

        if (audioSources[(int)ESoundType.BGM].isPlaying) // 실행 중일 경우
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0f)
            {
                time += Time.deltaTime / fadeSoundTime;
                volume = Mathf.Lerp(currVolume, 0, time);
                SetBgmAudioSource(volume);

                yield return null;
            }
        }

        currVolume = 0.0f;
        SetBgmAudioSource(currVolume);
        audioSources[(int)ESoundType.BGM].Stop();

        bgmStopCoroutine = null;
    }

    private IEnumerator IFadeOutIn_BGM(EBgmSoundType bgmType)
    {
        yield return new WaitUntil(() => bgmStopCoroutine == null);

        float time = 0f;
        float _currBgmVolume = volumeData.masterVolume * volumeData.bgmVolume;
        float currVolume = audioSources[(int)ESoundType.BGM].volume;

        string path = $"{LoadPath.SOUND_BGM_PATH}/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioSources[(int)ESoundType.BGM].isPlaying)
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0)
            {
                time += Time.deltaTime / fadeSoundTime;
                volume = Mathf.Lerp(currVolume, 0, time);
                SetBgmAudioSource(volume);

                yield return null;
            }
        }

        time = 0f;
        currVolume = 0.0f;
        SetBgmAudioSource(0.0f);

        if (audioClip != null)
            audioSources[(int)ESoundType.BGM].clip = audioClip;

        if (bgmType != EBgmSoundType.None)
        {
            audioSources[(int)ESoundType.BGM].Play();

            // FadeIn
            while (currVolume < _currBgmVolume)
            {
                time += Time.deltaTime / fadeSoundTime;
                currVolume = Mathf.Lerp(0, _currBgmVolume, time);
                SetBgmAudioSource(currVolume);

                yield return null;
            }

            currVolume = _currBgmVolume;
            SetBgmAudioSource(currVolume);
        }
        else // Bgm이 None인 경우
        {
            currVolume = 0.0f;
            SetBgmAudioSource(currVolume);
            audioSources[(int)ESoundType.BGM].Stop();
        }

        fadeOutInBGMCoroutine = null;
    }
}
