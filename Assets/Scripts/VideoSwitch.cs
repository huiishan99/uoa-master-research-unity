using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSwitch : MonoBehaviour
{
    [Serializable]
    public class ButtonVideoMapping
    {
        public Button button;         // ѡ����Ƶ�İ�ť
        public VideoClip videoClip;   // ��Ӧ����Ƶ
    }

    [Header("Video Buttons")]
    public ButtonVideoMapping button1;
    public ButtonVideoMapping button2;
    public ButtonVideoMapping button3;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;  // VideoPlayer ���
    public VideoControl videoControl; // ���� VideoControl �ű�

    private void Start()
    {
        // Ϊÿ����ť���¼���ֻ�л���Ƶ�����Զ����ţ�
        if (button1.button != null)
            button1.button.onClick.AddListener(() => SelectVideo(button1.videoClip));

        if (button2.button != null)
            button2.button.onClick.AddListener(() => SelectVideo(button2.videoClip));

        if (button3.button != null)
            button3.button.onClick.AddListener(() => SelectVideo(button3.videoClip));

        // ȡ�� Play On Awake����������ʱ�Զ�����
        videoPlayer.playOnAwake = false;
    }

    private void SelectVideo(VideoClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Video Clip is null!");
            return;
        }

        // ���ѡ�е���Ƶ�͵�ǰ���ŵ���Ƶ��ͬһ����ʲô������
        if (videoPlayer.clip == clip)
        {
            Debug.Log("Selected video is already playing. No reset needed.");
            return;
        }

        Debug.Log($"Selected New Video: {clip.name}");
        videoPlayer.clip = clip;

        // ���ɵ� prepareCompleted �¼�����ֹ�ظ���
        videoPlayer.prepareCompleted -= OnVideoPrepared;

        // �� prepare �¼���������Ƶ�ĵ�һ֡
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video Prepared, displaying first frame.");
        videoPlayer.Play();
        videoPlayer.Pause();  // ����Ƶ���ص�һ֡��������

        if (videoControl != null)
        {
            videoControl.ResetPlayButton();
        }
    }
}
