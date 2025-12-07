using Unity.Netcode;
using UnityEngine;

public class TestDamage : NetworkBehaviour
{
    #region Variables
    [SerializeField] private float overlapRadius = 1f;

    private Collider[] colliders = new Collider[5];
    private float damageCooldown = 0.5f;
    private float timeSinceLastDamage = 0;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= damageCooldown)
        {
            CheckDamage();
            timeSinceLastDamage = 0;
        }
    }

    void CheckDamage()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, overlapRadius);

        foreach (Collider other in target)
        {
            if (other.gameObject.CompareTag("Damage"))
            {
                Debug.Log("Je touche : " + other.gameObject.name);
                DamageReceiver dr = other.GetComponent<DamageReceiver>();
                dr.HS.TakeDamageServerRPC(1);
                Debug.Log("CurrentHealth de " + other.gameObject.name + " égal a " + dr.HS.CurrentHealth.Value);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }
}
