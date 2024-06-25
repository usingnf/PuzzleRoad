using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario002 : Stage
{
    [SerializeField] private Transform[] holdTiles;
    [SerializeField] private Inter_HoldColorBlock[] defaultHoldObj;
    [SerializeField] private Inter_HoldColorBlock[] holdObj;
    [SerializeField] private Material[] tileMats;
    [SerializeField] private Door door;
    [SerializeField] private Inter_Gate gate;
    [SerializeField] private bool leftSupply = false;
    [SerializeField] private bool rightSupply = false;

    [SerializeField] private List<int> answerList = new List<int>();

    [SerializeField] private SO_Dialog[] dialogs;

    private bool isFirstAnswer = false;
    private bool isSuccess = false;

    protected void Start()
    {
        base.Start();
        holdObj = new Inter_HoldColorBlock[defaultHoldObj.Length];
        tileMats = new Material[holdTiles.Length];
        for(int i = 0; i < holdTiles.Length; i++)
        {
            tileMats[i] = holdTiles[i].GetComponent<MeshRenderer>().material;
        }
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
        answerList = new List<int>();
        for(int i = 0; i < 3; i++)
        {
            answerList.Add(i);
        }
        int temp = 0;
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, 3);
            temp = answerList[rand];
            answerList[rand] = answerList[i];
            answerList[i] = temp;
        }
        if (answerList[0] == 0 &&
            answerList[1] == 1 &&
            answerList[2] == 2)
        {
            answerList[0] = 1;
            answerList[1] = 0;
        }
        defaultHoldObj[0].SetColorValue(ColorBlock.Red);
        defaultHoldObj[1].SetColorValue(ColorBlock.Green);
        defaultHoldObj[2].SetColorValue(ColorBlock.Blue);

    }

    private void CheckAnswer()
    {
        bool isAnswer = true;
        for (int i = 0; i < 3; i++)
        {
            if (holdObj[i] == null)
            {
                isAnswer = false;
                tileMats[i].color = Color.gray;
                continue;
            }
            if (answerList[i] == 0)
            {
                if (holdObj[i].myColor != ColorBlock.Red)
                {
                    isAnswer = false;
                    tileMats[i].color = Color.red;
                }
                else
                {
                    tileMats[i].color = Color.green;
                }
            }
            else if (answerList[i] == 1)
            {
                if (holdObj[i].myColor != ColorBlock.Green)
                {
                    isAnswer = false;
                    tileMats[i].color = Color.red;
                }
                else
                {
                    tileMats[i].color = Color.green;
                }
            }
            else if (answerList[i] == 2)
            {
                if (holdObj[i].myColor != ColorBlock.Blue)
                {
                    isAnswer = false;
                    tileMats[i].color = Color.red;
                }
                else
                {
                    tileMats[i].color = Color.green;
                }
            }
        }

        door.Lock(!isAnswer);
        if (isAnswer)
        {
            if(!isFirstAnswer)
            {
                isFirstAnswer = true;
                //door.Open();
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Senario002_Excellent");
            }
            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
        }

    }

    public void LeftSupply(bool b)
    {
        leftSupply = b;
        //SoundManager.Instance.StartSound("UI_Success", 1.0f);
        CheckSupply(b);
    }
    public void RightSupply(bool b)
    {
        rightSupply = b;
        //SoundManager.Instance.StartSound("UI_Success", 1.0f);
        CheckSupply(b);
    }
    public void CheckSupply(bool b)
    {
        if (leftSupply && rightSupply)
            Event_Success();
        else if(b)
            SoundManager.Instance.StartSound("UI_Success", 1.0f);
    }

    //Use Inspector
    public void SetTile(Inter_HoldColorBlock holdObj)
    {
        Transform target = null;
        float max = 9999;
        float dis = 0;
        int index = 0;
        for (int i = 0; i < holdTiles.Length; i++)
        {
            dis = Vector3.Distance(holdTiles[i].position, holdObj.transform.position);
            if (dis <= 1.25f && dis < max && this.holdObj[i] == null)
            {
                max = dis;
                target = holdTiles[i];
                index = i;
            }
        }
        if (target != null)
        {
            holdObj.transform.position = new Vector3(target.position.x, holdObj.transform.position.y, target.position.z);
            this.holdObj[index] = holdObj;
        }
        CheckAnswer();
    }

    //Use Inspector
    public void HoldOutObj(Inter_HoldColorBlock obj)
    {
        for (int i = 0; i < holdObj.Length; i++)
        {
            if (obj == holdObj[i])
            {
                holdObj[i] = null;
                CheckAnswer();
                return;
            }
        }
    }

    public void Event_Shake()
    {
        hintIndex = 2;
        PlayerUnit.player.SetIsCanControl(false);
        StartCoroutine(CoEvent_Shake());
    }

    private IEnumerator CoEvent_Shake()
    {
        float shakeTime = 3.0f;
        CameraManager.Instance.Shake(shakeTime);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_Screen_Shake2", 0.6f, 0.5f, 0.5f, shakeTime);
        yield return new WaitForSeconds(shakeTime + 1);
        LightManager.Instance.SetLightState(LightManager.LightState.LittleDark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);
        yield return new WaitForSeconds(0.1f);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        yield return new WaitForSeconds(0.4f);
        LightManager.Instance.SetLightState(LightManager.LightState.LittleDark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);
        yield return new WaitForSeconds(0.1f);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        yield return new WaitForSeconds(0.4f);
        LightManager.Instance.SetLightState(LightManager.LightState.LittleDark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);
        yield return new WaitForSeconds(0.1f);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        yield return new WaitForSeconds(0.8f);
        LightManager.Instance.SetLightState(LightManager.LightState.Dark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);

        if (UIManager.Exist())
            UIManager.Toast(MyText.ConvertText(Language.Instance.Get("UI_Toast_LightTutorial"), MyKey.Light.ToString()));

        PlayerUnit.player.SetIsCanControl(true);
        yield return new WaitForSeconds(2.0f);
        if (DialogManager.Exist())
        {
            float t = DialogManager.Instance.ShowDialog("Dialog_Senario002_LightOff");
            //if(coMusic != null)
            //    StopCoroutine(coMusic);
            //coMusic = CoMusic(t + 1.0f);
            //StartCoroutine(coMusic);
        }
    }

    //private IEnumerator coMusic = null;
    //private IEnumerator CoMusic(float t)
    //{
    //    yield return new WaitForSeconds(t);
    //    SoundManager.Instance.StartMusic("Music_Mesmerize_60", 0.2f, true);
    //}

    
    private void Event_Success()
    {
        if (isSuccess)
            return;
        isSuccess = true;
        gate.SetCanInteract(true);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        PlayerUnit.player.Swt_Light(false);
        hintIndex = 3;
        StartCoroutine(CoEvent_Success());
        //if (coMusic != null)
        //    StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic();
    }

    private IEnumerator CoEvent_Success()
    {
        yield return new WaitForSeconds(1.5f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario002_LightOn");
    }


    public void Event_FirstEnter()
    {
        //DialogManager.Instance.ShowDialog(dialogs[0]);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario002_FirstEnter");
        hintIndex = 1;
    }

    public override void Hint()
    {
        if(hintIndex == 1)
            UIManager.Toast(Language.Instance.Get("Hint_1002_1"));
        else if(hintIndex == 2)
            UIManager.Toast(Language.Instance.Get("Hint_1002_2"));
        else
            base.Hint();
    }
}
