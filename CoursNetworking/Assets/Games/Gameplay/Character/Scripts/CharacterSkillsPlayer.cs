using System;
using Unity.Netcode;
using UnityEngine;

public class CharacterSkillsPlayer : NetworkBehaviour
{
    #region Variables
    public event Action OnAttackTriggered;

    [SerializeField] private float attackCooldown = 5f;
    private float timeSinceLastAttack = 0f;
    #endregion

    private bool CanAttack => timeSinceLastAttack >= attackCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        timeSinceLastAttack += Time.deltaTime;
    }

    public void Skill1()
    {
        Debug.Log("Skill 1");
    }

    public void Skill2()
    {
        Debug.Log("Skill 2");
    }

    public void Skill3()
    {
        Debug.Log("Skill 3");
    }

    public void BaseAttack()
    {
        if (!IsOwner) return;
        if (!CanAttack) return;

        timeSinceLastAttack = 0f;

        SyncAttackEventClientRpc();
        RequestDamageExcutionServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void RequestDamageExcutionServerRpc()
    {
        AttackDamageLogic damageLogic = GetComponent<AttackDamageLogic>();

        if (damageLogic != null)
        {
            damageLogic.ExecuteDamageCheck(1);
        }
    }

    [ClientRpc]
    private void SyncAttackEventClientRpc()
    {
        OnAttackTriggered?.Invoke();
    }
}
