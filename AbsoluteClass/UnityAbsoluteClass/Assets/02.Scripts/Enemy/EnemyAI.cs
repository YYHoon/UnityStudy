using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;
    Transform playerTr;
    Transform enemyTr;

    public float attackDist = 5.0f;

    public float traceDist = 10.0f;

    public bool isDie = false;

    WaitForSeconds ws;

    MoveAgent moveAgent;
    EnemyFire enemyFire;
    Animator animator;
    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        enemyTr = GetComponent<Transform>();

        ws = new WaitForSeconds(0.3f);
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();
    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            if (state == State.DIE) yield break;

            //float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            float dist = (playerTr.position - enemyTr.position).sqrMagnitude;
            if(dist<attackDist)
            {
                state = State.ATTACK;
            }else if(dist<=traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }
    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if(enemyFire.isFire==false)
                    {
                        enemyFire.isFire = true;
                    }
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;
                default:
                    break;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
