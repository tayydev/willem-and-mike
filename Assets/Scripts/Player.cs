using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 1f; //lower is faster
    private float moveCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveCooldown);
        moveCooldown = Mathf.Max(moveCooldown - Time.deltaTime, 0f);
        if (moveCooldown > 0f) return; //we dont run any ticks while on cooldown
        if (!GetWASD().Equals(Vector2.zero)) moveCooldown = movementSpeed;
        Vector2 current = this.transform.position;
        Vector2 potential = current + GetWASD();
        Vector2 dir = potential - current;
        bool cast = Physics2D.Raycast(current, dir, 1);
        if (!cast)
        {
            transform.position = potential;
            // transform.LookAt(dir);
        }
    }

    private static Vector2 GetWASD()
    {
        var movement = new Vector2();
        if (Input.GetKey(KeyCode.W))
        {
            movement.y += 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.y -= 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement.x -= 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1;
        }
        return movement;
    }
}
