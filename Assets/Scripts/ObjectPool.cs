using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    [SerializeField]
    private GameObject spawningAsteroids;
    //[SerializeField]
    //private ObjectPoolTestUI inputUI;
    private int currentInt = 1;
    [SerializeField]
    public Queue<GameObject> objectQueue = new Queue<GameObject>();
    public List<GameObject> objectPool = new List<GameObject>();

    private void Awake()
    {
        SharedInstance = this;
    }

    public GameObject TakeFromPool()
    {
        if (1 > objectQueue.Count)
        {// do not have enough objects in the pool 
            CreatePooledObjects(1 - objectQueue.Count);
        }
        return ReturnPooledObjects(1)[0];
    }

    public List<GameObject> TakeMultipleFromPool(int amount)
    {
        List<GameObject> returningObjects = new List<GameObject>();
        if (amount > objectQueue.Count)
        {// do not have enough objects in the pool 
            CreatePooledObjects(amount - objectQueue.Count);
        }
        returningObjects = ReturnPooledObjects(amount);

        return returningObjects;
    }

    public void ReturnToPool(GameObject inputObject)
    {
        inputObject.SetActive(false);
        //inputObject.transform.parent = transform;
        inputObject.transform.localPosition = Vector3.zero;
        objectQueue.Enqueue(inputObject);
    }
    public void ReturnToPoolList(List<GameObject> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            inputList[i].SetActive(false);
            inputList[i].transform.parent = transform;
            inputList[i].transform.localPosition = Vector3.zero;
            objectQueue.Enqueue(inputList[i]);
        }
    }

    private List<GameObject> ReturnPooledObjects(int amount)
    {
        List<GameObject> returningObjects = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject returningObject = objectQueue.Dequeue();
            returningObject.SetActive(true);
            returningObjects.Add(returningObject);
        }
        return returningObjects;
    }

    public void CreatePooledObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject creatingObject = Instantiate(spawningAsteroids, transform);
            creatingObject.name = "Asteroid" + currentInt.ToString();
            currentInt++;
            creatingObject.SetActive(false);
            creatingObject.transform.position = Vector3.zero;
            objectQueue.Enqueue(creatingObject);
        }
    }
}
