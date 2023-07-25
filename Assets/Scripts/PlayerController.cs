using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeed;
    Rigidbody playerRigidBody;
    Transform playersTransform;

    // shooting variables 
    [SerializeField]
    private  GameObject laser;
    [SerializeField]
    private float cooldown = 0.5f;
    private float time = 0f;
    [SerializeField]
    private bool paused = false;


    private void Start()
    {//does the get at beginning so we don't constantly do it during the update
        playerRigidBody = GetComponent<Rigidbody>();
        playersTransform = transform;
        paused = false;
    }

    public void PausePlayer()
    {
        playerRigidBody.velocity = new Vector3(0, 0, 0);
        paused = true;
    }
    public void ResetPlayer()
    {
        paused = false;
        time = 0f;
        playerRigidBody.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ShootingUpdate();
        MovementUpdate();
    }

    public void ShootingUpdate()
    {
        if (paused)
            return;
        if (time > 0f)
        {// if our current time is less than 
            time -= Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space)|| Input.GetKey(KeyCode.P))
        {
            GameObject createdLaser= Instantiate(laser, playersTransform.TransformPoint(Vector3.forward * 1), playersTransform.rotation );
            createdLaser.transform.localRotation*= Quaternion.Euler(0, 0, 270);
            time = cooldown;
        }
    }

    public void MovementUpdate()
    {
        if (paused)
            return;
        if (Input.GetKey(KeyCode.W))
            playerRigidBody.AddForce(playersTransform.right * movementSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            playerRigidBody.AddForce(playersTransform.right * -movementSpeed * Time.deltaTime);


        if (Input.GetKey(KeyCode.A))
            playersTransform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            playersTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
