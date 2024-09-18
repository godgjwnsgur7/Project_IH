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
	[SerializeField] public GameObject exitbutton;

	void Awake()
	{
		StartCoroutine(ReadyToPlay());
		exitbutton.SetActive(false);
	}

	IEnumerator ReadyToPlay()
	{
		Time.timeScale = 0.0f;
		LinkedVideo.Prepare();

		while (!LinkedVideo.isPrepared)
		{
			yield return null;
		}

		LinkedVideo.loopPointReached += OnEndMovie;
		LinkedVideo.Play();

		MovieScreen.texture = LinkedVideo.texture;
		Time.timeScale = 1.0f;
	}

	void OnEndMovie(VideoPlayer vp)
	{
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

}
