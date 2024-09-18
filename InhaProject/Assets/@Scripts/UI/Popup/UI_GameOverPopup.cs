using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverPopup : UI_BasePopup
{
    [SerializeField, ReadOnly] public Slider progress;
    [SerializeField] public Slider progress2;

    public float moveSpeed;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartCoroutine(CoStartFillProgress(progress));
        StartCoroutine(CoStartFillProgress(progress2));
        return true;
    }

    public void OnClickRestartButton()
    {
        Managers.Game.StartContinueGame();
    }

    public void OnClickExitButton()
    {
        Managers.Scene.LoadScene(Define.EScene.TitleScene);
    }

    private IEnumerator CoStartFillProgress(Slider silder)
    {
        while (silder.value <= 0.98f )
        {
            silder.value += Time.deltaTime / moveSpeed;
            yield return null;
        }

        silder.value = 1;

        yield return new WaitForSeconds(1f);
        StartCoroutine(CoStartEraseProgress(silder));
    }

    private IEnumerator CoStartEraseProgress(Slider silder)
    {
        while (silder.value >= 0.02f)
        {
            silder.value -= Time.deltaTime / moveSpeed;
            yield return null;
        }

        silder.value = 0;

        yield return new WaitForSeconds(1f);
        StartCoroutine(CoStartFillProgress(silder));
    }
}
