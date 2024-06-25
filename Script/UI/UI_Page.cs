using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Page : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private Button btn_close;
    [SerializeField] private string title;
    [SerializeField] private List<string> contents = new List<string>();
    [SerializeField] private MyText[] myTexts;
    [SerializeField] private bool isId = false;

    [SerializeField] private Transform panel_content;
    [SerializeField] private MyText prefab_content;

    //private void OnEnable()
    //{
    //    SetText();
    //}
    private void Start()
    {
        btn_close.onClick.AddListener(Close);
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
        parent.SetActive(false);
        SoundManager.Instance.StartSound("UI_Button_Click2", 1.0f);
    }

    public void SetTitle(string title, bool isId = false)
    {
        this.title = title;
        myTexts[0].SetText(title, isId);
    }

    public void SetEnd(string end, bool isId = false, string addText = "")
    {
        myTexts[2].SetText(end, isId, addText);
    }

    public void SetEndColor(Color color)
    {
        myTexts[2].SetColor(color);
    }
    public void AddContent(string content, bool isId = false, string addText = "")
    {
        this.isId = isId;
        //contents.Add(content);
        //myTexts[1].SetText(content, isId);
        MyText myText = Instantiate(prefab_content, panel_content);
        myText.SetText(content, isId, addText);
    }

    public void Clear()
    {
        contents.Clear();
        foreach (Transform trans in panel_content)
        {
            Destroy(trans.gameObject);
        }
    }
    private void SetText()
    {
        string str = "";
        for(int i = 0; i < contents.Count; i++)
        {
            if(isId)
                str += Language.Instance.Get(contents[i]);
            else
                str += contents[i];
            str += "\n";
        }
        myTexts[1].SetText(str);
    }
}
