using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Stage : MonoBehaviour
{
    [SerializeField] private UI_Title title;
    [SerializeField] private Image[] images_btn;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private Image image_stage;
    [SerializeField] private Sprite[] sprites_stage;
    [SerializeField] private Button btn_close;
    [SerializeField] private Button btn_start;

    [SerializeField] private string[] string_scene;

    private int lastIndex = -1;
    private bool isReady = true;

    void Start()
    {
        btn_start.onClick.AddListener(() =>
        {
            Btn_Start();
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });

        btn_close.onClick.AddListener(() =>
        {
            Btn_Close();
            SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
        });
    }

    public void Click(int index)
    {
        if(lastIndex != -1)
            images_btn[lastIndex].color = offColor;
        lastIndex = index;
        images_btn[index].color = onColor;
        image_stage.sprite = sprites_stage[index];
        SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
    }

    
    public void Btn_Start()
    {
        if (!isReady)
            return;
        if (lastIndex < 0)
            return;
        isReady = false;
        title.SetIsReady(false);
        UIManager.Instance.SetIsPrologue(false);
        UIManager.Instance.FadeIn(1.0f, Color.black, () =>
        {
            SceneManager.LoadScene(string_scene[lastIndex]);
            UIManager.Instance.SetUIState(UIManager.UIState.Ingame);
            UIManager.Instance.FadeStop();
            isReady = true;
            title.SetIsReady(true);
        });
        this.gameObject.SetActive(false);
        SoundManager.Instance.StartSound("UI_GameStart", 1.0f);
        SoundManager.Instance.StopMusic(0.5f);
    }

    public void Btn_Close()
    {
        this.gameObject.SetActive(false);
    }
}
