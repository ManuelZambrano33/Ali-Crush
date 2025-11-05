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
Debug.LogWarning("⚠️ GameManager.Instance AAQUIIII "); 
        if (hudInstance != null)
            hudInstance.ActualizarVidas(vidas);

        if (vidas <= 0)
        {
            vidas = maxVidas;

            // Mostrar Game Over si hay prefab
         //   if (gameOverPrefab != null)
           //     Instantiate(gameOverPrefab);

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


}
