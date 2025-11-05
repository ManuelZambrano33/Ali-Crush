using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public Animator animator;
    private Mover mover; // tu script de movimiento

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        mover = GetComponent<Mover>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Aquí podrías poner animación de "hit" si la tienes
            Debug.Log("Jugador recibió daño, vida restante: " + currentHealth);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Jugador curado, vida actual: " + currentHealth);
    }

    private void Die()
    {
        isDead = true;
        mover.Die(); // dispara la animación de muerte
        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
