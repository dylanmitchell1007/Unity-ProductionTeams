﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class PlayerController : MonoBehaviour
{
    public Animator anim;
    private NavMeshAgent agent;
    [HideInInspector]
    public Wizard wiz;
    private bool attacking = false;
    private bool targeting = false;
    public int crystals;
    private GameObject target;
    public GameObject myTower;

    void Start()
    {
        crystals = 100;
        agent = GetComponent<NavMeshAgent>();
        wiz = ScriptableObject.CreateInstance<Wizard>();
        wiz.damage = 10;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (targeting)
            {
                this.transform.LookAt(new Vector3(target.transform.position.x, 6f, target.transform.position.z));
                if (!attacking)
                    anim.SetFloat("speed", agent.velocity.magnitude);
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 15;
            }
            if (targeting && Vector3.Distance(this.transform.position, target.transform.position) < 30 && !attacking)
            {
                anim.SetTrigger("attack");
                attacking = true;
            }
            if (attacking)
            {
                if (target.GetComponent<MinionBehaviour>().minion.health <= 0)
                {
                    crystals += target.GetComponent<MinionBehaviour>().minion.damage * 5;
                    anim.SetTrigger("targetdead");
                    targeting = false;
                    attacking = false;
                    target = null;
                }
            }

        }
        else
        {
            anim.SetFloat("speed", agent.velocity.magnitude);
        }
    }
    private void ShootLeftFireBall()
    {

        GetComponent<MaxFireBall>().ShootLeft(target);


    }

    private void ShootRightFireBall()
    {
        GetComponent<MaxFireBall>().ShootRight(target);

    }

    public void Target(GameObject go)
    {
        target = go;
        agent.SetDestination(go.transform.position);
        targeting = true;
    }
    public void SpawnMinion()
    {
        if (crystals >= 50)
        {
            var spawners = myTower.GetComponentsInChildren<MinionSpawner>().ToList();

            if (Time.time < 200)
            {
                spawners[0].Spawn(Random.Range(90, 116), Random.Range(4, 7));
                spawners[1].Spawn(Random.Range(90, 116), Random.Range(4, 7));
            }
            else if (Time.time < 400)
            {
                spawners[0].Spawn(Random.Range(118, 131), Random.Range(5, 11));
                spawners[1].Spawn(Random.Range(118, 131), Random.Range(5, 11));
            }
            else if (Time.time > 400 && Time.time < 430)
            {
                //Harder minions

                spawners[0].Spawn(Random.Range(500, 751), Random.Range(50, 76));
                spawners[1].Spawn(Random.Range(500, 751), Random.Range(50, 76));
            }
            else if (Time.time < 700)
            {
                spawners[0].Spawn(Random.Range(190, 216), Random.Range(17, 26));
                spawners[1].Spawn(Random.Range(190, 216), Random.Range(17, 26));
            }
            //add boss minions 
            else if (Time.time < 1200)
            {


                spawners[0].Spawn(Random.Range(240, 276), Random.Range(24, 41));
                spawners[1].Spawn(Random.Range(240, 276), Random.Range(24, 41));
            }
            else
            {
                spawners[0].Spawn(Random.Range(280, 351), Random.Range(35, 51));
                spawners[1].Spawn(Random.Range(280, 351), Random.Range(35, 51));
            }
            crystals -= 50;
        }
        else
        {
            //do a thing
        }
    }
}
