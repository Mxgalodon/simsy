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
    public enum StatsToRegain : int
    {
        boredness = 100,
        hungriness = 100,
        tiredness = 100,
        needToBeClean = 100,
    }

    public StatsToRegain stats
    {
        get; set;
    }

    private void Update()
    {
        if (!waitStatLose) StartCoroutine(StatsChangeEverySec());;
        waitStatRegain += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RayHit))
            {
                Hitpoint = RayHit.point;
                gameObject.GetComponent<NavMeshAgent>().destination = Hitpoint;
            }
        }
        if (isOnPlace(gameObject.transform.position, Hitpoint))
        {
            if (RayHit.collider.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObj))
            {
                RegainStatCoroutineStarter(interactableObj.stat);
            }
        }
    }


    float waitStatRegain = 0;
    private void RegainStatCoroutineStarter(string stat)
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
        => a.x == b.x && a.z == b.z;


    bool waitStatLose = false;
    IEnumerator StatsChangeEverySec()
    {
        waitStatLose = true;
        bored -= 1;
        hungry -= 1;
        tired -= 1;
        cleany -= 1;
        yield return new WaitForSeconds(1f);
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
