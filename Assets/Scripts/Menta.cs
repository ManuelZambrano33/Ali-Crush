using UnityEngine;
using System.Collections;

public class Menta : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] waypoints; // puntos de patrulla
    public float idleTime = 1.5f; // tiempo en Idle en cada esquina

    private Rigidbody2D rb;
    private Animator animator;
    private int currentPoint = 0;
    private bool isDead = false;
    private bool isIdling = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        animator.SetBool("Walk", true);

        // Voltear sprite según dirección
        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);

        // Si llegó al waypoint
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
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);

        yield return new WaitForSeconds(idleTime);

        animator.SetBool("Idle", false);
        isIdling = false;
    }

    public void Squash()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 1.5f);
    }
}
