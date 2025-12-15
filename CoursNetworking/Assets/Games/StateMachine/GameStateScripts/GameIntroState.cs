using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameIntroState : BaseState<GameManager.GameStates>
{
    #region Variables
    private GameManager _gameManager;
    private const float DisplayDuration = 7f;
    private float _timeRemaining;

    public static event Action OnIntroStart;
    #endregion

    public GameIntroState(GameManager context, GameManager.GameStates key) : base(key)
    {
        _gameManager = context;
    }

    public override void EnterState()
    {
        Debug.Log("[IntroState] -> Entr�e. D�sactivation des contr�les et affichage UI.");
        _gameManager.uiManager.ShowWaitingScreen();
        _timeRemaining = 0;
    }

    public override void ExitState()
    {
        _gameManager.uiManager.HideWaitingScreen();
        _gameManager.uiManager.HideWaitingStartGameScreen();
        Debug.Log("[IntroState] <- Sortie. Nettoyage (Masquer UI).");
    }

    public override void UpdateState()
    {
        if (_timeRemaining > 0)
        {
            _gameManager.uiManager.UpdateTimerBeforeStartGame(_timeRemaining);
            _timeRemaining -= Time.deltaTime;
        }
        else if (_gameManager.playerManager.GetPlayerCount() >= 2)
        {
            _gameManager.uiManager.HideWaitingScreen();
            _gameManager.uiManager.ShowWaitingStartGameScreen();
            OnIntroStart?.Invoke();
            _timeRemaining = DisplayDuration;
        }
        
    }

    public override GameManager.GameStates GetNextState()
    {
        if (Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            if (_gameManager.playerManager.GetPlayerCount() >= 2 && _timeRemaining <= 0f)
            {
                Debug.Log($"[IntroState] CONDITION R�USSIE : 2 joueurs pr�ts. Passage � InGame.");
                return GameManager.GameStates.InGame;
            }
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
