using UnityEngine;
using UnityEngine.UI;

public class BotonPausaManager : MonoBehaviour
{
    void Start()
    {
        Button boton = GetComponent<Button>();
        if (boton != null)
        {
            boton.onClick.RemoveAllListeners();
            boton.onClick.AddListener(AlPresionarPausa);
            Debug.Log("✅ Botón Pausa configurado");
        }
        else
        {
            Debug.LogError("Este GameObject no tiene componente Button");
        }
    }

    public void AlPresionarPausa()
    {
        Debug.Log(" Botón Pausa presionado");

        // Reproducir sonido
        if (SoundManager.Instance != null)
            SoundManager.Instance.ReproducirSonidoClic();

        // Mostrar menú pausa
        if (MenuPausaManager.Instance != null)
        {
            MenuPausaManager.Instance.MostrarMenuPausa();
        }
        else
        {
            Debug.LogError(" MenuPausaManager.Instance no encontrado");
        }
    }

    // Métodos para que el MenuPausaManager pueda ocultar/mostrar este botón
    public void OcultarBoton()
    {
        gameObject.SetActive(false);
    }

    public void MostrarBoton()
    {
        gameObject.SetActive(true);
    }
}