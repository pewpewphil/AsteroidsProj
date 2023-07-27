using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : SaveStateBase
{
    public int playerLives = 3;
    public int currentScore = 0;
    // datafor the player to controll
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameObject playerShip;
    [SerializeField]
    private ParticleSystem explosionParticles;
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private bool respawning=false;

    // newly added
    public delegate void OnDestoryAsteroid(int score);
    public static OnDestoryAsteroid onDestoryAsteroid;

    public delegate void OnCollisionAsteroid();
    public static OnCollisionAsteroid onAsteroidCollide;

    //save and laod 
    [System.Serializable]
    public struct PlayerValues
    {
        public bool respawning;
        public int lives;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public PlayerValues saveTransformState = new PlayerValues();

    // Start is called before the first frame update
    void Start()
    {
        onAsteroidCollide -= CollidedWithAsteroid;// newly added
        onAsteroidCollide += CollidedWithAsteroid;// newly added
        respawning = false;
        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region collisions 
    public void CollidedWithAsteroid()
    {
        if (respawning)//ignore if we are respawning 
            return;

        playerLives--;

        uiManager.UpdateCurrentLives(playerLives);
        if (playerLives == 0)
        {
            uiManager.PlayGameOverTransition();
            DestoryPlayer();
        }
        else
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    public void DestoryPlayer()
    {
        playerController.PausePlayer();
        playerShip.SetActive(false);
        respawning = true;
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();
    }

    IEnumerator RespawnCoroutine()
    {
        DestoryPlayer();
        uiManager.PlayRespawnTransition();
        //wait time 
        yield return new WaitForSeconds(3);

        PlayerReset();
    }

    public void PlayerReset()
    {
        uiManager.ResetRespawnTransition();
        explosionParticles.gameObject.SetActive(false);
        respawning = false;
        playerShip.SetActive(true);
        playerController.ResetPlayer();
    }

    public void DestoryedAsteroid(int scoreValue)
    {
        currentScore+= scoreValue;
    }
    #endregion

    #region save and load  

    public override string SaveState()
    {
        Transform currentTransform = transform;
        saveTransformState.position = currentTransform.position;
        saveTransformState.rotation = currentTransform.rotation.eulerAngles;
        saveTransformState.scale = currentTransform.localScale;
        saveTransformState.lives = playerLives;
        saveTransformState.respawning = respawning;

        return JsonUtility.ToJson(saveTransformState);
    }

    public override void LoadState(string loadedJson)
    {
        saveTransformState = JsonUtility.FromJson<PlayerValues>(loadedJson);

        transform.position = saveTransformState.position;
        transform.eulerAngles = saveTransformState.rotation;
        transform.localScale = saveTransformState.scale;
        // updating the player lives 
        playerLives = saveTransformState.lives;
        uiManager.UpdateCurrentLives(playerLives);
        //dealing with respawning
        bool currentRespawning = respawning;
        respawning =saveTransformState.respawning;
        if (currentRespawning!=respawning)
        {
            uiManager.ResetRespawnTransition();
            explosionParticles.gameObject.SetActive(false);
            respawning = false;
            playerShip.SetActive(true);
        }
    }

    public override bool ShouldSave()
    {
        if (saveTransformState.position == transform.position && saveTransformState.rotation == transform.eulerAngles &&
            saveTransformState.scale == transform.localScale) return false;

        return true;
    }
    #endregion
}
