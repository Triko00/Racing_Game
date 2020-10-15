using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;

    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        theRB.AddForce(new Vector3(0f, 0f, 100f));

        transform.position = theRB.position;
    }
}
