using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Supply : MonoBehaviour
{
    [SerializeField] private Button[] btns;
    [SerializeField] private bool[] isAnswer;
    [SerializeField] private RectTransform[] images;
    private bool isReady = false;
    [SerializeField] private UnityEvent answerFunc;
    [SerializeField] private UnityEvent inAnswerFunc;
    private bool isComplete = false;

    private void Start()
    {
        isReady = false;
        isAnswer = new bool[btns.Length]; ;
        for (int i = 0; i < isAnswer.Length; i++)
        {
            isAnswer[i] = false;
        }
        
        for(int i = 0; i < btns.Length; i++)
        {
            int temp = i;
            btns[i].onClick.AddListener(() =>
            {
                Click_Btn(temp);
            });
        }
        Click_Btn(2);
        Click_Btn(5);
        if (Random.Range(0, 2) == 0)
            Click_Btn(3);
        else
            Click_Btn(4);
        Set();
        isReady = true;
    }

    public void Set()
    {
        if (isComplete)
            return;
        bool isCorrect = true;
        for(int i = 0; i < isAnswer.Length; i++)
        {
            if (isAnswer[i])
            {
                images[i].anchoredPosition = new Vector2(0, -75);
                images[i].rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                images[i].anchoredPosition = new Vector2(0, 75);
                images[i].rotation = Quaternion.Euler(180, 0, 0);
                isCorrect = false;
            }
        }
        if(isCorrect)
        {
            if (answerFunc != null)
                answerFunc.Invoke();
            isComplete = true;
        }
        else
        {
            if (inAnswerFunc != null)
                inAnswerFunc.Invoke();
            isComplete = false;
        }
    }

    public void Click_Btn(int index)
    {
        if(index == 0)
        {
            isAnswer[0] = !isAnswer[0];
            isAnswer[1] = !isAnswer[1];
        }
        else if (index == 1)
        {
            isAnswer[1] = !isAnswer[1];
            isAnswer[3] = !isAnswer[3];
            isAnswer[5] = !isAnswer[5];
        }
        else if (index == 2)
        {
            isAnswer[2] = !isAnswer[2];
            isAnswer[3] = !isAnswer[3];
        }
        else if (index == 3)
        {
            isAnswer[3] = !isAnswer[3];
            isAnswer[4] = !isAnswer[4];
            isAnswer[5] = !isAnswer[5];
        }
        else if (index == 4)
        {
            isAnswer[4] = !isAnswer[4];
            isAnswer[5] = !isAnswer[5];
        }
        else if (index == 5)
        {
            isAnswer[0] = !isAnswer[0];
            isAnswer[1] = !isAnswer[1];
            isAnswer[5] = !isAnswer[5];
        }
        if(isReady)
        {
            Set();
            SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
        }
    }
}
