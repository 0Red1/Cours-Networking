using Unity.Netcode;
using UnityEngine;

public class CharacterMovementManager : StateManager<CharacterMovementManager.MovementStates>
{
    #region Variables
    public enum MovementStates
    {
        Stop,
        Walk,
        Run,
    }
    
    [SerializeField] private float moveSpeed = 5f;

    private bool facingRight = true;
    private Vector3 m_moveDirection;
    private CharacterController m_cc;
    private Character m_character;
    
    private MovementStates m_movementState = MovementStates.Stop;
    #endregion

    private void Awake()
    {
        States.Add(MovementStates.Stop, new MovementStopState(this, MovementStates.Stop));
        States.Add(MovementStates.Walk, new MovementWalkState(this, MovementStates.Walk));
        States.Add(MovementStates.Run, new MovementRunState(this, MovementStates.Run));
    }
    void Start()
    {
        m_cc = GetComponent<CharacterController>();
        CurrentState = States[m_movementState];
    }

    void Update()
    {
        if (!IsOwner)
            return;

        RunCurrentStateUpdate();
        
        MovementStates nextState = CurrentState.GetNextState();

        if (nextState != m_movementState)
        {
            TransitionToStateLocal(nextState);
        }
    }

    public void SetManager(Character manager)
    {
        m_character = manager;
    }
    
    public void SetInputDirection(Vector2 input)
    {
        m_moveDirection.x = input.x;
        m_moveDirection.z = input.y;
    }
    
    public void Dash()
    {
        TransitionToStateLocal(MovementStates.Run);
        m_movementState = MovementStates.Run;
    }

    public void Stop(bool isStop)
    {
        Debug.Log(isStop);
        if (isStop)
        {
            TransitionToStateLocal(MovementStates.Stop);
            m_movementState = MovementStates.Stop;
        }
        else
        {
            TransitionToStateLocal(MovementStates.Walk);
            m_movementState = MovementStates.Walk;
        }
    }

    public Vector3 GetMoveDirection()
    {
        return m_moveDirection;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public CharacterController GetCharacterController()
    {
        return m_cc;
    }

    public Character GetCharacter()
    {
        return m_character;
    }
}

