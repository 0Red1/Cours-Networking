using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;

public abstract class StateManager<EState> : NetworkBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    public void TransitionToStateLocal(EState stateKey)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
        }

        if (States.ContainsKey(stateKey))
        {
            CurrentState = States[stateKey];
            CurrentState.EnterState();
            Debug.Log($"[FSM Log] Transition locale de {CurrentState.StateKey} à {stateKey}");
        }
        else
        {
            Debug.LogError($"[FSM Log] Tentative de transition vers un état non existant : {stateKey}");
        }
    }

    public void RunCurrentStateUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (CurrentState != null)
        {
            CurrentState.OnTriggerEnter(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (CurrentState != null)
        {
            CurrentState.OnTriggerStay(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CurrentState != null)
        {
            CurrentState.OnTriggerExit(other);
        }
    }
}
