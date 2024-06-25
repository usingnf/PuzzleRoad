using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Stage003;

//[ExecuteInEditMode]
public class Obj_ColorSwtGround : MonoBehaviour
{
    public Stage003 stage;
    public GroundColor color = GroundColor.None;

    [SerializeField] private MeshRenderer render;
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material mat;
    [SerializeField] private Material defaultMat2;
    //[SerializeField] private bool isEnd = false;

    private static Color red = new Color(0.85f, 0, 0);
    private static Color green = new Color(0, 0.85f, 0);
    private static Color blue = new Color(0, 0, 0.85f);

    public int index = 0;
    void Awake()
    {
        mat = render.material;
        defaultMat2 = new Material(mat);
        SetColor(color);
    }

    public void SetColor(GroundColor c)
    {
        if(c == GroundColor.Red)
            mat.SetColor("_Color", red);
        else if (c == GroundColor.Green)
            mat.SetColor("_Color", green);
        else if (c == GroundColor.Blue)
            mat.SetColor("_Color", blue);
        //else if (c == GroundColor.Yellow)
        //    mat.SetColor("_Color", Color.yellow);
        //else if (c == GroundColor.Brown)
        //    mat.SetColor("_Color", new Color(0.6792453f, 0.4006338f, 0));
        //else if (c == GroundColor.Pink)
        //    mat.SetColor("_Color", new Color(1,0, 0.8728962f));
        //else if (c == GroundColor.Purple)
        //    mat.SetColor("_Color", new Color(0.6236438f, 0, 1));
    }

    public void SetMat(bool isDefault)
    {
        if(isDefault)
        {
            render.material = defaultMat2;
        }
        else
        {
            render.material = mat;
        }
    }

    private Tweener tween = null;
    public void StartHint()
    {
        if (tween != null)
            tween.Kill();
        if (!this.gameObject.activeSelf)
            return;
        mat.SetColor("_Color", defaultMat2.color);
        tween = mat.DOColor(defaultMat.color, "_Color", 1.0f);
    }

    public void StartVisible(bool b)
    {
        if (tween != null)
            tween.Kill();
        if (b)
        {
            if (defaultMat2 != null)
            {
                tween = mat.DOColor(defaultMat2.color, "_Color", 0.2f);
            }
        }
        else
        {
            if(defaultMat2 != null)
            {
                tween = mat.DOColor(defaultMat.color, "_Color", 0.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GroundColor lastColor = stage.GetSelectColor();
            //Debug.Log(stage.GetLastIndex() + "/" + index +"/"+ Mathf.Abs(stage.GetLastIndex() - index));
            if (stage.lastGround != null)
            {
                if(((int)(color - lastColor + 3) % 3 != 1 || color == GroundColor.None) && this != stage.lastGround)
                {
                    //Failed
                    stage.ReStage();
                    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
                else
                {
                    if (stage.GetLastIndex() != 0 && index != 0 && Mathf.Abs(stage.GetLastIndex() - index) != 1 && Mathf.Abs(stage.GetLastIndex() - index) != 11)
                    {
                        //Bug Failed
                        stage.ReStage();
                        SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                    }
                    else
                    {
                        //Success
                        stage.SelectColor(color);
                        stage.SwtMat(index);
                        stage.lastGround = this;
                        //SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
                        PlaySound();
                    }
                }
            }
            else
            {
                //Success
                stage.SelectColor(color);
                stage.SwtMat(index);
                stage.lastGround = this;
                //SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
                PlaySound();
            }
        }
    }


    private void PlaySound()
    {
        if (SoundManager.Exist())
            SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }
}
