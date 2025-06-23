using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizSelector : MonoBehaviour
{
    [System.Serializable]
    public class QuizEntry
    {
        public Button openButton;      // 打开 Quiz 的按钮
        public GameObject panel;       // 对应的 Quiz 面板
        public Button closeButton;     // 面板上的关闭按钮
    }

    public List<QuizEntry> quizEntries;

    private int currentOpenIndex = -1;

    void Start()
    {
        for (int i = 0; i < quizEntries.Count; i++)
        {
            int index = i; // 本地变量防止闭包问题

            // 初始化关闭所有面板
            quizEntries[index].panel.SetActive(false);

            // 点按钮切换该面板
            quizEntries[index].openButton.onClick.AddListener(() => ToggleQuiz(index));

            // 关闭按钮直接关闭自己
            quizEntries[index].closeButton.onClick.AddListener(() => CloseQuiz(index));
        }
    }

    void ToggleQuiz(int index)
    {
        if (currentOpenIndex == index)
        {
            // 如果点的是当前面板 → 关闭
            CloseQuiz(index);
        }
        else
        {
            // 打开当前，关闭其他
            for (int i = 0; i < quizEntries.Count; i++)
            {
                bool active = (i == index);
                quizEntries[i].panel.SetActive(active);
            }
            currentOpenIndex = index;
        }
    }

    void CloseQuiz(int index)
    {
        quizEntries[index].panel.SetActive(false);
        if (currentOpenIndex == index)
            currentOpenIndex = -1;
    }
}
