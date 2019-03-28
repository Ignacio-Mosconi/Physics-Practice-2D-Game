using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] [Range(1, 10)] int amount = 1;

    GameObject[] pool;

    void Awake()
    {
        pool = new GameObject[amount];

        for (int i = 0; i < amount; i++)
            pool[i] = Instantiate(prefab);
    }

    public GameObject GetObjectFromPool(int index)
    {
        return pool[index];
    }

    public int Amount
    {
        get { return amount; }
    }
}