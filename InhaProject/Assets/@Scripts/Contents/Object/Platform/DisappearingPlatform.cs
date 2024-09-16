using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : BasePlatform
{
    public float disappearDuration = 1f; // �÷����� ������� �ð�
    public float reappearDelay = 2f; // �÷����� �ٽ� ��Ÿ���� �ð�
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

        // �ݶ��̴��� Ʈ���ŷ� ����
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
        // ������� ����
        float elapsedTime = 0f;
        while (elapsedTime < disappearDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / disappearDuration);
            if (render != null)
                render.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ������ �����ϰ� �����
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
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / disappearDuration);
            if (render != null)
                render.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������� ���� �������� ����
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