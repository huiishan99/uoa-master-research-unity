using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using TMPro;
using UnityEngine.InputSystem;

namespace Whisper.Samples
{
    public class StreamingSampleMic : MonoBehaviour
    {
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;

        [Header("UI")]
        public Button button;
        public TMP_Text buttonText;
        public TMP_Text text;

        public Color recordingColor = Color.green;  // 录音时颜色
        public Color stoppedColor = Color.red;      // 停止时颜色

        private WhisperStream _stream;

        // 使用自动生成的类
        private XRInputActions inputActions;

        private async void Start()
        {
            inputActions = new XRInputActions(); // 实例化
            inputActions.Enable(); // 启用输入系统

            // 监听 A 键
            inputActions.Gameplay.ButtonA.performed += ctx => OnButtonPressed();

            _stream = await whisper.CreateStream(microphoneRecord);
            _stream.OnResultUpdated += OnResult;
            _stream.OnSegmentUpdated += OnSegmentUpdated;
            _stream.OnSegmentFinished += OnSegmentFinished;
            _stream.OnStreamFinished += OnFinished;

            microphoneRecord.OnRecordStop += OnRecordStop;
            button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDestroy()
        {
            // 清理事件监听
            inputActions.Gameplay.ButtonA.performed -= ctx => OnButtonPressed();
        }

        private void OnButtonPressed()
        {
            if (!microphoneRecord.IsRecording)
            {
                text.text = ""; // 清空文字
                _stream.StartStream();
                microphoneRecord.StartRecord();
                button.image.color = recordingColor; // 设置按钮为绿色
            }
            else
            {
                microphoneRecord.StopRecord();
                button.image.color = stoppedColor;   // 设置按钮为红色
            }

            buttonText.text = microphoneRecord.IsRecording ? "Stop" : "Record";
        }

        private void OnRecordStop(AudioChunk recordedAudio)
        {
            buttonText.text = "Record";
        }

        private void OnResult(string result)
        {
            text.text = result;
        }

        private void OnSegmentUpdated(WhisperResult segment)
        {
            print($"Segment updated: {segment.Result}");
        }

        private void OnSegmentFinished(WhisperResult segment)
        {
            print($"Segment finished: {segment.Result}");
        }

        private void OnFinished(string finalResult)
        {
            print("Stream finished!");
        }
    }
}
