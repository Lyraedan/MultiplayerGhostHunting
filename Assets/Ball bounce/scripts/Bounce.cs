using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Bounce : MonoBehaviour
{

    public List<AudioClip> bounceSfx = new List<AudioClip>();

    private AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        src.clip = bounceSfx[Random.Range(0, bounceSfx.Count)];
        src.Play();
    }
}
