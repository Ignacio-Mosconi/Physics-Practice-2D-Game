using UnityEngine;

[RequireComponent(typeof(BoundingBox))]
public class SpaceshipMovement : MonoBehaviour
{
    enum AccelerationType
    {
        Accelerate, Decelerate, Stop
    }

    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] float horizontalAcceleration = 2f;
    [SerializeField] float maxHorizontalSpeed = 15f;

    BoundingBox boundingBox;
    CameraBounds cameraBounds;
    Vector3 initialPosition;
    float[] boundaries = { 0f, 0f, 0f, 0f};
    float currentHorAcceleration = 0f;
    float currentHorSpeed = 0f;
    float initialHorSpeed = 0f;

    void Awake()
    {
        initialPosition = transform.position;
    }

    void Start()
    {
        boundingBox = GetComponent<BoundingBox>();
        cameraBounds = FindObjectOfType<CameraBounds>();

        boundingBox.OnCollision.AddListener(OnCollisionDetected);
    }

    void Update()
    {   
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Left") && currentHorAcceleration != -horizontalAcceleration)
            ChangeHorizontalAcceleration(AccelerationType.Decelerate);
        if (Input.GetButtonDown("Right") && currentHorAcceleration != horizontalAcceleration)
            ChangeHorizontalAcceleration(AccelerationType.Accelerate);

        initialHorSpeed = currentHorSpeed;
        currentHorSpeed = Mathf.Clamp(currentHorAcceleration * Time.deltaTime + initialHorSpeed, -maxHorizontalSpeed, maxHorizontalSpeed);
        
        transform.Translate(currentHorSpeed * Time.deltaTime, verticalMovement * verticalSpeed * Time.deltaTime, 0f);

        ClampPosition();
    }

    void ClampPosition()
    {
        float leftBorder = cameraBounds.GetBorder(Border.Left) + boundingBox.Width * 0.5f;
        float rightBorder = cameraBounds.GetBorder(Border.Right) - boundingBox.Width * 0.5f;
        float bottomBorder = cameraBounds.GetBorder(Border.Bottom) + boundingBox.Height * 0.5f;
        float topBorder = cameraBounds.GetBorder(Border.Top) - boundingBox.Height * 0.5f;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                                        Mathf.Clamp(transform.position.y, bottomBorder, topBorder),
                                        transform.position.z);

        if (transform.position.x == leftBorder || transform.position.x == rightBorder)
        {
            currentHorSpeed = 0f;
            if ((transform.position.x == leftBorder && currentHorAcceleration == -horizontalAcceleration) ||
                (transform.position.x == rightBorder && currentHorAcceleration == horizontalAcceleration))
                ChangeHorizontalAcceleration(AccelerationType.Stop);
        }
    }

    void ChangeHorizontalAcceleration(AccelerationType accelerationType)
    {
        switch (accelerationType)
        {
            case AccelerationType.Accelerate:
                currentHorAcceleration = horizontalAcceleration; 
                break;
            case AccelerationType.Decelerate:
                currentHorAcceleration = -horizontalAcceleration;
                break;
            case AccelerationType.Stop:
                currentHorAcceleration = 0f;
                break;
        }
    }

    void Respawn()
    {
        transform.position = initialPosition;
        ChangeHorizontalAcceleration(AccelerationType.Stop);
        initialHorSpeed = 0f;
        currentHorSpeed = 0f;
    }

    void OnCollisionDetected()
    {
        Respawn();
    }
}