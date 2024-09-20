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
    private UI_Dialogue uiDialogue;


	public override bool Init()
    {
        if (base.Init() == false)
            return false;
        

        if (fadeEffectCoroutine == null)
        {
            fadeEffectCoroutine = StartCoroutine(IfadeOutInEffect(2f));
        }

		uiDialogue = null;

        Managers.Sound.StopBgm();
        StartCoroutine(ReadyToStopBgm());
        exitbutton.SetActive(false);

        return true;
    }

    IEnumerator ReadyToStopBgm()
    {
        yield return new WaitForSeconds(0.75f);
        Time.timeScale = 0.0f;
        StartCoroutine(ReadyToPlay());
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
        string[] scripts = { "�������� ���� ���ƿ��� �ʱ⸦ �ٶ��", "�������� ���� ������ �����ϰ�", "�������� ���ƿ��� ���� ���� ��ٸ���."
        , "�׷��� �� ��� ���� ���� ã�� �ڸ��� ������ �� ������.", "���� �������� �ϴ� ��, ���� �տ� �鸰 ����� �������� ������ ���̴�."};
        UIParam dialogueParam = new UIDialogueParam("???", scripts);

		uiDialogue = Managers.UI.OpenPopupUI<UI_Dialogue>(dialogueParam);

        exitbutton.SetActive(true);
    }

    public void OnClickExitButton()
    {
        if (uiDialogue != null)
        {
            uiDialogue.ClosePopupUI();
        }
        Time.timeScale = 1.0f;
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
