using UnityEngine;

public class CharacterAnimationsController : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void SetDash(bool dash)
    {
        animator.SetBool("Dash", dash);
    }
}
