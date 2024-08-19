using UnityEngine;
using System.Linq;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public enum EBgm 
{ 
    Lobby,
    Main 
}

public enum EUISfx 
{ 
    OnBtnClick,
    OnBtnFail,
    GameOver,
    GameClear,
}

public enum EInGameSfx
{
    BaseAttack,
    UnitActiveSkill____,
    DarkSpear,
    FireFlameThrower,
    GroundSpear,
    IceAge,
    WaterTornado,
    UnitPassiveSkill____,
    GroundStun,
    IceStrike,
    FireExplosion,
    WaterSlide,
    DarkStrike,
    BossSkill____,
    SplitSkill,
    ToughnessSkill,
    BlockUnitSpawnSkill,
    FreezeUnitSkill,
    InvincibleSkill, 
    MeteorSkill, 
    ShuffleUnitSkill,
    SpawnEnemySkill, 
    TeleportSkill,
    Other_____,
    OnClickIce
}

public enum EAudioMixerType 
{ 
    Master, 
    Bgm, 
    SfxUI,
    SfxInGame
}

[Serializable]
public class AudioInfo<T> where T : Enum
{
    public T EAudioType;
    public AudioClip AudioClip;
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("# Audio Mixer ")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmAudioMixer;
    [SerializeField] private AudioMixerGroup sfxUIAudioMixer;
    [SerializeField] private AudioMixerGroup sfxInGameAudioMixer;

    [Header("# Bgm Info")]
    [SerializeField] private AudioInfo<EBgm>[] bgmClips;
    [SerializeField] private float bgmVolume;
    private AudioSource bgmPlayer;
    private Dictionary<int, AudioClip> bgmDictionary = new Dictionary<int, AudioClip>();

    [Header("# SFX UI Info")]
    [SerializeField] private AudioInfo<EUISfx>[] sfxUIClips;
    [SerializeField] private float sfxUIVolume;
    private AudioSource[] sfxUIPlayers;
    private Dictionary<int, AudioClip> sfxUIDictionary = new Dictionary<int, AudioClip>();

    [Header("# SFX InGame Info")]
    [SerializeField] private AudioInfo<EInGameSfx>[] sfxInGameClips;
    [SerializeField] private float sfxInGameVolume;
    private AudioSource[] sfxInGamePlayers;
    private Dictionary<int, AudioClip> sfxInGameDictionary = new Dictionary<int, AudioClip>();

    [Header("# Sound Info")]
    [SerializeField] private int channels; // 많은 효과음을 내기 위한 채널 시스템
    [SerializeField] [Range(0f, 1f)] private float soundEffectPitchVariance; // 피치가 높아지면 높은 소리가 남

    private int channelIndex; // 채널 갯수 만큼 순회하도록 맨 마지막에 플레이 했던 SFX의 인덱스번호를 저장하는 변수

    private bool[] isMute = new bool[4];
    private float[] audioVolumes = new float[4];

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    #region 초기화
    private void Init()
    {
        // BgmPlayer 초기화
        InitBgmPlayer();
        InitDicionary<EBgm>(bgmClips, bgmDictionary);

        // SFXUIPlayer 초기화
        sfxUIPlayers = InitSfxPlayer("SFXUIPlayer", sfxUIVolume, sfxUIAudioMixer);
        InitDicionary<EUISfx>(sfxUIClips, sfxUIDictionary);

        // SFXInGamePlayer 초기화
        sfxInGamePlayers = InitSfxPlayer("SFXInGamePlayer", sfxInGameVolume, sfxInGameAudioMixer);
        InitDicionary<EInGameSfx>(sfxInGameClips, sfxInGameDictionary);

    }

    private void InitBgmPlayer()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true; // 반복 true
        bgmPlayer.volume = bgmVolume; // 볼륨 
        bgmPlayer.outputAudioMixerGroup = bgmAudioMixer;
    }

    private AudioSource[] InitSfxPlayer(string objName, float volume, AudioMixerGroup sfxAudioMixer)
    {
        GameObject obj = new GameObject(objName);
        obj.transform.parent = transform;
        // Player 초기화
        AudioSource[] players = new AudioSource[channels];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = obj.AddComponent<AudioSource>();
            players[i].playOnAwake = false;
            players[i].volume = volume;
            players[i].bypassListenerEffects = true; // 하이패스에 안 걸리게 함
            players[i].outputAudioMixerGroup = sfxAudioMixer;
        }
        return players;
    }

    private void InitDicionary<T> (AudioInfo<T>[] audioClips, Dictionary<int, AudioClip> dictionary) where T : Enum
    {
        foreach (var clips in audioClips)
        {
            int key = Convert.ToInt32(clips.EAudioType);
            dictionary[key] = clips.AudioClip;
        }
    }
    #endregion

    #region Play
    public void PlayBgm(EBgm eBgm) // Bgm 플레이 함수
    {
        if (!bgmPlayer.loop) // BgmPlayer의 반복이 켜져있다면 false
        {
            bgmPlayer.loop = true;
        }
        bgmPlayer.clip = bgmDictionary[(int)eBgm];
        bgmPlayer.Play(); // Bgm 플레이
    }

    public void PlayUISfx(EUISfx eSfx)
    {
        // 효과음 중복 재생 방지
        AudioSource sfxPlayer = sfxUIPlayers.FirstOrDefault(audio => audio.clip == sfxUIDictionary[(int)eSfx] && audio.isPlaying);
        if(sfxPlayer != null )
            return;

        for (int i = 0; i < sfxUIPlayers.Length; i++) // 0번은 발자국 소리 채널이기 문에 1 ~ 15의 채널만 순회
        {
            // 예를들어 5번 인덱스를 마지막으로 사용했으면 6 7 8 9 10 1 2 3 4 5 이런식으로 순회하게 하기위한 계산임
            int loopIndex = (i + channelIndex) % sfxUIPlayers.Length;

            if (sfxUIPlayers[loopIndex].isPlaying) // 해당 채널이 Play 중이라면
            {
                continue;
            }

            int randomIndex = 0;

            channelIndex = loopIndex;
            sfxUIPlayers[loopIndex].clip = sfxUIDictionary[(int)eSfx + randomIndex];
            // UI는 TimeScale이 0이어도 소리 나도록 IgnoreListenerPause true로 설정
            sfxUIPlayers[loopIndex].ignoreListenerPause = true;
            SetRandomPitchToSfx(loopIndex, sfxUIPlayers);
            break; // 효과음이 빈 채널에서 재생 됐기 때문에 반드시 break로 반복문을 빠져나가야함
        }
    }

    public void PlayInGameSfx(EInGameSfx eSfx)
    {
        for (int i = 0; i < sfxInGamePlayers.Length; i++) 
        {
            int loopIndex = (i + channelIndex) % sfxInGamePlayers.Length;
            if (sfxInGamePlayers[loopIndex].isPlaying)
                continue;

            int randomIndex = 0;

            channelIndex = loopIndex;
            sfxInGamePlayers[loopIndex].clip = sfxInGameDictionary[(int)eSfx+randomIndex];
            SetRandomPitchToSfx(loopIndex, sfxInGamePlayers);
            break; 
        }
    }

    #endregion

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    private void SetRandomPitchToSfx(int loopIndex, AudioSource[] sfxPlayers)
    {
        sfxPlayers[loopIndex].pitch = 1f + UnityEngine.Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);
        sfxPlayers[loopIndex].Play();
    }

    public void SetAudioVolume(EAudioMixerType audioMixerType, float volume)
    {
        // 오디오 믹서의 값은 -80 ~ 0까지이기 때문에 0.0001 ~ 1의 Log10 * 20을 한다.
        // volume은 0.0001~1 값이 들어오도록 방어코드 작성
        if (volume < 0.0001f)
            volume = 0.0001f;
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20f);
    }

    public void SetAudioMute(EAudioMixerType audioMixerType)
    {
        int type = (int)audioMixerType;
        if (!isMute[type])
        {
            isMute[type] = true;
            audioMixer.GetFloat(audioMixerType.ToString(), out float curVolume);
            audioVolumes[type] = curVolume;
            SetAudioVolume(audioMixerType, 0.0001f);
        }
        else
        {
            isMute[type] = false;
            SetAudioVolume(audioMixerType, audioVolumes[type]);
        }
    }

    public float GetAudioVolume(EAudioMixerType audioMixerType)
    {
        // volumeDb는 -80~0 사이값 가짐
        audioMixer.GetFloat(audioMixerType.ToString(), out float volumeDb);
        // 10의 volumeDb/20 거듭제곱
        float linearVolume = Mathf.Pow(10, volumeDb / 20f);
        // linearVolume 값을 0과  1사이의 값으로 변한
        return Mathf.Clamp01(linearVolume);
    }

    public float GetAudioMixerVolume(EAudioMixerType audioMixerType)
    {
        audioMixer.GetFloat(audioMixerType.ToString(), out float curVolume);
        return curVolume;
    }

    public void SetAudioMixerVolume(SoundVolumeaveData volume)
    {
        SetAudioVolume(EAudioMixerType.Master, Mathf.Pow(10, volume.MasterVolume / 20f));
        SetAudioVolume(EAudioMixerType.Bgm, Mathf.Pow(10, volume.BgmVolume / 20f));
        SetAudioVolume(EAudioMixerType.SfxUI, Mathf.Pow(10, volume.SfxUIVolume / 20f));
        SetAudioVolume(EAudioMixerType.SfxInGame,  Mathf.Pow(10, volume.SfxInGameVolume / 20f));
    }
}