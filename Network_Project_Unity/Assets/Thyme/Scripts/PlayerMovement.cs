using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;

public class PlayerMovement : PunBehaviour, IPunObservable
{
    public float moveSpeed = 5.0f;
    public float gravity = 10.0f;
    public float cameraDistance = 10.0f;
    public float cameraHeight = 5.0f;
    public float cameraAngleLook = 30.0f;
    public float cameraSensitivity = 10.0f;

    private Vector3 velocity;
    private Vector3 dir;
    private CharacterController characterController;

    private Vector3 correctPosition;
    private Quaternion correctRotation;

    private Camera camera;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            Movement();
            UpdateCamera();
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, correctPosition, 5.0f * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, correctRotation, 5.0f * Time.deltaTime);
        }
    }

    void Movement()
    {
        dir = (this.transform.forward * Input.GetAxis("Vertical"))+
            (this.transform.right * Input.GetAxis("Horizontal"));

        dir.Normalize();

        if (characterController.isGrounded)
        {
            velocity = Vector3.zero;

            velocity += dir * moveSpeed;
        }

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    void UpdateCamera()
    {
        if(camera == null)
        {
            camera = Camera.main;
        }
        else
        {
            if(camera.transform.parent != this.transform)
            {
                camera.transform.parent = this.transform;
                camera.transform.localPosition = (-camera.transform.forward * cameraDistance) +
                    (camera.transform.up * cameraHeight);
            }
            else
            {
                //camera.transform.localEulerAngles = new Vector3(cameraAngleLook, 0.0f, 0.0f);
            }
        }

        this.transform.RotateAround(this.transform.position, Vector3.up, Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime);
    }

   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            correctPosition = (Vector3)stream.ReceiveNext();
            correctRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
