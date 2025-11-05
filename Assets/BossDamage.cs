using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [Header("Configuración")]
    public int maxHealth = 10;
    private int currentHealth;
    public AudioClip sonidoGolpe;

    private bool yaGolpeoJugador = false;

    private Rigidbody2D rb;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Trigger para la cabeza del boss
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // Verifica si el jugador viene desde arriba
        if (playerRb.velocity.y < 0 && other.transform.position.y > transform.position.y + 0.5f)
        {
            // Daño al boss
            TakeDamage(1);

            // Rebote del jugador
            playerRb.velocity = new Vector2(playerRb.velocity.x, 10f);

            if (sonidoGolpe != null)
                AudioSource.PlayClipAtPoint(sonidoGolpe, transform.position);
        }
        else
        {
            // Daño al jugador
            if (yaGolpeoJugador) return;
            yaGolpeoJugador = true;

            if (GameManager.Instance != null)
                GameManager.Instance.PerderVida();
            else
                Debug.LogWarning("⚠️ GameManager.Instance es NULL");

            if (sonidoGolpe != null)
                AudioSource.PlayClipAtPoint(sonidoGolpe, transform.position);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log($"Boss recibió {dmg} de daño, vida restante: {currentHealth}");

        // Puedes reproducir animación de daño
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Boss muerto");
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Death");

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        Destroy(gameObject, 2f);
    }
}
