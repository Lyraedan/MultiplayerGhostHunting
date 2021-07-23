using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public enum GhostCharacteristics {
        FREEZING_TEMPERATURES, EMF_LEVEL_V, FINGERPRINTS, GHOST_ORBS, GHOST_WRITING, SPIRIT_BOX
    }

    public GhostCharacteristics characteristicA, characteristicB, characteristicC;
    public int huntThreshold = 50;
    public AudioClip manifestSfx;
    public float visiblityTimer = 1f;
}
