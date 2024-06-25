using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obj_FlyingKey : MonoBehaviour
{
    public enum FlyingKeyState
    {
        None,
        Move,
        Escape,
    }

    [SerializeField] private FlyingKeyState state;
    [SerializeField] private Senario011 stage11;
    [SerializeField] private NavMeshAgent agent;
    public Obj_KeyWay way;

    private float lastFlySound = 0;

    private WaitForSeconds wait = new WaitForSeconds(0.1f);
    private void OnEnable()
    {
        StartCoroutine(CoCheckEscape());
    }

    void Start()
    {
        player = PlayerUnit.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Update_Order();
    }

    void Update_Order()
    {
        if(state == FlyingKeyState.None)
        {

        }
        else if(state == FlyingKeyState.Move)
        {
        }
        else if(state == FlyingKeyState.Escape)
        {
            if(Vector3.Distance(way.transform.position, this.transform.position) < 0.62f)
                SetState(FlyingKeyState.None);
        }
    }

    public void SetState(FlyingKeyState newState)
    {
        if (state != FlyingKeyState.Escape && newState == FlyingKeyState.Escape)
            SoundManager.Instance.StartSound("SE_Fly_Move", 0.5f);
        state = newState;
        
    }

    public void Escape(Vector3 playerVec)
    {
        if (way == null)
            return;
        Vector3 vec = this.transform.position;
        vec.y = 0;
        playerVec.y = 0;
        vec = vec - playerVec;
        float angle = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg - 90;
        if (angle < 0)
            angle += 360;
        //Debug.Log(angle);
        int distance = Random.Range(3, 5);
        
        if(angle < 45)
        {
            way = stage11.GetWay(way.x, way.y, Obj_KeyWay.WayAngle.Right, distance);
        }
        else if(angle < 135)
        {
            way = stage11.GetWay(way.x, way.y, Obj_KeyWay.WayAngle.Up, distance);
        }
        else if (angle < 225)
        {
            way = stage11.GetWay(way.x, way.y, Obj_KeyWay.WayAngle.Left, distance);
        }
        else if (angle < 315)
        {
            way = stage11.GetWay(way.x, way.y, Obj_KeyWay.WayAngle.Down, distance);
        }
        else
        {
            way = stage11.GetWay(way.x, way.y, Obj_KeyWay.WayAngle.Right, distance);
        }

        //Debug.Log(way.transform.position);
        agent.SetDestination(way.transform.position);
        SetState(FlyingKeyState.Escape);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (state == FlyingKeyState.None)
    //        {
    //            Escape(other.transform.position);
    //        }
    //    }
    //}

    private Transform player;
    private IEnumerator CoCheckEscape()
    {
        while(true)
        {
            if(player != null)
            {
                if(Vector3.Distance(this.transform.position, player.position) < 6.2f && state == FlyingKeyState.None)
                    Escape(player.position);
            }
            yield return wait;
        }
    }
}
