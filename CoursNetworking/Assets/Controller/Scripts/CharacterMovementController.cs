using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CharacterMovementController : NetworkBehaviour
{
    #region Variables
    [SerializeField] private float moveSpeed = 5f;

    private bool facingRight = true;
    private Vector3 m_moveDirection;
    private CharacterController m_cc;
    private Character m_character;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            
            return;
            
            
        Vector3 movement = m_moveDirection * moveSpeed * Time.deltaTime;
        m_cc.Move(movement);
        HandleFlip(m_moveDirection.x);

        float speed = m_moveDirection.magnitude;
        m_character.UpdateMovementAnimation(speed);
    }

    public void SetManager(Character manager)
    {
        m_character = manager;
    }

    public void SetInputDirection(Vector2 input)
    {
        m_moveDirection.x = input.x;
        m_moveDirection.z = input.y;
    }

    public void Dash()
    {
        StartCoroutine(DashCoroutine(1f));
    }

    IEnumerator DashCoroutine(float duration)
    {
        float dashSpeed = 7f;
        float startTime = Time.time;
        Vector3 dashDir = new Vector3(m_moveDirection.x, 0, m_moveDirection.z).normalized;

        if (dashDir == Vector3.zero)
        {
            dashDir = transform.right;
        }

        m_character.PlayDashAnim();

        while (Time.time < startTime + duration)
        {
            m_cc.Move(dashDir * dashSpeed * Time.deltaTime);

            yield return null;
        }

        m_character.StopDashAnim();
    }

    private void HandleFlip(float xInput)
    {
        if (xInput > 0.01f && !facingRight)
            Flip();

        if (xInput < -0.01f && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}
