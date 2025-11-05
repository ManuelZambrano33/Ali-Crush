using UnityEngine;
using System.Collections;

public class BossMenta : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 10;
    private int currentHealth;
    public float speed = 2f;
    public float idleTime = 2f;

    [Header("Patrulla")]
    public Transform[] waypoints; // puntos de patrulla
    private int currentPoint = 0;
    private int direction = 1; // 1 = hacia adelante, -1 = hacia atrás


    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;
    private bool isIdling = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead || isIdling) return;

        Patrol();
    }

    void Patrol()
    {
        Transform target = waypoints[currentPoint];
        Vector2 dir = (target.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

        // Activar animación de caminar
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);

        // Voltear sprite según dirección
        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);

        // Llegó al waypoint
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(IdleAtPoint());
            currentPoint = (currentPoint + 1) % waypoints.Length;
        }
    }

    IEnumerator IdleAtPoint()
    {
        isIdling = true;
        rb.velocity = Vector2.zero;

        // Activar Idle
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);

        yield return new WaitForSeconds(idleTime);

        // Salir de Idle
        animator.SetBool("Idle", false);
        isIdling = false;
    }


    // --- Recibir daño ---
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- Morir ---
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 2f);
    }

    // --- Atacar (puedes llamarlo desde un evento de animación) ---
    public void Attack()
    {
        animator.SetTrigger("Attack");
        // Aquí puedes instanciar proyectiles o aplicar daño al jugador si está cerca
    }
}
