using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ObjectHider : MonoBehaviour
{
    public Button targetButton;
    public GameObject[] objectsToToggle;
    public TMP_Text buttonText;

    public string textWhenVisible = "Active";
    public string textWhenHidden = "Hide";
    public Color colorWhenVisible = Color.green;
    public Color colorWhenHidden = Color.red;

    [SerializeField] private AudioSource audioSource; // 在 Inspector 拖 AudioSource 进来

    [HideInInspector] public bool isVisible = true;
    [HideInInspector] public bool wasAlreadyVisible = true;

    // log 文件路径
    private string logFilePath;

    // 外部拖拽引用，用于清空响应文本
    public TMP_Text responseText;

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(ToggleObjects);
        }
        else
        {
            Debug.LogWarning("未指定 targetButton，无法注册点击事件！");
        }

        wasAlreadyVisible = isVisible;
        UpdateButtonVisual();

        // 初始化 log 文件路径
        logFilePath = Path.Combine(Application.persistentDataPath, "UserLog.txt");
        // 启动时写入初始状态
        WriteToggleLog(isVisible ? "Active" : "Hide");
    }

    public void ToggleObjects()
    {
        isVisible = !isVisible;

        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
                obj.SetActive(isVisible);
        }

        UpdateButtonVisual();
        WriteToggleLog(isVisible ? "Active" : "Hide");

        if (responseText != null)
        {
            responseText.text = "";
        }

        // ✅ 每次点击按钮就强制停止音频，避免切换 Active 时复播旧音频
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (isVisible)
        {
            wasAlreadyVisible = true;
        }
        else
        {
            wasAlreadyVisible = false;
        }
    }

    private void UpdateButtonVisual()
    {
        if (buttonText != null)
        {
            buttonText.text = isVisible ? textWhenVisible : textWhenHidden;
        }

        if (targetButton != null)
        {
            Color targetColor = isVisible ? colorWhenVisible : colorWhenHidden;
            var colors = targetButton.colors;
            colors.normalColor = targetColor;
            colors.highlightedColor = targetColor;
            colors.pressedColor = targetColor * 0.9f;
            targetButton.colors = colors;

            Image image = targetButton.GetComponent<Image>();
            if (image != null)
            {
                image.color = targetColor;
            }
        }
    }

    // 写入 log 文件
    private void WriteToggleLog(string currentState)
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logEntry = $"[{timestamp}] AvatarToggle: {currentState}\n";
        File.AppendAllText(logFilePath, logEntry);
    }
}
