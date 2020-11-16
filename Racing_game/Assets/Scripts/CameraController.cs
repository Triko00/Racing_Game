using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CarController target;
    private Vector3 offsetDir;

    public float minDistance, maxDistance;
    private float acticeDistance;

    // Start is called before the first frame update
    void Start()
    {
        offsetDir = transform.position - target.transform.position;

        acticeDistance = minDistance;

        offsetDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        acticeDistance = minDistance + ((maxDistance - minDistance) * (target.theRB.velocity.magnitude / target.maxSpeed));

        transform.position = target.transform.position + (offsetDir * acticeDistance);
    }
}
