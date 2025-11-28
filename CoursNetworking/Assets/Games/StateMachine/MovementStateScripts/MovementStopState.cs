using UnityEngine;

public class MovementStopState : BaseState<CharacterMovementManager.MovementStates>
{
    #region Variables
    private CharacterMovementManager _movementManager;
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MovementStopState(CharacterMovementManager context, CharacterMovementManager.MovementStates key) : base(key)
    {
        _movementManager = context;
    }
    
    public override void EnterState() { }

    public override void ExitState() { }

    public override void UpdateState() { }

    public override CharacterMovementManager.MovementStates GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) { }
}
