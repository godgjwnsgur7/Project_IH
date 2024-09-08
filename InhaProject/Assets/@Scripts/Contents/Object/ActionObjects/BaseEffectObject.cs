using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum EEffectObjectType
{
    PlayerHitEffect,
    PlayerHitBlockEffect,
    MonsterSpawnEffect,

    MonsterHitEffect, // ¹Ì»ç¿ë
}

public class BaseEffectObject : InitBase, Poolable
{
    [SerializeField, ReadOnly] protected float maxDuration;
    [field: SerializeField, ReadOnly] public Vector3 SubPos { get; protected set; }

    public bool IsUsing { get; set; }
    public GameObject GameObject { get { return this.gameObject; } }

    Coroutine coDestroySelf;

    [System.Obsolete]
    protected virtual void Reset()
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
    }

    protected virtual void OnDisable()
    {
        if (coDestroySelf != null)
            StopCoroutine(coDestroySelf);
    }

    public virtual void SetInfo(EffectParam param = null)
    {
        coDestroySelf = StartCoroutine(CoDestroySelf());
    }

    protected virtual IEnumerator CoDestroySelf(float subTime = 0f)
    {
        yield return new WaitForSeconds(maxDuration - subTime);
        DestroySelf();
        coDestroySelf = null;
    }

    protected void DestroySelf()
    {
        Managers.Resource.Destroy(gameObject);
    }
}