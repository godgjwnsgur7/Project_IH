using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : BasePlatform
{
    public float disappearDuration = 2f; // �÷����� ������� �ð�
    public float reappearDelay = 2f; // �÷����� �ٽ� ��Ÿ���� �ð�
    private Color originalColor;
    private Color transparentColor;
    private bool isDisappearing = false;
    [SerializeField, ReadOnly] protected List<Renderer> rendererList;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        // �ݶ��̴��� Ʈ���ŷ� ����
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

        // ���� ���� �� ���� ���� �ʱ�ȭ
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
        // ������� ����
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

        // ���������� ������ �����ϰ� �����
        foreach (Renderer render in rendererList)
            if (render != null)
                render.material.color = transparentColor;

        // ���� ��ü�� ��Ȱ��ȭ
        SetChildrenActive(false);

        // ���
        yield return new WaitForSeconds(reappearDelay);

        // ���� ��ü�� �ٽ� Ȱ��ȭ
        SetChildrenActive(true);

        // �ٽ� ��Ÿ���� ����
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

        // ���������� ���� �������� ����
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