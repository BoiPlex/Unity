using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFollow : MonoBehaviour {

    public Transform ShipTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtShip = false;

    public bool RotateAroundShip = true;

    public float RotationsSpeed = 5.0f;

	// Use this for initialization
	void Start () {
        _cameraOffset = transform.position - ShipTransform.position;	
	}
	
	// LateUpdate is called after Update methods
	void LateUpdate () {

        if(RotateAroundShip)
        {
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset;
        }

        Vector3 newPos = ShipTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtShip || RotateAroundShip)
            transform.LookAt(ShipTransform);
	}
}
