using UnityEngine;

public class Ball : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Color Color { get => spriteRenderer.color; set => spriteRenderer.color = value; }

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
