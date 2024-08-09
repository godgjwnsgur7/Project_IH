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

    // Mouse InputEvent
    public event Action<Vector2> OnMousePointerAction;

    // Keyboard InputEvent
    public event Action<Vector2> OnArrowKeyEntered;
    public event Action OnSpaceKeyEntered;

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

    #region Mouse InputEvent
    public void OnMousePointer(InputValue value)
    {
        Vector2 inputVec = value.Get<Vector2>();
        OnMousePointerAction?.Invoke(inputVec);
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
    #endregion
}
