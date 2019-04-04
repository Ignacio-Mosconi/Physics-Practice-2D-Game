using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class BoundingBox : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnCollision = new UnityEvent();
    SpriteRenderer spriteRenderer; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        LayerMask layer = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

        CollisionManager.Instance.RegisterBoundingBox(layer, this);
    }

    public float Width
    {
        get { return spriteRenderer.bounds.size.x; }
    }

    public float Height
    {
        get { return spriteRenderer.bounds.size.y; }
    }
}