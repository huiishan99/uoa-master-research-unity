using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{

    public VideoPlayer videoPlayer;             // 拖入你的 VideoPlayer 组件

    public Button playPauseButton;              // 播放/暂停按钮
    public Button forwardButton;                // 快进按钮
    public Button backwardButton;               // 快退按钮

    public Sprite playIcon;                     // 播放图标
    public Sprite pauseIcon;                    // 暂停图标
    public Sprite forwardIcon;                  // 快进按钮图标
    public Sprite backwardIcon;                 // 快退按钮图标

    public float skipSeconds = 5f;              // 每次快进/快退的秒数

    private bool isPlaying = false;             // 当前是否处于播放状态

    void Start()
    {
        // 初始化按钮图标和播放状态
        ResetPlayButton();
        InitControlButtonIcons();

        // 添加按钮监听器
        if (playPauseButton != null)
            playPauseButton.onClick.AddListener(TogglePlayPause);

        if (forwardButton != null)
            forwardButton.onClick.AddListener(SkipForward);

        if (backwardButton != null)
            backwardButton.onClick.AddListener(SkipBackward);
    }

    //初始化快进和快退按钮的图标（如果有设置）
    private void InitControlButtonIcons()
    {
        if (forwardButton != null && forwardIcon != null)
            forwardButton.image.sprite = forwardIcon;

        if (backwardButton != null && backwardIcon != null)
            backwardButton.image.sprite = backwardIcon;
    }

    // 播放 / 暂停 切换功能
    public void TogglePlayPause()
    {
        if (videoPlayer == null) return;

        if (isPlaying)
        {
            videoPlayer.Pause();
            playPauseButton.image.sprite = playIcon;
            isPlaying = false;
        }
        else
        {
            videoPlayer.Play();
            playPauseButton.image.sprite = pauseIcon;
            isPlaying = true;
        }
    }

    // 快进 skipSeconds 秒
    public void SkipForward()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared) return;

        double targetTime = videoPlayer.time + skipSeconds;
        // 避免超过视频长度
        videoPlayer.time = Mathf.Min((float)targetTime, (float)videoPlayer.length);
    }


    // 快退 skipSeconds 秒
    public void SkipBackward()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared) return;

        double targetTime = videoPlayer.time - skipSeconds;
        // 避免小于0
        videoPlayer.time = Mathf.Max((float)targetTime, 0f);
    }

    // 外部调用此函数时（例如切换视频），可重置播放状态
    public void ResetPlayButton()
    {
        playPauseButton.image.sprite = playIcon;
        isPlaying = false;
    }
}
