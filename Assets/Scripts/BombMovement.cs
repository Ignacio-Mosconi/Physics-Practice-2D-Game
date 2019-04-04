using UnityEngine;

[RequireComponent(typeof(BoundingBox))]
public class BombMovement : MonoBehaviour
{
    CameraBounds cameraBounds;
    BoundingBox boundingBox;
    float initialSpeed;
    float currentSpeed;
    float initialVerPosition;

    void Awake()
    {
        cameraBounds = FindObjectOfType<CameraBounds>(); 
        boundingBox = GetComponent<BoundingBox>();

        boundingBox.OnCollision.AddListener(OnCollisionDetected);
    }

    void Start()
    {
        SetStartingPosition();
        SetStartingSpeed();
    }

    void SetStartingPosition()
    {
        float horizontalSpawnPos = Random.Range(cameraBounds.GetBorder(Border.Left) + boundingBox.Width * 0.5f,
                                                cameraBounds.GetBorder(Border.Right) - boundingBox.Width * 0.5f);
        float verticalSpawnPos = cameraBounds.GetBorder(Border.Bottom) + boundingBox.Height * 0.5f;

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