using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : StateManager<GameManager.GameStates>
{
    #region Variables
    public enum GameStates
    {
        Intro,
        InGame,
        EndGame,
        ReturnMenu
    }

    public PlayerManager playerManager;
    public UIManager uiManager;
    public CoinManager coinManager;

    private readonly NetworkVariable<float> _timer = new NetworkVariable<float>(120f);
    private readonly NetworkVariable<GameStates> _netState = new NetworkVariable<GameStates>(GameStates.Intro);

    private static GameManager _instance;
    #endregion

    #region Properties
    public static GameManager Instance => _instance;
    #endregion

    #region Built-in Methods
    private void Awake()
    {
        States.Add(GameStates.Intro, new GameIntroState(this,GameStates.Intro));
        States.Add(GameStates.InGame, new GameInGameState(this,GameStates.InGame));
        States.Add(GameStates.EndGame, new GameEndGameState(this, GameStates.EndGame));
        States.Add(GameStates.ReturnMenu, new GameReturnMenuState(this, GameStates.ReturnMenu));

        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _netState.OnValueChanged += OnGameStateChanged;
        
        _timer.OnValueChanged += OnTimerChanged;

        uiManager.UpdateUITimer(_timer.Value);

        CurrentState = States[_netState.Value];
        CurrentState.EnterState();
    }

    public void Update()
    {
        RunCurrentStateUpdate();

        if (!IsServer || CurrentState == null)
        {
            return;
        }

        GameStates nextStateKey = CurrentState.GetNextState();

        if (!nextStateKey.Equals(_netState.Value))
        {
            _netState.Value = nextStateKey;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsSpawned)
        {
            _netState.OnValueChanged -= OnGameStateChanged;
            _timer.OnValueChanged -= OnTimerChanged;
        }
    }
    #endregion

    private void OnGameStateChanged(GameStates oldState, GameStates newState)
    {
        TransitionToStateLocal(newState);
    }

    public void TransitionToState(GameStates stateKey)
    {
        if (!IsServer)
        {
            Debug.LogWarning($"[FSM Net] Le client a tenté de changer l'état. Requête ignorée.");
            return;
        }

        _netState.Value = stateKey;
    }

    #region Timer
    public void ResetTimer()
    {
        if (!IsServer) return;
        _timer.Value = 60f;
    }

    public void UpdateTime()
    {
        if (!IsServer) return;
        _timer.Value -= Time.deltaTime;
    }

    public float GetRemaingTimer()
    {
        return _timer.Value;
    }

    private void OnTimerChanged(float oldTime, float newTime)
    {
        uiManager.UpdateUITimer(newTime);
    }
    #endregion
}
