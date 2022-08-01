using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallInHoleScript : MonoBehaviour
{
    [SerializeField] private int Hole;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.attachedRigidbody)
            GameManager.GetInstance().SetBallInHole(collider.attachedRigidbody, Hole);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.attachedRigidbody)
            GameManager.GetInstance().SetBallInHole(null, Hole);
    }
}
