using Unity.Netcode;
using UnityEngine;

public class GameEndGameState : BaseState<GameManager.GameStates>
{
    #region Variables
    private GameManager _gameManager;
    private const float DisplayDuration = 7f;
    private float _timeRemaining;
    #endregion

    public GameEndGameState(GameManager context, GameManager.GameStates key) : base(key)
    {
        _gameManager = context;
    }

    public override void EnterState()
    {
        Debug.Log("[EndGame] >>> Entrée dans l'état de fin de jeu !");


        int scoreJ1 = _gameManager.playerManager.GetPlayerScore(1);
        int scoreJ2 = _gameManager.playerManager.GetPlayerScore(2);

        _gameManager.uiManager.UpdateScoreScreen(scoreJ1, scoreJ2);
        _gameManager.uiManager.ShowScoreScreen();

        if (NetworkManager.Singleton.IsServer)
        {
            _timeRemaining = DisplayDuration;
            Debug.Log($"[EndGame] Le Serveur va attendre {DisplayDuration} secondes avant de revenir au menu.");
        }
    }

    public override void ExitState()
    {
        Debug.Log("[EndGame] <<< Sortie de l'état de fin de jeu.");
    }

    public override void UpdateState()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            _timeRemaining -= Time.deltaTime;
        }
    }

    public override GameManager.GameStates GetNextState()
    {
        if (NetworkManager.Singleton.IsServer && _timeRemaining <= 0f)
        {
            Debug.Log("[EndGame] CONDITION RÉUSSIE : Délai d'affichage du score terminé. Passage à ReturnMenu.");
            return GameManager.GameStates.ReturnMenu;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
