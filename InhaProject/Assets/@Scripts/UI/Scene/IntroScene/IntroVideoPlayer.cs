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

    float time = 0;
	bool isPrepare = false;

	void Awake()
	{
		string moviePath = "Vedio/Intro";
        videoClip = Managers.Resource.Load<VideoClip>(moviePath);
		if (null == videoClip)
		{
			Debug.LogError("[MovieSceneDirector::Awake]not found movie - " + moviePath);
			return;
		}
		time = 0;
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

		isPrepare = true;
	}

	void OnEndMovie(VideoPlayer vp)
	{
		Managers.Scene.LoadScene(Define.EScene.TitleScene);
	}
}
