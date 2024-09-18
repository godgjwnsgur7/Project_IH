using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : BasePlatform
{
    public float disappearDuration = 2f; // 플랫폼이 사라지는 시간
    public float reappearDelay = 2f; // 플랫폼이 다시 나타나는 시간
    private Color originalColor;
    private Color transparentColor;
    private bool isDisappearing = false;
    [SerializeField, ReadOnly] protected List<Renderer> rendererList;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        // 콜라이더를 트리거로 설정
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        rendererList = new List<Renderer>();
        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<ParticleSystem>() == null && child.TryGetComponent<Renderer>(out Renderer renderer))
                rendererList.Add(renderer);
        }

        // 원래 색상 및 투명 색상 초기화
        if (rendererList.Count > 0)
        {
            originalColor = rendererList[0].material.color;
            transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
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
            float alpha = 1f - elapsedTime / disappearDuration;
            foreach (Renderer render in rendererList)
            {
                if (render != null)
                    render.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 완전히 투명하게 만들기
        foreach (Renderer render in rendererList)
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
            float alpha = elapsedTime / disappearDuration;
            foreach (Renderer render in rendererList)
            {
                if (render != null)
                    render.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 원래 색상으로 복구
        foreach (Renderer render in rendererList)
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