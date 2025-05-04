using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.EventSystems;

public class VideoManager : MonoBehaviour
{
    [System.Serializable]
    public class VideoData
    {
        public string videoName;
        public Sprite thumbnail;
        public VideoClip videoClip;
    }

    [Header("Video List Setup")]
    public List<VideoData> videoDataList;
    public GameObject videoItemPrefab;
    public Transform contentHolder;

    [Header("Video Player UI")]
    public GameObject videoPlayerPanel;
    public RawImage videoDisplay;
    public VideoPlayer videoPlayer;
    public Button closeButton;
    public Button pausePlayButton;
    public Button restartButton;
    public Slider progressBar;

    [Header("Control Sprites")]
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Sprite restartSprite;

    private bool isPaused = false;
    private bool isDraggingSlider = false;
    public TMP_Text timeText;
    string FormatTime(double time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    void Start()
    {
        PopulateVideoList();

        closeButton.onClick.AddListener(CloseVideo);
        pausePlayButton.onClick.AddListener(TogglePausePlay);
        restartButton.onClick.AddListener(RestartVideo);
        progressBar.onValueChanged.AddListener(OnSliderValueChanged);

        if (restartButton != null && restartSprite != null)
            restartButton.image.sprite = restartSprite;
    }

    void Update()
    {
        if (!isDraggingSlider && videoPlayer.clip != null && videoPlayer.length > 0f)
        {
            float progress = (float)(videoPlayer.time / videoPlayer.length);
            progressBar.value = progress;

            if (timeText != null)
            {
                timeText.text = $"{FormatTime(videoPlayer.time)} / {FormatTime(videoPlayer.length)}";
            }
        }
    }

    void PopulateVideoList()
    {
        foreach (var video in videoDataList)
        {
            GameObject item = Instantiate(videoItemPrefab, contentHolder);
            item.transform.Find("Thumbnail").GetComponent<Image>().sprite = video.thumbnail;
            item.transform.Find("Name").GetComponent<TMP_Text>().text = video.videoName;

            Button playButton = item.transform.Find("Thumbnail").GetComponent<Button>();
            VideoClip clip = video.videoClip; // avoid closure issue
            playButton.onClick.AddListener(() => PlayVideo(clip));
        }
    }

    void PlayVideo(VideoClip clip)
    {
        if (clip == null) return;

        videoPlayer.clip = clip;
        videoPlayerPanel.SetActive(true);
        videoPlayer.Play();
        isPaused = false;
        UpdatePausePlayIcon();
    }

    void CloseVideo()
    {
        videoPlayer.Stop();
        videoPlayerPanel.SetActive(false);
    }

    void TogglePausePlay()
    {
        if (isPaused)
            videoPlayer.Play();
        else
            videoPlayer.Pause();

        isPaused = !isPaused;
        UpdatePausePlayIcon();
    }

    void RestartVideo()
    {
        if (videoPlayer.clip != null)
        {
            videoPlayer.time = 0;
            videoPlayer.Play();
            isPaused = false;
            UpdatePausePlayIcon();
        }
    }

    void UpdatePausePlayIcon()
    {
        if (pausePlayButton != null && playSprite != null && pauseSprite != null)
        {
            pausePlayButton.image.sprite = isPaused ? playSprite : pauseSprite;
        }
    }

    // Called by the Slider's onValueChanged
    public void OnSliderValueChanged(float value)
    {
        if (isDraggingSlider && videoPlayer.clip != null && videoPlayer.length > 0)
        {
            videoPlayer.time = value * videoPlayer.length;
        }
    }

    // Called by EventTrigger -> Begin Drag
    public void OnBeginDrag(BaseEventData data)
    {
        isDraggingSlider = true;
    }

    // Called by EventTrigger -> End Drag
    public void OnEndDrag(BaseEventData data)
    {
        isDraggingSlider = false;

        if (videoPlayer.clip != null && videoPlayer.length > 0)
        {
            videoPlayer.time = progressBar.value * videoPlayer.length;
        }
    }
}
