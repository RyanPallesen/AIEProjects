using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(NetworkIdentity))]
public class LocalManager : NetworkBehaviour
{
    //Variables
    public float speed = 6.0F;
    public float mouseSpeed = 10.0F;

    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Camera myCamera;
    public AudioListener myAudioListener;

    public new Rigidbody rigidbody;
    private void Start()
    {
        if (!base.hasAuthority)
        {
            myCamera.enabled = false;
            myAudioListener.enabled = false;

        }
        else//is local client
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rigidbody = GetComponent<Rigidbody>();
        }
    }
    
    void Update()
    {
        //if (!base.hasAuthority)
        //{
        //    myCamera.enabled = false;
        //    myAudioListener.enabled = false;
        //}

        if (base.hasAuthority)
        {


            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.forward * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * Time.deltaTime * speed;
            }

            if(Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(transform.up * jumpSpeed);
            }
            transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSpeed, 0));
        }
    }
}
