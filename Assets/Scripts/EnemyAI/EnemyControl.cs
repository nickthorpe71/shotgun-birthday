using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    //Shooting
    float bornAimMod;
    float aimModX = 0;
    bool shooting;

    //Targeting
    float bornChangeTargetMod;
    float changeTargetMod;
    public GameObject lookTarget;
    public GameObject moveTarget;
    float changeShotTarget = 0.0f;
    //float changeWalkTarget = 0.0f;
    int detectionRadius = 20;

    //Wander
    bool wandering;

    //Components
    Battery battery;
    Shoot shootScript;
    Engine engine;

    //Game Manager
    GameManager manager;

    private void Start()
    {
        manager = GameManager.instance;
        battery = GetComponent<Battery>();
        shootScript = GetComponent<Shoot>();
        engine = GetComponent<Engine>();

        GameManager.instance.AddObject(gameObject);

        InvokeRepeating("LookForTargets", 0, 1f);

        InvokeRepeating("ChooseWhereToWalk", 0, 0.1f);

        bornAimMod = Random.Range(0, 4);
        bornChangeTargetMod = Random.Range(2, 20);
        

        engine.moveSpeed = 5;
    }

    private void FixedUpdate()
    {
        if(lookTarget != null)
            Look();
    }

    void LookForTargets()
    {
        if (ListOfShotTargets().Count > 0)
        {
            aimModX = Random.Range(-bornAimMod, bornAimMod);
            changeTargetMod = Random.Range(0, bornChangeTargetMod);

            if (changeShotTarget >= changeTargetMod || lookTarget == null || !ListOfShotTargets().Contains(lookTarget))
            {
                lookTarget = ListOfShotTargets()[Random.Range(0, ListOfShotTargets().Count)];
                changeShotTarget = 0.0f;
            }
            else
            {
                changeShotTarget += 1f;
            }

            if (!shooting && !battery.recharging && (lookTarget.CompareTag("Player") || lookTarget.CompareTag("Enemy")))
            {
                StartCoroutine(ShootRoutine1());
            }
        }
        else
        {
            aimModX = 0;
            lookTarget = moveTarget;
        }
    }

    void ChooseWhereToWalk()
    {
        if (ListOfWalkTargets().Count > 0)
        {
            if(moveTarget != null)
            {
                SendMoveTarget();
            }
            else if(moveTarget == null && !ListOfWalkTargets().Contains(moveTarget))
            {
                moveTarget = ListOfWalkTargets()[Random.Range(0, ListOfWalkTargets().Count)];
            }
        }
        else
        {
            Wander();
        }
    }

    void SendMoveTarget()
    {
        Vector3 direction = moveTarget.transform.position - transform.position;
        float x = direction.x;
        float z = direction.z;

        if ((x > -0.5f && x < 0) || (x < 0.5f && x > 0))
            x = 0;
        else if (x >= 0.5f)
            x = 1;
        else if(x <= -0.5f)
            x = -1;

        if ((z > -0.5f && x < 0) || (z < 0.5f && x > 0))
            z = 0;
        else if (z >= 0.5f)
            z = 1;
        else if (z <= -0.5f)
            z = -1;

        engine.movement.x = x;
        engine.movement.z = z;
    }

    void Wander()
    {
        //am I moving?
        if (wandering)
        {
            //if so should I change direction or keep going?
            int chance = Random.Range(0, 100);

            if (chance < 10)
            {
                engine.movement.x = Random.Range(-1, 1);
                engine.movement.z = Random.Range(-1, 1);
            }
        }
        else
        {
            engine.movement.x = Random.Range(-1, 1);
            engine.movement.z = Random.Range(-1, 1);
            wandering = true;
        }

        AdjustForBorders();
    }

    void AdjustForBorders()
    {
        if (transform.position.x >= manager.arenaX - 25)
            engine.movement.x = -1;
        if (transform.position.x <= -manager.arenaX + 25)
            engine.movement.x = 1;
        if (transform.position.z >= manager.arenaZ - 25)
            engine.movement.z = -1;
        if (transform.position.z <= -manager.arenaZ + 25)
            engine.movement.z = 1;
    }

    void Look()
    {
        Vector3 shotTargetPos = new Vector3(lookTarget.transform.position.x + aimModX, lookTarget.transform.position.y, lookTarget.transform.position.z);

        transform.LookAt(shotTargetPos);
    }

    List<GameObject> ListOfShotTargets()
    {
        List<GameObject> all = new List<GameObject>();

        int longest = 0;

        if (manager.players.Count > longest)
            longest = manager.players.Count;
        if (manager.enemies.Count > longest)
            longest = manager.enemies.Count;

        for(int i = 0; i < longest; i++)
        {
            if (manager.players.Count - 1 >= i)
                if (Vector3.Distance(manager.players[i].transform.position, transform.position) < detectionRadius)
                    all.Add(manager.players[i]);
            if (manager.enemies.Count - 1 >= i && manager.enemies[i] != gameObject)
                if (Vector3.Distance(manager.enemies[i].transform.position, transform.position) < detectionRadius)
                    all.Add(manager.enemies[i]);
        }

        return all;
    }

    List<GameObject> ListOfWalkTargets()
    {
        List<GameObject> all = new List<GameObject>();

        //add orbs
        for (int i = 0; i < manager.orbs.Count; i++)
        {
            if (Vector3.Distance(manager.orbs[i].transform.position, transform.position) < detectionRadius)
                all.Add(manager.orbs[i]);
        }

        //later add in the ability to walk toward shot targets as well

        return all;
    }

    IEnumerator ShootRoutine1()
    {
        shooting = true;
        yield return new WaitForSeconds(Random.Range(0.1f, Random.Range(1, 3)));
        Shoot();
        yield return new WaitForSeconds(Random.Range(0f, Random.Range(1,3)));
        Shoot();
        yield return new WaitForSeconds(Random.Range(0f, Random.Range(1, 3)));
        Shoot();
        yield return new WaitForSeconds(Random.Range(0f, Random.Range(1, 3)));

        if (battery.enabled)
            battery.SlowRecharge();

        shooting = false;

    }

    void Shoot()
    {
        if (battery.charges > 0)
        {
            shootScript.Shooting();
        }
        
    }

}
