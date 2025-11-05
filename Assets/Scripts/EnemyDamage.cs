using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("⚙️ Configuración de Daño")]
    public bool destruirDespuesDeDanio = true;

    [Header("🔊 Efecto de sonido")]
    public AudioClip musicaClip;

    private bool yaHizoDanio = false; // Para evitar daño repetido

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Referencia al Rigidbody2D del jugador
        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // Verificar si el jugador está cayendo sobre el enemigo (y arriba)
        if (playerRb.velocity.y < 0 && other.transform.position.y > transform.position.y + 0.3f)
        {
            // El jugador aplastó al enemigo
            Menta menta = GetComponent<Menta>();
            if (menta != null)
                menta.Squash();

            // Rebote del jugador al saltar sobre el enemigo
            playerRb.velocity = new Vector2(playerRb.velocity.x, 10f); // ajustar fuerza
        }
        else
        {
            // Daño al jugador
            if (yaHizoDanio) return;
            yaHizoDanio = true;

            Debug.Log("🎯 El jugador ha sido golpeado por el enemigo");

            if (GameManager.Instance != null)
                GameManager.Instance.PerderVida();
            else
                Debug.LogWarning("⚠️ GameManager.Instance es NULL");

            if (musicaClip != null)
                AudioSource.PlayClipAtPoint(musicaClip, transform.position);

            if (destruirDespuesDeDanio)
                Destroy(gameObject, 0.1f);
        }
    }
}
