using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroVideoPlayer : MonoBehaviour
{
	public RawImage MovieScreen;
	public VideoPlayer LinkedVideo;
	[SerializeField] VideoClip videoClip;

	void Awake()
	{
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

		Managers.UI.OpenPopupUI<UI_Dialogue>(dialogueParam);
		// Managers.Scene.LoadScene(Define.EScene.TitleScene);
	}


}
