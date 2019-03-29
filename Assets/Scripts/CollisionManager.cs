using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    static CollisionManager instance;

    [SerializeField] LayerMask[] collisionLayers;

    Dictionary<LayerMask, List<BoundingBox>> collisionBoxesDict = new Dictionary<LayerMask, List<BoundingBox>>();

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        foreach (LayerMask layerA in collisionBoxesDict.Keys)
        {
            foreach (LayerMask layerB in collisionBoxesDict.Keys)
            {
                if (layerA != layerB)
                {
                    foreach (BoundingBox boxA in collisionBoxesDict[layerA])
                    {
                        Vector2 posA = Camera.main.WorldToScreenPoint(boxA.transform.position);

                        foreach (BoundingBox boxB in collisionBoxesDict[layerB])
                        {
                            Vector2 posB = Camera.main.WorldToScreenPoint(boxB.transform.position);
                            Vector2 diff = posB - posA;

                            float minDistX = boxA.WidthOffset + boxB.WidthOffset;
                            float minDistY = boxA.HeightOffset + boxB.HeightOffset;

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
        if (!collisionBoxesDict.ContainsKey(layer))
        {
            List<BoundingBox> boxList = new List<BoundingBox>();
            collisionBoxesDict.Add(layer, boxList);
        }

        collisionBoxesDict[layer].Add(box);
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