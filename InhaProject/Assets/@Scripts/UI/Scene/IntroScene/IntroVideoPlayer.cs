using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroVideoPlayer : MonoBehaviour
{
	public RawImage MovieScreen;
	public VideoPlayer LinkedVideo;

	float time = 0;
	bool isPrepare = false;

	void Awake()
	{
		// 재생할 동영상 파일 불러오기.
		//string moviePath = Application.dataPath + "/Arts/Vedio/Intro.mp4";
		//var videoClip = Resources.Load<VideoClip>(moviePath);
		//if (null == videoClip)
		//{
		//	Debug.LogError("[MovieSceneDirector::Awake]not found movie - " + moviePath);
		//	return;
		//}
		time = 0;
		StartCoroutine(ReadyToPlay());
	}

	IEnumerator ReadyToPlay()
	{
		//LinkedVideo.clip = clip;
		LinkedVideo.Prepare();

		while (!LinkedVideo.isPrepared)
		{
			yield return null;
		}

		MovieScreen.texture = LinkedVideo.texture;
		LinkedVideo.Play();

		isPrepare = true;
	}


	void Start()
    {

	}

    void Update()
    {
		if ( isPrepare)
		{
			time += Time.deltaTime;
			if ( time >= LinkedVideo.clockTime )
			{
				//Debug.Log("Video ");
				Managers.Scene.LoadScene(Define.EScene.TitleScene);
			}
		}
	}
}
