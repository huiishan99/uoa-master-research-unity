using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizPanel
    {
        public GameObject panel;              // Quiz 面板
        public TMP_Text questionText;         // 题干
        public ToggleGroup toggleGroup;       // 单选组
        public Button sendButton;             // 发送按钮
        public string questionTitle;          // 题目标题（可以和Text相同）
    }

    [Header("Quiz 面板列表")]
    public List<QuizPanel> quizzes;

    private string outputPath;

    void Start()
    {
        // 生成保存路径
        outputPath = Path.Combine(Application.persistentDataPath, "quiz_answers.csv");

        // 如果文件不存在，添加表头
        if (!File.Exists(outputPath))
        {
            File.AppendAllText(outputPath, "\"Question\",\"Time\",\"Answer\"\n");
        }

        // 初始化面板和按钮
        foreach (var quiz in quizzes)
        {
            quiz.panel.SetActive(false); // 开始时隐藏
            quiz.sendButton.onClick.AddListener(() => SubmitQuiz(quiz));
        }
    }

    public void ShowQuiz(int index)
    {
        for (int i = 0; i < quizzes.Count; i++)
        {
            quizzes[i].panel.SetActive(i == index); // 只打开对应面板
        }
    }

    private void SubmitQuiz(QuizPanel quiz)
    {
        Toggle selected = quiz.toggleGroup.ActiveToggles().FirstOrDefault();

        if (selected != null)
        {
            string answer = selected.GetComponentInChildren<TMP_Text>().text;
            string time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string question = quiz.questionText.text;

            string line = $"\"{question}\",\"{time}\",\"{answer}\"";

            File.AppendAllText(outputPath, line + "\n");
            Debug.Log("答案已保存：" + line);

            quiz.panel.SetActive(false); // 提交后关闭面板
        }
        else
        {
            Debug.LogWarning("请先选择一个选项！");
        }
    }
}
