using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorImage;

    public int cursorWidth = 32;
    public int cursorHeight = 32;
    public float horizontalSpeed = 50.0F;
    public float verticalSpeed = 50.0F;
    private Vector2 cursorPosition;
    //private BoxCollider2D boxCollider2D;

    private void Start()
    {
        Cursor.visible = false;

        // optional place it in the center on start
        cursorPosition = new Vector2(200, 200);
        //boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnGUI()
    {
        // these are not actual positions but the change between last frame and now
        float h = horizontalSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        float v = verticalSpeed * Input.GetAxis("Vertical") * Time.deltaTime;

        // add the changes to the actual cursor position
        cursorPosition.x += h;
        cursorPosition.y += v;

        GUI.DrawTexture(new Rect(cursorPosition.x, Screen.height - cursorPosition.y, cursorWidth, cursorHeight), cursorImage);
        transform.position = new Vector2(cursorPosition.x, cursorPosition.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }
}
