using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private Dictionary<string, AudioClip> bgmClips;
    private Dictionary<string, AudioClip> sfxClips;
    private AudioSource bgmSource;
    private Dictionary<string, List<AudioSource>> sfxSources;

    [SerializeField] private float allVolume;
    private float tempAllVolume;
    [SerializeField] private float bgmVolume;
    private float tempBGMVolume;
    [SerializeField] private float sfxVolume;
    private float tempSFXVolume;
    [SerializeField] private GameObject popUp;
    public GameObject PopUp
    {
        get { return popUp; }
        set { popUp = value; }
    }

    [SerializeField] private Button exitButton;

    [SerializeField] private Slider allSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Toggle allToggle;
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (popUp.activeSelf) { popUp.SetActive(false); }
            else
            {
                PlaySFX("Pause");
                popUp.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlaySFX("Test");
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
        InitResources();
    }

    private void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSources = new Dictionary<string, List<AudioSource>>();

        exitButton.onClick.AddListener(ButtonClicked);

        allToggle.onValueChanged.AddListener(OnAllToggleValueChanged);
        bgmToggle.onValueChanged.AddListener(OnBGMToggleValueChanged);
        sfxToggle.onValueChanged.AddListener(OnSFXToggleValueChanged);

        allSlider.onValueChanged.AddListener(OnAllSliderValueChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);

        OnAllSliderValueChanged(allVolume);
        OnBGMSliderValueChanged(bgmVolume);
        OnSFXSliderValueChanged(sfxVolume);
    }
    private void InitResources()
    {
        bgmClips = new Dictionary<string, AudioClip>();
        sfxClips = new Dictionary<string, AudioClip>();

        // Load all BGM clips
        AudioClip[] bgmClipArray = Resources.LoadAll<AudioClip>("Sounds/BGM");
        foreach (AudioClip clip in bgmClipArray)
        {
            bgmClips[clip.name] = clip;
        }

        // Load all SFX clips
        AudioClip[] sfxClipArray = Resources.LoadAll<AudioClip>("Sounds/SFX");
        foreach (AudioClip clip in sfxClipArray)
        {
            sfxClips[clip.name] = clip;
        }
    }

    // BGM 재생
    public void PlayBGM(string clipName)
    {
        // Play the new BGM
        if (bgmClips.ContainsKey(clipName))
        {
            bgmSource.clip = bgmClips[clipName];
            bgmSource.volume = bgmVolume * allVolume;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogError("BGM clip not found: " + clipName);
        }
    }

    public void PlaySFX(string clipName)
    {

        if (sfxClips.ContainsKey(clipName))
        {
            AudioSource audioSource = GetAvailableAudioSource(clipName);

            audioSource.clip = sfxClips[clipName];
            audioSource.volume = sfxVolume * allVolume;
            audioSource.Play();

            StartCoroutine(ReturnToPool(clipName, audioSource));
        }
        else
        {
            Debug.LogError("SFX clip not found: " + clipName);
        }
    }

    private AudioSource GetAvailableAudioSource(string clipName)
    {
        // Initialize list if not present
        if (!sfxSources.ContainsKey(clipName))
        {
            sfxSources[clipName] = new List<AudioSource>();
        }

        // Find a source not playing
        foreach (var audioSource in sfxSources[clipName])
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        // If all are playing, create a new one
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        sfxSources[clipName].Add(newAudioSource);
        return newAudioSource;
    }

    private IEnumerator ReturnToPool(string clipName, AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);

        source.Stop();
        source.clip = null;
    }

    // 모든 사운드 스탑
    public void StopAll()
    {
        BGMStop();
        SFXStop();
    }

    public void BGMStop()
    {
        bgmSource.clip = null; // clear the BGM
    }

    public void SFXStop()
    {
        foreach (var pair in sfxSources)
        {
            foreach (var source in pair.Value)
            {
                source.Stop(); // stop the SFX
            }
        }
    }

    // 통합 볼륨 조절
    public void SetAllVolume(float volume)
    {
        if (volume > 0)
        {
            volume = Mathf.Clamp01(volume);
            allVolume = volume;
        }

        // BGM 볼륨 조절
        bgmSource.volume = Mathf.Clamp01(bgmVolume * volume);

        // SFX volume
        foreach (var pair in sfxSources)
        {
            foreach (var source in pair.Value)
            {
                source.volume = sfxVolume * volume;
            }
        }
    }

    // BGM 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        bgmVolume = volume;

        bgmSource.volume = Mathf.Clamp01(volume * allVolume);
    }

    // 모든 SFX 볼륨 조절
    public void SetAllSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        sfxVolume = volume;

        foreach (var kvp in sfxSources)
        {
            foreach (var source in kvp.Value)
            {
                source.volume = volume * allVolume;
            }
        }
    }

    void ButtonClicked()
    {
        popUp.SetActive(false);
    }

    void OnAllToggleValueChanged(bool isOn)
    {
        bgmToggle.isOn = isOn;
        sfxToggle.isOn = isOn;

        if (!isOn)
        {
            tempAllVolume = allVolume;
            OnAllSliderValueChanged(0);

            tempBGMVolume = bgmVolume;
            OnBGMSliderValueChanged(0);

            tempSFXVolume = sfxVolume;
            OnSFXSliderValueChanged(0);
        }
        else
        {
            allVolume = tempAllVolume;
            OnAllSliderValueChanged(allVolume);

            bgmVolume = tempBGMVolume;
            OnBGMSliderValueChanged(bgmVolume);

            sfxVolume = tempSFXVolume;
            OnSFXSliderValueChanged(sfxVolume);
        }
    }
    void OnBGMToggleValueChanged(bool isOn)
    {
        if (!isOn)
        {
            tempBGMVolume = bgmVolume;
            OnBGMSliderValueChanged(0);
        }
        else
        {
            bgmVolume = tempBGMVolume;
            OnBGMSliderValueChanged(bgmVolume);
        }
    }
    void OnSFXToggleValueChanged(bool isOn)
    {
        if (!isOn)
        {
            tempSFXVolume = sfxVolume;
            OnSFXSliderValueChanged(0);
        }
        else
        {
            sfxVolume = tempSFXVolume;
            OnSFXSliderValueChanged(sfxVolume);
        }
    }

    void OnAllSliderValueChanged(float value)
    {
        // Disable the toggle's onValueChanged event
        allToggle.onValueChanged.RemoveListener(OnAllToggleValueChanged);
        SetAllVolume(value);
        allSlider.value = Mathf.Clamp01(value);

        if (value != 0 && !allToggle.isOn)
        {
            allToggle.isOn = true;
        }
        else if (value == 0 && allToggle.isOn)
        {
            allToggle.isOn = false;
        }

        // Re-enable the toggle's onValueChanged event
        allToggle.onValueChanged.AddListener(OnAllToggleValueChanged);
    }
    void OnBGMSliderValueChanged(float value)
    {
        // Disable the toggle's onValueChanged event
        bgmToggle.onValueChanged.RemoveListener(OnBGMToggleValueChanged);

        SetBGMVolume(value);
        bgmSlider.value = Mathf.Clamp01(value);

        if (value != 0 && !bgmToggle.isOn)
        {
            bgmToggle.isOn = true;
        }
        else if (value == 0 && bgmToggle.isOn)
        {
            bgmToggle.isOn = false;
        }

        // Re-enable the toggle's onValueChanged event
        bgmToggle.onValueChanged.AddListener(OnBGMToggleValueChanged);
    }
    void OnSFXSliderValueChanged(float value)
    {
        // Disable the toggle's onValueChanged event
        sfxToggle.onValueChanged.RemoveListener(OnSFXToggleValueChanged);

        SetAllSFXVolume(value);
        sfxSlider.value = Mathf.Clamp01(value);

        if (value != 0 && !sfxToggle.isOn)
        {
            sfxToggle.isOn = true;
        }
        else if (value == 0 && sfxToggle.isOn)
        {
            sfxToggle.isOn = false;
        }

        // Re-enable the toggle's onValueChanged event
        sfxToggle.onValueChanged.AddListener(OnSFXToggleValueChanged);
    }
}
