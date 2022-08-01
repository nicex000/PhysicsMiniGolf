using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallChaseScript : MonoBehaviour
{
    [SerializeField] private float MinDistance;
    [SerializeField] private float MaxDistance;
    [SerializeField] private float Height;
    [SerializeField] private float OffCenterAngle;
    [SerializeField] private int DelayFrames = 5; //it really shouldn't be in frames

    private BallScript BallRef;

    private Queue<Vector3> VelocityBuffer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject ball = FindAndGetBall();
        if (!ball) return;

        //Vector3 velocity = VelocityBuffer.Dequeue();
        Vector3 velocity = AverageVel();
        VelocityBuffer.Dequeue();
        VelocityBuffer.Enqueue(ball.GetComponent<Rigidbody>().velocity);
        if (velocity.magnitude <= BallRef.GetMinMagnitude() && ball.transform.parent)
        {
            Vector3 Offset = (ball.transform.parent.right) * MinDistance + ball.transform.position;
            transform.position = Offset + Vector3.up * Height;

        }
        else
        {
            transform.position += velocity * Time.fixedDeltaTime;
            
            Vector3 Distance = ball.transform.position - transform.position;
            if (Distance.magnitude < MinDistance)
            {
                Vector3 Offset = velocity.normalized * MinDistance + ball.transform.position;
                transform.position = Offset + Vector3.up * Height;
            }
            else if (Distance.magnitude > MaxDistance)
            {
                Vector3 Offset = velocity.normalized * MaxDistance + ball.transform.position;
                transform.position = Offset + Vector3.up * Height;
            }
        }
        transform.LookAt(ball.transform);
        transform.Rotate(Vector3.right, OffCenterAngle);

    }

    GameObject FindAndGetBall()
    {
        if (!BallRef)
        {
            BallRef = FindObjectOfType<BallScript>();
            VelocityBuffer = new Queue<Vector3>();
            for (int i = 0; i < DelayFrames; ++i)
            {
                VelocityBuffer.Enqueue(Vector3.zero);
            }
        }

        return BallRef.gameObject;
    }

    Vector3 AverageVel()
    {
        Vector3 AverageVel = Vector3.zero;
        foreach (var vel in VelocityBuffer)
        {
            AverageVel += vel;
        }
        return AverageVel / VelocityBuffer.Count;
    }

    void WipeBallRef()
    {
        BallRef = null;
    }
}
