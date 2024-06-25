using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    [SerializeField] private BlackholeDebris[] debris;
    [SerializeField] private GameObject[] blackholes;
    [SerializeField] private float timeSpace = 1.0f;
    [SerializeField] private float startDelay = 5.0f;
    private GameObject[] obj_debris;
    private bool isOpen = false;
    private float lastTime = 0;

    private AudioSource audio_blackhole;

    // Start is called before the first frame update

    void Start()
    {
        obj_debris = new GameObject[debris.Length];
        for(int i = 0; i < debris.Length; i++)
        {
            obj_debris[i] = debris[i].gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen)
        {
            if (lastTime + timeSpace <= Time.time)
            {
                ShootDebris();
            }
        }
    }

    private void ShootDebris()
    {
        lastTime = Time.time;
        int index = -1;
        int rand = Random.Range(0, obj_debris.Length);
        for(int i = 0; i < obj_debris.Length; i++)
        {
            if (obj_debris[(i+rand)% obj_debris.Length].activeSelf)
                continue;
            index = (i + rand) % obj_debris.Length;
            break;
        }
        if(index >= 0)
        {
            debris[index].Shoot();
        }
    }

    public void Open()
    {
        isOpen = true;
        this.gameObject.SetActive(true);
        lastTime = Time.time + startDelay;
        blackholes[0].SetActive(true);
        blackholes[1].SetActive(false);
        blackholes[2].SetActive(false);
        SoundManager.Instance.StartSound("SE_Blackhole_Start", 1.0f);
        StartCoroutine(CoOpen());
    }

    private IEnumerator CoOpen()
    {
        yield return new WaitForSeconds(1.0f);
        audio_blackhole = SoundManager.Instance.StartSoundFadeLoop("SE_Blackhole_Loop", 1.0f, 1.0f, 0, 100000);
        yield return new WaitForSeconds(2.0f);
        blackholes[0].SetActive(false);
        blackholes[1].SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        blackholes[0].SetActive(false);
        blackholes[1].SetActive(false);
        blackholes[2].SetActive(true);
        StartCoroutine (CoClose());
    }

    private IEnumerator CoClose()
    {
        yield return new WaitForSeconds(1.0f);
        blackholes[2].SetActive(false);
        this.gameObject.SetActive(false);
        SoundManager.Instance.StopSound(audio_blackhole, 0.5f);
    }

    public void StopSound()
    {
        if(audio_blackhole != null)
            SoundManager.Instance.StopSound(audio_blackhole, 0.5f);
    }



}
