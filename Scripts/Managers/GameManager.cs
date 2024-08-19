using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field:SerializeField] public PoolManager Pool { get; set; }
    [field:SerializeField] public UnitSpawnManager UnitSpawn { get; set; }
    [field:SerializeField] public StageManager Stage { get; set; }
    [field:SerializeField] public FieldManager FieldManager { get; set; }
    [field:SerializeField] public GameSystemManager System { get; set; }
    [field:SerializeField] public LoadSceneManager LoadScene { get; set; }
    [field:SerializeField] public PlayerSkillManager PlayerSkill { get; set; }
    [field:SerializeField] public CameraEventHandler CameraEvent { get; set; }
    [field:SerializeField] public TutorialManager Tutorial { get; set; }
    [field: SerializeField] public StartSceneManager StartScene { get; set; }
    [field:SerializeField] public TransitionController Transition { get; set; }
    [field:SerializeField] public MapChangeEffect MapChangeEffect { get; set; }

    public Transform MapTransform { get; set; }
    
    [Header("# TestMode")]
    public bool isMainSceneTest;
    public bool isStartSceneTest;

    private int targetWidth;
    private int targetHeight;
    private float targetAspect;

    protected override void Awake()
    {
        base.Awake();
        //Screen.SetResolution(1080,1920,true);
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
        if (isMainSceneTest)
        {
            Debug.Log("TestMode");
            // SceneManager.LoadScene(ESceneType.TitleLoadingScene.ToString(), LoadSceneMode.Additive);
        }
    }

#if !UNITY_ANDROID
    private void Start()
    {
        targetWidth = 1080;
        targetHeight = 1920;
        targetAspect = (float)targetWidth / targetHeight;
    }

    private void Update()
    {
        if (Screen.width != targetWidth || Screen.height != targetHeight)
        {
            int newWidth = Screen.width;
            int newHeight = Mathf.RoundToInt(newWidth / targetAspect);

            if (newHeight > Screen.height)
            {
                newHeight = Screen.height;
                newWidth = Mathf.RoundToInt(newHeight * targetAspect);
            }

            Screen.SetResolution(newWidth, newHeight, false);
        }
    }
#endif

    private void OnApplicationQuit()
    {
        AnalyticsManager.Instance.AnalyticsGameOff();   
    }

    public void GameQuit()
    {
        PlayerDataManager.Instance.SavePlayerData();
        Application.Quit();
    }
}
