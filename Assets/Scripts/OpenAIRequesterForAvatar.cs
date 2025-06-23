using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using System.IO;

public class OpenAIRequesterForAvatar : MonoBehaviour
{
    [Header("服务器设置")]
    [SerializeField] private string serverUrl = "http://127.0.0.1:5000/ask_openai";

    [Header("UI 组件")]
    [SerializeField] public TextMeshProUGUI recordText;
    [SerializeField] public TextMeshProUGUI responseText;
    [SerializeField] public Button sendButton;
    [SerializeField] public TextMeshProUGUI sentStatusText;
    [SerializeField] public TextMeshProUGUI respondingStatusText;

    // Avatar Active/Hide 状态管理脚本
    [SerializeField] private ObjectHider objectHider;

    // Lipsync（TTS）播放脚本
    [SerializeField] private TextToSpeechOpenAI ttsScript;

    private float requestStartTime;
    private float requestEndTime;

    private XRInputActions inputActions;
    private string logFilePath;

    private void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "UserLog.txt");

        inputActions = new XRInputActions();
        inputActions.Enable();

        inputActions.Gameplay.ButtonX.performed += ctx => OnSendButtonClicked();

        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }

        // 启动时记录 AvatarToggle 的状态
        if (objectHider != null)
        {
            string initialState = objectHider.isVisible ? "Active" : "Hide";
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] Initial AvatarToggle: {initialState}\n";
            File.AppendAllText(logFilePath, logEntry);
        }
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Gameplay.ButtonX.performed -= ctx => OnSendButtonClicked();
        }
    }

    private void OnSendButtonClicked()
    {
        string userQuery = recordText.text;
        if (!string.IsNullOrEmpty(userQuery))
        {
            requestStartTime = Time.time;

            if (sentStatusText != null)
            {
                sentStatusText.text = "Sent!";
                StartCoroutine(HideSentStatus());
            }

            if (respondingStatusText != null)
            {
                respondingStatusText.text = "Responding...";
            }

            StartCoroutine(SendRequestToServer(userQuery));
        }
    }

    private IEnumerator HideSentStatus()
    {
        yield return new WaitForSeconds(0.8f);
        if (sentStatusText != null)
        {
            sentStatusText.text = "";
        }
    }

    private IEnumerator SendRequestToServer(string query)
    {
        string jsonData = "{\"query\":\"" + query + "\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 15;

            yield return request.SendWebRequest();

            requestEndTime = Time.time;
            float responseTime = requestEndTime - requestStartTime;
            Debug.Log($"Response time: {responseTime:F2} seconds");

            string responseMessage;

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(responseJson);

                responseMessage = response != null && !string.IsNullOrEmpty(response.response)
                    ? response.response
                    : "Hmm, I couldn't come up with an answer.";
            }
            else
            {
                responseMessage = "Oops, something went wrong. Please try again.";
            }

            responseText.text = responseMessage;
            WriteLog(query, responseMessage, responseTime);

            // 只在 Avatar 当前是 Active 且不是刚从 Hide → Active 触发时才播放 Lipsync
            if (ttsScript != null && objectHider != null && objectHider.isVisible && objectHider.wasAlreadyVisible)
            {
                ttsScript.PlayResponseAudio(responseMessage);
            }

            if (respondingStatusText != null)
            {
                respondingStatusText.text = "";
            }
        }
    }

    private void WriteLog(string question, string answer, float responseTime)
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logEntry = $"[{timestamp}] Question: {question}\n[{timestamp}] Response: {answer}\n[{timestamp}] ResponseTime: {responseTime:F2}s\n";
        File.AppendAllText(logFilePath, logEntry);
    }

    [System.Serializable]
    private class OpenAIResponse
    {
        public string response;
    }
}
