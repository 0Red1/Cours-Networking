using UnityEngine;

public class MovementRunState : BaseState<CharacterMovementManager.MovementStates>
{
    
    #region Variable

    private CharacterMovementManager m_movementManager;
    private Vector3 m_lastDirection;
    private float m_dashSpeed = 7f;
    private float m_dashTime = 1f;
    private float m_currentDashTime;
    private bool m_facingRight;

    #endregion
    public MovementRunState( CharacterMovementManager context, CharacterMovementManager.MovementStates key) : base(key)
    {
        m_movementManager = context;
    }

    public override void EnterState()
    {
        Vector3 scale = m_movementManager.transform.localScale;
        if (Mathf.Approximately(scale.x, 1f)) m_facingRight = true;
        else if (Mathf.Approximately(scale.x, -1f)) m_facingRight = false;
        
        m_lastDirection = m_movementManager.GetMoveDirection();
        m_movementManager.GetCharacter().animationsController.SetDash(true);
        m_currentDashTime = m_dashTime;
    }

    public override void ExitState()
    {
        Debug.Log("Fin du dash");
        m_movementManager.GetCharacter().animationsController.SetDash(false);
    }

    public override void UpdateState()
    {
        CharacterController cc = m_movementManager.GetCharacterController();
        
        m_currentDashTime -= Time.deltaTime;
        Vector3 dashDir = m_lastDirection * (m_dashSpeed * Time.deltaTime);
        cc.Move(dashDir);

        if (dashDir == Vector3.zero)
        {
            if (m_facingRight)
            {
                dashDir = Vector3.right;
            }
            else
            {
                dashDir = Vector3.left;
            }
        }
        
        cc.Move(dashDir * (m_dashSpeed * Time.deltaTime));
    }

    public override CharacterMovementManager.MovementStates GetNextState()
    {
        if (m_currentDashTime <= 0f)
        {
           
            return CharacterMovementManager.MovementStates.Walk;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) { }
}
