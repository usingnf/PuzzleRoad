using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// 사용하지 않음.
namespace NoneUse
{
    public class Function : System.Attribute
    {
        public string funcName = "";
        public Function(string funcName)
        {
            this.funcName = funcName;
        }
    }
    public class DialogManager2 : Singleton<DialogManager2>
    {
        public enum DialogState
        {
            None,
            Progress,
            Choice,
        }

        [Header("Inspector")]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private GameObject textPanel;
        [SerializeField] private GameObject choicePanel;
        [SerializeField] private MyText text_name;
        [SerializeField] private MyText text_dialog;
        [SerializeField] private GameObject prefab_btnChoice;
        [SerializeField] private Image img_leftChr;
        [SerializeField] private Image img_centerChr;
        [SerializeField] private Image img_rightChr;
        [SerializeField] private SO_Dialog2 tempDialog;


        [Header("Status")]
        private Dictionary<string, MethodInfo> functions = new Dictionary<string, MethodInfo>();
        private SO_Dialog2 currentDialog;
        private int dialogIndex = 0;
        private DialogState dialogState = DialogState.None;
        private bool isTextProgress = false;

        void Start()
        {
            MethodInfo[] method = this.GetType().GetMethods();
            foreach (MethodInfo methodInfo in method)
            {
                Function func = methodInfo.GetCustomAttribute<Function>();
                if (func != null)
                {
                    functions.Add(func.funcName, methodInfo);
                }
            }
            //method.Invoke(this, null);
        }

        // Update is called once per frame
        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        StartDialog(tempDialog);
        //    }
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (dialogState == DialogState.Progress)
        //            NextDialog();
        //    }
        //}

        public void StartDialog(SO_Dialog2 dialog)
        {
            if (dialog == null)
                return;
            dialogIndex = -1;
            currentDialog = dialog;
            dialogState = DialogState.Progress;
            dialogPanel.SetActive(true);

            NextDialog();
        }

        public void NextDialog()
        {
            dialogIndex += 1;
            if (dialogIndex >= currentDialog.datas.Length)
            {
                if (currentDialog.choices.Length > 0)
                {
                    OpenChoices();
                }
                else
                {
                    EndDialog();
                }
            }
            else
            {
                SetText(currentDialog.datas[dialogIndex]);
            }
        }

        private void EndDialog()
        {
            text_name.text = "";
            text_name.color = Color.white;
            text_dialog.text = "";
            dialogState = DialogState.None;

            dialogPanel.SetActive(false);
            textPanel.SetActive(false);
            choicePanel.SetActive(false);
            ExcuteFunction(currentDialog.endFunc);
            currentDialog = null;
        }

        private void SetText(DialogData2 dialogData)
        {
            textPanel.SetActive(true);

            text_name.text = dialogData.talkChr.chrName;
            text_name.color = dialogData.nameColor;
            text_dialog.text = dialogData.text;

            SetChrSprite(img_leftChr, dialogData.leftChr);
            SetChrSprite(img_centerChr, dialogData.centerChr);
            SetChrSprite(img_rightChr, dialogData.rightChr);
        }

        private void SetChrSprite(Image img, DialogChrData data)
        {
            if (data.chr == null)
            {
                img.sprite = null;
                img.gameObject.SetActive(false);
                return;
            }
            img.gameObject.SetActive(true);
            img.sprite = data.chr.chrSprites[data.imgIndex];
            if (img.sprite.textureRect.y > 1080)
            {
                img.rectTransform.sizeDelta = new Vector2(img.sprite.textureRect.x / img.sprite.textureRect.y * 1080, 1080);
            }
            else
            {
                img.SetNativeSize();
            }
            if (data.isActive)
            {
                img.color = Color.white;
            }
            else
            {
                img.color = new Color(0.2f, 0.2f, 0.2f);
            }

        }

        private void OpenChoices()
        {
            dialogState = DialogState.Choice;
            foreach (Transform trans in choicePanel.transform)
            {
                Destroy(trans.gameObject);
            }
            choicePanel.SetActive(true);
            for (int i = 0; i < currentDialog.choices.Length; i++)
            {
                GameObject obj = Instantiate(prefab_btnChoice, choicePanel.transform);
                int index = i;
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SelectChoice(currentDialog.choices[index].func);
                });
                obj.GetComponent<MyText>().SetText(currentDialog.choices[i].text);
            }
        }

        private void SelectChoice(string str)
        {
            ExcuteFunction(str);
            EndDialog();
        }

        public void ExcuteFunction(string func)
        {
            if (!functions.ContainsKey(func))
                return;
            if (string.IsNullOrEmpty(func))
                return;
            functions[func].Invoke(this, null);
        }

        [Function("Test")]
        public void Test()
        {
            Debug.Log("@Test");
        }

        [Function("Test1")]
        public void Test1()
        {
            Debug.Log("@Test1");
        }

        [Function("Test2")]
        public void Test2()
        {
            Debug.Log("@Test2");
        }
    }
}

