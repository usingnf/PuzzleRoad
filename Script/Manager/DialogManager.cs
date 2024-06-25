using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static SO_Dialog;

/// <summary>
/// 다이얼로그+선택지 기능 관리.
/// 다이얼로그 데이터는 Scriptable Object로 관리.
/// 다이얼로그 관련 모든 Scriptable Object는 Inspector에 할당.
/// </summary>

public struct Struct_Choice
{
    public string str;
    public UnityAction<int> func;
    public int index;

    public Struct_Choice(string str, UnityAction<int> func, int index)
    {
        this.str = str;
        this.func = func;
        this.index = index;
    }
}
public class DialogManager : Singleton<DialogManager>
{
    public GameObject canvas_dialog;
    public GameObject canvas_choice;

    public MyText text_name;
    public MyText text_dialog;

    public LayoutElement layout_name;
    public RectTransform rect_name;

    public GameObject panel_choice;
    public GameObject prefab_choice;

    //초기 Scriptable Object 다이얼로그 데이터
    [SerializeField] private SO_Dialog[] dialogList;
    private Dictionary<string, SO_Dialog> dic_dialog = new Dictionary<string, SO_Dialog>();

    //모든 다이얼로그 데이터를 이름으로 등록.
    private void Awake()
    {
        Instance = this;
        foreach (SO_Dialog dialog in dialogList)
            dic_dialog[dialog.name] = dialog;
    }


    public void ShowDialog(SO_Dialog dialogData, UnityAction startFunc = null, UnityAction endFunc = null, string addText = "")
    {
        StopDialog();
        coDialog = CoDialog(dialogData, startFunc, endFunc, addText);
        StartCoroutine(coDialog);
    }

    public float ShowDialog(string str, UnityAction startFunc = null, UnityAction endFunc = null, string addText = "", bool isForce = true)
    {
        if (!dic_dialog.ContainsKey(str))
            return 0;
        if (!isForce && canvas_dialog.activeSelf)
            return 0;

        StopDialog();
        coDialog = CoDialog(dic_dialog[str], startFunc, endFunc, addText);
        StartCoroutine(coDialog);

        float t = 0;
        foreach(DialogData d in dic_dialog[str].data)
        {
            t += d.time + d.delayTime;
            if(!string.IsNullOrEmpty(d.voice) && SoundManager.Exist() && SoundManager.Instance.GetVoiceAudioClip($"{Language.Instance.GetVoiceLang()}_{d.voice}") != null)
            {
                t += SoundManager.Instance.GetVoiceAudioClip($"{Language.Instance.GetVoiceLang()}_{d.voice}").length;
            }
        }
        return t;
    }


    private IEnumerator coDialog = null;
    private IEnumerator coDialogText = null;
    private IEnumerator CoDialog(SO_Dialog dialogData, UnityAction startFunc, UnityAction endFunc, string addText)
    {
        if (startFunc != null)
            startFunc.Invoke();
        canvas_dialog.SetActive(true);
        
        // 다이얼로그 실행시 음악 소리 감소
        if(SoundManager.Exist())
            SoundManager.Instance.StartMusicCalm();

        for(int i = 0; i < dialogData.data.Length; i++)
        {
            coDialogText = CoDialogText(dialogData.data[i], addText);
            yield return coDialogText;
        }
        
        StopDialog();
        if (endFunc != null)
            endFunc.Invoke();
    }

    private IEnumerator CoDialogText(SO_Dialog.DialogData data, string addText)
    {
        layout_name.enabled = false;
        text_name.SetText(Language.Instance.Get(data.cName));
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect_name);
        layout_name.minWidth = rect_name.sizeDelta.x;
        layout_name.enabled = true;
        text_dialog.SetText(Language.Instance.Get(data.text, addText));

        if (UIManager.Exist())
            UIManager.Instance.AddLog(data.cName, data.text, true, addText);
        SoundManager.Instance.StopVoice();

        // 보이스 데이터가 있을 경우 음성 재생.
        if (!string.IsNullOrEmpty(data.voice))
            SoundManager.Instance.StartVoice(data.voice, 1.0f);

        if(data.time == 0)
        {
            string voice = $"{Language.Instance.GetVoiceLang()}_{data.voice}";
            if (SoundManager.Instance.GetVoiceAudioClip(voice) == null)
                yield return null;
            else
                yield return new WaitForSeconds(SoundManager.Instance.GetVoiceAudioClip(voice).length + data.delayTime); // voiceSound로
        }
        else
            yield return new WaitForSeconds(data.time);
    }

    public void StopDialog()
    {
        if (coDialog != null)
            StopCoroutine(coDialog);
        canvas_dialog.SetActive(false);
        StopDialogText();
        if (SoundManager.Exist())
            SoundManager.Instance.StopMusicCalm();
    }
    private void StopDialogText()
    {
        if (coDialogText != null)
            StopCoroutine(coDialogText);
    }

    public void ShowChoice(Struct_Choice[] choice)
    {
        if (choice.Length == 0)
            return;
        canvas_choice.SetActive(true);
        foreach(Transform trans in panel_choice.transform)
            Destroy(trans.gameObject);

        foreach(Struct_Choice c in choice)
        {
            GameObject obj = Instantiate(prefab_choice, panel_choice.transform);
            if(obj.TryGetComponent<MyText>(out MyText myText))
            {
                myText.SetText(c.str);
                if (obj.TryGetComponent<Button>(out Button btn))
                {
                    if (c.func != null)
                    {
                        btn.onClick.AddListener(() =>
                        {
                            c.func.Invoke(c.index);
                            CloseChoice();
                            SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
                        });
                    }
                    else
                    {
                        btn.onClick.AddListener(() =>
                        {
                            CloseChoice();
                            SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
                        });
                    }
                }
                
            }
        }
    }

    public void CloseChoice()
    {
        canvas_choice.SetActive(false);
    }
}
