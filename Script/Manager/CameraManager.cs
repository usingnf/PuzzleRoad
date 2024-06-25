using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 카메라 기능.
/// 카메라 회전, 카메라 추적 이동, 카메라 진동, 스크린샷
/// </summary>


public enum CamAngle
{
    _1 = 0,
    _3 = 1,
    _9 = 2,
    _7 = 3,
}
public class CameraManager : Singleton<CameraManager>
{
    [Header("Inspector")]
    [SerializeField] private Camera         currentCam;
    [SerializeField] private Transform      currentCamTrans;
    [SerializeField] private GameObject     obj_captureCam;
    [SerializeField] private Camera         captureCam;
    [SerializeField] private Transform      target;         // Camera Follow Target
    public RenderTexture                    captureRT;      // ScreenShot Temp Texture

    [SerializeField] private Vector3        offset = Vector3.zero;
    [SerializeField] private float          distance = 7;
    private float                           maxDistance = 7;
    [SerializeField] private float          angle = 0;
    [SerializeField] private CamAngle       camAngle = CamAngle._1;

    [SerializeField] private float          wheelSpeed = 2.0f;
    [SerializeField] private Vector3        shakePos = Vector3.zero;
    [SerializeField] private Vector3        shakeTargetPos = Vector3.zero;

    public UnityAction<CamAngle>            action_angle;   // Camera Rotate Event

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Update_KeyInput();
    }

    private void LateUpdate()
    {
        if (target != null)
            this.transform.position = target.position + offset + shakePos;
    }

    public void Update_KeyInput()
    {
        
        // Camera Rotate Left;
        if(Input.GetKeyDown(MyKey.CameraZ))
        {
            if (GameManager.Exist() && GameManager.GetState() != GameState.Start)
                return;
            angle += -90;
            this.transform.DORotate(new Vector3(0, angle, 0), 1.0f);
            SetCamAngle();
        }

        //Camera Rotate Right
        if (Input.GetKeyDown(MyKey.CameraC))
        {
            if (GameManager.Exist() && GameManager.GetState() != GameState.Start)
                return;
            angle += 90;
            this.transform.DORotate(new Vector3(0, angle, 0), 1.0f);
            SetCamAngle();
        }

        // Camera Zoom
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0)
        {
            if (GameManager.Exist() && GameManager.GetState() != GameState.Start)
                return;
            distance += -wheel * wheelSpeed;
            if (distance < 4)
                distance = 4;
            else if(distance > maxDistance)
                distance = maxDistance;
            currentCam.orthographicSize = distance;
            captureCam.orthographicSize = distance;
        }
    }

    public void ResetCamAngle()
    {
        angle = 0;
        this.transform.DORotate(new Vector3(0, angle, 0), 1.0f);
        SetCamAngle();
    }

    public void SetCamAngle()
    {
        float ang = angle % 360;
        if (ang < 0)
            ang += 360;
        if (ang == 0)
            camAngle = CamAngle._1;
        else if(ang == 90) 
            camAngle = CamAngle._7;
        else if (ang == 180)
            camAngle = CamAngle._9;
        else if (ang == 270)
            camAngle = CamAngle._3;

        if(action_angle != null)
            action_angle.Invoke(camAngle);
    }


    private IEnumerator coShake = null;
    public void Shake(float time, float power = 0.7f, float speed = 30.0f)
    {
        if(coShake != null)
            StopCoroutine(coShake);
        coShake = CoShake(power, speed, time);
        StartCoroutine(coShake);
    }

    public void StopShake()
    {
        if (coShake != null)
            StopCoroutine(coShake);
        shakePos = Vector3.zero;
        shakeTargetPos = Vector3.zero;
    }

    private void SetShakeTargetPos(float power)
    {
        shakeTargetPos = new Vector3 (Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * power;
    }
    private IEnumerator CoShake(float power, float speed, float time)
    {
        float accTime = 0;
        float sensitive = power * 0.1f;
        bool isReturn = false;
        bool isUp = true;
        while(true)
        {
            accTime += Time.deltaTime;
            if (accTime >= time)
                break;
            if (isReturn )
            {
                shakePos = Vector3.Lerp(shakePos, Vector3.zero, Time.deltaTime * speed);
                if (Vector3.Distance(shakePos, Vector3.zero) < sensitive)
                    isReturn = false;
            }
            else
            {
                shakePos = Vector3.Lerp(shakePos, shakeTargetPos, Time.deltaTime * speed);
                if (Vector3.Distance(shakePos, shakeTargetPos) < sensitive)
                {
                    //SetShakeTargetPos(power);
                    if(isUp)
                        shakeTargetPos = new Vector3(0, power, 0);
                    else
                        shakeTargetPos = new Vector3(0, -power, 0);
                    isReturn = true;
                }
            }
            
            yield return null;
        }
        shakeTargetPos = Vector3.zero;
        while (true)
        {
            shakePos = Vector3.Lerp(shakePos, Vector3.zero, Time.deltaTime * speed);
            if (Vector3.Distance(shakePos, Vector3.zero) < sensitive)
            {
                shakePos = Vector3.zero;
                break;
            }
            yield return null;
        }
        coShake = null;
    }


    public void CapturePlay()
    {
        //captureCam.SetActive(true);
        //captureCam.SetActive(false);
        StartCoroutine(CoCapture());
    }

    private IEnumerator CoCapture()
    {
        captureRT = null;

        // 전체화면 또는 창모드 크기 가져오기
        if (Screen.fullScreen)
            captureRT = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 32);
        else
            captureRT = new RenderTexture(Screen.width, Screen.height, 32);

        captureCam.targetTexture = captureRT;
        obj_captureCam.SetActive(true);
        yield return null;
        obj_captureCam.SetActive(false);
    }


    public void SetTarget(Transform taget)
    {
        this.target = taget;
    }

    public void SetDistance(float distance, float t)
    {
        if(coSetDistance!=null)
            StopCoroutine(coSetDistance);
        coSetDistance = CoSetDistance(distance, t);
        StartCoroutine(coSetDistance);
    }

    private IEnumerator coSetDistance = null;
    private IEnumerator CoSetDistance(float distance, float t)
    {
        float time = 0;
        float start = this.distance;
        while(true)
        {
            time += Time.deltaTime / 3;
            Mathf.Lerp(start, distance, time);
            if (time >= t)
                break;
            yield return null;
        }
        this.distance = distance;
    }

    public void SetTarget(Transform target, float t)
    {
        if(coCamTarget!=null)
            StopCoroutine (coCamTarget);
        coCamTarget = CoCamTarget(t);
        StartCoroutine (coCamTarget);
    }

    private IEnumerator coCamTarget = null;
    private IEnumerator CoCamTarget(float t)
    {
        Transform cam = transform;
        Transform player = PlayerUnit.player.transform;
        Vector3 startPos = cam.position;
        float time = 0;
        float add = 1 / t;
        while (true)
        {
            time += Time.deltaTime * add;
            cam.position = Vector3.Lerp(startPos, player.position + offset, time);
            if(time >= 1)
                break;
            yield return null;
        }
        CameraManager.Instance.SetTarget(player);
    }


}
