using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : NetworkBehaviour
{
    #region Variables
    public readonly NetworkVariable<int> score = new NetworkVariable<int>(0);

    public CharacterMovementManager movementController;
    public CharacterAnimationsController animationsController;
    public CharacterSkillsPlayer skillsPlayer;
    public HealthSystem healthSystem;
    public AttackDamageLogic attackDamageLogic;

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
        movementController = GetComponent<CharacterMovementManager>();
        skillsPlayer = GetComponent<CharacterSkillsPlayer>();
        animationsController = GetComponent<CharacterAnimationsController>();
        healthSystem = GetComponent<HealthSystem>();
        attackDamageLogic = GetComponent<AttackDamageLogic>();
        _playerManager = PlayerManager.Instance;

        if (movementController != null)
        {
            movementController.SetManager(this);
        }

        if (attackDamageLogic != null) 
        { 
            attackDamageLogic.SetOwner(gameObject);
        }

        if (skillsPlayer != null && animationsController != null)
        {
            skillsPlayer.OnAttackTriggered += animationsController.SetAttack;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playerManager != null)
        {
            _playerManager.AddPlayer(OwnerClientId, this);
        }
    }
}