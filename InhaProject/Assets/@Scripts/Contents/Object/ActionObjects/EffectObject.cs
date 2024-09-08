using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum EEffectObjectType
{
    PlayerHitEffect,
    PlayerHitBlockEffect,
    MonsterHitEffect, // �̻��
}

public class EffectObject : InitBase, Poolable
{
    [SerializeField, ReadOnly] float maxDuration;
    public bool IsUsing { get; set; }
    public GameObject GameObject { get { return this.gameObject; } }

    Coroutine coDestroySelf;

    [System.Obsolete]
    private void Reset()
    {
        Transform[] myChildren = this.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {
            if(child.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
            {
                float time = (particle.duration + particle.startSpeed);
                maxDuration = Mathf.Max(maxDuration, time);
            }
        }
        Debug.Log(maxDuration);
    }

    private void OnEnable()
    {
        coDestroySelf = StartCoroutine(CoDestroySelf());
    }

    private void OnDisable()
    {
        if (coDestroySelf != null)
            StopCoroutine(coDestroySelf);
    }

    private IEnumerator CoDestroySelf()
    {
        yield return new WaitForSeconds(maxDuration);
        DestroySelf();
        coDestroySelf = null;
    }

    private void DestroySelf()
    {
        Managers.Resource.Destroy(gameObject);
    }
}