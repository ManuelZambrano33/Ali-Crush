using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed;
    enum Direction { Left=-1, None=0, Right=1 };
    Direction currentDirection = Direction.None;
    
    Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentDirection = Direction.None;

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { 
            currentDirection = Direction.Left;
        }
        if ( Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = Direction.Right;
        }

        
    }

    private void FixedUpdate()
    {
        Vector2 velocity = new Vector2((int)currentDirection, 0f);
    }

    void Jump ()
    {
        Vector2 fuerza = new Vector2(0, 10f);
        rb2D.AddForce(fuerza, ForceMode2D.Impulse);
    }


}
