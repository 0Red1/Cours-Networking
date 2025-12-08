using Unity.Netcode;
using UnityEngine;

public class CharacterSkillsPlayer : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
