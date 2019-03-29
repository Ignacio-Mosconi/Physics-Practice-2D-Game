using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoundingBox))]
public class SpaceshipMovement : MonoBehaviour
{
    enum AccelerationType
    {
        Accelerate, Decelerate, Stop
    }

    enum Boundary
    {
        Top, Bottom, Left, Right
    }

    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] float horizontalAcceleration = 2f;
    [SerializeField] float maxHorizontalSpeed = 15f;

    Vector3 initialPosition;
    float[] boundaries = { 0f, 0f, 0f, 0f};
    float currentHorAcceleration = 0f;
    float currentHorSpeed = 0f;
    float initialHorSpeed = 0f;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float widthOffset = spriteRenderer.sprite.rect.width * 0.5f;
        float heightOffset = spriteRenderer.sprite.rect.height * 0.5f;
        
        boundaries[(int)Boundary.Top] = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height - heightOffset, 0f)).y;
        boundaries[(int)Boundary.Bottom] = Camera.main.ScreenToWorldPoint(new Vector3(0f, heightOffset, 0f)).y ;
        boundaries[(int)Boundary.Left] = Camera.main.ScreenToWorldPoint(new Vector3(widthOffset, 0f, 0f)).x;
        boundaries[(int)Boundary.Right] = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - widthOffset, 0f, 0f)).x;

        initialPosition = transform.position;
    }

    void Start()
    {
        BoundingBox box = GetComponent<BoundingBox>();
        LayerMask layer = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
        
        CollisionManager.Instance.RegisterBoundingBox(layer, box);

        box.OnCollision.AddListener(OnCollisionDetected);
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
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundaries[(int)Boundary.Left], boundaries[(int)Boundary.Right]),
                                Mathf.Clamp(transform.position.y, boundaries[(int)Boundary.Bottom], boundaries[(int)Boundary.Top]),
                                transform.position.z);

        if (transform.position.x == boundaries[(int)Boundary.Left] || transform.position.x == boundaries[(int)Boundary.Right])
        {
            currentHorSpeed = 0f;
            if ((transform.position.x == boundaries[(int)Boundary.Left] && currentHorAcceleration == -horizontalAcceleration) ||
                (transform.position.x == boundaries[(int)Boundary.Right] && currentHorAcceleration == horizontalAcceleration))
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