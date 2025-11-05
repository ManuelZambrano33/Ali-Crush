using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BOTÓN SONIDO - PREFAB")]
    public GameObject botonSoundPrefab;

    [Header("SPRITES BOTÓN")]
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    [Header("CONFIGURACIÓN AUDIO")]
    public AudioClip musicaMenu;
    public AudioClip musicaJuego;
    public AudioClip sonidoClic;

    [Header("VOLÚMENES")]
    [Range(0f, 50f)] public float volumenMusica = 40f;
    [Range(0f, 50f)] public float volumenEfectos = 40f;

    private AudioSource audioSourceMusica;
    private AudioSource audioSourceEfectos;
    private bool soundEnabled = true;
    private Button botonSoundInstance;
    private Image imagenBoton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ PERSISTENTE

            // Crear AudioSources
            audioSourceMusica = gameObject.AddComponent<AudioSource>();
            audioSourceEfectos = gameObject.AddComponent<AudioSource>();

            // Configurar AudioSource de música
            audioSourceMusica.loop = true;
            audioSourceMusica.volume = volumenMusica;

            // Cargar estado
            soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

            Debug.Log("🔊 SoundManager persistente iniciado");

            // Suscribirse al evento de carga de escenas
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Crear botón de sonido automáticamente
        CrearBotonSound();

        // Reproducir música del menú al inicio
        ReproducirMusicaMenu();
    }

    // ✅ MÉTODO QUE SE EJECUTA CADA VEZ QUE SE CARGA UNA NUEVA ESCENA
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"🔄 SoundManager - Nueva escena cargada: {scene.name}");

        // Recrear el botón en la nueva escena
        CrearBotonSound();

        // Si es una escena de juego, cambiar la música
        if (scene.name != "MenuPrincipal" && scene.name != "MainMenu") // Ajusta estos nombres
        {
            ReproducirMusicaJuego();
        }
    }

    void CrearBotonSound()
    {
        // Destruir botón anterior si existe
        if (botonSoundInstance != null)
        {
            Destroy(botonSoundInstance.gameObject);
            botonSoundInstance = null;
            imagenBoton = null;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ No se encontró Canvas en la escena!");
            return;
        }

        if (botonSoundPrefab == null)
        {
            Debug.LogError("❌ BotonSoundPrefab no asignado!");
            return;
        }

        // Instanciar el prefab como hijo del Canvas
        GameObject botonObj = Instantiate(botonSoundPrefab, canvas.transform);

        // Obtener componentes
        botonSoundInstance = botonObj.GetComponent<Button>();
        imagenBoton = botonObj.GetComponent<Image>();

        if (botonSoundInstance == null)
        {
            Debug.LogError("❌ El prefab no tiene componente Button!");
            return;
        }

        if (imagenBoton == null)
        {
            imagenBoton = botonObj.GetComponentInChildren<Image>();
        }

        if (imagenBoton == null)
        {
            Debug.LogError("❌ No se encontró Image en el prefab!");
            return;
        }

        // Configurar botón
        botonSoundInstance.onClick.RemoveAllListeners();
        botonSoundInstance.onClick.AddListener(ToggleSound);

        // Posicionar en esquina superior derecha
        PosicionarBoton(botonObj);

        // Actualizar icono inicial
        ActualizarIconoBoton();

        Debug.Log($"✅ Botón de sonido creado en escena: {SceneManager.GetActiveScene().name}");
    }

    void PosicionarBoton(GameObject botonObj)
    {
        RectTransform rt = botonObj.GetComponent<RectTransform>();
        if (rt != null)
        {
            // Esquina superior derecha - Tamaño grande
            rt.anchorMin = new Vector2(1f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 1f);
            rt.anchoredPosition = new Vector2(-50f, -50f);
            rt.sizeDelta = new Vector2(250f, 250f);
        }
    }

    void ActualizarIconoBoton()
    {
        if (imagenBoton != null && soundOnSprite != null && soundOffSprite != null)
        {
            imagenBoton.sprite = soundEnabled ? soundOnSprite : soundOffSprite;
        }
    }

    // MÉTODOS PARA CONTROLAR MÚSICA
    public void ReproducirMusicaMenu()
    {
        if (!soundEnabled || musicaMenu == null) return;

        audioSourceMusica.clip = musicaMenu;
        audioSourceMusica.Play();
        Debug.Log("🎵 Reproduciendo música del menú");
    }

    public void ReproducirMusicaJuego()
    {
        if (!soundEnabled || musicaJuego == null) return;

        audioSourceMusica.clip = musicaJuego;
        audioSourceMusica.Play();
        Debug.Log("🎵 Reproduciendo música del juego");
    }

    public void PausarMusica()
    {
        audioSourceMusica.Pause();
        Debug.Log("⏸️ Música pausada");
    }

    public void ReanudarMusica()
    {
        if (soundEnabled && audioSourceMusica.clip != null)
        {
            audioSourceMusica.UnPause();
            Debug.Log("▶️ Música reanudada");
        }
    }

    public void DetenerMusica()
    {
        audioSourceMusica.Stop();
        Debug.Log("⏹️ Música detenida");
    }

    public void ReproducirSonidoClic()
    {
        if (!soundEnabled || sonidoClic == null) return;

        audioSourceEfectos.PlayOneShot(sonidoClic, volumenEfectos);
    }

    // Control de estado de sonido
    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;

        if (soundEnabled)
        {
            ReanudarMusica();
        }
        else
        {
            PausarMusica();
        }

        // Actualizar icono del botón
        ActualizarIconoBoton();

        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("🔊 Sonido: " + (soundEnabled ? "ACTIVADO" : "DESACTIVADO"));
    }

    public bool IsSoundEnabled()
    {
        return soundEnabled;
    }

    // ✅ IMPORTANTE: Limpiar el evento al destruir
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}