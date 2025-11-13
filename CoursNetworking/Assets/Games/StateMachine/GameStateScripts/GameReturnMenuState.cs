using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReturnMenuState : BaseState<GameManager.GameStates>
{
    #region Variables
    private GameManager _gameManager;
    #endregion

    public GameReturnMenuState(GameManager context, GameManager.GameStates key) : base(key)
    {
        _gameManager = context;
    }

    public override void EnterState()
    {
        Debug.Log("[ReturnMenu] !!! DECONNEXION ET RETOUR AU MENU !!!");
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenuScene");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }

    public override GameManager.GameStates GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
