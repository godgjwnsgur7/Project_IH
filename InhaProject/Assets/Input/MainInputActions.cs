//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/MainInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MainInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainInputActions"",
    ""maps"": [
        {
            ""name"": ""InGame"",
            ""id"": ""b1fe882a-13d1-44e3-a2b4-f67b1e0335cc"",
            ""actions"": [
                {
                    ""name"": ""ArrowKey"",
                    ""type"": ""Value"",
                    ""id"": ""f70674f4-ed7a-4e5b-aa89-b5c24d0d03a3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SpaceKey"",
                    ""type"": ""Button"",
                    ""id"": ""354b95dd-9076-4f08-9778-0797692fd590"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""EKey"",
                    ""type"": ""Button"",
                    ""id"": ""eba49e48-9feb-4819-afa6-3702ed0d4ebc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MousePointer"",
                    ""type"": ""Value"",
                    ""id"": ""e0bd43b1-bcdf-4e00-96d4-1df52978fa5b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseLeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""56f68478-b141-4dfa-9896-a8297246411f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""edadc6c2-28c7-46ec-9a2a-74238b943552"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SpaceKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0269a506-a01f-4a40-91b8-c1c5a871a4b8"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""261d276c-2bb2-4fad-8f86-622c749175fd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e22d34cf-96d6-4c45-91ff-87e6b42f989e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a536dc0b-1bbb-4674-8ed0-e052ad5c17d8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9e23b79c-3a23-4d04-a8df-d1ae70fa11c5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""58efabd2-6199-4460-85f8-781f4b30f43d"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""031ed740-b113-47fd-907e-a35805ce0c2c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bf5481a6-114b-4111-be29-3f168ec3a7a7"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f7e624b4-ab14-488f-94d3-1c78da2389dd"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e5e23daf-efa6-4e91-b6e4-3f885fa6a82f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8ea0563f-787c-4bd7-97e9-574a3982c8b1"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""EKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83d4ec47-f655-496e-8238-c2da559bd5be"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=0.05,y=0.05)"",
                    ""groups"": """",
                    ""action"": ""MousePointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a43d8d4-49da-40e9-996f-c9aa16ef0d99"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),StickDeadzone,ScaleVector2(x=300,y=300)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MousePointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a25ef974-41f3-40b4-a678-927e65dfb042"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""OutGame"",
            ""id"": ""6b7d8e8f-195e-46e5-9ad8-2670d1700e5b"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // InGame
        m_InGame = asset.FindActionMap("InGame", throwIfNotFound: true);
        m_InGame_ArrowKey = m_InGame.FindAction("ArrowKey", throwIfNotFound: true);
        m_InGame_SpaceKey = m_InGame.FindAction("SpaceKey", throwIfNotFound: true);
        m_InGame_EKey = m_InGame.FindAction("EKey", throwIfNotFound: true);
        m_InGame_MousePointer = m_InGame.FindAction("MousePointer", throwIfNotFound: true);
        m_InGame_MouseLeftClick = m_InGame.FindAction("MouseLeftClick", throwIfNotFound: true);
        // OutGame
        m_OutGame = asset.FindActionMap("OutGame", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // InGame
    private readonly InputActionMap m_InGame;
    private List<IInGameActions> m_InGameActionsCallbackInterfaces = new List<IInGameActions>();
    private readonly InputAction m_InGame_ArrowKey;
    private readonly InputAction m_InGame_SpaceKey;
    private readonly InputAction m_InGame_EKey;
    private readonly InputAction m_InGame_MousePointer;
    private readonly InputAction m_InGame_MouseLeftClick;
    public struct InGameActions
    {
        private @MainInputActions m_Wrapper;
        public InGameActions(@MainInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @ArrowKey => m_Wrapper.m_InGame_ArrowKey;
        public InputAction @SpaceKey => m_Wrapper.m_InGame_SpaceKey;
        public InputAction @EKey => m_Wrapper.m_InGame_EKey;
        public InputAction @MousePointer => m_Wrapper.m_InGame_MousePointer;
        public InputAction @MouseLeftClick => m_Wrapper.m_InGame_MouseLeftClick;
        public InputActionMap Get() { return m_Wrapper.m_InGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
        public void AddCallbacks(IInGameActions instance)
        {
            if (instance == null || m_Wrapper.m_InGameActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InGameActionsCallbackInterfaces.Add(instance);
            @ArrowKey.started += instance.OnArrowKey;
            @ArrowKey.performed += instance.OnArrowKey;
            @ArrowKey.canceled += instance.OnArrowKey;
            @SpaceKey.started += instance.OnSpaceKey;
            @SpaceKey.performed += instance.OnSpaceKey;
            @SpaceKey.canceled += instance.OnSpaceKey;
            @EKey.started += instance.OnEKey;
            @EKey.performed += instance.OnEKey;
            @EKey.canceled += instance.OnEKey;
            @MousePointer.started += instance.OnMousePointer;
            @MousePointer.performed += instance.OnMousePointer;
            @MousePointer.canceled += instance.OnMousePointer;
            @MouseLeftClick.started += instance.OnMouseLeftClick;
            @MouseLeftClick.performed += instance.OnMouseLeftClick;
            @MouseLeftClick.canceled += instance.OnMouseLeftClick;
        }

        private void UnregisterCallbacks(IInGameActions instance)
        {
            @ArrowKey.started -= instance.OnArrowKey;
            @ArrowKey.performed -= instance.OnArrowKey;
            @ArrowKey.canceled -= instance.OnArrowKey;
            @SpaceKey.started -= instance.OnSpaceKey;
            @SpaceKey.performed -= instance.OnSpaceKey;
            @SpaceKey.canceled -= instance.OnSpaceKey;
            @EKey.started -= instance.OnEKey;
            @EKey.performed -= instance.OnEKey;
            @EKey.canceled -= instance.OnEKey;
            @MousePointer.started -= instance.OnMousePointer;
            @MousePointer.performed -= instance.OnMousePointer;
            @MousePointer.canceled -= instance.OnMousePointer;
            @MouseLeftClick.started -= instance.OnMouseLeftClick;
            @MouseLeftClick.performed -= instance.OnMouseLeftClick;
            @MouseLeftClick.canceled -= instance.OnMouseLeftClick;
        }

        public void RemoveCallbacks(IInGameActions instance)
        {
            if (m_Wrapper.m_InGameActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInGameActions instance)
        {
            foreach (var item in m_Wrapper.m_InGameActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InGameActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InGameActions @InGame => new InGameActions(this);

    // OutGame
    private readonly InputActionMap m_OutGame;
    private List<IOutGameActions> m_OutGameActionsCallbackInterfaces = new List<IOutGameActions>();
    public struct OutGameActions
    {
        private @MainInputActions m_Wrapper;
        public OutGameActions(@MainInputActions wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_OutGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OutGameActions set) { return set.Get(); }
        public void AddCallbacks(IOutGameActions instance)
        {
            if (instance == null || m_Wrapper.m_OutGameActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_OutGameActionsCallbackInterfaces.Add(instance);
        }

        private void UnregisterCallbacks(IOutGameActions instance)
        {
        }

        public void RemoveCallbacks(IOutGameActions instance)
        {
            if (m_Wrapper.m_OutGameActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IOutGameActions instance)
        {
            foreach (var item in m_Wrapper.m_OutGameActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_OutGameActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public OutGameActions @OutGame => new OutGameActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IInGameActions
    {
        void OnArrowKey(InputAction.CallbackContext context);
        void OnSpaceKey(InputAction.CallbackContext context);
        void OnEKey(InputAction.CallbackContext context);
        void OnMousePointer(InputAction.CallbackContext context);
        void OnMouseLeftClick(InputAction.CallbackContext context);
    }
    public interface IOutGameActions
    {
    }
}
