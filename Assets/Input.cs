// GENERATED AUTOMATICALLY FROM 'Assets/Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Input : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Input()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""3027dae1-3ce0-40e4-900d-2d4ef68916ec"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""da539230-6b9e-4f61-8f5e-41188dd93125"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""23fd9a77-d6b1-42b8-9ac8-57f4efaf8efa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""c30ccb0a-aa59-4db8-a8c5-9302db2225b1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""2c93b75c-851c-4bae-bdf1-928d018d7fee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Value"",
                    ""id"": ""fef1fff4-bec9-4533-bb48-14f0d94df9aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSwitching"",
                    ""type"": ""Value"",
                    ""id"": ""2881ac44-7507-47c9-82c7-53a4b8bda50b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponSwitchingScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1ed66e5d-28e8-4517-aeab-704ed1d142f0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""566a66bd-ece9-47db-8334-db08881c56e2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""663846e1-42ee-4a21-883e-5b5177067cbd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""173ace7b-c201-4ea5-aed1-92e7d6fa84b2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1e4c9dc9-4440-4d21-a9f1-fdb011cea244"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a7a5077e-2f4f-4b75-a256-9c33416f9f53"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1040b5da-f2e8-4a1f-8cef-ce232669913b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73ef2b13-4dba-475d-96e6-e0f251775be1"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5596261-233d-4b08-93f5-ed6b5f68cdce"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0050ab16-5e7e-4c70-ac25-f42f20097736"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e669e84-537b-4777-a32c-f7fdd6679941"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": ""Press"",
                    ""processors"": ""Clamp(max=1)"",
                    ""groups"": """",
                    ""action"": ""WeaponSwitching"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8fe93a2-88c7-4f43-adb4-4b623585cf7c"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": ""Press"",
                    ""processors"": ""Clamp(min=2,max=2)"",
                    ""groups"": """",
                    ""action"": ""WeaponSwitching"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7db542cb-7fa1-4ede-a886-b37feb8fd5bb"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": ""Press"",
                    ""processors"": ""Clamp(min=3,max=3)"",
                    ""groups"": """",
                    ""action"": ""WeaponSwitching"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e66c9af7-a55e-4763-b4ed-133861eb8c03"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""WeaponSwitchingScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Computer"",
            ""bindingGroup"": ""Computer"",
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
        }
    ]
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_Movement = m_Game.FindAction("Movement", throwIfNotFound: true);
        m_Game_Fire = m_Game.FindAction("Fire", throwIfNotFound: true);
        m_Game_Mouse = m_Game.FindAction("Mouse", throwIfNotFound: true);
        m_Game_Reload = m_Game.FindAction("Reload", throwIfNotFound: true);
        m_Game_Interact = m_Game.FindAction("Interact", throwIfNotFound: true);
        m_Game_WeaponSwitching = m_Game.FindAction("WeaponSwitching", throwIfNotFound: true);
        m_Game_WeaponSwitchingScroll = m_Game.FindAction("WeaponSwitchingScroll", throwIfNotFound: true);
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

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_Movement;
    private readonly InputAction m_Game_Fire;
    private readonly InputAction m_Game_Mouse;
    private readonly InputAction m_Game_Reload;
    private readonly InputAction m_Game_Interact;
    private readonly InputAction m_Game_WeaponSwitching;
    private readonly InputAction m_Game_WeaponSwitchingScroll;
    public struct GameActions
    {
        private @Input m_Wrapper;
        public GameActions(@Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Game_Movement;
        public InputAction @Fire => m_Wrapper.m_Game_Fire;
        public InputAction @Mouse => m_Wrapper.m_Game_Mouse;
        public InputAction @Reload => m_Wrapper.m_Game_Reload;
        public InputAction @Interact => m_Wrapper.m_Game_Interact;
        public InputAction @WeaponSwitching => m_Wrapper.m_Game_WeaponSwitching;
        public InputAction @WeaponSwitchingScroll => m_Wrapper.m_Game_WeaponSwitchingScroll;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMovement;
                @Fire.started -= m_Wrapper.m_GameActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnFire;
                @Mouse.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMouse;
                @Reload.started -= m_Wrapper.m_GameActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnReload;
                @Interact.started -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnInteract;
                @WeaponSwitching.started -= m_Wrapper.m_GameActionsCallbackInterface.OnWeaponSwitching;
                @WeaponSwitching.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnWeaponSwitching;
                @WeaponSwitching.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnWeaponSwitching;
                @WeaponSwitchingScroll.started -= m_Wrapper.m_GameActionsCallbackInterface.OnWeaponSwitchingScroll;
                @WeaponSwitchingScroll.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnWeaponSwitchingScroll;
                @WeaponSwitchingScroll.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnWeaponSwitchingScroll;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @WeaponSwitching.started += instance.OnWeaponSwitching;
                @WeaponSwitching.performed += instance.OnWeaponSwitching;
                @WeaponSwitching.canceled += instance.OnWeaponSwitching;
                @WeaponSwitchingScroll.started += instance.OnWeaponSwitchingScroll;
                @WeaponSwitchingScroll.performed += instance.OnWeaponSwitchingScroll;
                @WeaponSwitchingScroll.canceled += instance.OnWeaponSwitchingScroll;
            }
        }
    }
    public GameActions @Game => new GameActions(this);
    private int m_ComputerSchemeIndex = -1;
    public InputControlScheme ComputerScheme
    {
        get
        {
            if (m_ComputerSchemeIndex == -1) m_ComputerSchemeIndex = asset.FindControlSchemeIndex("Computer");
            return asset.controlSchemes[m_ComputerSchemeIndex];
        }
    }
    public interface IGameActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnWeaponSwitching(InputAction.CallbackContext context);
        void OnWeaponSwitchingScroll(InputAction.CallbackContext context);
    }
}
