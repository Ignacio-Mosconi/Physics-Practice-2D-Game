using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    static CollisionManager instance;

    [SerializeField] LayerMask[] collisionLayers;

    Dictionary<LayerMask, List<BoundingBox>> collisionBoxes = new Dictionary<LayerMask, List<BoundingBox>>();

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        foreach (LayerMask layerA in collisionBoxes.Keys)
        {
            foreach (LayerMask layerB in collisionBoxes.Keys)
            {
                if (layerA != layerB)
                {
                    foreach (BoundingBox boxA in collisionBoxes[layerA])
                    {
                        Vector2 posA = boxA.transform.position;

                        foreach (BoundingBox boxB in collisionBoxes[layerB])
                        {
                            Vector2 posB = boxB.transform.position;
                            Vector2 diff = posB - posA;

                            float minDistX = (boxA.Width + boxB.Width) * 0.5f;
                            float minDistY = (boxA.Height + boxB.Height) * 0.5f;

                            float deltaX = Mathf.Abs(diff.x);
                            float deltaY = Mathf.Abs(diff.y);

                            if (deltaX < minDistX && deltaY < minDistY)
                            {
                                boxA.OnCollision.Invoke();
                                boxB.OnCollision.Invoke();
                            }
                        }
                    }
                }
            }
        }
    }

    public void RegisterBoundingBox(LayerMask layer, BoundingBox box)
    {
        if (!collisionBoxes.ContainsKey(layer))
        {
            List<BoundingBox> boxList = new List<BoundingBox>();
            collisionBoxes.Add(layer, boxList);
        }

        collisionBoxes[layer].Add(box);
    }

    public static CollisionManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<CollisionManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("CollisionManager");
                    instance = gameObj.AddComponent<CollisionManager>();
                }
            }

            return instance;
        }
    }
}