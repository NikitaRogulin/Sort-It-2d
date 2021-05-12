using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator increase;

    public UnityEvent Arrived;

    public Color Color { get => spriteRenderer.color; set => spriteRenderer.color = value; }
    public float Radius => transform.localScale.x * 0.5f;

    private void Awake()
    {
        increase = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Use(bool value)
    {
        increase.SetBool("IsTake", value);
    }

    public void Move(Vector2 pos)
    {
        StartCoroutine(MoveCoroutine(pos));
    }

    private IEnumerator MoveCoroutine(Vector3 point)
    {
        float totalMovementTime = 15f;
        while (Vector3.Distance(transform.localPosition, point) > 0)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, point, Time.deltaTime * totalMovementTime);
            yield return null;
        }
        Arrived.Invoke();
    }
}
