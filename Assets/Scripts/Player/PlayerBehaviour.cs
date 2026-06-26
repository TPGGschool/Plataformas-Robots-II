using UnityEngine;
public class PlayerBehaviour : MonoBehaviour{
    private int t_movSpeed = 300, t_jumpCount = 2;
        //private int t_layerint = 3; 
    private float t_inputHorizontal, t_jumpForce = 8f, t_maxJumpForce = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    [System.NonSerialized]
    public bool hasKey;
    void Start(){
        getComponents();
    }
    void Update(){
        playerJump();
        playerDirection();
    }
    private void FixedUpdate(){
        playerMovement();
    }
    
    void getComponents(){
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void playerMovement(){
        t_inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(t_inputHorizontal * t_movSpeed * Time.deltaTime, rb.linearVelocity.y);
    }
    void playerJump(){
        if (Input.GetKeyDown(KeyCode.Space) && t_jumpCount > 0){
            rb.AddForce(Vector2.up * t_jumpForce, ForceMode2D.Impulse);
            t_jumpCount--;
            animator.SetBool("Jumping", true);
        }
        if (rb.linearVelocity.magnitude > t_maxJumpForce){
            rb.linearVelocity = rb.linearVelocity.normalized * t_maxJumpForce;
        }
    }

    bool hasjumpbeenreseted = false;
    void OnCollisionStay2D(Collision2D collision)
    {

        // resetear salto
        if (collision.collider.CompareTag("Floor"))
        {


            // esto es para que el salto solo se resetee cuando toca el suelo, y no una pared, techo etc...
            // Aqu� se utiliza un condicional para verificar el �ngulo entre la normal de la colisi�n y el vector hacia arriba (Vector2.up).
            // La normal es un vector perpendicular a la superficie de colisi�n. 
            // Este condicional verifica si el �ngulo entre la normal y el vector hacia arriba es menor a 45 grados.
            // conseguido de stack overflow de usuario "Voidsay"

            // revisar todos los puntos de contacto
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Angle(contact.normal, Vector2.up) < 45 && !hasjumpbeenreseted)
                {
                    t_jumpCount = 2;
                    animator.SetBool("Jumping", false);
                    hasjumpbeenreseted = true;
                }
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.collider.CompareTag("Floor"))
        {
            hasjumpbeenreseted = false;
        }
    }
    void playerDirection(){
        if (t_inputHorizontal < 0){
            sprite.flipX = true;
            animator.SetBool("Walking", true);
        }
        if (t_inputHorizontal > 0){
            sprite.flipX = false;
            animator.SetBool("Walking", true);
        }
        else if (t_inputHorizontal == 0){
            animator.SetBool("Walking", false);
        }
    }
   
}
