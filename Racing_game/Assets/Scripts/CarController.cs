using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;

    public float maxSpeed;

    public float forwardAccel = 8f, reverseAccel = 4f;

    private float speedInput;

    public float turnStrength = 180f;
    private float turnInput;

    private bool grounded;

    public Transform groundRayPoint;
    public LayerMask whatIsGround;
    public float groundRayLength = .75f;

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

        turnInput = Input.GetAxis("Horizontal");

        if (grounded && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.velocity.magnitude / maxSpeed), 0f));
        }









        transform.position = theRB.position;
    }

    void FixedUpdate()
    {
        grounded = false;

        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
        }

        //accelerate the car
        if (grounded)
        { 
        theRB.AddForce(transform.forward * speedInput * 1000f);
        }
        if (theRB.velocity.magnitude > maxSpeed)
        {
            theRB.velocity = theRB.velocity.normalized * maxSpeed;
        }

        Debug.Log(theRB.velocity.magnitude);
    }
}
