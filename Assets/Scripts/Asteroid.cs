using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : SaveStateBase
{
    public AsteroidData asteroidData;//newly added
    public int scoreValue=10;
    PlayerData playerData;
    //saving variables 
    [System.Serializable]
    public struct AsteroidValues
    {
        public bool isActive;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
    public AsteroidValues saveState = new AsteroidValues();

    public void Start()
    {
        // can cache this using serializable and grabbing it before runtime
        GetComponent<Rigidbody>().AddForce(transform.forward * 300f);// this is apparently need for the asteroid to move 
    }

    public void ActivateAsteroid(PlayerData _playerData)
    {
        playerData = _playerData;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        //GetComponent<Rigidbody>().velocity= (transform.forward * 300f);
        GetComponent<Rigidbody>().AddForce(transform.forward * 100f);
    }
    
    //if the asteroid exits the field 
    private void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy)
        {
            //Debug.Log("on become inviislbe");
            DeactivateAsteroid();
        }
            

    }

    public void DeactivateAsteroid()
    {
        ObjectPool.SharedInstance.ReturnToPool(this.gameObject);
    }

    #region collisions 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            CollisionWithPlayer();
        }
        else if (other.gameObject.tag == "Laser")
        {
            CollisionWithLaser(other.gameObject);
        }
    }

    private void CollisionWithPlayer()
    {
        DeactivateAsteroid();
        //Debug.Log("Player");
        playerData.CollidedWithAsteroid();
        //newly added
        //PlayerData.onAsteroidCollide.Invoke();
    }

    private void CollisionWithLaser(GameObject laserObject)
    {
        //Debug.Log("Laser");
        laserObject.GetComponent<Laser>().DestoryObject();
        DeactivateAsteroid();
        playerData.DestoryedAsteroid(scoreValue);
    }
    #endregion
    #region SaveLoad
    public override string SaveState()
    {
        Transform currentTransform = transform;
        saveState.position = currentTransform.position;
        saveState.rotation = currentTransform.rotation.eulerAngles;
        saveState.scale = currentTransform.localScale;
        saveState.isActive = gameObject.activeInHierarchy;

        return JsonUtility.ToJson(saveState);
    }

    public override void LoadState(string loadedJson)
    {
        saveState = JsonUtility.FromJson<AsteroidValues>(loadedJson);

        transform.position = saveState.position;
        transform.eulerAngles = saveState.rotation;
        transform.localScale = saveState.scale;

        //dealing with respawning since it will only load if it was active 
        if (saveState.isActive)
        {
            gameObject.SetActive(true);
            ActivateAsteroid(GameController.SharedInstance.player.GetComponent<PlayerData>());
        }
            
    }

    public override bool ShouldSave()
    {
        if (!gameObject.activeInHierarchy)// don't save if we have something not active 
            return false;

        return true;
    }
    #endregion
}
