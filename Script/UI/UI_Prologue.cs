using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Prologue : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button btn_plus;
    [SerializeField] private Button btn_minus;
    [SerializeField] private Button btn_confirm;
    private AudioSource voiceAudio;

    private float volume = 0.0f;
    private bool isReady = true;
    private bool isProgress = false;

    void Start()
    {
        btn_plus.onClick.AddListener(Btn_Plus);
        btn_minus.onClick.AddListener(Btn_Minus);
        btn_confirm.onClick.AddListener(Btn_Confirm);
        volumeSlider.value = SoundManager.Instance.GetVoiceVolumeOption();

        volumeSlider.onValueChanged.AddListener((volume) =>
        {
        if (!SoundManager.Instance.GetVoiceAudio().isPlaying)
            SoundManager.Instance.StartVoice("Voice_Prologue_1", 1.0f);
            SoundManager.Instance.SetVoiceVolume((int)volume);
        });
    }


    public void StartPrologue()
    {
        UIManager.Instance.FadeOutIn(1.5f, 1.5f, () =>
        {
            UIManager.Instance.FadeOut(1.5f, null);
            panels[0].SetActive(false);
            panels[1].SetActive(true);
            SoundManager.Instance.StartVoice("Voice_Prologue_1", 1.0f);
        });
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        this.gameObject.SetActive(true);

        //panels[1].SetActive(false);

        //float inTime = 1.5f;
        //float outTime = 1.5f;
        //panel1_text.color = Color.black;
        //panel1_text.DOColor(Color.white, inTime);
        //panel1_images[0].color = Color.black;
        //panel1_images[0].DOColor(Color.white, inTime).OnComplete(() =>
        //{
        //    panel1_text.DOColor(Color.black, outTime);
        //    panel1_images[0].DOColor(Color.black, outTime).OnComplete(() =>
        //    {
        //        panels[0].SetActive(false);
        //        panels[1].SetActive(true);
        //    });
        //});
    }

    private void Btn_Plus()
    {
        float volume = SoundManager.Instance.SetVoiceVolume(SoundManager.Instance.GetVoiceVolumeOption() + 5);
        //SoundManager.Instance.StartVoice("Piano_Do", 1.0f);
        if(volumeSlider.value != volume)
            volumeSlider.value = volume;
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }

    private void Btn_Minus()
    {
        float volume = SoundManager.Instance.SetVoiceVolume(SoundManager.Instance.GetVoiceVolumeOption() - 5);
        //SoundManager.Instance.StartVoice("Piano_Do", 1.0f);
        if (volumeSlider.value != volume)
            volumeSlider.value = volume;
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }

    private void Btn_Confirm()
    {
        if (!isReady)
            return;
        if (isProgress)
            return;
        isProgress = true;
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        p.GetAnimator().SetTrigger("lie1");
        UIManager.Instance.FadeIn(1.5f,Color.black, () =>
        {
            this.gameObject.SetActive(false);
            p.WakeUp(1.0f);
            UIManager.Instance.FadeOut(3.0f, () =>
            {
                isReady = true;
                isProgress = false;
                if(MyKey.Move == KeyCode.Mouse1)
                    UIManager.Toast(Language.Instance.Get("UI_Toast_Move", "Right Mouse"));
                else
                    UIManager.Toast(Language.Instance.Get("UI_Toast_Move", MyKey.Move.ToString()));
                UI_Ingame.isActive = true;
            });
        });
        SoundManager.Instance.StopVoice(0.5f);
        SoundManager.Instance.StartSound("UI_GameStart2", 1.0f);
    }
}
