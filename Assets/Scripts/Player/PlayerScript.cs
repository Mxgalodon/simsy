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
        => a.x + 1 >= b.x && a.x - 1 <= b.x &&
        a.z + 1 >= b.z && a.z - 1 <= b.z;


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
