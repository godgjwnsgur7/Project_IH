using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class CreatureFoot : InitBase
{
    public bool IsLandingGround { get { return (landingCount > 0); } }
    public Rigidbody Rigid { get; private set; }
    public BoxCollider Collider { get; private set; }

    [SerializeField, ReadOnly] int landingCount = 0;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Rigid = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();

        InitLandingCount();

        return true;
    }

    public void InitLandingCount()
    {
        landingCount = 0;
        Collider.enabled = false;
        Collider.enabled = true;
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
            }
        }
    }
}
