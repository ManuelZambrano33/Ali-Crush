using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour
{
    Animator animator;
    Colisiones colisiones;
    Mover mover;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        colisiones = GetComponent<Colisiones>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
