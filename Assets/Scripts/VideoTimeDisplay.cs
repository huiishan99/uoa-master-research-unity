using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoTimeDisplay : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // 播放器组件
    public TMP_Text currentTimeText;         // 当前播放时间显示
    public TMP_Text totalTimeText;           // 视频总时长显示

    private bool isPrepared = false;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare(); // 准备视频（获取时长）
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        isPrepared = true;
        if (totalTimeText != null)
        {
            totalTimeText.text = FormatTime((float)vp.length);
        }
    }

    void Update()
    {
        if (videoPlayer == null || !videoPlayer.isPlaying || !isPrepared) return;

        float currentTime = (float)videoPlayer.time;

        // 更新当前时间文本
        if (currentTimeText != null)
            currentTimeText.text = FormatTime(currentTime);

    }

    // 秒 → "mm:ss" 格式化函数
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
