using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : InitBase
{
    [SerializeField, ReadOnly] MainCameraController mainCameraController;
    [SerializeField, ReadOnly] Camera cam = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        mainCameraController = Camera.main.GetComponent<MainCameraController>();
        cam = this.GetComponent<Camera>();

        return true;
    }

    public void SetEnabled(bool isEnable)
    {
        cam.enabled = isEnable;
        mainCameraController.IsUsingSubCamera = isEnable;
    }
}
