using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [SerializeField] private Button btn_option;

    [SerializeField] private Button btn_hint;
    [SerializeField] private Button btn_setting;
    [SerializeField] private Button btn_goTitle;
    [SerializeField] private Button btn_close;
    [SerializeField] private Button btn_log;
    [SerializeField] private Button btn_logClose;
    [SerializeField] private GameObject panel_option;
    [SerializeField] private GameObject panel_setting;
    [SerializeField] private GameObject panel_btns;
    [SerializeField] private GameObject panel_log;

    [SerializeField] private UI_Prologue prologue;

    public static bool isActive = false;
    private void OnEnable()
    {
        isActive = true;
        if(UIManager.Exist())
        {
            if (UIManager.Instance.GetIsPrologue())
            {
                isActive = false;
                prologue.StartPrologue();
            }
            else
                StartCoroutine(CoCheckAutoLoad());
        }
    }

    private IEnumerator CoCheckAutoLoad()
    {
        while(true)
        {
            if(GameManager.Exist())
            {
                GameManager.Instance.CheckAutoLoad();
                break;
            }
            yield return null;
        }
    }
    void Start()
    {
        if (btn_option != null)
            btn_option.onClick.AddListener(Btn_Option);
        if(btn_hint != null)
            btn_hint.onClick.AddListener(Btn_Hint);
        if(btn_setting != null)
            btn_setting.onClick.AddListener(Btn_Setting);
        if(btn_goTitle != null)
            btn_goTitle.onClick.AddListener(Btn_GoTitle);
        if(btn_close != null)
            btn_close.onClick.AddListener(Btn_Close);
        if (btn_log != null)
            btn_log.onClick.AddListener(Btn_OpenLog);
        if (btn_logClose != null)
            btn_logClose.onClick.AddListener(Btn_Close);
        
    }
    public void Btn_Option()
    {
        if (UIManager.Instance.GetUIState() != UIManager.UIState.Ingame)
            return;
        if (!isActive)
            return;

        if (panel_option.activeSelf)
        {
            Btn_Close();
        }
        else
        {
            panel_btns.SetActive(true);
            panel_setting.SetActive(false);
            panel_option.SetActive(true);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        }
    }
    public void Btn_Hint()
    {
        if(GameManager.Exist())
        {
            GameManager.Instance.GetStage(GameManager.Instance.GetCurrentStage()).Hint();
            //UIManager.Toast(Language.Instance.Get("UI_Option_NoHint"));
        }
        else
        {
            UIManager.Toast(Language.Instance.Get("UI_Option_NoHint"));
        }
        
        SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
    }

    public void Btn_Setting()
    {
        //panel_btns.SetActive(false);
        //panel_setting.SetActive(true);
        UIManager.Instance.OpenSetting();
        panel_log.SetActive(false);
        SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
    }

    public void Btn_CloseSetting()
    {
        panel_setting.SetActive(false);
        panel_log.SetActive(false);
        panel_btns.SetActive(true);
        SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
    }

    public void Btn_GoTitle()
    {
        SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        UIManager.MessageYesNo(Language.Instance.Get("UI_Msg_Notice"), Language.Instance.Get("UI_Msg_GoTitle"), () =>
        {
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
            SceneManager.LoadScene("Scene_Title");
            panel_option.SetActive(false);
            UIManager.Instance.SetUIState(UIManager.UIState.Title);
        });
    }

    public void Btn_OpenLog()
    {
        panel_setting.SetActive(false);
        panel_option.SetActive(false);
        panel_log.SetActive(true);
        SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
    }
    public void Btn_Close()
    {
        panel_option.SetActive(false);
        panel_setting.SetActive(false);
        panel_log.SetActive(false);
        UIManager.Instance.CloseSetting();
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }


    private void Update()
    {
        if(Input.GetKeyDown(MyKey.Escape))
        {
            Btn_Option();
        }
    }

    private void OnDisable()
    {
        panel_option.SetActive(false);
        panel_setting.SetActive(false);
        panel_log.SetActive(false);
    }

}
