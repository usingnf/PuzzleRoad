using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Senario007 : Stage
{
    [SerializeField] private GameObject ui_water;
    [SerializeField] private Image image_water;
    [SerializeField] private MeshRenderer water_flow;
    [SerializeField] private MeshRenderer water_left;
    [SerializeField] private MeshRenderer water_right;

    private Material mat_flow;
    private Material mat_left;
    private Material mat_right;
    //[SerializeField] private Material fastX;
    //[SerializeField] private Material slowX;
    //[SerializeField] private Material fastY;
    //[SerializeField] private Material slowY;

    [SerializeField] private bool isWater = false;
    [SerializeField] private bool isFlow = false;
    [SerializeField] private float waterIntensity = 0;
    [SerializeField] private float waterSpeed = 0;
    [SerializeField] private float flowSpeed = 0;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] waterClip;
    [SerializeField] private float upFlowSpeed = 8.0f;

    private bool isStartFlow = false;
    private bool isFirstFlow = false;

    private float lastSwimTime = 0;
    private float swimSpaceTime = 0.4f;
    private float lastBubbleTime = 0;

    private void OnEnable()
    {
        StartCoroutine(CoFlow());
    }
    protected void Start()
    {
        base.Start();
        mat_flow = water_flow.material;
        mat_left = water_left.material;
        mat_right = water_right.material;
        SetQuestion(); ;
    }
    public override void ReStage()
    {
        base.ReStage();
        PlayerUnit.player.SetPos(startPos.position);
        PlayerUnit.player.Revive();
        if (coDelayRestage != null)
            StopCoroutine(coDelayRestage);
        SetQuestion();
    }
    public void SetQuestion()
    {
        isFlow = false;
        flowSpeed = 0;
        upFlowSpeed = 8.0f;
        waterIntensity = 0;
        mat_flow.SetVector("_NormalMapASpeeds", new Vector2(0, 1));
        mat_left.SetVector("_NormalMapASpeeds", new Vector2(-1, 0));
        mat_right.SetVector("_NormalMapASpeeds", new Vector2(-1, 0));
        StopFlow();
    }

    public void StartFlow()
    {
        if (isStartFlow)
            return;
        isStartFlow = true;

        mat_flow.SetVector("_NormalMapASpeeds", new Vector2(0, 10));
        mat_left.SetVector("_NormalMapASpeeds", new Vector2(-10, 0));
        mat_right.SetVector("_NormalMapASpeeds", new Vector2(-10, 0));
        audioSource.clip = waterClip[1];
        audioSource.Play();
        UIManager.Toast(Language.Instance.Get("UI_Toast_SwimKey", MyKey.Interact.ToString()));
        if(!isFirstFlow)
        {
            isFirstFlow = true;
            StartCoroutine(CoStartFlow());
        }
    }

    private IEnumerator CoStartFlow()
    {
        yield return new WaitForSeconds(2.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario007_Flow");
    }

    public void StopFlow()
    {
        isStartFlow = false;
        audioSource.clip = waterClip[0];
        audioSource.Play();
    }

    public void EnterFlow()
    {
        isFlow = true;
        flowSpeed = 0;
    }

    public void ExitFlow()
    {
        isFlow = false;
    }
    public void EnterWater()
    {
        isWater = true;
    }

    public void ExitWater()
    {
        isWater = false;
    }

    private void Update()
    {
        if(isWater)
        {
            ui_water.SetActive(true);
            waterIntensity += Time.deltaTime * waterSpeed;
            if (waterIntensity >= 1)
            {
                ReStage();
                SoundManager.Instance.StartSound("SE_Water_Die", 0.5f);
                if(upFlowSpeed >= 6.0f)
                    upFlowSpeed += - 1.0f;
            }
            if (lastBubbleTime + 4 < Time.time)
            {
                lastBubbleTime = Time.time;
                SoundManager.Instance.StartSoundFadeLoop("SE_Water_InLoop", waterIntensity * 0.4f, 0, 0.5f, 1.4f);
            }
        }
        else
        {
            if(waterIntensity <= 0)
            {
                waterIntensity = 0;
                ui_water.SetActive(false);
            }
            else
            {
                waterIntensity -= Time.deltaTime * waterSpeed * 8;
            }
        }
        if(ui_water.activeSelf)
        {
            Color c = image_water.color;
            c.a = waterIntensity;
            image_water.color = c;
        }
        if(isStartFlow)
        {
            if (isFlow)
            {
                if(lastSwimTime + 0.15f < Time.time)
                {
                    flowSpeed += Time.deltaTime * upFlowSpeed;
                }
                if (flowSpeed >= 4.5f)
                {
                    flowSpeed = 4.5f;
                }
            }
            if(isWater)
            {
                if (Input.GetKeyDown(MyKey.Interact))
                {
                    if (Time.time - lastSwimTime > swimSpaceTime)
                    {
                        flowSpeed = 0;
                        lastSwimTime = Time.time;
                        SoundManager.Instance.StartSound("SE_Water_Swim", 0.5f);
                    }
                }
            }
            
        }
    }

    private IEnumerator CoFlow()
    {
        Vector3 dest;
        float time = 0;
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        PlayerUnit p = PlayerUnit.player;
        while (true)
        {
            if (isStartFlow && isFlow)
            {
                if (lastSwimTime + 0.15f < Time.time)
                {
                    dest = p.GetDest();
                    p.SwtAgent(false);
                    float delta = Time.time - time;
                    if (delta >= 0.1f)
                        delta = 0.1f;
                    p.transform.position += new Vector3(-1, 0, 0) * delta * flowSpeed;
                    p.SwtAgent(true);
                    p.SetDestination(dest);
                }
                time = Time.time;
            }
            yield return wait;
        }
    }

    public void Falling()
    {
        PlayerUnit.player.Falling();
        if (upFlowSpeed >= 6.0f)
            upFlowSpeed += -1.0f;
        if (coDelayRestage != null)
            StopCoroutine(coDelayRestage);
        coDelayRestage = CoDelayRestage();
        StartCoroutine(coDelayRestage);
    }

    private IEnumerator coDelayRestage = null;
    private IEnumerator CoDelayRestage()
    {
        yield return new WaitForSeconds(3.0f);
        ReStage();
    }

}
