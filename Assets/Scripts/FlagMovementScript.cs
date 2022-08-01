using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EDirection
{
    Forward,
    Backward
}


public class FlagMovementScript : MonoBehaviour
{

    [SerializeField] private AnimationCurve MovementCurve;
    [SerializeField] private float Speed;

    private float PlaybackPosition;
    private float EndTime;
    private EDirection Direction;
    private float InitialHeight;

    // Start is called before the first frame update
    void Start()
    {
        PlaybackPosition = 0f;
        Direction = EDirection.Backward;
        EndTime = MovementCurve[MovementCurve.length - 1].time;
        InitialHeight = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Direction)
        {
            case EDirection.Forward:
                if (PlaybackPosition < EndTime)
                {
                    PlaybackPosition += Speed * Time.deltaTime;
                    if (PlaybackPosition > EndTime) PlaybackPosition = EndTime;

                }
                break;
            case EDirection.Backward:
                if (PlaybackPosition > 0)
                    PlaybackPosition -= Speed * Time.deltaTime;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (PlaybackPosition < 0) PlaybackPosition = 0;

        this.transform.position = new Vector3(
            this.transform.position.x,
            InitialHeight + MovementCurve.Evaluate(PlaybackPosition),
            this.transform.position.z);

    }

    void OnTriggerEnter(Collider collider)
    {
        Direction = EDirection.Forward;
    }

    void OnTriggerExit(Collider collider)
    {
        Direction = EDirection.Backward;
    }
}
