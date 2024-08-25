using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField, ReadOnly] private BaseObject target;
    [SerializeField] Vector3 cameraAddFixedValue;

    private void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            FollowingTarget();
        }
    }

    private void Init()
    {
        cam = Camera.main;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 1.5f;

        cameraAddFixedValue.z = -10;
    }

    public void SetTarget(BaseObject target)
    {
        this.target = target;
    }

    private void FollowingTarget()
    {
        transform.position = target.GetCameraTargetPos() + cameraAddFixedValue;
    }
}
