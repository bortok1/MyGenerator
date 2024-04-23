using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CameraMovement : MonoBehaviour
{
    Vector3 movementOffset = new Vector3(0, 0, 0);

    float scrollValue = 0;
    public float scrollSpeed = 2;

    Camera cameraComponent = null;

    void Start()
    {
        cameraComponent = GetComponentInParent<Camera>();
    }

    void Update()
    {
        movementOffset.x = Input.GetAxis("Horizontal");
        movementOffset.y = Input.GetAxis("Vertical");

        if (movementOffset != Vector3.zero)
            this.transform.position += movementOffset;

        scrollValue = -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (scrollValue != 0)
            cameraComponent.orthographicSize += scrollValue;
    }
}
