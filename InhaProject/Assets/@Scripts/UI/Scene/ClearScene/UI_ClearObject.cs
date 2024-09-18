using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_ClearObject : UI_BaseObject
{
    [SerializeField] public Image fadeEffectImage;
    private Coroutine fadeEffectCoroutine = null;

    public RawImage MovieScreen;
    public VideoPlayer LinkedVideo;
    [SerializeField] VideoClip videoClip;
    [SerializeField] public GameObject exitbutton;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Sound.StopBgm();
        Time.timeScale = 0.0f;

        if (fadeEffectCoroutine == null)
        {
            fadeEffectCoroutine = StartCoroutine(IfadeOutInEffect(2f));
        }

        StartCoroutine(ReadyToPlay());
        exitbutton.SetActive(false);

        return true;
    }

    IEnumerator ReadyToPlay()
    {
        LinkedVideo.Prepare();

        while (!LinkedVideo.isPrepared)
        {
            yield return null;
        }

        LinkedVideo.loopPointReached += OnEndMovie;
        LinkedVideo.Play();

        MovieScreen.texture = LinkedVideo.texture;
    }


    void OnEndMovie(VideoPlayer vp)
    {
        Time.timeScale = 1.0f;
        string[] scripts = { "누군가는 신이 돌아오지 않기를 바라고", "누군가는 신의 죽음을 날조하고", "누군가는 돌아오지 않을 신을 기다린다."
        , "그러나 이 모든 것은 신을 찾는 자만이 시작할 수 있으니.", "신을 만나고자 하는 자, 그의 손에 들린 고대의 흔적에서 시작할 것이다."};
        UIParam dialogueParam = new UIDialogueParam("???", scripts);

        Managers.UI.OpenPopupUI<UI_Dialogue>(dialogueParam);

        exitbutton.SetActive(true);
    }

    public void OnClickExitButton()
    {
        Managers.Scene.LoadScene(Define.EScene.TitleScene);
    }

    public void OnClickSkipButton()
    {
        fadeEffectImage.color = new Color(0, 0, 0, 1);
        LinkedVideo.frame = (long)LinkedVideo.frameCount;
    }

    private IEnumerator IfadeOutInEffect(float fadeTime)
    {
        fadeEffectImage.color = new Color(0, 0, 0, 1);
        Color tempColor = fadeEffectImage.color;

        while (tempColor.a > 0.01f)
        {
            tempColor.a -= 0.01f / fadeTime;
            fadeEffectImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 0f;
        fadeEffectImage.color = tempColor;
    }
}
