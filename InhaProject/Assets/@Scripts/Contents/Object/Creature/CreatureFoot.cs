using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static Define;

public class CreatureFoot : InitBase
{
    public bool IsLandingGround { get; private set; }
    public Rigidbody Rigid { get; private set; }
    public BoxCollider Collider { get; private set; }

    [SerializeField, ReadOnly] int landingCount = 0;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        IsLandingGround = false;

        return true;
    }

    private void Reset()
    {
        Rigid = Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider = Util.GetOrAddComponent<BoxCollider>(this.gameObject);

        Rigid.isKinematic = true;
        Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == ETag.Ground.ToString())
        {
            landingCount++;
            IsLandingGround = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag == ETag.Ground.ToString())
        {
            landingCount--;
            if(landingCount <= 0)
            {
                landingCount = 0;
                IsLandingGround = false;
            }
        }
    }
}
