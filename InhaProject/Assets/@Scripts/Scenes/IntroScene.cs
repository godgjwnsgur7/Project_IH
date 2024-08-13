using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroScene : BaseScene
{
	private float time;
	private VideoPlayer video;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = Define.EScene.IntroScene;

		return true;
	}

	private void Start()
	{
		time = 0f;
	}

	private void Update()
	{
		time += Time.deltaTime;
		//if ( time > )
	}

	public override void Clear()
	{

	}
}
