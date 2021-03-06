using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraLook : MonoBehaviour
{
    [SerializeField]
    public float sensitivity = 5.0f;
    [SerializeField]
    public float smoothing = 2.0f;
    [SerializeField]
    public float tiltStrengh = 3f;
    public GameObject character;
    private Vector2 mouseLook;
    private Vector2 smoothV;

    void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");
        var horizontal = Input.GetAxis("Horizontal");

        var md = new Vector2(mouseX, mouseY);

        float rotZ = 0;

        // This makes it so we can at least stop mouse rotation

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        if (horizontal != 0)
            rotZ = horizontal * tiltStrengh;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);

        // Rotate the camera with the tilting
        transform.localRotation = Quaternion.Euler(-mouseLook.y, transform.localRotation.y, transform.localRotation.z + -rotZ);

        // Rotate the character with the camera
        var xRot = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
        character.transform.localRotation = xRot;
    }
}
