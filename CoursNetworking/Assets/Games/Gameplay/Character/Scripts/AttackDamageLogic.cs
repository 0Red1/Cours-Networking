using Unity.Netcode;
using UnityEngine;

public class AttackDamageLogic : NetworkBehaviour
{
    #region Variables
    [SerializeField] private float overlapRadius = 1f;
    [SerializeField] private float yOffset = 1f;
    [SerializeField] private LayerMask damageableLayer;
    private float damageAmount = 1f;
    #endregion

    private GameObject ownerObject;

    public void SetOwner(GameObject owner)
    {
        ownerObject = owner;
    }

    public void ExecuteDamageCheck(float damage)
    {
        if (!IsServer)
        {
            return;
        }

        Collider[] targets = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), overlapRadius);
        damageAmount = damage;

        foreach (Collider other in targets)
        {
            if (other.gameObject == ownerObject)
            {
                Debug.Log("Je veux pas me taper tout seul");
                continue;
            }

            if (ownerObject.CompareTag("Player"))
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    continue;
                }
            }
            else if (ownerObject.CompareTag("Enemy"))
            {
                if (!other.gameObject.CompareTag("Player"))
                {
                    continue;
                }
            }

            DamageReceiver dr = other.GetComponent<DamageReceiver>();

            if (dr != null && dr.HS != null)
            {
                dr.HS.TakeDamageServerRPC(damageAmount);
                Debug.Log("CurrentHealth de " + other.gameObject.name + " �gal a " + dr.HS.CurrentHealth.Value);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), overlapRadius);
    }
}
