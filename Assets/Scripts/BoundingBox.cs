using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class BoundingBox : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnCollision = new UnityEvent(); 
    
    float widthOffset;
    float heightOffset;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        widthOffset = spriteRenderer.sprite.rect.width * 0.5f;
        heightOffset = spriteRenderer.sprite.rect.height * 0.5f;
    }

    public float WidthOffset
    {
        get { return widthOffset; }
    }

    public float HeightOffset
    {
        get { return heightOffset; }
    }
}