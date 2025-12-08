using Unity.Netcode;
using UnityEngine;

public class GameIntroState : BaseState<GameManager.GameStates>
{
    #region Variables
    private GameManager _gameManager;
    private const float DisplayDuration = 7f;
    private float _timeRemaining;
    private bool changePan = false;
    #endregion

    public GameIntroState(GameManager context, GameManager.GameStates key) : base(key)
    {
        _gameManager = context;
    }

    public override void EnterState()
    {
        _gameManager.uiManager.ShowWaitingScreen();
        _timeRemaining = DisplayDuration;
    }

    public override void ExitState()
    {
        _gameManager.uiManager.HideWaitingStartGameScreen();
    }

    public override void UpdateState()
    {
        if (_gameManager.playerManager.GetPlayerCount() >= 2)
        {
            if (changePan == false) {
                _gameManager.uiManager.HideWaitingScreen();
                _gameManager.uiManager.ShowWaitingStartGameScreen();
                changePan = true;
            }
            
            _timeRemaining -= Time.deltaTime;
            _gameManager.uiManager.UpdateTimerBeforeStartGame(_timeRemaining);
        }
    }

    public override GameManager.GameStates GetNextState()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (_gameManager.playerManager.GetPlayerCount() >= 2 && _timeRemaining <= 0f)
            {
                return GameManager.GameStates.InGame;
            }
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
