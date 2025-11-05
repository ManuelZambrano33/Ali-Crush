using UnityEngine;

public class FoodPickup : MonoBehaviour
{

    public AudioClip musicaClip; // asigna aquí el clip de audio


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Solo destruye el pickup si realmente recuperó vida
            bool recupero = GameManager.Instance.RecuperarVida();
            if (recupero)
            {

                // Reproduce música aunque el objeto se destruya
                if (musicaClip != null)
                {
                    AudioSource.PlayClipAtPoint(musicaClip, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}
