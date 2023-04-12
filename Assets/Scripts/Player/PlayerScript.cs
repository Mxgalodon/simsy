using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private RaycastHit RayHit;
    private Vector3 Hitpoint = Vector3.zero;
    private Ray ray;

    public int bored = 100;
    public int hungry = 100;
    public int tired = 100;
    public int cleany = 100;

    private bool isGoingByPlayerCommand = false;

    public GameObject BoredInteractPoint;
    public GameObject HungryInteractPoint;
    public GameObject TiredInteractPoint;
    public GameObject CleanyInteractPoint;

    private int currentInteractPoint = 0;

    public bool canMove = true;
    private float timeToStartMoving = 1.5f;
    public float time = 0f;

    private void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().destination = GetLowestStatInteractPoint().transform.position;
    }

    private void Update()
    {
        ManageAnims();

        if (!waitStatLose) StartCoroutine(StatsChangeEverySec()); ;
        waitStatRegain += Time.deltaTime;

        AutoGoing();

        ManualGoing();
    }

    private void ManageAnims()
    {
        if (isOnPlace(gameObject.transform.position, gameObject.GetComponent<NavMeshAgent>().destination)) gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        else gameObject.GetComponent<Animator>().SetBool("isWalking", false);
    }

    private void AutoGoing()
    {
        if (!isGoingByPlayerCommand && isOnPlace(transform.position, gameObject.GetComponent<NavMeshAgent>().destination))
        {
            canMove = false;
        }

        if (!canMove && isOnPlace(transform.position, gameObject.GetComponent<NavMeshAgent>().destination))
        {
            time += Time.deltaTime;
            if (time >= timeToStartMoving)
            {
                canMove = true;
                time = 0f;
            }
        }

        if (!isGoingByPlayerCommand && canMove)
        {
            gameObject.GetComponent<NavMeshAgent>().destination = GetLowestStatInteractPoint().transform.position;
            canMove = false;
        }
    }

    private void ManualGoing()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RayHit))
            {
                Hitpoint = RayHit.point;
                gameObject.GetComponent<NavMeshAgent>().destination = Hitpoint;
                isGoingByPlayerCommand = true;
            }
        }

        if (isGoingByPlayerCommand && isOnPlace(gameObject.transform.position, gameObject.GetComponent<NavMeshAgent>().destination))
        {
            isGoingByPlayerCommand = false;
        }
    }

    private GameObject GetLowestStatInteractPoint()
    {
        if (bored < hungry && bored < tired && bored < cleany) return BoredInteractPoint;
        else if (hungry < tired && hungry < cleany) return HungryInteractPoint;
        else if (tired < cleany) return TiredInteractPoint;
        return CleanyInteractPoint;
    }


    float waitStatRegain = 0;
    public void RegainStatCoroutineStarter(string stat)
    {
        if (waitStatRegain < 1) return;
        waitStatRegain = 0;
        switch (stat)
        {
            case nameof(bored):
                bored = addToStat(bored);
                break;
            case nameof(hungry):
                hungry = addToStat(hungry);
                break;
            case nameof(tired):
                tired = addToStat(tired);
                break;
            case nameof(cleany):
                cleany = addToStat(cleany);
                break;
        }
    }


    bool isOnPlace(Vector3 a, Vector3 b)
        => a.x == b.x &&
        a.z == b.z;


    bool waitStatLose = false;
    IEnumerator StatsChangeEverySec()
    {
        waitStatLose = true;
        bored -= 1;
        hungry -= 1;
        tired -= 1;
        cleany -= 1;
        yield return new WaitForSeconds(2f);
        waitStatLose = false;
    }

    private int addToStat(int a)
    {
        if (a > 99) return a;

        a += 10;
        if (a > 100) a = 100;
        return a;
    }
}
