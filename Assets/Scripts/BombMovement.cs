using UnityEngine;

[RequireComponent(typeof(BoundingBox))]
public class BombMovement : MonoBehaviour
{
    float initialSpeed;
    float currentSpeed;
    float initialVerPosition;
    float widthOffset;
    float heightOffset;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        BoundingBox box = GetComponent<BoundingBox>();

        widthOffset = spriteRenderer.sprite.rect.width * 0.5f;
        heightOffset = spriteRenderer.sprite.rect.height * 0.5f;
        
        SetStartingPosition();
        SetStartingSpeed();

        box.OnCollision.AddListener(OnCollisionDetected);
    }

    void SetStartingPosition()
    {
        float horizontalSpawnPos = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(widthOffset, 0f, 0f)).x,
                                            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - widthOffset, 0f)).x);
        float verticalSpawnPos = Camera.main.ScreenToWorldPoint(new Vector3(0f, heightOffset, 0f)).y;

        transform.position = new Vector3(horizontalSpawnPos, verticalSpawnPos, 0f);
    }

    void SetStartingSpeed()
    {
        initialSpeed = BombManager.Instance.GetRandomStartSpeed();
        currentSpeed = initialSpeed;
    }

    void OnCollisionDetected()
    {
        SetStartingPosition();
        SetStartingSpeed();
    }

    public void Bounce()
    {
        initialSpeed = currentSpeed;
        currentSpeed = -BombManager.Instance.Gravity * Time.deltaTime + initialSpeed;

        transform.Translate(0f, currentSpeed * Time.deltaTime, 0f);

        if (Camera.main.WorldToScreenPoint(transform.position).y <= 0f)
            SetStartingSpeed();
    }
}