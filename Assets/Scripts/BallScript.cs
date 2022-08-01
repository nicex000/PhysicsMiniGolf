using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float StopMovingTime;
    [SerializeField] private float StopMovingMinMagnitude;

    private float StopTimer;

    private bool WasHit;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = -1;

        StopTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude <= StopMovingMinMagnitude)
        {
            StopTimer += Time.deltaTime;
            if (StopTimer > StopMovingTime)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                StopTimer = 0f;
                if (WasHit)
                {
                    GameManager.GetInstance().SpawnClub(this.transform);
                    WasHit = false;
                }
            }
        }
        else if (StopTimer > 0f)
        {
            StopTimer = 0f;
        }
        if (transform.position.y <= GameManager.GetInstance().KillZ)
        {
            Destroy(this);
            GameManager.GetInstance().SpawnClub();
        }
    }

    public void HitBall(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        WasHit = true;
    }

    public float GetMinMagnitude()
    {
        return StopMovingMinMagnitude;
    }
}
