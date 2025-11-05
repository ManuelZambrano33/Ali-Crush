using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausaManager : MonoBehaviour
{
    public static MenuPausaManager Instance;

    [Header("REFERENCIAS UI")]
    public GameObject panelPausa; // El panel completo del menú pausa
    public Button botonReanudar;
    public Button botonMenuPrincipal;

    [Header("CONFIGURACIÓN")]
    public bool pausaActivada = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MenuPausaManager - Singleton creado");

            // Ocultar al inicio
            if (panelPausa != null)
                panelPausa.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Configurar botones
        if (botonReanudar != null)
        {
            botonReanudar.onClick.RemoveAllListeners();
            botonReanudar.onClick.AddListener(ReanudarJuego);
        }

        if (botonMenuPrincipal != null)
        {
            botonMenuPrincipal.onClick.RemoveAllListeners();
            botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);
        }

        Debug.Log(" MenuPausaManager - Configuración completada");
    }

    void Update()
    {
        // Tecla ESC para pausar/reanudar
        if (Input.GetKeyDown(KeyCode.Escape) && pausaActivada)
        {
            if (IsPausaActiva())
                ReanudarJuego();
            else
                MostrarMenuPausa();
        }
    }

    public void MostrarMenuPausa()
    {
        if (panelPausa != null)
        {
            panelPausa.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego

            // Pausar música
            if (SoundManager.Instance != null)
                SoundManager.Instance.PausarMusica();

            Debug.Log(" Menú Pausa mostrado");
        }
    }

    public void OcultarMenuPausa()
    {
        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
            Time.timeScale = 1f; // Reanudar juego

            // Reanudar música
            if (SoundManager.Instance != null)
                SoundManager.Instance.ReanudarMusica();

            Debug.Log(" Menú Pausa ocultado");
        }
    }

    void ReanudarJuego()
    {
        // Reproducir sonido
        if (SoundManager.Instance != null)
            SoundManager.Instance.ReproducirSonidoClic();

        OcultarMenuPausa();
    }

    void IrAlMenuPrincipal()
    {
        Debug.Log(" Volviendo al Menu Principal desde Nivel1");


        Time.timeScale = 1f;
        OcultarMenuPausa();

        // CARGAR LA ESCENA DEL MENU PRINCIPAL
        SceneManager.LoadScene("Natalia"); // Cambia por el nombre exacto de tu escena

        Debug.Log(" Cargando escena Menu Principal");

        // La música se cambiará automáticamente cuando se cargue la escena
        // o en el Start() del MenuPrincipalManager
    }
    public bool IsPausaActiva()
    {
        return panelPausa != null && panelPausa.activeInHierarchy;
    }

    // Método para activar/desactivar la pausa desde otros scripts
    public void SetPausaActivada(bool activada)
    {
        pausaActivada = activada;
    }
}