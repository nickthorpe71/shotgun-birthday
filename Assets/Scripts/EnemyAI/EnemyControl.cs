using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlOld : MonoBehaviour
{
    //Pursue
    float speed = 3.0f;
    public GameObject target;
    bool pursuing;

    //Shooting
    float bornAimMod;
    float aimModX = 0;
    Vector3 shotTarget;
    bool shooting;

    //Targets
    float bornChangeTargetMod;
    float changeTargetMod;

    //Wander
    bool wandering;

    //Searching
    float changeTarget = 0.0f;
    int detectionRadius = 20;

    //Components
    Battery battery;
    Shoot shootScript;
    Engine engine;

    //Game Manager
    GameManager manager;

    private void Start()
    {
        manager = GameManager.instance;

        GameManager.instance.AddObject(gameObject);

        InvokeRepeating("MakeDecisions", 0, 0.25f);
        battery = GetComponent<Battery>();
        shootScript = GetComponent<Shoot>();
        bornAimMod = Random.Range(0, 7);
        bornChangeTargetMod = Random.Range(2, 20);
        engine = GetComponent<Engine>();

        engine.moveSpeed = 5;
    }

    private void FixedUpdate()
    {
        if (wandering && engine.enabled == false)
            engine.enabled = true;
        else if (wandering && engine.enabled == true)
            engine.enabled = false;

        if (pursuing && target != null)
            Pursue();

    }

    void MakeDecisions()
    {
        if (Time.time > 5 && target == null)
        {
            SearchForTargets();
            aimModX = Random.Range(-bornAimMod, bornAimMod);
            changeTargetMod = Random.Range(0, bornChangeTargetMod);

            if (battery.charges == 0)
            {
                pursuing = false;
                wandering = true;
            }
        }
        else if (!pursuing)
        {
            Wander();
        }


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

    void Pursue()
    {
        float step = (speed + engine.moveSpeed) * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) > 15)
            step = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < 10)
            step = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < 3)
            step = (speed + engine.moveSpeed) * Time.deltaTime;

        Vector3 targPos = new Vector3(target.transform.position.x, 1, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targPos, step);

        if (changeTarget >= changeTargetMod || target == null)
        {
            SearchForTargets();
            changeTarget = 0.0f;
        }
        else
        {
            changeTarget += 0.1f;
            if (!shooting && !battery.recharging && target.tag != "Collect")
            {
                StartCoroutine(ShootRoutine1());
            }
        }

        shotTarget = new Vector3(target.transform.position.x + aimModX, target.transform.position.y, target.transform.position.z);

        transform.LookAt(shotTarget);
    }

    List<GameObject> ListOfTargets()
    {
        List<GameObject> all = new List<GameObject>();

        int longest = 0;

        if (manager.players.Count > longest)
            longest = manager.players.Count;
        if (manager.enemies.Count > longest)
            longest = manager.enemies.Count;
        if (manager.orbs.Count > longest)
            longest = manager.orbs.Count;

        for (int i = 0; i < longest; i++)
        {
            if (manager.players.Count - 1 >= i)
                if (Vector3.Distance(manager.players[i].transform.position, transform.position) < detectionRadius)
                    all.Add(manager.players[i]);
            if (manager.enemies.Count - 1 >= i && manager.enemies[i] != gameObject)
                if (Vector3.Distance(manager.enemies[i].transform.position, transform.position) < detectionRadius)
                    all.Add(manager.enemies[i]);
            if (manager.orbs.Count - 1 >= i)
                if (Vector3.Distance(manager.orbs[i].transform.position, transform.position) < detectionRadius)
                    all.Add(manager.orbs[i]);
        }

        return all;
    }

    List<GameObject> ListOfTargetsOld()
    {
        List<List<GameObject>> choice = new List<List<GameObject>>();
        List<GameObject> safe = new List<GameObject>();

        int choiceInt = 0;
        int testInt = 0;
        int safety = 0;

        choice.Add(manager.enemies);
        choice.Add(manager.players);
        choice.Add(manager.orbs);


        while (testInt == 0)
        {
            choiceInt = Random.Range(0, choice.Count);
            testInt = choice[choiceInt].Count;

            safety++;

            if (safety >= 100)
            {
                print("Safety triggered");
                return safe;
            }

        }

        print("TestInt = " + testInt + " ChoiceInt = " + choiceInt);

        if (choice[choiceInt].Contains(gameObject))
            choice[choiceInt].Remove(gameObject);

        /*
        else
        {
            if (others.Length > collectables.Length)
            {
                for (int i = 0; i < others.Length; i++)
                {
                    if (others.Length > 0 && others[i] != gameObject && others[i] != null)
                        all.Add(others[i]);
                    if (collectables.Length > 0 && collectables[i] != null)
                        all.Add(collectables[i]);
                    if (players.Length > 0 && players[i] != null)
                        all.Add(players[i]);
                }
            }
            else if (collectables.Length > others.Length)
            {
                for (int i = 0; i < collectables.Length; i++)
                {
                    if (others.Length >= i && others[i] != gameObject && others[i] != null)
                        all.Add(others[i]);
                    if (collectables.Length >= i && collectables[i] != null)
                        all.Add(collectables[i]);
                    if (players.Length >= i) && players[i] != null)
                        all.Add(players[i]);
                }
            }
        }*/

        return choice[choiceInt];
    }

    void SearchForTargets()
    {
        List<GameObject> targets = ListOfTargets();

        if (targets.Count <= 0)
        {
            pursuing = false;
            wandering = true;
            print("empty targets");
            return;
        }

        for (int i = 0; i < targets.Count; i++) //If the new version of ListOfTargets is kept this for loop can be removed
        {
            if (Vector3.Distance(targets[i].transform.position, transform.position) < detectionRadius)
            {
                target = targets[i];
                pursuing = true;
                wandering = false;
                print("target selected!");
                return;
            }
        }

        pursuing = false;
        wandering = true;
        return;
    }

    IEnumerator ShootRoutine1()
    {
        shooting = true;
        yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        Shoot();
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        Shoot();
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        Shoot();
        yield return new WaitForSeconds(Random.Range(0f, 3f));

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
