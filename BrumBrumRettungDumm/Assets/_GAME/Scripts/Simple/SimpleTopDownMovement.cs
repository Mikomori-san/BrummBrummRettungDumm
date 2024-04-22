using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleTopDownMovement : MonoBehaviour
{
    private Vector2 velocity;
    [Range(0.01f, 15)] public float speed = 1;

    void Update()
    {
        if (Keyboard.current.aKey.isPressed) { velocity.x = -1; } else { if (Keyboard.current.dKey.isPressed) { velocity.x = 1; } else { velocity.x = 0; } }
        if (Keyboard.current.sKey.isPressed) { velocity.y = -1; } else { if (Keyboard.current.wKey.isPressed) { velocity.y = 1; } else { velocity.y = 0; } }

        //Old Input System
        //velocity.x = Input.GetAxisRaw("Horizontal");
        //velocity.y = Input.GetAxisRaw("Vertical");

        this.transform.Translate(velocity * speed);
    }
}
