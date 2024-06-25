using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Title : MonoBehaviour
{
    [SerializeField] private Button btn_start;
    [SerializeField] private Button btn_stage;
    [SerializeField] private Button btn_option;
    [SerializeField] private Button btn_exit;
    [SerializeField] private Image img;
    [SerializeField] private Sprite normalTitle_CloseEyes;
    [SerializeField] private Sprite normalTitle_OpenEyes;
    [SerializeField] private Sprite trueTitle;
    [SerializeField] private MyText text_title;
    [SerializeField] private MyText text_stage;
    [SerializeField] private GameObject obj_flower;
    [SerializeField] private GameObject obj_crown;
    [SerializeField] private GameObject obj_stage;
    [SerializeField] private Material mat;

    private int isTrueEnd = 0;
    private bool isReady = true;
    private void OnEnable()
    {
        mat = img.material;
        GameManager.SetState(GameState.None);
        //PlayerPrefs.SetInt("TrueEnd", 0);
        int _isTrueEnd = PlayerPrefs.GetInt("TrueEnd", 0);
        int isNormalEnd = PlayerPrefs.GetInt("NormalEnd", 0);
        int isBadEnd = PlayerPrefs.GetInt("BadEnd", 0);
        isTrueEnd = _isTrueEnd;
        //_isTrueEnd = 1;

        if (_isTrueEnd == 0)
        {
            mat.SetFloat("_Brightness", 1.0f);
            mat.SetFloat("_Flow", -20.0f);
            mat.SetFloat("_Lerp", 0.4f);
            img.sprite = normalTitle_CloseEyes;
            text_title.SetText("UI_GameTitle", true);
            MoveShadow();
            obj_flower.SetActive(false);
            obj_crown.SetActive(false);
        }
        else
        {
            mat.SetFloat("_Lerp", 1);
            img.sprite = trueTitle;
            text_title.SetText("UI_GameTitle_True", true);
            obj_flower.SetActive(true);
            obj_crown.SetActive(true);
        }

        if(_isTrueEnd == 1 || isNormalEnd == 1 || isBadEnd == 1)
        {
            btn_stage.interactable = true;
            text_stage.SetColor(Color.white);
        }
        else
        {
            btn_stage.interactable = false;
            text_stage.SetColor(Color.gray);
        }
    }
    void Start()
    {
        if (btn_start != null)
            btn_start.onClick.AddListener(() =>
            {
                Btn_Start();
            });
        if (btn_stage != null)
            btn_stage.onClick.AddListener(() =>
            {
                Btn_Stage();
            });
        if (btn_option != null)
            btn_option.onClick.AddListener(() =>
            {
                Btn_Option();
            });
        if (btn_exit != null)
            btn_exit.onClick.AddListener(() =>
            {
                Btn_Exit();
            });
    }


    private void Btn_Start()
    {
        if (!isReady)
            return;
        int autoSave = PlayerPrefs.GetInt("AutoSave", 0);
        if (autoSave == 0)
        {
            GameStart();
        }
        else
        {
            UIManager.MessageYesNo(Language.Instance.Get("UI_Msg_Notice"), Language.Instance.Get("UI_Msg_AutoLoad"), 
            () =>
            {
                isReady = false;
                GameManager.AutoLoad = autoSave;
                StopMoveShadow();
                SoundManager.Instance.StartSound("UI_GameStart", 1.0f);
                UIManager.Instance.SetIsPrologue(false);
                UIManager.Instance.FadeIn(1.0f, Color.black, () =>
                {
                    SceneManager.LoadScene("Scene_Game");
                    UIManager.Instance.SetUIState(UIManager.UIState.Ingame);
                    GameManager.SetState(GameState.Loading);
                    isReady = true;
                });
                SoundManager.Instance.StopMusic(0.5f);
                StartCoroutine(CoTitleOpenEyes());
            },
            ()=>
            {
                GameStart();
            });
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        }
    }

    private void GameStart()
    {
        isReady = false;
        GameManager.AutoLoad = -1;
        StopMoveShadow();
        SoundManager.Instance.StartSound("UI_GameStart", 1.0f);
        UIManager.Instance.SetIsPrologue(true);
        UIManager.Instance.FadeIn(1.0f, Color.black, () =>
        {
            SceneManager.LoadScene("Scene_Game");
            UIManager.Instance.SetUIState(UIManager.UIState.Ingame);
            GameManager.SetState(GameState.Loading);
            isReady = true;
        });
        SoundManager.Instance.StopMusic(0.5f);
        StartCoroutine(CoTitleOpenEyes());
    }

    private IEnumerator CoTitleOpenEyes()
    {
        //mat.DOFloat(1.0f, "_Brighrness", 0.5f);
        if (isTrueEnd > 0)
            yield break;
        mat.DOFloat(0.85f, "_Lerp", 1.5f);
        yield return new WaitForSeconds(0.20f);
        img.sprite = normalTitle_OpenEyes;
    }

    private void Btn_Stage()
    {
        if (!isReady)
            return;
        int isTrueEnd = PlayerPrefs.GetInt("TrueEnd", 0);
        int isNormalEnd = PlayerPrefs.GetInt("NormalEnd", 0);
        int isBadEnd = PlayerPrefs.GetInt("BadEnd", 0);
        if(isTrueEnd + isNormalEnd + isBadEnd == 0)
        {
            UIManager.Toast(Language.Instance.Get("UI_Toast_Stage"));
        }
        else
        {
            obj_stage.SetActive(true);
        }
        
        SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
    }

    private void Btn_Option()
    {
        if (!isReady)
            return;
        SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        UIManager.Instance.OpenSetting();
    }

    private void Btn_Exit()
    {
        if (!isReady)
            return;
        SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        if (UIManager.Exist())
        {
            UIManager.MessageYesNo(Language.Instance.Get("UI_Msg_Notice"), Language.Instance.Get("UI_Msg_Exit"), ()=>
            {
                #if UNITY_EDITOR
                                UnityEditor.EditorApplication.isPlaying = false;
                #else
                        Application.Quit(); 
                #endif
            });
        }
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //        Application.Quit(); 
        //#endif
    }


    public void SetIsReady(bool b)
    {
        isReady = b;
    }

    private IEnumerator coMoveShadow = null;
    private IEnumerator CoMoveShadow()
    {
        mat.SetFloat("_Flow", -20.0f);
        while (true)
        {
            yield return null;
        }
    }

    private Tweener shadowTween = null;
    private bool isMoveShadow = false;
    private void MoveShadow()
    {
        if (isMoveShadow)
            return;
        if (shadowTween != null)
            shadowTween.Kill();
        isMoveShadow = true;
        DoMoveShadow();
    }

    private void StopMoveShadow()
    {
        isMoveShadow = false;
        if (shadowTween != null)
        {
            shadowTween.Kill();
        }
    }

    private void DoMoveShadow()
    {
        if (!isMoveShadow)
            return;
        shadowTween = mat.DOFloat(mat.GetFloat("_Flow") + 1.0f, "_Flow", 3.0f).OnComplete(() =>
        {
            if(isMoveShadow)
                DoMoveShadow();
        }).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        StopMoveShadow();
    }

}
