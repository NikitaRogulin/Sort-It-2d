using UnityEngine;

public class Ball : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Color Color { get => spriteRenderer.color; set => spriteRenderer.color = value; }
    public float Radius => transform.localScale.x * 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
