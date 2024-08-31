using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.Rendering;

public class A : MonoBehaviour
{
    float speed;
    Coroutine coASDF = null;

    float useMP = 0f;
    float _currMP = 0;
    public float CurrMP
    {
        get { return _currMP; }
        protected set
        {
            if(coASDF == null)
            {
                coASDF = StartCoroutine(CoASDF());
            }

            useMP = _currMP - value;
            _currMP = value;
        }
    }

    private void OnDisable()
    {
        if (coASDF != null)
            StopCoroutine(coASDF);
    }
    
    /// <summary>
    /// UI Bar를 갱신시키는 코루틴
    /// </summary>
    private IEnumerator CoASDF()
    {
        while (useMP > 0f)
        {

            yield return null;
        }

        coASDF = null; // 코루틴이 끝났음을 알리는 용도.

        yield return null;
    }

    
    private void ChangeASDF()
    {
        CurrMP -= 10;
    }
}

