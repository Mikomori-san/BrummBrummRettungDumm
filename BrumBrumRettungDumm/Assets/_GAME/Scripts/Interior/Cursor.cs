using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 cursorOffset = new Vector2(cursor.width / 2, cursor.height / 2);
        UnityEngine.Cursor.SetCursor(cursor, cursorOffset, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
