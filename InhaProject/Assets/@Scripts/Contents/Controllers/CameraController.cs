using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;

    public BaseObject _target;
    public BaseObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public Vector2 maxBound, minBound;
    public float halfHeight, halfWidth;
    public float clampedX, clampedY;

    private void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        if(_target != null)
        {
            FollowingTarget();
        }
    }

    private void Init()
    {
        cam = Camera.main;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 1.5f;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    private void FollowingTarget()
    {
        transform.position = new Vector3(clampedX, clampedY, -10);
    }

}
