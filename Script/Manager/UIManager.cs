using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI와 관련된 모든 기능.
/// Fade 연출 기능, UI 조작, Toast Message 기능.
/// </summary>


public class UIManager : Singleton<UIManager>
{
    public enum UIState
    {
        Title = 0,
        Ingame = 1,
    }

    [SerializeField] private GameObject[] panel_ui;
    [SerializeField] private GameObject panel_fade;
    [SerializeField] private Image image_fade;
    [SerializeField] private GameObject panel_colorFade;
    [SerializeField] private Image image_colorFade;
    [SerializeField] private GameObject panel_setting;
    [SerializeField] private Image clickImg;
    private UIState uistate;

    [Header("Message")]
    [SerializeField] private GameObject panel_msg;
    [SerializeField] private Button btn_msgOk;
    [SerializeField] private Button btn_msgYes;
    [SerializeField] private Button btn_msgNo;
    [SerializeField] private MyText text_title;
    [SerializeField] private MyText text_msg;

    private UnityAction func_yes = null;
    private UnityAction func_no = null;
    private UnityAction func_ok = null;

    [Header("InGame")]
    [SerializeField] private GameObject panel_ingame;
    [SerializeField] private UI_Log panel_log;
    private bool isPrologue = false;

    [Header("ToastMessage")]
    [SerializeField] private GameObject panel_toast;
    [SerializeField] private MyText text_toast;

    private void Awake()
    {
        isDontDestroyed = true;
        Instance = this;
        if(Instance == this)
            DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if(btn_msgOk != null)
            btn_msgOk.onClick.AddListener(Btn_Ok);
        if (btn_msgYes != null)
            btn_msgYes.onClick.AddListener(Btn_Yes);
        if (btn_msgNo != null)
            btn_msgNo.onClick.AddListener(Btn_No);
        
        if(SceneManager.GetActiveScene().buildIndex == 1)
            StartTrueMusic();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartMouseClick();
        }
    }

    private TweenerCore<Color, Color, ColorOptions> clickTweener1 = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> clickTweener2 = null;
    private void StartMouseClick()
    {
        if(clickTweener1 != null)
            clickTweener1.Kill();
        if(clickTweener2 != null)
            clickTweener2.Kill();
        clickImg.transform.localScale = Vector3.zero;
        clickImg.transform.position = Input.mousePosition;
        clickImg.gameObject.SetActive(true);
        clickImg.color = new Color(1, 1, 1, 1);
        clickTweener1 = clickImg.DOColor(new Color(1, 1, 1, 0), 0.25f).SetEase(Ease.Linear);
        clickTweener2 = clickImg.transform.DOScale(1, 0.25f).OnComplete(() =>
        {
            clickImg.gameObject.SetActive(false);
        }).SetEase(Ease.Linear);
    }

    public void SetUIState(UIState state)
    {
        foreach (GameObject panel in panel_ui)
            panel.SetActive(false);
        if((int)state < panel_ui.Length && panel_ui[(int)state] != null)
            panel_ui[(int)state].SetActive(true);
        if (state == UIState.Title)
        {
            FadeStop();
            panel_log.Clear();

            StartTrueMusic();
        }
        uistate = state;
    }

    public void CloseGameUI()
    {
        panel_ui[(int)UIState.Ingame].SetActive(false);
    }

    public UIState GetUIState()
    {
        return uistate;
    }

    public void OpenSetting()
    {
        panel_setting.SetActive(true);
    }

    public void CloseSetting()
    {
        panel_setting.SetActive(false);
    }

    #region Fade
    private Tweener tween_Fade = null;
    public void FadeStop()
    {
        panel_fade.SetActive(false);
    }

    public void FadeInOut(float inTime, float outTime, UnityAction func)
    {
        if (tween_Fade != null)
        {
            tween_Fade.Complete();
            tween_Fade.Kill();
        }

        image_fade.color = new Color(0, 0, 0, 0);
        panel_fade.SetActive(true);
        tween_Fade = image_fade.DOColor(new Color(0, 0, 0, 1), inTime).OnComplete(() =>
        {
            image_fade.color = new Color(0, 0, 0, 1);
            tween_Fade = image_fade.DOColor(new Color(0, 0, 0, 0), outTime).OnComplete(() =>
            {
                if (func != null)
                    func.Invoke();
                panel_fade.SetActive(false);
            }).SetEase(Ease.InQuad);
        }).SetEase(Ease.InQuad);
    }

    public void FadeOutIn(float outTime, float inTime, UnityAction func)
    {
        if (tween_Fade != null)
        {
            tween_Fade.Complete();
            tween_Fade.Kill();
        }

        image_fade.color = new Color(0, 0, 0, 1);
        panel_fade.SetActive(true);

        tween_Fade = image_fade.DOColor(new Color(0, 0, 0, 0), outTime).OnComplete(() =>
        {
            image_fade.color = new Color(0, 0, 0, 0);
            tween_Fade = image_fade.DOColor(new Color(0, 0, 0, 1), inTime).OnComplete(() =>
            {
                if (func != null)
                    func.Invoke();
            }).SetEase(Ease.InQuad);
        }).SetEase(Ease.InQuad);
    }

    
    public void FadeIn(float time, Color c, UnityAction func, Ease ease = Ease.InQuad)
    {
        if (tween_Fade != null)
        {
            tween_Fade.Complete();
            tween_Fade.Kill();
        }
        image_fade.color = new Color(c.r, c.g, c.b, 0);
        panel_fade.SetActive(true);
        tween_Fade = image_fade.DOColor(new Color(c.r, c.g, c.b, 1), time).OnComplete(() =>
        {
            if (func != null)
                func.Invoke();
        }).SetEase(ease);
    }

    public void FadeOut(float time, UnityAction func)
    {
        if (tween_Fade != null)
        {
            tween_Fade.Complete();
            tween_Fade.Kill();
        }

        image_fade.color = new Color(image_fade.color.r, image_fade.color.g, image_fade.color.b, 1);
        panel_fade.SetActive(true);
        tween_Fade = image_fade.DOColor(new Color(image_fade.color.r, image_fade.color.g, image_fade.color.b, 0), time).OnComplete(() =>
        {
            if (func != null)
                func.Invoke();
            panel_fade.SetActive(false);
        }).SetEase(Ease.InQuad);
    }


    #endregion

    #region Message

    public static void MessageOk(string title, string str, UnityAction ok = null)
    {
        if (Instance == null)
            return;
        Instance.ViewMessage(title, str, ok);
    }

    public static void MessageYesNo(string title, string str, UnityAction yes = null, UnityAction no = null)
    {
        if (Instance == null)
            return;
        Instance.ViewMessage(title, str, yes, no);
    }

    private void ViewMessage(string title, string str, UnityAction ok = null)
    {
        text_title.SetText(title);
        text_msg.SetText(str);
        func_ok = ok;
        btn_msgYes.gameObject.SetActive(false);
        btn_msgNo.gameObject.SetActive(false);
        btn_msgOk.gameObject.SetActive(true);
        panel_msg.SetActive(true);
    }

    private void ViewMessage(string title, string str, UnityAction yes = null, UnityAction no = null)
    {
        text_title.SetText(title);
        text_msg.SetText(str);
        func_yes = yes;
        func_no = no;
        btn_msgYes.gameObject.SetActive(true);
        btn_msgNo.gameObject.SetActive(true);
        btn_msgOk.gameObject.SetActive(false);
        panel_msg.SetActive(true);
    }

    private void Btn_Ok()
    {
        panel_msg.SetActive(false);
        if (func_ok != null)
            func_ok();
        func_ok = null;
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }

    private void Btn_Yes()
    {
        panel_msg.SetActive(false);
        if(func_yes != null)
            func_yes();
        func_yes = null;
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }
    private void Btn_No()
    {
        panel_msg.SetActive(false);
        if (func_no != null)
            func_no();
        func_no = null;
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }

    #endregion

    #region Toast

    public static void Toast(string str)
    {
        if (Instance == null)
            return;
        Instance.ViewToast(str);
    }

    private void ViewToast(string str)
    {
        string result = text_toast.SetText(str);
        panel_toast.SetActive(true);
        if(coToast != null)
            StopCoroutine(coToast);
        float time = 1;
        if(Language.Instance.GetCurrentLang() == Language.LanguageEnum.English)
            time += result.Length * 0.15f;
        else
            time += result.Length * 0.2f;
        if (time >= 10)
            time = 10;
        coToast = CoToast(time);
        StartCoroutine(coToast);
    }

    private IEnumerator coToast = null;
    private IEnumerator CoToast(float t)
    {
        yield return new WaitForSeconds(t);
        panel_toast.SetActive(false);
        coToast = null;
    }

    #endregion

    [SerializeField] private Image img_badEnd;
    [SerializeField] private MyText text_badEnd;
    public void StartBadEnd()
    {
        PlayerPrefs.SetInt("BadEnd", 1);
        CloseGameUI();
        img_badEnd.gameObject.SetActive(true);
        img_badEnd.color = new Color(0, 0, 0, 0);
        text_badEnd.GetTMP().color = new Color(1, 1, 1, 0);
        text_badEnd.GetTMP().DOColor(new Color(1,1,1,1), 7.0f);
        text_badEnd.SetText(Language.Instance.Get("UI_BadEnd_1"));
        img_badEnd.DOColor(new Color(0, 0, 0, 1), 5.0f).OnComplete(() =>
        {
            if (PlayerUnit.player != null)
                PlayerUnit.player.SetIsCanControl(false);
            text_badEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
            {
                text_badEnd.SetText(Language.Instance.Get("UI_BadEnd_2"));
                text_badEnd.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f).OnComplete(() =>
                {
                    text_badEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
                    {
                        text_badEnd.SetText(Language.Instance.Get("UI_BadEnd_3"));
                        text_badEnd.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f).OnComplete(() =>
                        {
                            text_badEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
                            {
                                SetUIState(UIState.Title);
                                img_badEnd.gameObject.SetActive(false);
                                SceneManager.LoadScene("Scene_Title");
                            });
                        }).SetEase(Ease.Linear);
                    });
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);
            
        }).SetEase(Ease.Linear);
    }

    [SerializeField] private Image img_badEnd2;
    [SerializeField] private MyText text_badEnd2;
    public void StartBadEnd2()
    {
        CloseGameUI();
        img_badEnd2.gameObject.SetActive(true);
        img_badEnd2.color = new Color(0, 0, 0, 0);
        text_badEnd2.GetTMP().color = new Color(1, 1, 1, 0);
        text_badEnd2.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f);
        text_badEnd2.SetText(Language.Instance.Get("UI_BadEnd2_1"));
        img_badEnd2.DOColor(new Color(0, 0, 0, 1), 5.0f).OnComplete(() =>
        {
            if (PlayerUnit.player != null)
                PlayerUnit.player.SetIsCanControl(false);
            text_badEnd2.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
            {
                SetUIState(UIState.Title);
                img_badEnd2.gameObject.SetActive(false);
                SceneManager.LoadScene("Scene_Title");
            });

        }).SetEase(Ease.Linear);
    }

    [SerializeField] private Image img_normalEnd;
    [SerializeField] private MyText text_normalEnd;

    public void StartNormalEnd()
    {
        PlayerPrefs.SetInt("NormalEnd", 1);
        CloseGameUI();
        img_normalEnd.gameObject.SetActive(true);
        img_normalEnd.color = new Color(0, 0, 0, 0);
        text_normalEnd.GetTMP().color = new Color(1, 1, 1, 0);
        text_normalEnd.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f);
        text_normalEnd.SetText(Language.Instance.Get("UI_NormalEnd_1"));
        img_normalEnd.DOColor(new Color(0, 0, 0, 1), 5.0f).OnComplete(() =>
        {
            text_normalEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
            {
                text_normalEnd.SetText(Language.Instance.Get("UI_NormalEnd_2"));
                text_normalEnd.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f).OnComplete(() =>
                {
                    text_normalEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
                    {
                        SetUIState(UIState.Title);
                        img_normalEnd.gameObject.SetActive(false);
                        SceneManager.LoadScene("Scene_Title");
                    });
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);

        }).SetEase(Ease.Linear);
    }

    [SerializeField] private Image img_trueEnd;
    [SerializeField] private MyText text_trueEnd;

    public void StartTrueEnd()
    {
        PlayerPrefs.SetInt("TrueEnd", 1);
        CloseGameUI();
        img_trueEnd.gameObject.SetActive(true);
        img_trueEnd.color = new Color(0, 0, 0, 0);
        text_trueEnd.GetTMP().color = new Color(1, 1, 1, 0);
        text_trueEnd.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f);
        text_trueEnd.SetText(Language.Instance.Get("UI_TrueEnd_1"));
        img_trueEnd.DOColor(new Color(0, 0, 0, 1), 5.0f).OnComplete(() =>
        {
            text_trueEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
            {
                text_trueEnd.SetText(Language.Instance.Get("UI_TrueEnd_2"));
                text_trueEnd.GetTMP().DOColor(new Color(1, 1, 1, 1), 7.0f).OnComplete(() =>
                {
                    text_trueEnd.GetTMP().DOColor(new Color(1, 1, 1, 0), 5.0f).OnComplete(() =>
                    {
                        SetUIState(UIState.Title);
                        img_trueEnd.gameObject.SetActive(false);
                        SceneManager.LoadScene("Scene_Title");
                    });
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);

        }).SetEase(Ease.Linear);
    }

    public void SetIsPrologue(bool b)
    {
        isPrologue = b;
    }

    public bool GetIsPrologue()
    {
        return isPrologue;
    }

    public void AddLog(string name, string content, bool isId, string addText = "")
    {
        panel_log.AddLog(name, content, isId, addText);
    }

    private void StartTrueMusic()
    {
        if (PlayerPrefs.GetInt("TrueEnd", 0) == 0)
            SoundManager.Instance.StartMusic("Music_Timeless_Romance", 0.4f, true); // normal
        else
            SoundManager.Instance.StartMusic("Music_Dreams_of_Success_full", 0.4f, true); // true
    }
}
