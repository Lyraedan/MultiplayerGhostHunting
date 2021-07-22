using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class NetworkedPlayer : MonoBehaviour
{

    public CharacterController characterController;
    public new Camera camera;
    public PhotonView photonView;
    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine)
        {
            Destroy(camera.gameObject);
            Destroy(characterController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine)
        {
            body.velocity = Vector3.zero;
            body.drag = 0;
            body.angularDrag = 0;
            body.angularVelocity = Vector3.zero;
        }
    }
}
