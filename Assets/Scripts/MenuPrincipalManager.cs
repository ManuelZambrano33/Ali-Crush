using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public static MenuPrincipalManager Instance;

    [Header("BOTONES MENÚ PRINCIPAL")]
    public Button botonJugar;
    public Button botonSalir;

    public GameObject panelMenuPrincipal; //


    [Header("CONFIGURACIÓN ESCENAS")]
    public string nombreEscenaJuego = "";


    void Awake()
    {
        Debug.Log("=== MENU PRINCIPAL AWAKE ===");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MenuPrincipalManager - Singleton creado y persistente");

            // ✅ SUSCRIBIRSE al evento de carga de escena
            SceneManager.sceneLoaded += OnEscenaCargada;
        }
        else
        {
            Debug.Log("MenuPrincipalManager - Duplicado destruido");
            Destroy(gameObject);
            return;
        }
    }

    // ✅ VERIFICAR que esta escucha esté activa
    void OnEscenaCargada(Scene escena, LoadSceneMode modo)
    {
        Debug.Log($"🔄 Escena cargada: {escena.name}");

        if (escena.name == "Nivel_Andrea") // ✅ CAMBIAR por el nombre exacto
        {
            Debug.Log("✅ Escena Menu Principal detectada - Mostrando menú");
            MostrarMenu();

            // Cambiar música
            if (SoundManager.Instance != null)
                SoundManager.Instance.ReproducirMusicaMenu();
        }
        else if (escena.name == nombreEscenaJuego)
        {
            Debug.Log("🎮 Escena de Juego detectada - Ocultando menú");
            OcultarMenu();
        }
    }

    // ✅ AÑADIR para limpiar el evento
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnEscenaCargada;
    }

    void Start()
    {
        Debug.Log("=== MENU PRINCIPAL START ===");

        // Verificar botones
        if (botonJugar == null)
        {
            Debug.LogError(" BotonJugar NO está asignado en el Inspector!");
        }
        else
        {
            botonJugar.onClick.RemoveAllListeners();
            botonJugar.onClick.AddListener(Jugar);
            Debug.Log(" BotonJugar configurado correctamente");
        }

        if (botonSalir == null)
        {
            Debug.LogError(" BotonSalir NO está asignado en el Inspector!");
        }
        else
        {
            botonSalir.onClick.RemoveAllListeners();
            botonSalir.onClick.AddListener(Salir);
            Debug.Log(" BotonSalir configurado correctamente");
        }

        //  ELIMINADO: La búsqueda del SoundManager - usaremos el Singleton directamente

        // Escanear escenas disponibles
        EscanearEscenasBuild();

        MostrarMenu();
        Debug.Log(" Menú Principal completamente configurado");
    }

    void EscanearEscenasBuild()
    {
        Debug.Log("=== ESCANEO DE ESCENAS EN BUILD SETTINGS ===");
        Debug.Log($"Buscando escena: '{nombreEscenaJuego}'");

        bool escenaEncontrada = false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneManager.GetSceneByBuildIndex(i).path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            Debug.Log($"Índice {i}: '{sceneName}'");

            if (sceneName == nombreEscenaJuego)
            {
                escenaEncontrada = true;
                Debug.Log($" ESCENA ENCONTRADA: '{nombreEscenaJuego}' en índice {i}");
            }
        }

        if (!escenaEncontrada)
        {
            Debug.LogError($" ESCENA NO ENCONTRADA: '{nombreEscenaJuego}'");
            Debug.Log(" Posibles soluciones:");
            Debug.Log("1. Verifica el nombre EXACTO (mayúsculas/minúsculas)");
            Debug.Log("2. Asegúrate de que la escena está en Build Settings");
            Debug.Log("3. Revisa la carpeta de la escena");
        }
        else
        {
            Debug.Log($"✅ La escena '{nombreEscenaJuego}' está lista para cargarse");
        }
    }

    public void Jugar()
    {
        Debug.Log("=== BOTÓN JUGAR PRESIONADO ===");

        ReproducirSonidoClic();

        Debug.Log($"🔍 Intentando cargar escena: '{nombreEscenaJuego}'");

        try
        {
            // ✅ USAR SoundManager.Instance DIRECTAMENTE
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.ReproducirMusicaJuego();
                Debug.Log("✅ Música de juego iniciada");
            }
            else
            {
                Debug.LogError("❌ SoundManager.Instance es NULL!");
            }

            // Intentar cargar la escena
            SceneManager.LoadScene(nombreEscenaJuego);
            Debug.Log($"🎯 Carga de escena ejecutada: '{nombreEscenaJuego}'");

            OcultarMenu();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"💥 ERROR al cargar escena: {e.Message}");
            Debug.LogError($"Stack Trace: {e.StackTrace}");
        }
    }

    public void Salir()
    {
        Debug.Log("=== BOTÓN SALIR PRESIONADO ===");
        ReproducirSonidoClic();
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void MostrarMenu()
    {
        Debug.Log(" Mostrando Menu Principal");

        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(true);
            Debug.Log($"Panel activado");
        }

        if (botonJugar != null) botonJugar.gameObject.SetActive(true);
        if (botonSalir != null) botonSalir.gameObject.SetActive(true);

        Debug.Log(" Menú Principal visible");
    }
    public void OcultarMenu()
    {
        Debug.Log("Ocultando Menu Principal");


        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false);
            Debug.Log($" Panel ocultado");
        }

        Debug.Log(" Menú Principal ocultado");
    }

    public void VolverAlMenuPrincipal()
    {
        Debug.Log(" Volviendo al Menu Principal");

        //  USAR SoundManager.Instance DIRECTAMENTE
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ReproducirMusicaMenu();
            Debug.Log(" Música de menú restaurada");
        }

        MostrarMenu();
        Time.timeScale = 1f;
    }

    void ReproducirSonidoClic()
    {
        // ✅ USAR SoundManager.Instance DIRECTAMENTE - NO soundManager
        if (SoundManager.Instance != null && SoundManager.Instance.IsSoundEnabled())
        {
            Debug.Log("🔊 Reproduciendo sonido de clic");
            SoundManager.Instance.ReproducirSonidoClic(); // ✅ DESCOMENTADO
        }
        else
        {
            Debug.Log("🔇 Sonido desactivado o SoundManager no disponible");
        }
    }

    // Método para prueba manual desde consola
    [ContextMenu("Probar Carga de Escena")]
    void ProbarCargaEscena()
    {
        Debug.Log("=== PRUEBA MANUAL DESDE CONSOLA ===");
        Jugar();
    }
}