using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{

    public VideoPlayer videoPlayer;             // ������� VideoPlayer ���

    public Button playPauseButton;              // ����/��ͣ��ť
    public Button forwardButton;                // �����ť
    public Button backwardButton;               // ���˰�ť

    public Sprite playIcon;                     // ����ͼ��
    public Sprite pauseIcon;                    // ��ͣͼ��
    public Sprite forwardIcon;                  // �����ťͼ��
    public Sprite backwardIcon;                 // ���˰�ťͼ��

    public float skipSeconds = 5f;              // ÿ�ο��/���˵�����

    private bool isPlaying = false;             // ��ǰ�Ƿ��ڲ���״̬

    void Start()
    {
        // ��ʼ����ťͼ��Ͳ���״̬
        ResetPlayButton();
        InitControlButtonIcons();

        // ��Ӱ�ť������
        if (playPauseButton != null)
            playPauseButton.onClick.AddListener(TogglePlayPause);

        if (forwardButton != null)
            forwardButton.onClick.AddListener(SkipForward);

        if (backwardButton != null)
            backwardButton.onClick.AddListener(SkipBackward);
    }

    //��ʼ������Ϳ��˰�ť��ͼ�꣨��������ã�
    private void InitControlButtonIcons()
    {
        if (forwardButton != null && forwardIcon != null)
            forwardButton.image.sprite = forwardIcon;

        if (backwardButton != null && backwardIcon != null)
            backwardButton.image.sprite = backwardIcon;
    }

    // ���� / ��ͣ �л�����
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

    // ��� skipSeconds ��
    public void SkipForward()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared) return;

        double targetTime = videoPlayer.time + skipSeconds;
        // ���ⳬ����Ƶ����
        videoPlayer.time = Mathf.Min((float)targetTime, (float)videoPlayer.length);
    }


    // ���� skipSeconds ��
    public void SkipBackward()
    {
        if (videoPlayer == null || !videoPlayer.isPrepared) return;

        double targetTime = videoPlayer.time - skipSeconds;
        // ����С��0
        videoPlayer.time = Mathf.Max((float)targetTime, 0f);
    }

    // �ⲿ���ô˺���ʱ�������л���Ƶ���������ò���״̬
    public void ResetPlayButton()
    {
        playPauseButton.image.sprite = playIcon;
        isPlaying = false;
    }
}
