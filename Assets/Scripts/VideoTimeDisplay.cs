using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoTimeDisplay : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // ���������
    public TMP_Text currentTimeText;         // ��ǰ����ʱ����ʾ
    public TMP_Text totalTimeText;           // ��Ƶ��ʱ����ʾ

    private bool isPrepared = false;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare(); // ׼����Ƶ����ȡʱ����
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

        // ���µ�ǰʱ���ı�
        if (currentTimeText != null)
            currentTimeText.text = FormatTime(currentTime);

    }

    // �� �� "mm:ss" ��ʽ������
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
