using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSetupScript : MonoBehaviour
{
    [SerializeField] private Transform Ball;
    [SerializeField] private Transform ClubPivot;
    [SerializeField] private Transform Tee;
    [SerializeField] private float DespawnTime;
    public GameObject ExistingBall;


    private Vector3 Offset;

    private bool IsAttached;

    // Start is called before the first frame update
    void Start()
    {
        Offset = ClubPivot.localPosition - Ball.localPosition;
        if (ExistingBall)
        {
            Vector3 ballOffset = this.transform.position - Ball.transform.position;
            DestroyImmediate(Ball.gameObject);
            Ball = ExistingBall.transform;
            this.transform.position = ballOffset + Ball.transform.position;
            Ball.SetParent(this.transform, true);
            Ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        IsAttached = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttached)
            ClubPivot.localPosition = Ball.localPosition + Offset;
    }

    public void Detach()
    {
        IsAttached = false;
    }

    public void SetBall(Transform ball)
    {
        ExistingBall = ball.gameObject;
        Tee.gameObject.SetActive(false);
        ClubPivot.GetComponent<ClubSwinger>().MaxAngle = 180f;
    }

    public void DetachBallAndDestroy()
    {
        Ball.SetParent(null, true);
        Destroy(this.gameObject, DespawnTime);
        Ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

    }
}
