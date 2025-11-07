using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region Variables
    [SerializeField] InputActionAsset inputAction;

    private Character targetCharacter;

    private InputAction m_moveAction;
    private InputAction m_dashAction;
    private InputAction m_skill1Action;
    private InputAction m_skill2Action;
    private InputAction m_skill3Action;

    private Vector2 m_move;
    #endregion
    
    #region Singleton
    public static InputController Instance {get; private set;}
    

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_dashAction = InputSystem.actions.FindAction("Dash");
        m_skill1Action = InputSystem.actions.FindAction("Skill1");
        m_skill2Action = InputSystem.actions.FindAction("Skill2");
        m_skill3Action = InputSystem.actions.FindAction("Skill3");
    }
    #endregion
    
    private void OnEnable()
    {
        inputAction.FindActionMap("Player").Enable();
        m_dashAction.performed += OnDashPerformed;
        m_skill1Action.performed += OnSkill1Performed;
        m_skill2Action.performed += OnSkill2Performed;
        m_skill3Action.performed += OnSkill3Performed;
    }

    private void OnDisable()
    {
        inputAction.FindActionMap("Player").Disable();
        m_dashAction.performed -= OnDashPerformed;
        m_skill1Action.performed -= OnSkill1Performed;
        m_skill2Action.performed -= OnSkill2Performed;
        m_skill3Action.performed -= OnSkill3Performed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.unityLogger.Log(targetCharacter);
        if (!targetCharacter) 
            return;
        
        m_move = m_moveAction.ReadValue<Vector2>();
        targetCharacter.movementController.SetInputDirection(m_move);
        Debug.Log(m_move);
    }

    public void SetCharacter(Character character)
    {
        targetCharacter = character;
    }

    void OnDashPerformed(InputAction.CallbackContext context)
    {
        targetCharacter.movementController.Dash();
    }

    void OnSkill1Performed(InputAction.CallbackContext context)
    {
        targetCharacter.skillsPlayer.Skill1();
    }

    void OnSkill2Performed(InputAction.CallbackContext context)
    {
        targetCharacter.skillsPlayer.Skill2();
    }

    void OnSkill3Performed(InputAction.CallbackContext context)
    {
        targetCharacter.skillsPlayer.Skill3();
    }
}
