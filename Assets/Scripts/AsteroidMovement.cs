using UnityEngine;

[RequireComponent(typeof(BoundingBox))]
public class AsteroidMovement : MonoBehaviour
{
    CameraBounds cameraBounds;
    BoundingBox boundingBox;
    float speed;

    void Awake()
    {
        cameraBounds = FindObjectOfType<CameraBounds>();
        boundingBox = GetComponent<BoundingBox>();
        
        boundingBox.OnCollision.AddListener(OnCollsionDetected);
    }

    void OnCollsionDetected()
    {
        Respawn();
    }

    public void Respawn()
    {
        float horizontalSpawnPos = cameraBounds.GetBorder(Border.Right);
        float verticalSpawnPos = Random.Range(cameraBounds.GetBorder(Border.Bottom) + boundingBox.Height * 0.5f, 
                                            cameraBounds.GetBorder(Border.Top) - boundingBox.Height * 0.5f);

        transform.position = new Vector3(horizontalSpawnPos, verticalSpawnPos, 0f);
        speed = AsteroidManager.Instance.GetRandomAsteroidSpeed();
    }

    public void Move()
    {
        transform.Translate(-speed * Time.deltaTime, 0f, 0f);

        if (Camera.main.WorldToViewportPoint(transform.position).x < 0f)
            Respawn();
    }
}