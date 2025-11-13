using UnityEngine;

public class GameInGameState : BaseState<GameManager.GameStates>
{
    #region Variables
    private GameManager _gameManager;
    #endregion

    public GameInGameState(GameManager context, GameManager.GameStates key) : base(key)
    {
        _gameManager = context;
    }

    public override void EnterState()
    {
        Debug.Log("[InGame] >>> Entrée dans l'état de jeu !");
        _gameManager.playerManager.SetPlayerActive(true);
        _gameManager.ResetTimer();

        if (Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            if (_gameManager.coinManager != null)
            {
                _gameManager.coinManager.InitializeCoin(5);
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("[InGame] <<< Sortie de l'état de jeu.");
        _gameManager.playerManager.SetPlayerActive(false);
    }

    public override void UpdateState()
    {
        _gameManager.UpdateTime();
        int scoreJ1 = _gameManager.playerManager.GetPlayerScore(1);
        int scoreJ2 = _gameManager.playerManager.GetPlayerScore(2);
        _gameManager.uiManager.UpdateScoreInGameScreen(scoreJ1, scoreJ2);
    }

    public override GameManager.GameStates GetNextState()
    {
        if (_gameManager.GetRemaingTimer() <= 0f)
        {
            Debug.Log("[InGame] CONDITION RÉUSSIE : Le timer est à zéro. Passage à EndGame.");
            return GameManager.GameStates.EndGame;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
