using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureFoot : InitBase
{
    public bool IsLandingGround { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        IsLandingGround = false;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ETag.Ground.ToString())
        {
            IsLandingGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == ETag.Ground.ToString())
        {
            IsLandingGround = false;
        }
    }
}
