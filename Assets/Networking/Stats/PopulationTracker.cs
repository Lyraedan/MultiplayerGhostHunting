using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationTracker : MonoBehaviour
{

    public Text text;

    void Update()
    {
        text.text = "Population: " + PhotonNetwork.CountOfPlayers;
    }
}
