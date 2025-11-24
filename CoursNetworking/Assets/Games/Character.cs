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
    public HealthSystem healthSystem; // a changer comme les autres en properties plutot que des var public

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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            if (_playerManager != null)
            {
                _playerManager.AddPlayer(OwnerClientId, this); // Ajout du joueur a la liste des joueurs actifs
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.uiManager != null)
        {
            GameManager.Instance.uiManager.RegisterHealthSystem(OwnerClientId, healthSystem);
            Debug.Log($"[UI] Connexion de l'UI pour Joueur ID {OwnerClientId} sur la machine {NetworkManager.Singleton.LocalClientId}.");
        }
        else
        {
            Debug.LogError("[UI] GameManager ou UIManager n'est pas prêt.");
        }
    }

    private void Awake()
    {
        movementController = GetComponent<CharacterMovementController>();
        animationsController = GetComponent<CharacterAnimationsController>();
        skillsPlayer = GetComponent<CharacterSkillsPlayer>();
        healthSystem = GetComponent<HealthSystem>();

        movementController.SetManager(this);
        _playerManager = PlayerManager.Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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