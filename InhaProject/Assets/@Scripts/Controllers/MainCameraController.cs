using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MainCameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField, ReadOnly] private BaseObject target;
    [SerializeField] Vector3 cameraAddFixedValue;

    [SerializeField, ReadOnly] private bool _isUsingSubCamera = false;
    public bool IsUsingSubCamera 
    { 
        get { return _isUsingSubCamera; }
        set
        {
            if (_isUsingSubCamera == value)
                return;

            _isUsingSubCamera = value;
            OnChangedSubCameraEnable?.Invoke(value);
        }
    }

    public event Action<bool> OnChangedSubCameraEnable;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        coCameraControll = StartCoroutine(CoCameraControll());
    }

    Coroutine coCameraControll = null;
    private IEnumerator CoCameraControll()
    {
        yield return new WaitUntil(() => target != null);

        while(target != null)
        {

            yield return new LateUpdate();
            FollowingTarget();
        }
    }

    private void Init()
    {
        cam = Camera.main;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.fieldOfView = 70;

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
