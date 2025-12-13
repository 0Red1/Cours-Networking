using System;
using Unity.Netcode;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    #region Variables
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private CharacterAnimationsController characterAnimationsController;

    private readonly NetworkVariable<float> _currentHealth = new NetworkVariable<float>();

    public event Action<ulong, float> OnHealthChanged;
    #endregion

    #region Properties
    public NetworkVariable<float> CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;
    #endregion

    #region Built-in Methods
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _currentHealth.OnValueChanged += HandleNetworkHealthChanged;

        if (UIManager.Instance != null) 
        {
            UIManager.Instance.RegisterHealthSystem(OwnerClientId, this);
            Debug.Log("Je m'enregistre !");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsServer)
        {
            _currentHealth.Value = maxHealth;
            characterAnimationsController = GetComponent<CharacterAnimationsController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkDespawn()
    {
        if (IsSpawned)
        {
            _currentHealth.OnValueChanged -= HandleNetworkHealthChanged;
        }
        base.OnNetworkDespawn();
    }
    #endregion

    private void HandleNetworkHealthChanged(float oldHealth,float newHealth)
    {
        OnHealthChanged?.Invoke(OwnerClientId,newHealth);
    }

    private void ApplyDamage(float damage)
    {
        if (!IsServer) return;

        _currentHealth.Value -= damage;
        characterAnimationsController.SetDamage();

        if (_currentHealth.Value <= 0)
        {
            Die();
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void TakeDamageServerRPC(float damage)
    {
        ApplyDamage(damage);
    }

    void Die()
    {
        if (!IsServer) return;

        NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject != null) 
        {
            networkObject.Despawn(true);
        }
    }
}
