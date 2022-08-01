using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float BallWinTime;
    [SerializeField] private float killZ;
    [SerializeField] private GameObject SwingPoint;

    private Rigidbody ballInHole;
    private float BallWinTimer;




    public float KillZ => killZ;

    private static GameManager _instance;
    private int CurrentHole;

    private List<StartpointComponent> Startpoints;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        BallWinTimer = 0f;
        Startpoints = FindObjectsOfType<StartpointComponent>().ToList();
        CurrentHole = 0;
        SpawnClub();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballInHole)
        {
            BallWinTimer += Time.deltaTime;
            if (BallWinTimer > BallWinTime)
            {
                CurrentHole++;
                Destroy(ballInHole.gameObject);
                ballInHole = null;
                SpawnClub();
            }
        }
        else if (BallWinTimer > 0f)
        {
            BallWinTimer = 0f;

        }
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public void SetBallInHole(Rigidbody ball, int hole)
    {
        if (!ball) ballInHole = null;
        if (hole == CurrentHole) ballInHole = ball;
    }

    public void SpawnClub(Transform ball = null)
    {
        StartpointComponent spawnpoint = Startpoints.FirstOrDefault(point => point.Hole == CurrentHole);
        if (spawnpoint != null)
        {
            GameObject go = Instantiate(SwingPoint);
            go.transform.position = spawnpoint.transform.position;
            go.transform.Rotate(spawnpoint.transform.rotation.eulerAngles);
            if (ball != null)
            {
                go.GetComponent<SwingSetupScript>().SetBall(ball);
            }
        }

    }
}
