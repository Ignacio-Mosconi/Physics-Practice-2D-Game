using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class AsteroidManager : MonoBehaviour
{
    static AsteroidManager instance;

    [SerializeField] [Range(1f, 5f)] float minAsteroidSpeed = 1f;
    [SerializeField] [Range(5f, 10f)] float maxAsteroidSpeed = 10f;
    
    ObjectPool asteroidPool;
    AsteroidMovement[] asteroidMovements;

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        asteroidPool = GetComponent<ObjectPool>();
        asteroidMovements = new AsteroidMovement[asteroidPool.Amount];

        for (int i = 0; i < asteroidPool.Amount; i++)
        {
            GameObject asteroidObject = asteroidPool.GetObjectFromPool(i);
            
            BoundingBox box = asteroidObject.GetComponent<BoundingBox>();

            if (box)
            {
                LayerMask layer = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
                CollisionManager.Instance.RegisterBoundingBox(layer, box);
            }
            
            asteroidMovements[i] = asteroidObject.GetComponent<AsteroidMovement>();
            asteroidMovements[i].Respawn();
        }
    }

    void Update()
    {
        foreach (AsteroidMovement asteroidMovement in asteroidMovements)
            asteroidMovement.Move();
    }

    public static AsteroidManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<AsteroidManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Asteroid Manager");
                    gameObj.AddComponent<ObjectPool>();
                    instance = gameObj.AddComponent<AsteroidManager>();
                }
            }

            return instance;
        }
    }

    public float GetRandomAsteroidSpeed()
    {
        return Random.Range(minAsteroidSpeed, maxAsteroidSpeed);
    }
}