using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class BombManager : MonoBehaviour
{
    static BombManager instance;

    [SerializeField] [Range(0f, 10f)] float gravity = 9.81f;
    [SerializeField] [Range(5f, 10f)] float minBombStartSpeed = 5f;
    [SerializeField] [Range(10f, 15f)] float maxBombStartSpeed = 15f;

    ObjectPool bombPool;
    BombMovement[] bombMovements;

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        bombPool = GetComponent<ObjectPool>();

        bombMovements = new BombMovement[bombPool.Amount];

        for (int i = 0; i < bombPool.Amount; i++)
        {
            GameObject bombObject = bombPool.GetObjectFromPool(i);

            BoundingBox box = bombObject.GetComponent<BoundingBox>();

            if (box)
            {
                LayerMask layer = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
                CollisionManager.Instance.RegisterBoundingBox(layer, box);
            }
            
            bombMovements[i] = bombObject.GetComponent<BombMovement>();
        }
    }

    void Update()
    {
        foreach (BombMovement bombMovement in bombMovements)
            bombMovement.Bounce();
    }

    public static BombManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<BombManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Bomb Manager");
                    gameObj.AddComponent<ObjectPool>();
                    instance = gameObj.AddComponent<BombManager>();
                }
            }

            return instance;
        }
    }

    public float Gravity
    {
        get { return gravity; }
    }

    public float GetRandomStartSpeed()
    {
        return Random.Range(minBombStartSpeed, maxBombStartSpeed);
    }
}
