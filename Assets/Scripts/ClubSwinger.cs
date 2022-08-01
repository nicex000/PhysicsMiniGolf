using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum ESwingStage
{
    PreSwing,
    BackSwing,
    ReturnSwing,
    HitSwing,
    PostSwing,
    End
}

public class ClubSwinger : MonoBehaviour
{

    [SerializeField] private float MaxPower;
    [SerializeField] private float MinPower;
    [SerializeField] private float PowerChangePerSecond;
    [SerializeField] public float MaxAngle;
    [SerializeField] private float AngleChangePerSecond;
    [SerializeField] private float BackswingAngle;
    [SerializeField] private float EndswingAngle;
    [SerializeField] private Transform AnglePivot;


    private float Power;
    private float Angle;
    private float InitialAngle;
    private Quaternion InitialSwingRotation;
    
    private float velocity;
    private ESwingStage SwingStage;

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.SetParent(null, true);
        GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
        InitialAngle = AnglePivot.rotation.eulerAngles.y;
        Power = MinPower;
        SwingStage = ESwingStage.PreSwing;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (SwingStage)
        {
            case ESwingStage.PreSwing:
                break;
            case ESwingStage.BackSwing:
                GetComponentInParent<SwingSetupScript>().Detach();
                GetComponent<Rigidbody>().AddTorque(AnglePivot.forward * velocity, ForceMode.VelocityChange);
                if (Quaternion.Angle(InitialSwingRotation, this.transform.rotation) > BackswingAngle)
                {
                    SwingStage++;
                }
                break;
            case ESwingStage.ReturnSwing:
                GetComponent<Rigidbody>().AddTorque(-AnglePivot.forward * velocity, ForceMode.VelocityChange);
                if (Quaternion.Angle(InitialSwingRotation, this.transform.rotation) < EndswingAngle)
                {
                    SwingStage++;
                }
                break;
            case ESwingStage.HitSwing:
                GetComponent<Rigidbody>().AddTorque(-AnglePivot.forward * velocity, ForceMode.VelocityChange);
                if (Quaternion.Angle(InitialSwingRotation, this.transform.rotation) > EndswingAngle)
                {
                    SwingStage++;
                }
                break;
            case ESwingStage.PostSwing:
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponentInParent<SwingSetupScript>().DetachBallAndDestroy();
                SwingStage++;
                break;
            default:
                return;
        }
    }

    void Update()
    {
        float powerButton = Input.GetAxis("Power");
        float angleButton = Input.GetAxis("Angle");
        bool swingRequested = Input.GetButton("Swing");


        if (powerButton != 0)
        {
            Power += PowerChangePerSecond * Time.deltaTime * powerButton;
            Power = Mathf.Clamp(Power, MinPower, MaxPower);
        }

        if (angleButton != 0)
        {
            Angle += AngleChangePerSecond * Time.deltaTime * angleButton;
            Angle = Mathf.Clamp(Angle, -MaxAngle, MaxAngle);
            AnglePivot.rotation = Quaternion.Euler(
                AnglePivot.eulerAngles.x, 
                InitialAngle + Angle, 
                AnglePivot.eulerAngles.z);
        }

        if (swingRequested && SwingStage == ESwingStage.PreSwing)
        {
            SwingStage++;
            velocity = Power;
            InitialSwingRotation = this.transform.rotation;
        }

        //Debug.Log($"Power: {Power} - Angle: {Angle} - Swing: {swingRequested}");

    }

    void OnCollisionEnter(Collision collision)
    {
        if (SwingStage > ESwingStage.BackSwing && collision.gameObject.GetComponent<BallScript>())
        {
            Vector3 dir = -AnglePivot.right;
            dir.y = 0;
            collision.rigidbody.velocity = Vector3.zero;
            collision.gameObject.GetComponent<BallScript>().HitBall( dir * velocity * 5f);
            this.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("ClubNoCollision");
        }
        
    }

    public float GetPowerPercent()
    {
            return (Power - MinPower) / (MaxPower - MinPower);
    }
}
