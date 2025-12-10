using UnityEngine;

public class MovementWalkState : BaseState<CharacterMovementManager.MovementStates>
{
    #region Variable

    private CharacterMovementManager m_movementManager;
    private bool m_facingRight;
    private float m_walkSpeed;

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MovementWalkState(CharacterMovementManager context, CharacterMovementManager.MovementStates key) : base(key)
    {
        m_movementManager = context;
    }

    public override void EnterState()
    {
        Vector3 scale = m_movementManager.transform.localScale;
        if (Mathf.Approximately(scale.x, 1f)) m_facingRight = true;
        else if (Mathf.Approximately(scale.x, -1f)) m_facingRight = false;

        m_walkSpeed = m_movementManager.GetMoveSpeed();
    }

    public override void ExitState() { }

    public override void UpdateState()
    {
        Vector3 direction = m_movementManager.GetMoveDirection();
        Vector3 movement = direction * (m_walkSpeed * Time.deltaTime);
        m_movementManager.GetCharacterController().Move(movement);
        HandleFlip(direction.x);

        float speed = direction.magnitude;
        m_movementManager.GetCharacter().animationsController.SetSpeed(speed);
    }
    
    private void HandleFlip(float xInput)
    {
        if (xInput > 0.01f && !m_facingRight)
            Flip();

        if (xInput < -0.01f && m_facingRight)
            Flip();
    }

    private void Flip()
    {
        m_facingRight = !m_facingRight;

        Vector3 scale = m_movementManager.transform.localScale;
        scale.x *= -1f;
        m_movementManager.transform.localScale = scale;
    }

    public override CharacterMovementManager.MovementStates GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) {}
}
