using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Save : MonoBehaviour
{
    public Senario003 stage;

    public Canvas canvas_save;
    public GameObject canvas_noise;
    public Camera rt_cam;
    private RenderTexture rt = null;

    public bool isCanControl = true;
    public Transform[] ui_slot;
    public Button[] btn_save;
    public Button btn_close;
    //public RenderTexture rt;

    public RawImage image_noise;
    public Material mat;
    void Start()
    {
        mat = new Material(image_noise.material);
        image_noise.material = mat;
        for(int i = 0; i < btn_save.Length; i++)
        {
            int temp = i;
            btn_save[i].onClick.AddListener(() =>
            {
                Save(temp);
            });
        }
        btn_close.onClick.AddListener(() =>
        {
            Close();
            SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
        });
    }

    public void Close(bool isForce = false)
    {
        if (!isForce && !isCanControl)
            return;
        this.gameObject.SetActive(false);

    }

    public void Save(int index)
    {
        if (!isCanControl)
            return;
        SoundManager.Instance.StartSound("UI_Success2", 0.8f);
        PlayerUnit.player.SetIsCanControl(false);
        StartCoroutine(CoSave(index));
    }

    private IEnumerator CoSave(int index)
    {
        CameraManager.Instance.CapturePlay();
        yield return null;
        yield return null;
        
        if (Screen.fullScreen)
        {
            rt = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 32);
            //rt.width = Screen.currentResolution.width;
            //rt.height = Screen.currentResolution.height;
        }
        else
        {
            rt = new RenderTexture(Screen.width, Screen.height, 32);
            //rt.width = Screen.width;
            //rt.height = Screen.height;
        }

        ui_slot[index].GetChild(1).GetComponent<RawImage>().texture = CameraManager.Instance.captureRT;
        ui_slot[index].GetChild(2).GetComponent<TextMeshProUGUI>().text = "3Floor";
        System.DateTime time = System.DateTime.Now;
        ui_slot[index].GetChild(3).GetComponent<TextMeshProUGUI>().text = $"{time.Year}/{time.Month}/{time.Day}\n{time.Hour.ToString("D2")}:{time.Minute.ToString("D2")}:{time.Second.ToString("D2")}";
        //rt_cam.transform.parent = Camera.main.transform;
        
        rt_cam.targetTexture = rt;
        image_noise.texture = rt;
        rt_cam.gameObject.SetActive(true);
        rt_cam.transform.position = Camera.main.transform.position;
        rt_cam.transform.rotation = Camera.main.transform.rotation;
        rt_cam.orthographicSize = Camera.main.orthographicSize;
        canvas_save.renderMode = RenderMode.ScreenSpaceCamera;
        canvas_save.worldCamera = rt_cam;
        canvas_save.planeDistance = 1.0f;
        canvas_noise.SetActive(true);
        stage.Event_Save(mat);
        yield return null;
        rt_cam.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        rt_cam.targetTexture = null;
        Destroy(rt);
    }
}
