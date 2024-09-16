using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : BasePlatform
{
    public float disappearDuration = 1f; // 플랫폼이 사라지는 시간
    public float reappearDelay = 2f; // 플랫폼이 다시 나타나는 시간
    private Renderer render;
    private Color originalColor;
    private Color transparentColor;
    private bool isDisappearing = false;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        render = GetComponent<Renderer>();
        if (render != null)
        {
            originalColor = render.material.color;
            transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }

        // 콜라이더를 트리거로 설정
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDisappearing)
        {
            isDisappearing = true;
            StartCoroutine(DisappearAndReappearCoroutine());
        }
    }

    private IEnumerator DisappearAndReappearCoroutine()
    {
        // 사라지는 과정
        float elapsedTime = 0f;
        while (elapsedTime < disappearDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / disappearDuration);
            if (render != null)
                render.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 완전히 투명하게 만들기
        if (render != null)
            render.material.color = transparentColor;

        // 하위 객체를 비활성화
        SetChildrenActive(false);

        // 대기
        yield return new WaitForSeconds(reappearDelay);

        // 하위 객체를 다시 활성화
        SetChildrenActive(true);

        // 다시 나타나는 과정
        elapsedTime = 0f;
        while (elapsedTime < disappearDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / disappearDuration);
            if (render != null)
                render.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 원래 색상으로 복구
        if (render != null)
            render.material.color = originalColor;

        isDisappearing = false;
    }

    private void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}