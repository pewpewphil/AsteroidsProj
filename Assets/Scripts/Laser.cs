using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;

    public void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
    }

    private void OnBecameInvisible()
    {
        DestoryObject();
    }

    public void DestoryObject()
    {
        Destroy(gameObject);
    }
}
