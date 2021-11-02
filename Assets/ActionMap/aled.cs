// GENERATED AUTOMATICALLY FROM 'Assets/ActionMap/aled.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Aled : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Aled()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""aled"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""53c4daf9-10b6-4403-b3a9-b613d9c9757a"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""54c02ea3-b81f-49fc-aae8-df1dde3026b5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""017194bf-5eb0-49f8-b482-d32824b4ab14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialShoot"",
                    ""type"": ""Button"",
                    ""id"": ""ca59645c-5917-49c2-b36a-6c1389e66c86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MovementGamepad"",
                    ""type"": ""Value"",
                    ""id"": ""52fa242d-93ec-4a6a-8b93-5625c4553cec"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShootGamepad"",
                    ""type"": ""Button"",
                    ""id"": ""33146830-b4d3-43ce-92c9-e35e7c5b77df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialShootGamepad"",
                    ""type"": ""Button"",
                    ""id"": ""cf39609b-ff4f-4308-b988-13407a39752f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ViewPad"",
                    ""type"": ""Value"",
                    ""id"": ""8cdb07c4-76a6-4fd7-beb1-a0b1909f7a58"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d3839d33-874c-4248-b4fb-43b19b1f9a12"",
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
                    ""id"": ""4e882589-52c1-455e-b2c5-ef42e45450d2"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""45bbca44-f1d4-4bda-a1a9-53ea0ec24d64"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""55692c94-672b-42a7-8d7f-d3416e3f4133"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8fe49489-9caa-46b8-bffc-b59553def3fd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ecde346f-1a96-4eda-bdb9-63dab121235e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord & Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4855738d-2aed-4ea0-8e39-20ad65421aea"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ShootGamepad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e28b0b7-24c9-4bcb-b8e2-7de550b21445"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SpecialShootGamepad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c73ab0c-2ac0-470f-8b75-55264889c9d7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MovementGamepad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ebf991b8-2d8e-4d3e-ae89-111fad77dbfc"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord & Mouse"",
                    ""action"": ""SpecialShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb818ff4-9472-4548-955d-7f9fe5eef374"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ViewPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keybord & Mouse"",
            ""bindingGroup"": ""Keybord & Mouse"",
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
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_SpecialShoot = m_Player.FindAction("SpecialShoot", throwIfNotFound: true);
        m_Player_MovementGamepad = m_Player.FindAction("MovementGamepad", throwIfNotFound: true);
        m_Player_ShootGamepad = m_Player.FindAction("ShootGamepad", throwIfNotFound: true);
        m_Player_SpecialShootGamepad = m_Player.FindAction("SpecialShootGamepad", throwIfNotFound: true);
        m_Player_ViewPad = m_Player.FindAction("ViewPad", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_SpecialShoot;
    private readonly InputAction m_Player_MovementGamepad;
    private readonly InputAction m_Player_ShootGamepad;
    private readonly InputAction m_Player_SpecialShootGamepad;
    private readonly InputAction m_Player_ViewPad;
    public struct PlayerActions
    {
        private @Aled m_Wrapper;
        public PlayerActions(@Aled wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @SpecialShoot => m_Wrapper.m_Player_SpecialShoot;
        public InputAction @MovementGamepad => m_Wrapper.m_Player_MovementGamepad;
        public InputAction @ShootGamepad => m_Wrapper.m_Player_ShootGamepad;
        public InputAction @SpecialShootGamepad => m_Wrapper.m_Player_SpecialShootGamepad;
        public InputAction @ViewPad => m_Wrapper.m_Player_ViewPad;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @SpecialShoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpecialShoot;
                @SpecialShoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpecialShoot;
                @SpecialShoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpecialShoot;
                @MovementGamepad.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementGamepad;
                @MovementGamepad.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementGamepad;
                @MovementGamepad.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementGamepad;
                @ShootGamepad.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShootGamepad;
                @ShootGamepad.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShootGamepad;
                @ShootGamepad.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShootGamepad;
                @SpecialShootGamepad.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpecialShootGamepad;
                @SpecialShootGamepad.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpecialShootGamepad;
                @SpecialShootGamepad.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpecialShootGamepad;
                @ViewPad.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnViewPad;
                @ViewPad.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnViewPad;
                @ViewPad.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnViewPad;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @SpecialShoot.started += instance.OnSpecialShoot;
                @SpecialShoot.performed += instance.OnSpecialShoot;
                @SpecialShoot.canceled += instance.OnSpecialShoot;
                @MovementGamepad.started += instance.OnMovementGamepad;
                @MovementGamepad.performed += instance.OnMovementGamepad;
                @MovementGamepad.canceled += instance.OnMovementGamepad;
                @ShootGamepad.started += instance.OnShootGamepad;
                @ShootGamepad.performed += instance.OnShootGamepad;
                @ShootGamepad.canceled += instance.OnShootGamepad;
                @SpecialShootGamepad.started += instance.OnSpecialShootGamepad;
                @SpecialShootGamepad.performed += instance.OnSpecialShootGamepad;
                @SpecialShootGamepad.canceled += instance.OnSpecialShootGamepad;
                @ViewPad.started += instance.OnViewPad;
                @ViewPad.performed += instance.OnViewPad;
                @ViewPad.canceled += instance.OnViewPad;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeybordMouseSchemeIndex = -1;
    public InputControlScheme KeybordMouseScheme
    {
        get
        {
            if (m_KeybordMouseSchemeIndex == -1) m_KeybordMouseSchemeIndex = asset.FindControlSchemeIndex("Keybord & Mouse");
            return asset.controlSchemes[m_KeybordMouseSchemeIndex];
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
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnSpecialShoot(InputAction.CallbackContext context);
        void OnMovementGamepad(InputAction.CallbackContext context);
        void OnShootGamepad(InputAction.CallbackContext context);
        void OnSpecialShootGamepad(InputAction.CallbackContext context);
        void OnViewPad(InputAction.CallbackContext context);
    }
}
