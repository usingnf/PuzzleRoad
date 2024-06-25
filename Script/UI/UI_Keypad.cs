using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Keypad : MonoBehaviour
{
    [SerializeField] private Button btn_close;
    [SerializeField] private Button[] btn_keyPad;

    [SerializeField] private int answer = 0;
    [SerializeField] private string input_answer;
    [SerializeField] private TextMeshProUGUI text_answer;

    [SerializeField] private UnityEvent event_answer_void;
    [SerializeField] private UnityEvent<bool> event_answer_bool;
    [SerializeField] private UnityEvent event_InCorrect_void;

    private void OnEnable()
    {
        if (SoundManager.Exist())
            SoundManager.Instance.StartSound("SE_Keypad_On", 0.3f);
    }
    private void Start()
    {
        btn_close.onClick.AddListener(Close);
        for(int i = 0; i < 9; i++)
        {
            int num = i + 1;
            btn_keyPad[i].onClick.AddListener(() =>
            {
                InputNum(num.ToString());
            });
        }
        btn_keyPad[9].onClick.AddListener(()=> { Clear(true); });
        btn_keyPad[10].onClick.AddListener(() => { InputNum("0"); });
        btn_keyPad[11].onClick.AddListener(() => { CheckAnswer(); });
    }

    private void Update()
    {
        if(Input.GetKeyDown(MyKey.Escape))
        {
            Close();
        }
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }

    private void InputNum(string num)
    {
        if (input_answer.Length > 10)
            return;
        input_answer += num;
        text_answer.text = input_answer;
        SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
    }

    private void Clear(bool isButton = false)
    {
        input_answer = string.Empty;
        text_answer.text = string.Empty;
        SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
    }

    private void CheckAnswer()
    {
        string temp = input_answer;
        Clear();
        if(string.Equals(answer.ToString(), temp))
        {
            text_answer.text = "CORRECT";
            if(event_answer_void != null)
                event_answer_void.Invoke();
            if (event_answer_bool != null)
                event_answer_bool.Invoke(true);
            SoundManager.Instance.StartSound("UI_Success", 0.5f);
        }
        else
        {
            text_answer.text = "ERROR";
            if (event_InCorrect_void != null)
                event_InCorrect_void.Invoke();
            SoundManager.Instance.StartSound("SE_Button_Fail2", 0.5f);
        }
    }

    public void SetAnswer(int answer)
    {
        this.answer = answer;
    }

    public string GetInputAnswer()
    {
        return input_answer;
    }
}
