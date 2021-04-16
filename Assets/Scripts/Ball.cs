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

    public void Take()
    {
        increase.SetBool("IsTake", true);
    }

    public void Put()
    {
        increase.SetBool("IsTake", false);
    }

    public void Move(Vector2 pos)
    {
        StartCoroutine(ISteps(pos));
    }

    private IEnumerator ISteps(Vector3 point)
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
