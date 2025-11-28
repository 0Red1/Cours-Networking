using UnityEngine;

public class TestDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            DamageReceiver dr = other.GetComponent<DamageReceiver>();

            dr.HS.TakeDamageServerRPC(1);
            Debug.Log("CurrentHealth de " +  other.gameObject.name + " égal a " + dr.HS.CurrentHealth.Value);
        }
    }
}
