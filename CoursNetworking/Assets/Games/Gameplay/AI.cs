using UnityEngine;

public class AI : MonoBehaviour
{
    #region Variables
    [SerializeField] private int healthPoint = 3;
    #endregion

    #region Built-in Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
    }
}
