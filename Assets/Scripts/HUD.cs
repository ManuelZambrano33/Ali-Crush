using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] vidas;

    public void ActualizarVidas(int cantidad)
    {
        for (int i = 0; i < vidas.Length; i++)
            vidas[i].SetActive(i < cantidad);
    }
}
