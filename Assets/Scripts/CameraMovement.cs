using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform attachedTarget;    // object that gets followed by the camera
    public float smoothing;

    void Start()
    {
        
    }

    //occurs at the end of the update
    void LateUpdate() {
        if (transform.position != attachedTarget.position) {
            Vector3 targetPosition = new Vector3(attachedTarget.position.x, attachedTarget.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
