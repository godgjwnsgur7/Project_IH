using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum EInputControlSchemeType
{
    PC,
    Gamepad,
}

public enum EInputActionMapType
{
    InGame,
    OutGame,
}

public class InputMgr : MonoBehaviour
{
    private PlayerInput playerInput;

    EInputControlSchemeType currControlSchemeType = EInputControlSchemeType.PC;
    EInputActionMapType currActionMapType = EInputActionMapType.InGame;

    // Keyboard InputEvent
    public event Action<Vector2> OnArrowKeyEntered;
    public event Action OnSpaceKeyEntered;
    public event Action OnFKeyEntered;
    public event Action OnEKeyEntered;

    public void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultControlScheme = currControlSchemeType.ToString();
        playerInput.defaultActionMap = currActionMapType.ToString();
        playerInput.notificationBehavior = PlayerNotifications.SendMessages;

    }

    public void Clear()
    {
    }

    #region Basic InputEvent
    public void OnDeviceLost()
    {
        Debug.Log("OnDeviceLost");
    }

    public void OnDeviceRegained()
    {
        Debug.Log("OnDeviceRegained");
    }

    public void OnControlsChanged()
    {
        Debug.Log("OnControlsChanged");
    }
    #endregion

    #region Keyboard InputEvent
    public void OnArrowKey(InputValue value)
    {
        Vector2 inputVec = value.Get<Vector2>();
        OnArrowKeyEntered?.Invoke(inputVec);
    }

    public void OnSpaceKey()
    {
        OnSpaceKeyEntered?.Invoke();
    }

    public void OnFKey()
    {
        OnFKeyEntered?.Invoke();
    }

    public void OnEKey()
    {
        OnEKeyEntered?.Invoke();
    }
    #endregion
}
