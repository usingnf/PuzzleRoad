using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senario008 : Stage
{
    [SerializeField] private SO_Dialog dialog_class;
    [SerializeField] private SO_Dialog dialog_class2;
    [SerializeField] private SO_Dialog dialog_class3;
    [SerializeField] private SO_Dialog dialog_art;
    [SerializeField] private SO_Dialog dialog_art2;
    [SerializeField] private SO_Dialog dialog_art3;
    [SerializeField] private SO_Dialog dialog_play;
    [SerializeField] private SO_Dialog dialog_play2;

    [SerializeField] private Door door_class;
    [SerializeField] private Door door_art;
    [SerializeField] private Door door_play;

    [SerializeField] private SpriteRenderer img_chr;
    private Material mat_chr;

    [SerializeField] private Sprite[] sprite_chr;
    protected void Start()
    {
        base.Start();
        mat_chr = img_chr.material;
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        //PlayerUnit.player.SetPos(startPos.position);
        SetQuestion();
    }
    public void SetQuestion()
    {

    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.T))
    //    {
    //        Event_Dialog_ClassRoom();
    //    }
    //}

    public void OpenDoor(Door door)
    {
        if (door == null)
            return;
        door.Lock(false);
        door.Open();
    }

    public void Event_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
    }

    public void Event_Dialog_ClassRoom()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Classroom1", null, Event_Choice_ClassRoom, ((int)(Time.time - GameManager.Instance.GetPlayStartTime())).ToString());
    }

    public void Event_Choice_ClassRoom()
    {
        Struct_Choice[] c = new Struct_Choice[4];
        c[0] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_1_1"), Event_Dialog_ClassRoom2, 0);
        c[1] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_1_2"), Event_Dialog_ClassRoom2, 1);
        c[2] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_1_3"), Event_Dialog_ClassRoom2, 2);
        c[3] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_1_4"), Event_Dialog_ClassRoom2, 3);
        DialogManager.Instance.ShowChoice(c);
        //DialogManager.Instance.ShowDialog(dialog_class, null, Event_Dialog_ClassRoom2);
    }


    public void Event_Dialog_ClassRoom2(int index)
    {
        StartCoroutine(Co_EventDialog_ClassRoom2());
    }

    private IEnumerator Co_EventDialog_ClassRoom2()
    {
        yield return new WaitForSeconds(2.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Classroom2", null, Event_Choice_ClassRoom2);
    }

    public void Event_Choice_ClassRoom2()
    {
        Struct_Choice[] c = new Struct_Choice[2];
        c[0] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_2_1"), Event_Dialog_ClassRoom3, 0);
        c[1] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_2_2"), Event_Dialog_ClassRoom3, 1);
        DialogManager.Instance.ShowChoice(c);
        //DialogManager.Instance.ShowDialog(dialog_class, null, Event_Dialog_ClassRoom2);
    }

    public void Event_Dialog_ClassRoom3(int index)
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Classroom3", null, Event_Dialog_ClassRoom4);
    }

    private void Event_Dialog_ClassRoom4()
    {
        StartCoroutine(Co_Event_Dialog_ClassRoom4());
    }

    private IEnumerator Co_Event_Dialog_ClassRoom4()
    {
        yield return new WaitForSeconds(0.5f);
        OpenDoor(door_class);
    }

    public void Event_Dialog_ArtRoom()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Artroom", null, Event_Choice_ArtRoom);
    }

    public void Event_Choice_ArtRoom()
    {
        Struct_Choice[] c = new Struct_Choice[3];
        c[0] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_3_1"), Event_Dialog_ArtRoom2, 0);
        c[1] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_3_2"), Event_Dialog_ArtRoom2, 1);
        c[2] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_3_3"), Event_Dialog_ArtRoom2, 2);
        DialogManager.Instance.ShowChoice(c);
        //DialogManager.Instance.ShowDialog(dialog_class, null, Event_Dialog_ClassRoom2);
    }
    public void Event_Dialog_ArtRoom2(int index)
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Artroom2", null, Event_Choice_ArtRoom2);
    }

    public void Event_Choice_ArtRoom2()
    {
        Struct_Choice[] c = new Struct_Choice[3];
        c[0] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_4_1"), Event_Dialog_ArtRoom3, 0);
        c[1] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_4_2"), Event_Dialog_ArtRoom3, 1);
        c[2] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_4_3"), Event_Dialog_ArtRoom3, 2);
        DialogManager.Instance.ShowChoice(c);
        //DialogManager.Instance.ShowDialog(dialog_class, null, Event_Dialog_ClassRoom2);
    }

    public void Event_Dialog_ArtRoom3(int index)
    {
        img_chr.gameObject.SetActive(true);
        img_chr.sprite = sprite_chr[index];
        mat_chr.SetFloat("_Intencity", 0);
        mat_chr.DOFloat(1.0f, "_Intencity", 5.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Artroom3", null, Event_Dialog_ArtRoom4);
    }


    public void Event_Dialog_ArtRoom4()
    {
        StartCoroutine(Co_Event_Dialog_ArtRoom4());
    }

    private IEnumerator Co_Event_Dialog_ArtRoom4()
    {
        yield return new WaitForSeconds(1.0f);
        OpenDoor(door_art);
    }

    public void Event_Dialog_PlayRoom()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Playroom", null, Event_Choice_PlayRoom);
    }

    public void Event_Choice_PlayRoom()
    {
        Struct_Choice[] c = new Struct_Choice[3];
        c[0] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_5_1"), Event_Dialog_PlayRoom2, 0);
        c[1] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_5_2"), Event_Dialog_PlayRoom2, 1);
        c[2] = new Struct_Choice(Language.Instance.Get("Narration_1008_Choice_5_3"), Event_Dialog_PlayRoom2, 2);
        DialogManager.Instance.ShowChoice(c);
        //DialogManager.Instance.ShowDialog(dialog_class, null, Event_Dialog_ClassRoom2);
    }

    public void Event_Dialog_PlayRoom2(int index)
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario008_Playroom2", null, Event_Dialog_PlayRoom3);
    }

    public void Event_Dialog_PlayRoom3()
    {
        StartCoroutine(Co_Event_Dialog_PlayRoom3());
    }

    private IEnumerator Co_Event_Dialog_PlayRoom3()
    {
        yield return new WaitForSeconds(1.0f);
        OpenDoor(door_play);
        StopMusic();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Sweet_Positive_Fun", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(1.0f);
    }
}
