using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ==========================
    // 🔴 SECCIÓN: VIDAS Y HUD
    // ==========================
    [Header("Configuración de Vidas")]
    [SerializeField] private int maxVidas = 3;
    private int vidas;

    [Header("Prefab del HUD")]
    public HUD hudPrefab;
    private HUD hudInstance;

    // ==========================
    // 🟣 SECCIÓN: MENÚS Y UI
    // ==========================
    [Header("Prefabs de UI")]
    public GameObject menuPausaPrefab;
    public GameObject gameOverPrefab;

    private GameObject menuPausaInstance;
    public Button botonPausa;

    // ==========================
    // 🟢 CICLO DE VIDA UNITY
    // ==========================
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Iniciar vidas
        vidas = maxVidas;

        // Instanciar HUD persistente
        if (hudInstance == null && hudPrefab != null)
        {
            hudInstance = Instantiate(hudPrefab);
            DontDestroyOnLoad(hudInstance.gameObject);
        }

        // Sincronizar HUD
        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);

        // Crear interfaz de usuario (pausa, etc.)
        CrearInterfazUsuario();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-sincronizar HUD al cambiar de escena
        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);
    }

    // ==========================
    // ❤️ LÓGICA DE VIDAS
    // ==========================
    public void PerderVida()
    {
        vidas--;

        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);

        if (vidas <= 0)
        {
            vidas = maxVidas;

            // Mostrar Game Over si hay prefab
            if (gameOverPrefab != null)
                Instantiate(gameOverPrefab);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public bool RecuperarVida()
    {
        if (vidas >= maxVidas)
            return false;

        vidas++;

        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);

        return true;
    }

    public int GetVidas()
    {
        return vidas;
    }

    // ==========================
    // ⏸️ MENÚ DE PAUSA Y UI
    // ==========================
    private void CrearInterfazUsuario()
    {
        if (menuPausaPrefab != null && menuPausaInstance == null)
        {
            menuPausaInstance = Instantiate(menuPausaPrefab);
            DontDestroyOnLoad(menuPausaInstance);
            menuPausaInstance.SetActive(false);
        }
    }

    public void MostrarMenuPausa()
    {
        if (menuPausaInstance != null)
        {
            menuPausaInstance.SetActive(true);
            Time.timeScale = 0f;
            if (botonPausa != null)
                botonPausa.gameObject.SetActive(false);
        }
    }

    public void OcultarMenuPausa()
    {
        if (menuPausaInstance != null)
        {
            menuPausaInstance.SetActive(false);
            Time.timeScale = 1f;
            if (botonPausa != null)
                botonPausa.gameObject.SetActive(true);
        }
    }

    public void ConfigurarBotonPausa(Button nuevoBoton)
    {
        botonPausa = nuevoBoton;
        if (botonPausa != null)
        {
            botonPausa.onClick.RemoveAllListeners();
            botonPausa.onClick.AddListener(MostrarMenuPausa);
        }
    }
}
