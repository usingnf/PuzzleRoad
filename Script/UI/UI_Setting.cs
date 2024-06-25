using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Language;

public class UI_Setting : MonoBehaviour
{
    [SerializeField] private Button btn_close;
    [SerializeField] private Button[] btn_displayMode;
    [SerializeField] private Button[] btn_windowSize;
    [SerializeField] private Button[] btn_language;
    [SerializeField] private Button[] btn_voiceLanguage;
    [SerializeField] private Button[] btn_music;
    [SerializeField] private Button[] btn_se;
    [SerializeField] private Button[] btn_voice;

    [SerializeField] private Button btn_keyMove;
    [SerializeField] private Button btn_keyStop;
    [SerializeField] private Button btn_keyInteract;
    [SerializeField] private Button btn_keyLight;
    [SerializeField] private Button btn_keyTurnZ;
    [SerializeField] private Button btn_keyTurnC;
    [SerializeField] private Button btn_keyWalk;

    [SerializeField] private MyText[] texts;

    [SerializeField] private GameObject panel_ChangeKey;
    [SerializeField] private RectTransform panel_title;

    private DisplayMode displayMode;
    private WindowSize windowSize;
    private LanguageEnum language;
    private LanguageEnum voiceLanguage;

    public enum DisplayMode
    {
        WindowMode = 0,
        FullScreen = 1,
    }

    public enum WindowSize
    {
        _2560x1440 = 0,
        _1920x1080 = 1,
        _1600x900 = 2,
        _1280x720 = 3,
        End = 4,
    }

    private void OnEnable()
    {
        if(Screen.fullScreen)
        {
            texts[0].SetText("UI_Setting_FullScreen", true);
            displayMode = DisplayMode.FullScreen;
        }
        else
        {
            texts[0].SetText("UI_Setting_Window", true);
            displayMode= DisplayMode.WindowMode;
        }

        string wSize = $"{Screen.currentResolution.width}x{Screen.currentResolution.height}";
        if(!Screen.fullScreen)
            wSize = $"{Screen.width}x{Screen.height}";
        texts[1].SetText(wSize);
        wSize = $"_{wSize}";
        if (wSize == WindowSize._2560x1440.ToString())
            windowSize = WindowSize._2560x1440;
        else if (wSize == WindowSize._1920x1080.ToString())
            windowSize = WindowSize._1920x1080;
        else if(wSize == WindowSize._1600x900.ToString())
            windowSize = WindowSize._1600x900;
        else if(wSize== WindowSize._1280x720.ToString())
            windowSize = WindowSize._1280x720;

        texts[2].SetText($"UI_Setting_{Language.Instance.GetCurrentLang()}", true);
        language = Language.Instance.GetCurrentLang();
        texts[3].SetText($"UI_Setting_{Language.Instance.GetVoiceLang()}", true);
        voiceLanguage = Language.Instance.GetVoiceLang();

        texts[4].SetText(SoundManager.Instance.GetMusicVolumeOption().ToString(), false);
        texts[5].SetText(SoundManager.Instance.GetSoundVolumeOption().ToString(), false);
        texts[6].SetText(SoundManager.Instance.GetVoiceVolumeOption().ToString(), false);
        texts[7].SetText(MyKey.Move.ToString(), false);
        texts[8].SetText(MyKey.Stop.ToString(), false);
        texts[9].SetText(MyKey.Interact.ToString(), false);
        texts[10].SetText(MyKey.Light.ToString(), false);
        texts[11].SetText(MyKey.CameraZ.ToString(), false);
        texts[12].SetText(MyKey.CameraC.ToString(), false);
        texts[13].SetText(MyKey.Walk.ToString(), false);


        //if (strs.Length < 2)
        //    return;
        //Screen.SetResolution(int.Parse(strs[0]), int.Parse(strs[1]), Screen.fullScreen);
        //if (!Screen.fullScreen)
        //{
        //    StartCoroutine(CoResize());
        //}
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !b);
    }

    void Start()
    {
        if(btn_close != null)
            btn_close.onClick.AddListener(Btn_Close);

        btn_displayMode[0].onClick.AddListener(() =>
        {
            if (displayMode == DisplayMode.FullScreen)
            {
                displayMode = DisplayMode.WindowMode;
                StartCoroutine(CoResize());
                //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
                texts[0].SetText("UI_Setting_Window", true);
            }
            else if(displayMode == DisplayMode.WindowMode)
            {
                displayMode = DisplayMode.FullScreen;
                StartCoroutine(CoResize());
                //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                texts[0].SetText("UI_Setting_FullScreen", true);
            }
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_displayMode[1].onClick.AddListener(() =>
        {
            if (displayMode == DisplayMode.FullScreen)
            {
                displayMode = DisplayMode.WindowMode;
                StartCoroutine(CoResize());
                //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
                texts[0].SetText("UI_Setting_Window", true);
            }
            else if (displayMode == DisplayMode.WindowMode)
            {
                displayMode = DisplayMode.FullScreen;
                StartCoroutine(CoResize());
                //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                texts[0].SetText("UI_Setting_FullScreen", true);
            }
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });

        btn_windowSize[0].onClick.AddListener(() =>
        {
            if ((int)windowSize == 0)
                windowSize = WindowSize.End;
            windowSize += -1;
            string[] size = windowSize.ToString().Replace("_", "").Split("x");
            Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), displayMode == DisplayMode.FullScreen);
            string wSize = $"{size[0]}x{size[1]}";
            texts[1].SetText(wSize);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_windowSize[1].onClick.AddListener(() =>
        {
            windowSize += 1;
            if (windowSize == WindowSize.End)
                windowSize = 0;
            string[] size = windowSize.ToString().Replace("_", "").Split("x");
            Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), displayMode == DisplayMode.FullScreen);
            string wSize = $"{size[0]}x{size[1]}";
            texts[1].SetText(wSize);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });

        btn_language[0].onClick.AddListener(() =>
        {
            if (language == 0)
                language = LanguageEnum.End;
            language += -1;
            Language.Instance.SetCurrentLang(language);
            texts[2].SetText($"UI_Setting_{Language.Instance.GetCurrentLang()}", true);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_language[1].onClick.AddListener(() =>
        {
            language += 1;
            if (language == LanguageEnum.End)
                language = 0;
            Language.Instance.SetCurrentLang(language);
            texts[2].SetText($"UI_Setting_{Language.Instance.GetCurrentLang()}", true);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_voiceLanguage[0].onClick.AddListener(() =>
        {
            if (voiceLanguage == 0)
                voiceLanguage = LanguageEnum.End;
            voiceLanguage += -1;
            Language.Instance.SetCurrentVoiceLang(voiceLanguage);
            texts[3].SetText($"UI_Setting_{Language.Instance.GetVoiceLang()}", true);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_voiceLanguage[1].onClick.AddListener(() =>
        {
            voiceLanguage += 1;
            if(voiceLanguage == LanguageEnum.End)
                voiceLanguage = 0;
            Language.Instance.SetCurrentVoiceLang(voiceLanguage);
            texts[3].SetText($"UI_Setting_{Language.Instance.GetVoiceLang()}", true);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });

        btn_music[0].onClick.AddListener(() =>
        {
            SoundManager.Instance.SetMusicVolume(SoundManager.Instance.GetMusicVolumeOption() - 5);
            texts[4].SetText((SoundManager.Instance.GetMusicVolumeOption()).ToString(), false);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_music[1].onClick.AddListener(() =>
        {
            SoundManager.Instance.SetMusicVolume(SoundManager.Instance.GetMusicVolumeOption() + 5);
            texts[4].SetText(SoundManager.Instance.GetMusicVolumeOption().ToString().ToString(), false);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });

        btn_se[0].onClick.AddListener(() =>
        {
            SoundManager.Instance.SetSoundVolume(SoundManager.Instance.GetSoundVolumeOption() - 5);
            texts[5].SetText(SoundManager.Instance.GetSoundVolumeOption().ToString(), false);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_se[1].onClick.AddListener(() =>
        {
            SoundManager.Instance.SetSoundVolume(SoundManager.Instance.GetSoundVolumeOption() + 5);
            texts[5].SetText(SoundManager.Instance.GetSoundVolumeOption().ToString(), false);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });

        btn_voice[0].onClick.AddListener(() =>
        {
            SoundManager.Instance.SetVoiceVolume(SoundManager.Instance.GetVoiceVolumeOption() - 5);
            texts[6].SetText(SoundManager.Instance.GetVoiceVolumeOption().ToString(), false);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });
        btn_voice[1].onClick.AddListener(() =>
        {
            SoundManager.Instance.SetVoiceVolume(SoundManager.Instance.GetVoiceVolumeOption() + 5);
            texts[6].SetText(SoundManager.Instance.GetVoiceVolumeOption().ToString(), false);
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        });

        btn_keyMove.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.Move);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });
        btn_keyStop.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.Stop);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });
        btn_keyInteract.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.Interact);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });
        btn_keyLight.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.Light);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });
        btn_keyTurnZ.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.CameraZ);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });
        btn_keyTurnC.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.CameraC);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });
        btn_keyWalk.onClick.AddListener(() =>
        {
            ChangeKeyReady(KeyType.Walk);
            SoundManager.Instance.StartSound("UI_Button_Click1", 1.0f);
        });

        LayoutRebuilder.ForceRebuildLayoutImmediate(panel_title);
    }

    private void Btn_Close()
    {
        this.gameObject.SetActive(false);
        SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
        //ui_ingame.Btn_CloseSetting();
    }

    private IEnumerator CoResize()
    {
        yield return null;
        string[] size = windowSize.ToString().Replace("_", "").Split("x");
        Screen.SetResolution(int.Parse(size[0]), int.Parse(size[1]), displayMode == DisplayMode.FullScreen);
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
    }

    private bool isKeyReady = false;
    private KeyType changeKeyType;
    private void ChangeKeyReady(KeyType type)
    {
        isKeyReady = true;
        changeKeyType = type;
        panel_ChangeKey.SetActive(true);
    }

    private void Update()
    {
        if(isKeyReady)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(MyKey.Escape))
                {
                    CancelChangeKey();
                    return;
                }
                for (int i = 0; i < 330; i++)
                {
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        SetKey(changeKeyType, (KeyCode)i);
                    }
                }
            }
        }
    }

    private void SetKey(KeyType type, KeyCode key)
    {
        if (type == KeyType.Move)
        {
            MyKey.Move = key;
            texts[7].SetText(MyKey.Move.ToString(), false);
        }
        else if(type == KeyType.Stop)
        {
            MyKey.Stop = key;
            texts[8].SetText(MyKey.Stop.ToString(), false);
        }
        else if (type == KeyType.Interact)
        {
            MyKey.Interact = key;
            texts[9].SetText(MyKey.Interact.ToString(), false);
        }
        else if (type == KeyType.Light)
        {
            MyKey.Light = key;
            texts[10].SetText(MyKey.Light.ToString(), false);
        }
        else if (type == KeyType.CameraZ)
        {
            MyKey.CameraZ = key;
            texts[11].SetText(MyKey.CameraZ.ToString(), false);
        }
        else if (type == KeyType.CameraC)
        {
            MyKey.CameraC = key;
            texts[12].SetText(MyKey.CameraC.ToString(), false);
        }
        else if (type == KeyType.Walk)
        {
            MyKey.CameraC = key;
            texts[13].SetText(MyKey.Walk.ToString(), false);
        }
        DataManager.Instance.SetKey(type, key);

        isKeyReady = false;
        panel_ChangeKey.SetActive(false);
        SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
    }

    private void CancelChangeKey()
    {
        isKeyReady = false;
        panel_ChangeKey.SetActive(false);
    }
}
