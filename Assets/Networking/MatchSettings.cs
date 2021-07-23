using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MatchSettings : MonoBehaviour
{

    public List<GameObject> ghosts = new List<GameObject>();

    public enum Difficulty {
        Amature = 1,
        Intermedite = 2,
        Professional = 3
    }

    public Difficulty difficulty = Difficulty.Amature;

    private PhotonView photonView;

    public bool started = false;

    public void StartMatch()
    {
        if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.InRoom)
            return;
        Debug.Log("Do start");
        photonView = GetComponent<PhotonView>();
        photonView.RPC("RPC_Start", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_Start()
    {
        Debug.Log("RPC -> Start");
        if(PhotonNetwork.IsMasterClient && !started)
        {
            SpawnGhost();
        }
        started = true;
    }

    public void SpawnGhost()
    {
        int chosen = UnityEngine.Random.Range(0, ghosts.Count);
        Vector3 spawn = RandomNavmeshLocation(100f);
        PhotonNetwork.Instantiate(this.ghosts[chosen].name, spawn, Quaternion.identity, 0);
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
