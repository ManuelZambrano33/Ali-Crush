using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return; // Evita movimiento y salto al morir

        // Movimiento
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Voltear sprite
        if (moveInput != 0)
            transform.localScale = new Vector3(moveInput, 1, 1);

        // Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Animaciones
        animator.SetFloat("Velocity", Mathf.Abs(moveInput));
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Jumping", !isGrounded);
        animator.SetBool("Skid", isGrounded && moveInput == 0 && Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // --- Enemigos normales (Menta) ---
        if (col.collider.CompareTag("Enemy"))
        {
            foreach (var contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * 8f, ForceMode2D.Impulse);

                    var menta = col.collider.GetComponent<Menta>();
                    if (menta != null) menta.Squash();
                    return;
                }
            }
            GameManager.Instance.PerderVida();
        }

        // --- Boss (BossMenta) ---
        if (col.collider.CompareTag("Boss"))
        {
            foreach (var contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

                    var boss = col.collider.GetComponent<BossMenta>();
                    if (boss != null) boss.TakeDamage(1);
                    return;
                }
            }
            GameManager.Instance.PerderVida();
        }
    }


    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.LogWarning("Die() llamado, activando bool Death");
        animator.SetBool("Death", true); // Se activa la animación de muerte
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
    }
}
