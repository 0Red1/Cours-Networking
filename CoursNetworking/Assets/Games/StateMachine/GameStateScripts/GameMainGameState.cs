using UnityEngine;

public class GameMainGameState : BaseState<GameManager.GameStates>
{
    private GameManager _gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameMainGameState(GameManager context, GameManager.GameStates key) : base(key)
    {
        _gameManager = context;
    }

    public override void EnterState()
    {
        _gameManager.playerManager.SetPlayerActive(true);
    }

    public override void ExitState() { }

    public override void UpdateState()
    { }

    public override GameManager.GameStates GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) { }
}
