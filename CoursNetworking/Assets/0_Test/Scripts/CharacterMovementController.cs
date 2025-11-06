using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private bool facingRight = true;
    private Vector3 movement = new Vector3(0f, 0f, 0f);
    
    void FixedUpdate()
    {
        transform.Translate(movement * (Time.fixedDeltaTime * speed));
        //Vector3 move = transform.forward * movement.x * (speed * Time.deltaTime);
        //transform.position += move;
    }
    
    public void Move(Vector2 direction)
    {
        // On se déplace le long de l'axe avant/arrière local
        
        movement = new Vector3(direction.y, 0f, direction.x);
        
        HandleFlip(direction.x);
    }
    
    private void HandleFlip(float xInput)
    {
        // Si on appuie vers la droite et qu'on n'est pas tourné dans cette direction → flip
        if (xInput > 0 && !facingRight)
            Flip();

        // Si on appuie vers la gauche et qu'on regarde à droite → flip
        if (xInput < 0 && facingRight)
            Flip();
    }
    
    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.z *= -1f; // inversion de l'axe X, uniquement le visuel
        transform.localScale = scale;
    }
}
