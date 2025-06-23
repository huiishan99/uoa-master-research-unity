using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSwitch : MonoBehaviour
{
    [Serializable]
    public class ButtonVideoMapping
    {
        public Button button;         // 选择视频的按钮
        public VideoClip videoClip;   // 对应的视频
    }

    [Header("Video Buttons")]
    public ButtonVideoMapping button1;
    public ButtonVideoMapping button2;
    public ButtonVideoMapping button3;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;  // VideoPlayer 组件
    public VideoControl videoControl; // 连接 VideoControl 脚本

    private void Start()
    {
        // 为每个按钮绑定事件（只切换视频，不自动播放）
        if (button1.button != null)
            button1.button.onClick.AddListener(() => SelectVideo(button1.videoClip));

        if (button2.button != null)
            button2.button.onClick.AddListener(() => SelectVideo(button2.videoClip));

        if (button3.button != null)
            button3.button.onClick.AddListener(() => SelectVideo(button3.videoClip));

        // 取消 Play On Awake，避免启动时自动播放
        videoPlayer.playOnAwake = false;
    }

    private void SelectVideo(VideoClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Video Clip is null!");
            return;
        }

        // 如果选中的视频和当前播放的视频是同一个，什么都不做
        if (videoPlayer.clip == clip)
        {
            Debug.Log("Selected video is already playing. No reset needed.");
            return;
        }

        Debug.Log($"Selected New Video: {clip.name}");
        videoPlayer.clip = clip;

        // 解绑旧的 prepareCompleted 事件，防止重复绑定
        videoPlayer.prepareCompleted -= OnVideoPrepared;

        // 绑定 prepare 事件，加载视频的第一帧
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video Prepared, displaying first frame.");
        videoPlayer.Play();
        videoPlayer.Pause();  // 让视频加载第一帧但不播放

        if (videoControl != null)
        {
            videoControl.ResetPlayButton();
        }
    }
}
