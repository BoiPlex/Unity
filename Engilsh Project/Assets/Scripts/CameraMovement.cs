using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float distanceToTarget = 10;
    public float RotationsSpeed = 5.0f;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    private Vector3 previousPosition;
    private Vector3 _cameraOffset;

    void Start()
    {
        _cameraOffset = transform.position - target.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically
            
            cam.transform.position = target.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }
    }
}