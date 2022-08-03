using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum WinType
{
    Albatross = -3,
    Eagle = -2,
    Birdie = -1,
    Par = 0,
    Bogey = 1,
    Double_Bogey = 2,
    Hole_Cleared = 3
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private float BallWinTime;
    [SerializeField] private float killZ;
    [SerializeField] private float VictoryScreenTime;
    [SerializeField] private GameObject SwingPoint;
    private Rigidbody ballInHole;
    private float BallWinTimer;



    public float KillZ => killZ;

    private static GameManager _instance;
    private int CurrentHole;
    private int CurrentHits;
    private int CurrentHolePar;

    private List<StartpointComponent> Startpoints;
    private UIScript UIScript;
    private float VictoryScreenTimer;

    // Start is called before the first frame update
    void Start()
    {
        UIScript = FindObjectOfType<UIScript>();
        _instance = this;
        BallWinTimer = 0f;
        Startpoints = FindObjectsOfType<StartpointComponent>().ToList();
        CurrentHole = -1;
        IncrementHole();
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
                DisplayWin();
                VictoryScreenTimer += Time.deltaTime;
            }
        }
        else if (BallWinTimer > 0f)
        {
            BallWinTimer = 0f;

        }
        if (VictoryScreenTimer > 0f)
        {
            if (VictoryScreenTimer >= VictoryScreenTime)
            {
                IncrementHole();
                Destroy(ballInHole.gameObject);
                ballInHole = null;
                SpawnClub();
                VictoryScreenTimer = 0f;
                BallWinTimer = 0f;
            }
            else
            {
                VictoryScreenTimer += Time.deltaTime;
            }
        }
    }

    void DisplayWin()
    {
        WinType winType;
        if (CurrentHits < CurrentHolePar - 2)
            winType = WinType.Albatross;
        else if (CurrentHits > CurrentHolePar + 2)
        {
            winType = WinType.Hole_Cleared;
        }
        else
        {
            int x = CurrentHits - CurrentHolePar;
            winType = (WinType)(x);
        }

        UIScript.ClearHole(winType);
    }

    void IncrementHole()
    {
        CurrentHole++;
        StartpointComponent spawnpoint = Startpoints.FirstOrDefault(point => point.Hole == CurrentHole);
        if (spawnpoint != null)
        {
            UIScript.NewHole();
            CurrentHolePar = spawnpoint.Par;
            UIScript.SetHoleAndPar(CurrentHole, CurrentHolePar);
            CurrentHits = 0;
            UIScript.SetStrokes(CurrentHits);
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
        if (ballInHole) return;

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

    public void HitBall()
    {
        CurrentHits++;
        UIScript.SetStrokes(CurrentHits);
    }
}
