using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Llamamos al método Die() del Player
            Mover mover = other.GetComponent<Mover>();
            if (mover != null)
            {
                mover.Die(); // aquí se activa el Trigger "Death" en el Animator
            }

            // Reiniciamos la escena después de 0.6 segundos
            Invoke(nameof(Restart), 0.6f);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
