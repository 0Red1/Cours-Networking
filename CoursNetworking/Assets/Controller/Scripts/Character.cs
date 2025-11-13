using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : NetworkBehaviour
{
    #region Variables
    public readonly NetworkVariable<int> score = new NetworkVariable<int>(0);

    public CharacterMovementController movementController;
    public CharacterAnimationsController animationsController;
    public CharacterSkillsPlayer skillsPlayer;

    private PlayerManager _playerManager;
    #endregion

    protected override void OnNetworkPostSpawn()
    {
        base.OnNetworkPostSpawn();

        if (IsOwner)
        {
            InputController.Instance.SetCharacter(this);
        }
    }

    private void Awake()
    {
        movementController = GetComponent<CharacterMovementController>();
        animationsController = GetComponent<CharacterAnimationsController>();
        skillsPlayer = GetComponent<CharacterSkillsPlayer>();

        movementController.SetManager(this);
        _playerManager = PlayerManager.Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playerManager != null)
        {
            _playerManager.AddPlayer(this);
            Debug.Log($"[Char Log] Joueur {gameObject.name} enregistré. Compte PlayerManager : {_playerManager.GetPlayerCount()}");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMovementAnimation(float speed)
    {
        animationsController.SetSpeed(speed);
    }

    public void PlayDashAnim()
    {
        animationsController.SetDash(true);
    }

    public void StopDashAnim()
    {
        animationsController.SetDash(false);
    }
}