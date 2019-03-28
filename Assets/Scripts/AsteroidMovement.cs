using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AsteroidMovement : MonoBehaviour
{   
    float speed;
    float heightOffset;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        heightOffset = spriteRenderer.sprite.rect.height * 0.5f; 
    }

    public void Respawn()
    {
        float horizontalSpawnPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;
        float verticalSpawnPos = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(0f, heightOffset, 0f)).y, 
                                            Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height - heightOffset, 0f)).y);

        transform.position = new Vector3(horizontalSpawnPos, verticalSpawnPos, 0f);
        speed = AsteroidManager.Instance.GetRandomAsteroidSpeed();
    }

    public void Move()
    {
        transform.Translate(-speed * Time.deltaTime, 0f, 0f);

        if (Camera.main.WorldToScreenPoint(transform.position).x < 0f)
            Respawn();
    }
}