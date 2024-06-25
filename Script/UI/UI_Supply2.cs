using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Supply2 : MonoBehaviour
{
    [SerializeField] private float[] answers;
    [SerializeField] private bool[] isClicks;
    [SerializeField] private Image[] images_first;
    [SerializeField] private Image[] images_second;
    [SerializeField] private Image image_reset;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private UnityEvent answerFunc;
    [SerializeField] private UnityEvent inAnswerFunc;

    private bool isSound = true;
    private bool isFailed = false;
    private bool isCompleted = false;
    void Start()
    {
        isSound = false;
        ReStart();
        isSound = true;
    }

    public void ReStart()
    {
        isFailed = false;
        isCompleted = false;
        answers = new float[3];
        isClicks = new bool[3];
        for (int i = 0; i < 3; i++)
        {
            answers[i] = 0;
            isClicks[i] = false;
        }
        for (int i = 0; i < 3; i++)
        {
            images_first[i].fillAmount = 0;
            images_second[i].fillAmount = 0;
            images_second[i].color = Color.green;
            texts[i].text = $"{(int)answers[i]}%";
            image_reset.color = Color.red;
        }
        if (isSound)
            SoundManager.Instance.StartSound("UI_Button_Click5", 1.0f);
    }

    private void Update()
    {
        if (isFailed)
            return;
        if (isCompleted)
            return;
        bool isCheck = false;
        if (isClicks[0])
        {
            isCheck = true;
            answers[0] += Time.deltaTime * 6.0f;
            answers[1] += Time.deltaTime * 12.0f;
            answers[2] += Time.deltaTime * 2.0f;
        }
        else if (isClicks[1])
        {
            isCheck = true;
            answers[0] += Time.deltaTime * 2.0f;
            answers[1] += Time.deltaTime * 6.0f;
            answers[2] += Time.deltaTime * 12.0f;
        }
        else if (isClicks[2])
        {
            isCheck = true;
            answers[0] += Time.deltaTime * 12.0f;
            answers[1] += Time.deltaTime * 2.0f;
            answers[2] += Time.deltaTime * 6.0f;
        }
        for(int i = 0; i < 3;i++)
        {
            if (answers[i] < 100)
                images_first[i].fillAmount = answers[i] * 0.01f;
            else
            {
                images_first[i].fillAmount = 1;
                if (answers[i] < 200)
                    images_second[i].fillAmount = (answers[i] - 100) * 0.01f;
                else
                    images_second[i].fillAmount = 1;
                if (answers[i] >= 120)
                {
                    images_second[i].color = Color.red;
                    isFailed = true;
                    SoundManager.Instance.StopSound(audioSource);
                    audioSource = null;
                    SoundManager.Instance.StartSound("SE_Supply_Failed", 0.8f);
                }
                else
                    images_second[i].color = Color.green;
            }
            texts[i].text = $"{(int)answers[i]}%";
        }

        if(isCheck)
        {
            bool isAnswer = true;
            for (int i = 0; i < 3; i++)
            {
                if (answers[i] < 100 || answers[i] >= 120)
                {
                    isAnswer = false;
                }
            }
            if (isAnswer)
            {
                if (answerFunc != null)
                    answerFunc.Invoke();
                image_reset.color = Color.green;
                isCompleted = true;
            }
            else
            {
                if(inAnswerFunc != null)
                    inAnswerFunc.Invoke();
                image_reset.color = Color.red;
            }
        }
        

    }

    private AudioSource audioSource = null;
    //Inspector
    public void Click(int index)
    {
        if (isFailed)
        {
            SoundManager.Instance.StartSound("SE_Supply_Failed", 0.8f);
            return;
        }
        isClicks[index] = true;
        audioSource = SoundManager.Instance.StartSoundFadeLoop("SE_Supply2_Excute", 0.5f, 0f, 0, 1000);
    }

    //Inspector
    public void ClickOut(int index)
    {
        isClicks[index] = false;
        if(audioSource != null)
            SoundManager.Instance.StopSound(audioSource);
    }

}
