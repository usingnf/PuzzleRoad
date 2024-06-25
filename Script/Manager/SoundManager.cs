using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 사운드(BGM, Sound Effect, Voice)를 관리.
/// 볼륨 조절 기능.
/// </summary>

public class SoundManager : Singleton<SoundManager>
{
    [Header("Insepctor")]
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private AudioSource audio_music;
    [SerializeField] private GameObject prefab_3Dsound;
    [SerializeField] private AudioSource audio_voice;

    [Header("Status")]
    private Dictionary<string, AudioClip> dic_clips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> dic_voiceClips = new Dictionary<string, AudioClip>();
    private float voice_volume = 1.0f;
    private float music_volume = 1.0f;
    private float sound_volume = 1.0f;
    private bool isMusic = false;
    private float lastMusicVolume = 0;
    private bool isMusicCalm = false;

    private UnityAction<float> sound_func;

    private List<AudioSource> listAudio = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
        if (Instance != this)
            return;
        DontDestroyOnLoad(this.gameObject);

        //모든 Audio 데이터를 Inspector에 등록.
        foreach (AudioClip clip in clips)
            dic_clips.Add(clip.name, clip);
        foreach (AudioClip clip in voiceClips)
            dic_voiceClips.Add(clip.name, clip);
    }
    // Start is called before the first frame update
    private void Start()
    {
        music_volume = PlayerPrefs.GetInt("music_volume", 50) * 0.01f;
        sound_volume = PlayerPrefs.GetInt("sound_volume", 50) * 0.01f;
        voice_volume = PlayerPrefs.GetInt("voice_volume", 50) * 0.01f;
        audio_music.volume = music_volume * 0.01f;
        lastMusicVolume = 1.0f;
    }

    public void SetMusicVolume(int volume)
    {
        if (volume > 100)
            volume = 100;
        if (volume < 0)
            volume = 0;
        //audio_music.volume = volume;
        this.music_volume = volume * 0.01f;
        RefreshMusicVolume();
        PlayerPrefs.SetInt("music_volume", volume);
    }

    public float GetMusicVolume()
    {
        return this.music_volume;
    }
    public int GetMusicVolumeOption()
    {
        return PlayerPrefs.GetInt("music_volume", (int)(this.music_volume * 100));
    }

    public float SetVoiceVolume(int volume)
    {
        if (volume > 100)
            volume = 100;
        if(volume < 0) 
            volume = 0;
        this.voice_volume = volume * 0.01f;
        PlayerPrefs.SetInt("voice_volume", volume);
        audio_voice.volume = volume * 0.01f;
        return volume;
    }

    public float GetVoiceVolume()
    {
        return this.voice_volume;
    }
    public int GetVoiceVolumeOption()
    {
        return PlayerPrefs.GetInt("voice_volume", (int)(this.voice_volume * 100));
    }

    public void SetSoundVolume(int volume)
    {
        if (volume > 100)
            volume = 100;
        if (volume < 0)
            volume = 0;
        this.sound_volume = volume * 0.01f;
        PlayerPrefs.SetInt("sound_volume", volume);
        if (sound_func != null)
            sound_func.Invoke(volume*0.01f);
    }
    public float GetSoundVolume()
    {
        return this.sound_volume;
    }
    public int GetSoundVolumeOption()
    {
        return PlayerPrefs.GetInt("sound_volume", (int)(this.sound_volume*100));
    }

    public void AddSoundFunc(UnityAction<float> func)
    {
        sound_func += func;
    }

    public void RemoveSoundFunc(UnityAction<float> func)
    {
        sound_func -= func;
    }

    public AudioSource GetAudioMusic()
    {
        return audio_music;
    }

    public void StartMusic(string musicName, float volume, bool isLoop = true, float fadeTime = 0.0f)
    {
        if (!dic_clips.ContainsKey(musicName))
            return;

        lastMusicVolume = volume;
        isMusic = true;
        this.audio_music.loop = isLoop;
        this.audio_music.clip = dic_clips[musicName];
        if (fadeTime > 0.0f)
            RefreshMusicVolume(fadeTime);
        else
            RefreshMusicVolume();
        this.audio_music.Play();
        saveMusicName = "";
        if (coInterupt != null)
            StopCoroutine(coInterupt);
    }

    private string interuptMusicName = "";
    private string saveMusicName = "";
    private float saveVolume = 0;
    private bool saveLoop = false;
    public void StartMusicInterupt(string musicName, float volume, float fadeTime = 0.0f)
    {
        if (!dic_clips.ContainsKey(musicName))
            return;

        if(this.audio_music.isPlaying && string.IsNullOrEmpty(saveMusicName))
        {
            //saveVolume = this.audio_music.volume / music_volume;
            saveVolume = lastMusicVolume;
            saveLoop = audio_music.loop;
            saveMusicName = audio_music.clip.name;
        }

        lastMusicVolume = volume;
        isMusic = true;

        this.audio_music.loop = false;
        this.audio_music.clip = dic_clips[musicName];
        if (fadeTime > 0.0f)
            RefreshMusicVolume(fadeTime);
        else
            RefreshMusicVolume();
        this.audio_music.Play();

        if(coInterupt != null)
            StopCoroutine(coInterupt);
        if(!string.IsNullOrEmpty(saveMusicName))
        {
            coInterupt = CoInterupt(musicName, this.audio_music.time);
            StartCoroutine(coInterupt);
        }
    }

    private IEnumerator coInterupt = null;
    private IEnumerator CoInterupt(string musicName, float time)
    {
        yield return new WaitForSeconds(time);
        if (this.audio_music.name == musicName)
            StartMusic(saveMusicName, saveVolume, saveLoop);
    }

    public void StopMusic(float fadeTime = 0)
    {
        isMusic = false;
        if (fadeTime <= 0)
            this.audio_music.Stop();
        else
            this.audio_music.DOFade(0, fadeTime);
    }

    public void StopMusicInterupt()
    {
        if (coInterupt != null)
            StopCoroutine(coInterupt);
        isMusic = false;
        this.audio_music.Stop();
        if(!string.IsNullOrEmpty(saveMusicName))
            StartMusic(saveMusicName, saveVolume, saveLoop);
    }

    public void StartMusicCalm()
    {
        if (isMusicCalm)
            return;
        isMusicCalm = true;
        //this.music_volume = lastMusicVolume * music_volume * 0.5f;
        RefreshMusicVolume();
    }

    public void StopMusicCalm()
    {
        if (!isMusicCalm)
            return;
        isMusicCalm = false;
        //this.music_volume = lastMusicVolume * music_volume;
        RefreshMusicVolume();
    }

    private void RefreshMusicVolume(float fadeTime = 0.0f)
    {
        float volume = lastMusicVolume * music_volume;
        if (isMusicCalm)
            volume = volume * 0.5f;
        if (!isMusic)
            volume = 0;
        
        if(fadeTime <= 0)
            this.audio_music.volume = volume;
        else
            this.audio_music.DOFade(volume, fadeTime);
    }
    public AudioSource StartSound(string soundName, float volume, float trimTime = 0.0f)
    {
        if (!dic_clips.ContainsKey(soundName))
            return null;
        
        AudioSource resultAudio = null;
        foreach (AudioSource audio in listAudio)
        {
            if (audio.gameObject.activeSelf)
                continue;
            resultAudio = audio;
        }

        if (resultAudio == null)
        {
            resultAudio = Instantiate(prefab_3Dsound, this.transform).GetComponent<AudioSource>();
            //DontDestroyOnLoad(resultAudio);
            listAudio.Add(resultAudio);
        }
        resultAudio.loop = false;
        resultAudio.transform.position = Camera.main.transform.position;
        resultAudio.volume = volume * sound_volume;
        resultAudio.pitch = 1;
        resultAudio.spatialBlend = 0;
        resultAudio.clip = dic_clips[soundName];
        resultAudio.gameObject.SetActive(true);
        resultAudio.time = trimTime;
        resultAudio.Play();
        StartCoroutine(CoDisable(resultAudio.gameObject, resultAudio.clip.length));
        return resultAudio;
    }

    public void DelayStartSound(string soundName, float volume, float delayTime, float trimTime = 0.0f)
    {
        StartCoroutine(CoDelayStartSound(soundName, volume, delayTime, trimTime));
    }

    public IEnumerator CoDelayStartSound(string soundName, float volume, float delayTime, float trimTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        if (!dic_clips.ContainsKey(soundName))
            yield break;

        AudioSource resultAudio = null;
        foreach (AudioSource audio in listAudio)
        {
            if (audio.gameObject.activeSelf)
                continue;
            resultAudio = audio;
        }

        if (resultAudio == null)
        {
            resultAudio = Instantiate(prefab_3Dsound, this.transform).GetComponent<AudioSource>();
            //DontDestroyOnLoad(resultAudio);
            listAudio.Add(resultAudio);
        }
        resultAudio.loop = false;
        resultAudio.transform.position = Camera.main.transform.position;
        resultAudio.volume = volume * sound_volume;
        resultAudio.pitch = 1;
        resultAudio.spatialBlend = 0;
        resultAudio.clip = dic_clips[soundName];
        resultAudio.gameObject.SetActive(true);
        resultAudio.time = trimTime;
        resultAudio.Play();
        StartCoroutine(CoDisable(resultAudio.gameObject, resultAudio.clip.length));
    }

    public AudioSource StartSoundFadeLoop(string soundName, float volume, float startFadeTime, float endFadeTime, float loopTime, float trimTime = 0.0f)
    {
        if (!dic_clips.ContainsKey(soundName))
            return null;

        AudioSource resultAudio = null;
        foreach (AudioSource audio in listAudio)
        {
            if (audio.gameObject.activeSelf)
                continue;
            resultAudio = audio;
        }

        if (resultAudio == null)
        {
            resultAudio = Instantiate(prefab_3Dsound, this.transform).GetComponent<AudioSource>();
            //DontDestroyOnLoad(resultAudio);
            listAudio.Add(resultAudio);
        }
        resultAudio.loop = true;
        resultAudio.transform.position = Camera.main.transform.position;
        resultAudio.volume = 0;
        resultAudio.pitch = 1;
        resultAudio.DOFade(volume * sound_volume, startFadeTime).OnComplete(() =>
        {
            StartCoroutine(CoStartAudioFadeLoop(startFadeTime, endFadeTime, loopTime, resultAudio));
            //if(loopTime > 0.0f)
            //    await Task.Delay((int)((loopTime * 1000) - ((startFadeTime + endFadeTime) * 1000)));
            //resultAudio.DOFade(0, endFadeTime).OnComplete(() =>
            //{
            //    resultAudio.gameObject.SetActive(false);
            //}).SetEase(Ease.OutCubic);
        }).SetEase(Ease.InCubic);
        resultAudio.spatialBlend = 0;
        resultAudio.clip = dic_clips[soundName];
        resultAudio.gameObject.SetActive(true);
        resultAudio.time = trimTime;
        resultAudio.Play();
        return resultAudio;
    }

    private IEnumerator CoStartAudioFadeLoop(float startFadeTime, float endFadeTime, float loopTime, AudioSource resultAudio)
    {
        if (loopTime > 0.0f)
            yield return new WaitForSeconds(loopTime - (startFadeTime + endFadeTime));
            //await Task.Delay((int)((loopTime * 1000) - ((startFadeTime + endFadeTime) * 1000)));
        resultAudio.DOFade(0, endFadeTime).OnComplete(() =>
        {
            resultAudio.gameObject.SetActive(false);
        }).SetEase(Ease.OutCubic);
    }

    public void SetPitch(AudioSource audio, float pitch, float time)
    {
        if(time > 0)
            audio.DOPitch(pitch, time).SetEase(Ease.InCubic);
        else
            audio.pitch = pitch;
    }

    public void StopSound(AudioSource audio, float fadeTime = 0.0f)
    {
        if (audio == null)
            return;
        if (fadeTime > 0.0f)
            audio.DOFade(0.0f, fadeTime).OnComplete(() =>
            {
                audio.gameObject.SetActive(false);
            });
        else
        {
            audio.Stop();
            audio.gameObject.SetActive(false);
        }
    }

    public void Start3DSound(string soundName, float volume, Vector3 pos)
    {
        if (!dic_clips.ContainsKey(soundName))
            return;
        AudioSource resultAudio = null;
        foreach (AudioSource audio in listAudio)
        {
            if (audio.gameObject.activeSelf)
                continue;
            resultAudio = audio;
        }
        if (resultAudio == null)
        {
            resultAudio = Instantiate(prefab_3Dsound).GetComponent<AudioSource>();
            //DontDestroyOnLoad(resultAudio);
            listAudio.Add(resultAudio);
        }
        resultAudio.loop = false;
        resultAudio.volume = volume * sound_volume;
        resultAudio.pitch = 1;
        resultAudio.spatialBlend = 1;
        resultAudio.clip = dic_clips[soundName];
        resultAudio.transform.position = pos;
        resultAudio.gameObject.SetActive(true);
        resultAudio.Play();
        StartCoroutine(CoDisable(resultAudio.gameObject, resultAudio.clip.length));
    }

    public AudioClip GetAudioClip(string soundName)
    {
        return dic_clips[soundName];
    }

    private IEnumerator CoDisable(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t + 0.1f);
        obj.SetActive(false);
    }

    public void StartVoice(string voiceName, float volume)
    {
        Language.LanguageEnum lang = Language.Instance.GetVoiceLang();
        voiceName = $"{lang}_{voiceName}";
        //if (lang == Language.LanguageEnum.Korean)
        //    voiceName = $"{lang}_{voiceName}";
        //else if(lang == Language.LanguageEnum.English)
        //    voiceName = $"EN_{voiceName}";
        if (!dic_voiceClips.ContainsKey(voiceName))
            return;
        this.audio_voice.clip = dic_voiceClips[voiceName];
        this.audio_voice.volume = volume * voice_volume;
        this.audio_voice.Play();
    }

    public void StopVoice(float fadeTime = 0.0f)
    {
        if(fadeTime > 0)
            this.audio_voice.DOFade(0, fadeTime);
        else
            this.audio_voice.Stop();
    }

    public AudioSource GetVoiceAudio()
    { 
        return this.audio_voice; 
    }

    public AudioClip GetVoiceAudioClip(string voiceName)
    {
        if (!dic_voiceClips.ContainsKey(voiceName))
            return null;
        return dic_voiceClips[voiceName];
    }
}
