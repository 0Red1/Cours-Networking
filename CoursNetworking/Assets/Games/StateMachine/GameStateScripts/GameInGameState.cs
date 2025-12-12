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
        Debug.Log("[InGame] >>> Entr�e dans l'�tat de jeu !");
        _gameManager.playerManager.SetPlayerActive(true);
        _gameManager.ResetTimer();

        if (Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            if (_gameManager.coinManager != null)
            {
                _gameManager.coinManager.InitializeCoin(5);
            }
            if (_gameManager.enemyManager != null)
            {
                Debug.Log("Instanciate EnnemyManager");
                _gameManager.enemyManager.InitializeEnemy(3);
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("[InGame] <<< Sortie de l'�tat de jeu.");
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
        if (_gameManager.enemyManager.allEnemiesDead())
        {
            return GameManager.GameStates.EndGame;
        }
        /*if (_gameManager.GetRemaingTimer() <= 0f)
        {
            Debug.Log("[InGame] CONDITION R�USSIE : Le timer est � z�ro. Passage � EndGame.");
            return GameManager.GameStates.EndGame;
        }*/
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
