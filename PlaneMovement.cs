using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneMovement : MonoBehaviour
{
    const float LIFT_CONSTANT = 0.0786942f;
    const float DRAG_CONSTANT = 0.0786942f;

    [SerializeField] private float maxSpeed;

    [SerializeField] private GameObject engineLeft;
    [SerializeField] private GameObject engineRight;
    [SerializeField] private GameObject wingLeft;
    [SerializeField] private GameObject wingRight;
    [SerializeField] private GameObject hzStabilizerLeft;
    [SerializeField] private GameObject hzStabilizerRight;
    //[SerializeField] private WheelCollider frontWheel;
    //[SerializeField] private WheelCollider blWheel;
    //[SerializeField] private WheelCollider brWheel;

    [Range(0f, 50000f)] [SerializeField] private float engineThrust;
    [Range(-1f, 1f)] [SerializeField] private float leftWingActivation;
    [Range(-1f, 1f)] [SerializeField] private float rightWingActivation;
    [Range(-1f, 1f)] [SerializeField] private float hzStabilizerActivation;
    [SerializeField] private float wingSurfaceArea;
    [SerializeField] private float hzStablzrSurfaceArea;
    [SerializeField] private float planeBodyAirresistanceArea;
    [SerializeField] private float desiredPitchHigh;
    [SerializeField] private float desiredPitchLow;

    [SerializeField] private Text altitudeReading;
    [SerializeField] private Text speedReading;

    private Rigidbody planeBodyRb;
    private Rigidbody engineLeftRb;
    private Rigidbody engineRightRb;
    private Rigidbody wingLeftRb;
    private Rigidbody wingRightRb;
    private Rigidbody hzStabilizerLeftRb;
    private Rigidbody hzStabilizerRightRb;

    private float currentEngineThrust = 0.0f;


    public bool autoPilot = false;

    void Start()
    {
        planeBodyRb = this.gameObject.GetComponent<Rigidbody>();
        engineLeftRb = engineLeft.GetComponent<Rigidbody>();
        engineRightRb = engineRight.GetComponent<Rigidbody>();
        wingLeftRb = wingLeft.GetComponent<Rigidbody>();
        wingRightRb = wingRight.GetComponent<Rigidbody>();
        hzStabilizerLeftRb = hzStabilizerLeft.GetComponent<Rigidbody>();
        hzStabilizerRightRb = hzStabilizerRight.GetComponent<Rigidbody>();

        //frontWheel.motorTorque = 0.0001f;
        //blWheel.motorTorque = 0.0001f;
        //brWheel.motorTorque = 0.0001f;
        //
        //frontWheel.brakeTorque = 10f;
        //blWheel.brakeTorque =    10f;
        //brWheel.brakeTorque =    10f;
    }

    void Update()
    {
        GetInputs();
        float speed = Vector3.Dot(planeBodyRb.velocity, planeBodyRb.transform.TransformDirection(Vector3.forward));
        if(autoPilot)
        {
            float currentX = this.transform.eulerAngles.x;
            if(currentX < desiredPitchHigh && currentX > 270)
            {
                hzStabilizerActivation = 0.35f;// * Mathf.Abs(this.transform.eulerAngles.x - desiredPitch);
                Debug.Log("pushing down");
            }
            else if(currentX > desiredPitchLow)
            {
                hzStabilizerActivation = 0.28f;// * Mathf.Abs(this.transform.eulerAngles.x - desiredPitch);
                Debug.Log("pulling up");
            }
            else
            {
                hzStabilizerActivation = 0.32f;
            }
            hzStabilizerActivation = Mathf.Clamp(hzStabilizerActivation, -1f, 1f);
        }

        engineLeftRb.GetComponent<ConstantForce>().relativeForce =  engineLeftRb.transform.TransformDirection(Vector3.forward) * engineThrust;
        engineRightRb.GetComponent<ConstantForce>().relativeForce =  engineLeftRb.transform.TransformDirection(Vector3.forward) * engineThrust;
        //engineLeftRb.AddForce(engineLeft.transform.TransformDirection(Vector3.forward) * engineThrust);
        //engineRightRb.AddForce(engineLeft.transform.TransformDirection(Vector3.forward) * engineThrust);
        Debug.DrawRay(engineLeft.transform.position, engineLeft.transform.TransformDirection(Vector3.forward), Color.red);
        
        planeBodyRb.AddForce(planeBodyRb.transform.TransformDirection(Vector3.back) * CalculateDrag(speed, planeBodyAirresistanceArea));
        
        wingLeftRb.AddForce((wingLeft.transform.TransformDirection(Vector3.up) * CalculateLift(speed, wingSurfaceArea) * leftWingActivation));
        wingRightRb.AddForce((wingRight.transform.TransformDirection(Vector3.up) * CalculateLift(speed, wingSurfaceArea) * rightWingActivation));
        Debug.DrawRay(wingLeft.transform.position, wingLeft.transform.TransformDirection(Vector3.up), Color.red);
        
        hzStabilizerLeftRb.AddForce((hzStabilizerLeft.transform.TransformDirection(Vector3.up) * CalculateLift(speed, hzStablzrSurfaceArea) * hzStabilizerActivation));
        hzStabilizerRightRb.AddForce((hzStabilizerRight.transform.TransformDirection(Vector3.up) * CalculateLift(speed, hzStablzrSurfaceArea) * hzStabilizerActivation));
        Debug.DrawRay(hzStabilizerLeft.transform.position, hzStabilizerLeft.transform.TransformDirection(Vector3.up), Color.red);
        
        altitudeReading.text = this.transform.position.y.ToString();
        speedReading.text = speed.ToString();
    }

    private void GetInputs()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //frontWheel.brakeTorque = 0f;
            //blWheel.brakeTorque = 0f;
            //brWheel.brakeTorque = 0f;
            engineThrust += 4f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            engineThrust -= 4f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            hzStabilizerActivation += 0.01f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            hzStabilizerActivation -= 0.01f;
        }
    }


    private float CalculateLift(float velocity, float wingSurfaceArea)
    {
        float lift = Mathf.Pow(velocity, 2) * (wingSurfaceArea / 2) * LIFT_CONSTANT;
        ///Debug.Log(lift);
        return lift;
    }

    private float CalculateDrag(float velocity, float surfaceArea)
    {
        float drag = Mathf.Pow(velocity, 2) * (surfaceArea / 2) * DRAG_CONSTANT;
        return drag;
    }

}
