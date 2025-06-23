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
        public GameObject panel;              // Quiz ���
        public TMP_Text questionText;         // ���
        public ToggleGroup toggleGroup;       // ��ѡ��
        public Button sendButton;             // ���Ͱ�ť
        public string questionTitle;          // ��Ŀ���⣨���Ժ�Text��ͬ��
    }

    [Header("Quiz ����б�")]
    public List<QuizPanel> quizzes;

    private string outputPath;

    void Start()
    {
        // ���ɱ���·��
        outputPath = Path.Combine(Application.persistentDataPath, "quiz_answers.csv");

        // ����ļ������ڣ���ӱ�ͷ
        if (!File.Exists(outputPath))
        {
            File.AppendAllText(outputPath, "\"Question\",\"Time\",\"Answer\"\n");
        }

        // ��ʼ�����Ͱ�ť
        foreach (var quiz in quizzes)
        {
            quiz.panel.SetActive(false); // ��ʼʱ����
            quiz.sendButton.onClick.AddListener(() => SubmitQuiz(quiz));
        }
    }

    public void ShowQuiz(int index)
    {
        for (int i = 0; i < quizzes.Count; i++)
        {
            quizzes[i].panel.SetActive(i == index); // ֻ�򿪶�Ӧ���
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
            Debug.Log("���ѱ��棺" + line);

            quiz.panel.SetActive(false); // �ύ��ر����
        }
        else
        {
            Debug.LogWarning("����ѡ��һ��ѡ�");
        }
    }
}
