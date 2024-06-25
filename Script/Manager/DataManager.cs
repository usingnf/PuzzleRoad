using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 실행시 최초 실행하는 클래스.
/// 초기값을 설정.
/// </summary>

public class DataManager : Singleton<DataManager>
{
    private void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        //PlayerPrefs.DeleteAll();
        Application.targetFrameRate = 60;
        Instance = this;
        if (Instance != this)
            return;
        DontDestroyOnLoad(this.gameObject);
        MyKey.Move = (KeyCode)PlayerPrefs.GetInt(KeyType.Move.ToString(), (int)MyKey.Move);
        MyKey.Stop = (KeyCode)PlayerPrefs.GetInt(KeyType.Stop.ToString(), (int)MyKey.Stop);
        MyKey.Interact = (KeyCode)PlayerPrefs.GetInt(KeyType.Interact.ToString(), (int)MyKey.Interact);
        MyKey.Light = (KeyCode)PlayerPrefs.GetInt(KeyType.Light.ToString(), (int)MyKey.Light);
        MyKey.CameraZ = (KeyCode)PlayerPrefs.GetInt(KeyType.CameraZ.ToString(), (int)MyKey.CameraZ);
        MyKey.CameraC = (KeyCode)PlayerPrefs.GetInt(KeyType.CameraC.ToString(), (int)MyKey.CameraC);
        MyKey.Walk = (KeyCode)PlayerPrefs.GetInt(KeyType.Walk.ToString(), (int)MyKey.Walk);
        //PlayerPrefs.SetInt("TrueEnd", 1);
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Scene_Init")
            SceneManager.LoadScene("Scene_Title");
    }
    public void SetKey(KeyType type, KeyCode key)
    {
        PlayerPrefs.SetInt(type.ToString(), (int)key);
    }    
}
