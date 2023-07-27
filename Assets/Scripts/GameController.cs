using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{//  ref s
    public GameObject player;
    public GameObject asteroid;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private SaveManager saveManager;
    //screen and range of the spawns 
    [SerializeField]
    private float maxRange = 10f;
    [SerializeField]
    private float minRange = 5f;
    [SerializeField]
    private float spawnInterval = 3f;
    [SerializeField]
    private Vector3 screenCenter;

    public bool isPlayerAlive;
    // privarte found variables 
    float spawnTimer = 0.0f;
    float minimumY;
    float maximumY;
    float minimumX;
    float maximumX;

    public static GameController SharedInstance;
    private void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(this);
        }
        SharedInstance = this;
    }
    void Start()
    {
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //get the min and max screen positions 
        minimumY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        maximumY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -Camera.main.transform.position.z)).y;
        minimumX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        maximumX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;
        //object pool stuff 
       ObjectPool.SharedInstance.CreatePooledObjects(28);
        // ui stuff 
        uiManager.PlayIntroTransition();
        // save manage stuff 
        saveManager.Setup();

    }
    /// <summary>
    /// getting new player position 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 GetNewPosition(Vector3 position)
    {
        return new Vector3(screenCenter.x - position.x, screenCenter.y - position.y, 0);
    }

    /// <summary>
    /// finding if our player is outside the camera furs
    /// </summary>
    /// <returns></returns>
    bool PlayerInArea()
    {
        Collider collider = player.GetComponent<Collider>();

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            return true;
        else
            return false;

    }

    void InstantiateRandomAsteroid()
    {
        float spawnX = 0;
        float spawnY = 0;

        bool targetPending = true;
        while (targetPending)
        {// looping until we can find the right spot 

            if (UnityEngine.Random.value > 0.5f)
            {
                Range[] rangesX = new Range[] { new Range(minimumX - maxRange, minimumX - minRange), new Range(maximumX + minRange, maximumX + maxRange) };
                spawnX = RandomValueFromRanges(rangesX);
                spawnY = UnityEngine.Random.Range(minimumY - maxRange, maximumY + maxRange);
            }
            else
            {
                Range[] rangesY = new Range[] { new Range(minimumY - maxRange, minimumY - minRange), new Range(maximumY + minRange, maximumY + maxRange) };
                spawnX = UnityEngine.Random.Range(minimumX - maxRange, maximumX + maxRange);
                spawnY = RandomValueFromRanges(rangesY);
            }

            // Avoiding spawning 2 asteroids ont op of each other
            Collider[] colliders = Physics.OverlapBox(new Vector3(spawnX, spawnY, 0), new Vector3(1, 1, 1));

            targetPending = colliders.Length > 0;

        }
        //creating asteroid 
        GameObject asteroidObject = ObjectPool.SharedInstance.TakeFromPool();
        //ObjectPool.SharedInstance.TakeFromPool();//Instantiate(asteroid, new Vector3(spawnX, spawnY, 0), Quaternion.Euler(0, 0, 0));
        asteroidObject.GetComponent<Transform>().position = new Vector3(spawnX, spawnY, 0);
        asteroidObject.GetComponent<Asteroid>().ActivateAsteroid(player.GetComponent<PlayerData>());
        asteroidObject.transform.LookAt(screenCenter);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PlayerInArea())
        {
            player.transform.position = GetNewPosition(player.transform.position);

        }
        SpawnAsteroid();
    }

    /// <summary>
    /// looking at the spawner timer, 
    /// if it is less then our time then we spawn 
    /// </summary>
    void SpawnAsteroid()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {// reset and spawn 
            spawnTimer = spawnTimer - spawnInterval;

            InstantiateRandomAsteroid();
        }
    }

    public static float RandomValueFromRanges(Range[] ranges)
    {
        if (ranges.Length == 0)
            return 0;
        float count = 0;
        foreach (Range r in ranges)
            count += r.range;
        float sel = UnityEngine.Random.Range(0, count);
        foreach (Range r in ranges)
        {
            if (sel < r.range)
            {
                return r.min + sel;
            }
            sel -= r.range;
        }
        throw new Exception("This should never happen");
    }
}
public struct Range
{
    public float min;
    public float max;
    public float range { get { return max - min + 1; } }
    public Range(float aMin, float aMax)
    {
        min = aMin; max = aMax;
    }
}
