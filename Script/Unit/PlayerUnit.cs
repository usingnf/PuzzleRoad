using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 플레이어 유닛 스크립트.
/// 유닛 조작, 애니메이션 관련 기능.
/// </summary>

public class PlayerUnit : Unit
{
    public static PlayerUnit player;

    [Header("Player Inspector")]
    [SerializeField] private Transform maskTrans;
    [SerializeField] private List<InteractableObject> inters = new List<InteractableObject>();
    [SerializeField] private GameObject playerLight;
    [SerializeField] private GameObject prefab_blood;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private GameObject shield;
    [SerializeField] private Animator ani;
    [SerializeField] private SkinnedMeshRenderer faceSkin;
    [SerializeField] private Transform moveTileTarget;

    [Header("Player Status")]
    [SerializeField] private Vector3 maskOffset;
    [SerializeField] private float maskDistance = 0.1f;
    [SerializeField] private InteractableObject holdObj;
    private float defaultSpeed = 6.0f;
    //private Transform camTrans;
    private bool isLight = false;
    private bool isInWater = false;
    private bool isInGrass = false;
    private bool forceRun = false;
    private bool forceWalk = false;
    private bool isMoveTileTarget = false;

    public float DefaultSpeed { get { return defaultSpeed; } }
    

    public enum PlayerOrderState
    {
        Stop,
        Move,
        Hold,
    }

    private void Awake()
    {
        player = this;
        arr_lightTime = new float[9];
    }
    private void Start()
    {
        if(!GameManager.Exist())
            SetIsCanControl(true);
        else if(GameManager.GetState() != GameState.None)
            SetIsCanControl(false);

        SwtBlink(true);
        //camTrans = Camera.main.transform;
    }
    protected void Update()
    {
        //maskTrans.position = this.transform.position + (camTrans.position - this.transform.position).normalized * maskDistance + maskOffset;
        //maskTrans.rotation = camTrans.rotation;

        Update_KeyInput();
        Update_Animation();
        Update_MoveTileTarget();
        if (isLight)
        {
            lightTime += Time.deltaTime;
        }

        if (agent.isOnOffMeshLink)
        {
            agent.speed = 3.0f;
            if (coNavLink != null)
                StopCoroutine(coNavLink);
            coNavLink = CoNavLink();
            StartCoroutine(coNavLink);
        }
       

        //if (agent.isOnOffMeshLink)
        //{
        //    agent.CompleteOffMeshLink();
        //}
        //else
        //{
        //    if(agent.remainingDistance >= 100.0f)
        //        SetDestination(this.transform.position);
        //}
    }

    private void Update_KeyInput()
    {
        if(Input.GetKeyDown(MyKey.Interact))
        {
            Interact();
        }
        if(Input.GetKeyDown(MyKey.Light))
        {
            Swt_Light(!isLight);
        }
        if(Input.GetKeyDown(MyKey.Walk))
        {
            isWalkMode = true;
            SetSpeed(agent.speed, false);
        }
        if(Input.GetKeyUp(MyKey.Walk))
        {
            isWalkMode = false;
            SetSpeed(saveSpeed, false);
        }
    }
    


    private void Update_Animation()
    {
        if(agent.enabled)
        {
            //Debug.Log(agent.remainingDistance);
            if (agent.isStopped || agent.remainingDistance < 0.1f)
                ani.SetFloat("speed", 0);
            else
            {
                if(isWalkMode)
                    ani.SetFloat("speed", 2);
                else
                    ani.SetFloat("speed", 10);
            }
        }
        else
        {
            ani.SetFloat("speed", 0);
        }

        if (forceRun)
            ani.SetFloat("speed", 10);
        if(forceWalk)
        {
            ani.SetFloat("speed", 1);
            if (agent.enabled &&(agent.isStopped || agent.remainingDistance < 0.04f))
                ani.SetFloat("speed", 0);
        }

        if (holdObj == null)
            ani.SetFloat("ishold", 0);
        else
            ani.SetFloat("ishold", 1);
    }

    private void Update_MoveTileTarget()
    {
        if (!isMoveTileTarget)
            return;
        if (agent.enabled)
            SetDestination(moveTileTarget.position);
    }
    public void TrySetOrder(PlayerOrderState newOrder, Vector3 pos, Unit u = null, bool isForce = false)
    {
        if (!isForce && !isCanControl)
            return;
        if (newOrder == PlayerOrderState.Stop)
        {
            if(isMoveTileTarget)
            {
                moveTileTarget.position = this.transform.position;
            }
            else
            {
                if (agent.enabled)
                    SetDestination(this.transform.position);
            }
            
        }
        else if (newOrder == PlayerOrderState.Move)
        {
            if (isMoveTileTarget)
            {
                moveTileTarget.position = pos;
            }
            else
            {
                if (agent.enabled)
                    SetDestination(pos);
            }
        }
    }

    public void SetOffMoveTile()
    {
        isMoveTileTarget = false;
        moveTileTarget.parent = this.transform;
    }
    public void SetMoveTile(bool b, Transform target, Vector3 pos)
    {
        if(target == null)
        {
            isMoveTileTarget = false;
            moveTileTarget.parent = this.transform;
            return;
        }
        isMoveTileTarget = b;
        if(b)
        {
            moveTileTarget.position = pos;
            moveTileTarget.parent = target;
        }
        else
        {
            moveTileTarget.parent = this.transform;
        }
    }


    private void Interact()
    {
        if (!isCanControl)
            return;
        if(holdObj != null)
        {
            holdObj.Interact(this);
            return;
        }
        float dis = 999999;
        float nearDis = 999999;
        InteractableObject nearInter = null;
        foreach(InteractableObject inter in inters)
        {
            dis = Vector3.Distance(this.transform.position, inter.transform.position);
            if (dis < nearDis)
            {
                nearInter = inter;
                nearDis = dis;
            }
        }
        if(nearInter != null)
        {
            nearInter.Interact(this);
        }
    }

    public void AddInteract(InteractableObject inter)
    {
        if (inters.Contains(inter))
            return;
        inters.Add(inter);
    }

    public void RemoveInteract(InteractableObject inter)
    {
        inters.Remove(inter);
    }

    public bool GetIsLight()
    {
        return isLight;
    }

    private float lightTime = 0;
    [SerializeField] private float[] arr_lightTime;
    private int lightIndex = -1;
    public void Swt_Light(bool b)
    {
        if(isLight != b)
        {
            if(b)
            {
                lightTime = 0;
            }
            else
            {
                lightIndex += 1;
                arr_lightTime[lightIndex % 9] = lightTime;
                CheckSOS();
            }
            SoundManager.Instance.StartSound("SE_Player_Light_Switch", 1.0f, 0.1f);
        }
        isLight = b;
        playerLight.SetActive(b);
        Event_Light(b);
    }

    private UnityAction<bool> func_light;

    public void Add_LightEvent(UnityAction<bool> func)
    {
        if (func != null)
            func_light += func;
    }

    public void Remove_LightEvent(UnityAction<bool> func)
    {
        if(func != null && func_light != null)
            func_light -= func;
    }
    private void Event_Light(bool b)
    {
        if (func_light != null)
            func_light.Invoke(b);
    }

    private void CheckSOS()
    {
        int index = 0;

        for (int i = lightIndex + 1; i < arr_lightTime.Length + lightIndex + 1; i++)
        {
            index = i % 9;
            if (arr_lightTime[index] <= 0.001f || arr_lightTime[index] >= 6.0f)
                return;
        }

        float shortavg = 0;
        float longavg = 0;

        shortavg += arr_lightTime[(lightIndex + 1) % 9];
        shortavg += arr_lightTime[(lightIndex + 2) % 9];
        shortavg += arr_lightTime[(lightIndex + 3) % 9];
        shortavg += arr_lightTime[(lightIndex + 7) % 9];
        shortavg += arr_lightTime[(lightIndex + 8) % 9];
        shortavg += arr_lightTime[(lightIndex) % 9];
        shortavg = shortavg / 6;
        float shortavg1 = shortavg * 0.35f;

        longavg += arr_lightTime[(lightIndex + 4) % 9];
        longavg += arr_lightTime[(lightIndex + 5) % 9];
        longavg += arr_lightTime[(lightIndex + 6) % 9];
        longavg = longavg / 3;
        float longavg1 = longavg * 0.35f;

        if(Mathf.Abs(arr_lightTime[(lightIndex + 1) % 9] - shortavg) > shortavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex + 2) % 9] - shortavg) > shortavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex + 3) % 9] - shortavg) > shortavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex + 7) % 9] - shortavg) > shortavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex + 8) % 9] - shortavg) > shortavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex) % 9] - shortavg) > shortavg1)
            return;

        if (Mathf.Abs(arr_lightTime[(lightIndex + 4) % 9] - longavg) > longavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex + 5) % 9] - longavg) > longavg1)
            return;
        if (Mathf.Abs(arr_lightTime[(lightIndex + 6) % 9] - longavg) > longavg1)
            return;

        if (arr_lightTime[(lightIndex + 4) % 9] < shortavg * 2)
            return;
        if (arr_lightTime[(lightIndex + 5) % 9] < shortavg * 2)
            return;
        if (arr_lightTime[(lightIndex + 6) % 9] < shortavg * 2)
            return;

        if(GameManager.Exist())
        {
            GameManager.Instance.TrueRouteOn();
        }
        else
        {
            UIManager.Toast(Language.Instance.Get("UI_Toast_SOSFail"));
        }
    }

    public void SetHoldObj(InteractableObject obj)
    {
        holdObj = obj;
    }

    public InteractableObject GetHoldObj()
    {
        return holdObj; 
    }

    public void StageReStart()
    {
        agent.speed = 6.0f;
        if (holdObj != null)
        {
            Destroy(holdObj.gameObject);
        }
        inters.Clear();
    }

    public void DestroyHoldObj()
    {
        if (holdObj == null)
            return;
        holdObj.gameObject.SetActive(false);
        SetHoldObj(null);
    }

    public override void Death()
    {
        SetIsCanControl(false);
    }

    public void Revive()
    {
        SetIsCanControl(true);
        SetSpeed(DefaultSpeed, false);
        model.transform.localPosition = new Vector3(0, 0.0f, 0);
    }

    public Transform CreateBlood()
    {
        Transform trans = Instantiate(prefab_blood, this.transform.position, Quaternion.identity).transform;
        trans.localScale = new Vector3(0.01f,0.01f,0.01f);
        trans.DOScale(new Vector3(1, 1, 1), 2.0f).SetEase(Ease.OutSine);
        return trans;
    }

    
    public void Shield(bool b)
    {
        shield.SetActive(b);
    }

    public void Falling(UnityAction func = null)
    {
        ani.SetBool("isfall", true);
        ani.SetTrigger("fallnormal");
        model.transform.DOLocalMoveY(-20, 1.0f).OnComplete(()=>
        {
            Death();
            ani.SetBool("isfall", false);
            if (func != null)
                func.Invoke();
        }).SetEase(Ease.InSine);
        //SoundManager.Instance.DelayStartSound("SE_Player_Fall", 1.0f, 0.1f);
        SoundManager.Instance.StartSoundFadeLoop("SE_Player_Fall", 1.0f, 0.2f, 0.6f, 0f);
    }

    public Vector3 GetDest()
    {
        return agent.destination;
    }

    public Rigidbody GetRigid()
    {
        return rigid;
    }

    public Animator GetAnimator()
    {
        return ani;
    }

    public void WakeUp(float time = 0)
    {
        StartCoroutine(CoWakeUp());
    }

    private IEnumerator CoWakeUp(float time = 0)
    {
        yield return new WaitForSeconds(time);
        ani.SetTrigger("lie1");
        yield return new WaitForSeconds(0.5f);
        ani.SetTrigger("lie2");
        yield return new WaitForSeconds(3.0f);
        SetIsCanControl(true);
    }

    public void MoveSound(int index, float volume)
    {
        if(index == 1)
            if(isInWater)
                SoundManager.Instance.StartSound("SE_Run_Water", volume);
            else
            {
                if(isInGrass)
                    SoundManager.Instance.StartSound("SE_Run_Grass", volume);
                else
                    SoundManager.Instance.StartSound("SE_Run_Normal1", volume);
            }
        else if(index == 2)
            if (isInWater)
                SoundManager.Instance.StartSound("SE_Run_Water", volume);
            else
            {
                if (isInGrass)
                    SoundManager.Instance.StartSound("SE_Run_Grass", volume);
                else
                    SoundManager.Instance.StartSound("SE_Run_Normal2", volume);
            }
        else if (index == 3)
            if (isInWater)
                SoundManager.Instance.StartSound("SE_Run_Water", volume);
            else
            {
                if (isInGrass)
                    SoundManager.Instance.StartSound("SE_Walk_Grass", volume);
                else
                    SoundManager.Instance.StartSound("SE_Walk_Normal1", volume);
            }
        else if (index == 4)
            if (isInWater)
                SoundManager.Instance.StartSound("SE_Run_Water", volume);
            else
            {
                if (isInGrass)
                    SoundManager.Instance.StartSound("SE_Walk_Grass", volume);
                else
                    SoundManager.Instance.StartSound("SE_Walk_Normal2", volume);
            }
    }

    public void SetIsInWater(bool b)
    {
        isInWater = b;
    }

    public void SetIsInGrass(bool b)
    {
        isInGrass = b; 
    }

    private IEnumerator coNavLink = null;
    private IEnumerator CoNavLink()
    {
        while (true)
        {
            if (!agent.isOnOffMeshLink)
                break;
            yield return null;
        }
        agent.speed = defaultSpeed;
    }

    public void SetForceRun(bool b)
    {
        forceRun = b;
    }

    public void SetForceWalk(bool b)
    {
        forceWalk = b;
    }

    public void LookAt(Vector3 pos, float time)
    {
        Vector3 vec = pos - transform.position;
        vec.y = transform.position.y;
        float ang = Mathf.Atan2(-vec.z, vec.x) * Mathf.Rad2Deg + 90;
        transform.DORotate(new Vector3(0, ang, 0), time);
    }

    public void SwtBlink(bool b)
    {
        if (b)
        {
            if (coEye != null)
                StopCoroutine(coEye);
            coEye = CoEye();
            StartCoroutine(coEye);
        }
        else
        {
            if(coEye != null)
                StopCoroutine(coEye);
        }
    }

    private IEnumerator coEye = null;
    private IEnumerator CoEye()
    {
        WaitForSeconds wait = new WaitForSeconds(5.0f);
        while(true)
        {
            yield return CoClose(0.25f);
            yield return CoOpen(0.25f);
            yield return wait;
        }
    }

    private IEnumerator CoClose(float t)
    {
        float time = 0;
        while(true)
        {
            time += Time.deltaTime;
            if (time >= t)
                break;
            faceSkin.SetBlendShapeWeight(0, time / t * 100);
            yield return null;
        }
        faceSkin.SetBlendShapeWeight(0, 100);
    }

    private IEnumerator CoOpen(float t)
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= t)
                break;
            faceSkin.SetBlendShapeWeight(0, (1 - (time / t)) * 100);
            yield return null;
        }
        faceSkin.SetBlendShapeWeight(0, 0);
    }

    public void Skin_Greet()
    {
        SwtBlink(false);
        ani.SetTrigger("greet");
    }
}
