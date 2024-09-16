using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlatform : BaseObject
{
    protected Player player = null;

    protected virtual void  OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player ??= collision.collider.gameObject.GetComponent<Player>();

        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = null;

        }
    }



    public override bool Init()
    {
        if (!base.Init())
            return false;


        return true;
    }


}
