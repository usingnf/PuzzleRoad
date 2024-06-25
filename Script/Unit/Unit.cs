using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public enum UnitOrderState
    {
        Stop,
        Move,
        Follow,
        Hold,
        MoveAttack,
        TargetAttack,
    }

    public static List<Unit> allUnits = new List<Unit>();

    public static List<Unit> AllUnits { get { return allUnits; } }

    [Header("Insepector")]
    [SerializeField] private bool isEnemy = false;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected bool isCanControl = true;
    [SerializeField] protected GameObject model;

    [Header("Status")]
    [SerializeField] private bool isVisible = true;
    protected bool isShield = false;
    protected bool isDead = false;
    protected float saveSpeed;
    protected bool isWalkMode = false;


    public bool IsEnemy { get { return isEnemy; } set { isEnemy = value; } }
    public bool IsVisible { get { return isVisible; } set { isVisible = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    public bool IsShield { get {  return isShield; } set { isShield = value; } }

    private void OnEnable()
    {
        allUnits.Add(this);
    }

    private void OnDisable()
    {
        allUnits.Remove(this);
    }

    public void StartCoSelectCircle()
    {

    }

    public void SwtSelectCircle(bool b)
    {

    }

    public void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    public void SetSpeed(float speed, float time = 0)
    {
        if (coSpeed != null)
            StopCoroutine(coSpeed);
        if(time > 0)
        {
            coSpeed = CoSpeed(speed, time);
            StartCoroutine(coSpeed);
        }
        else
        {
            agent.speed = speed;
        }
    }

    private IEnumerator coSpeed = null;
    private IEnumerator CoSpeed(float speed, float time)
    {
        float start = agent.speed;
        float t = 0;
        while(true)
        {
            t += Time.deltaTime;
            if (t >= time) break;
            agent.speed = Mathf.Lerp(start, speed, t/time);
            yield return null;
        }
        agent.speed = speed;
    }

    public void SetPos(Vector3 pos)
    {
        agent.enabled = false;
        this.transform.position = pos;
        agent.enabled = true;
    }
    

    public void SetSpeed(float speed, bool isAdd = false)
    {
        if(isAdd)
        {
            agent.speed = agent.speed + speed;
        }
        else
        {
            agent.speed = speed;
        }
        saveSpeed = agent.speed;
        if(isWalkMode)
        {
            agent.speed = 1.0f;
        }
       
    }

    public void SetSpeedHalf(bool b)
    {
        if(b)
            agent.speed = agent.speed * 0.5f;
        else
            agent.speed = agent.speed * 2.0f;
    }

    public void SwtAgent(bool b)
    {
        this.agent.enabled = b;
    }

    public void SetIsCanControl(bool b)
    {
        if(!b)
            SetDestination(this.transform.position);
        isCanControl = b;
    }

    public virtual void Death()
    {

    }
}
