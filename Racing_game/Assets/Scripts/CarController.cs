using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;

    public float maxSpeed;

    public float forwardAccel = 8f, reverseAccel = 4f;

    private float speedInput;

    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0 )
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }
        else if((Input.GetAxis("Vertical") < 0))
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel;
        }

        transform.position = theRB.position;
    }

    void FixedUpdate()
    {
        theRB.AddForce(new Vector3(0f, 0f, speedInput * 1000f));
    }
}
