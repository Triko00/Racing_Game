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

    public Transform groundRayPoint, groundRayPoint2;
    public LayerMask whatIsGround;
    public float groundRayLength = .75f;

    private float dragOnGround;
    public float gravityMod = 10f;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;

    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f, emissionFadeSpeed = 20f;
    private float emissionRate;

    public AudioSource engineSound, skidSound;
    public float skidFadeSpeed;

    private int nextCheckpoint;
    public int currentLap;

    public float lapTime, bestLapTime;

    public bool isAI;

    public int currentTarget;
    private Vector3 targetPoint;
    public float aiAccelerateSpeed = 1f, aiTurnSpeed = 0.8f, aiReachPointRange = 5f, aiPointVatriance = 3f, aiMaxTurn = 15f;
    private float aiSpeedInput;

    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;

        dragOnGround = theRB.drag;

        if(isAI)
        {
            targetPoint = RaceManager.instance.allCheckpoints[currentTarget].transform.position;
            RandomiseAITarget();
        }

        UIManager.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps;
    }

    // Update is called once per frame
    void Update()
    {
        lapTime += Time.deltaTime;

        if (!isAI)
        {
            var ts = System.TimeSpan.FromSeconds(lapTime);
            UIManager.instance.currentLapTimeText.text = string.Format("{0:00}min {1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds);

            speedInput = 0f;
            if (Input.GetAxis("Vertical") > 0)
            {
                speedInput = Input.GetAxis("Vertical") * forwardAccel;
            }
            else if ((Input.GetAxis("Vertical") < 0))
            {
                speedInput = Input.GetAxis("Vertical") * reverseAccel;
            }

            turnInput = Input.GetAxis("Horizontal");

            /*if (grounded && Input.GetAxis("Vertical") != 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.velocity.magnitude / maxSpeed), 0f));
            }*/
        } else
        {
            targetPoint.y = transform.position.y;

            if(Vector3.Distance(transform.position, targetPoint) < aiReachPointRange)
            {
                currentTarget++;
                if(currentTarget >= RaceManager.instance.allCheckpoints.Length)
                {
                    currentTarget = 0;
                }

                targetPoint = RaceManager.instance.allCheckpoints[currentTarget].transform.position;
                RandomiseAITarget();
            }

            Vector3 targetDir = targetPoint - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            Vector3 localPos = transform.InverseTransformPoint(targetPoint);
            if(localPos.x < 0)
            {
                angle = -angle;
            }

            turnInput = Mathf.Clamp(angle / aiMaxTurn, -1f, 1f);

            if(Mathf.Abs(angle) < aiMaxTurn)
            {
                aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, 1f, aiAccelerateSpeed);
            } else
            {
                aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, aiTurnSpeed, aiAccelerateSpeed);
            }

            aiSpeedInput = 1f;
            speedInput = aiSpeedInput * forwardAccel;
        }

        //turning the wheels
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z);

        //transform.position = theRB.position;

        //control particle emissions
        emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed * Time.deltaTime);

        if (grounded && (Mathf.Abs(turnInput) > .5f || theRB.velocity.magnitude < maxSpeed * .5f && theRB.velocity.magnitude != 0))
        {
            emissionRate = maxEmission;
        }

        if (theRB.velocity.magnitude <= 1f)
        {
            emissionRate = 0;
        }

        for (int i = 0; i < dustTrail.Length; i++)
        {
            var emissionModule = dustTrail[i].emission;

            emissionModule.rateOverTime = emissionRate;
        }

        if(engineSound != null)
        {
            engineSound.pitch = 1f + ((theRB.velocity.magnitude / maxSpeed) * 2f);
        }

        if (skidSound != null)
        {
            if(Mathf.Abs(turnInput) > 0.5f)
            {
                skidSound.volume = 1.0f;
            }else
            {
                skidSound.volume = Mathf.MoveTowards(skidSound.volume, 0.0f, skidFadeSpeed * Time.deltaTime);
            }
        }
    }


    private void FixedUpdate()
    {
        grounded = false;

        RaycastHit hit;
        Vector3 normalTarget = Vector3.zero;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            normalTarget = hit.normal;
        }

        if (Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            normalTarget = (normalTarget + hit.normal) / 2f;
        }
        //when on ground rotate to match the normal
        if (grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
        }

        //accelerates the car
        if (grounded)
        {
            theRB.drag = dragOnGround;

            theRB.AddForce(transform.forward * speedInput * 1000f);
        }
        else
        {
            theRB.drag = 0.1f;

            theRB.AddForce(-Vector3.up * gravityMod * 100f);
        }

        if (theRB.velocity.magnitude > maxSpeed)
        {
            theRB.velocity = theRB.velocity.normalized * maxSpeed;
        }

        //Debug.Log(theRB.velocity.magnitude);

        transform.position = theRB.position;

        if (grounded && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.velocity.magnitude / maxSpeed), 0f));
        }
    }

    public void CheckpointHit(int cpNumber)
    {
        if(cpNumber == nextCheckpoint)
        {
            nextCheckpoint++;
            if (nextCheckpoint == RaceManager.instance.allCheckpoints.Length)
            {
                nextCheckpoint = 0;
                LapCompleted();
            }
        }
    }

    public void LapCompleted()
    {
        currentLap++;

        if (lapTime <= bestLapTime || bestLapTime == 0)
        {
            bestLapTime = lapTime;
        }

        lapTime = 0f;

        if (!isAI)
        {
            var ts = System.TimeSpan.FromSeconds(bestLapTime);
            UIManager.instance.bestLapTimeText.text = string.Format("{0:00}min {1:00}.{2:000}s", ts.Minutes, ts.Seconds, ts.Milliseconds);
            UIManager.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps;
        }
    }

    public void RandomiseAITarget()
    {
        targetPoint += new Vector3(Random.Range(-aiPointVatriance, aiPointVatriance), 0f, Random.Range(-aiPointVatriance, aiPointVatriance));
    }
}
