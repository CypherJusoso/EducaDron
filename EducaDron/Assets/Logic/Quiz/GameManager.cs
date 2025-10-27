using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    private Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }

    [SerializeField] GameEvents events = null;

    [SerializeField] SendPointsApi pointsApi;

    [SerializeField] Animator timerAnimtor = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfWayOutColor = Color.yellow;
    [SerializeField] Color timerAlmostOutColor = Color.red;
    private Color timerDefaultColor = Color.white;

    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    private int timerStateParaHash = 0;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer = null;

    private bool IsFinished
    {
        get
        {
            return (FinishedQuestions.Count < Questions.Length) ? false : true;
        }
    }

    #endregion

    #region Default Unity methods

    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    void Awake()
    {
        events.CurrentFinalScore = 0;
    }

    void Start()
    {
        timerDefaultColor = timerText.color;
        LoadQuestions();

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    #endregion

    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);

        var type
            = (IsFinished)
            ? UIManager.ResolutionScreenType.Finish
            : (isCorrect) ? UIManager.ResolutionScreenType.Correct
            : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, Questions[currentQuestion].AddScore);
        }

        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    #region Timer Methods

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimtor.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }
    }
    IEnumerator StartTimer()
    {
        var totalTime = Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }
    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    #endregion

    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    void LoadQuestions()
    {
        var dm = DataManager.instance;
        if (dm == null)
        {
            Debug.LogError("DataManager no encontrado en la escena. Asegúrate de que existe un objeto con DataManager antes de cargar preguntas.");
            _questions = new Question[0];
            return;
        }

        int currentLevel = dm.currentLvl;

        Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        List<Question> filtered = new List<Question>(objs.Length);

        foreach (var o in objs)
        {
            var q = o as Question;
            if (q == null) continue;
            if (q.LevelQuestion == currentLevel)
            {
                filtered.Add(q);
            }
        }

        // Si hay más de 6, barajamos y tomamos 6
        int takeCount = Mathf.Min(6, filtered.Count);

        if (filtered.Count > 1)
        {
            // Fisher-Yates shuffle para aleatorizar la lista
            for (int i = filtered.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                var temp = filtered[i];
                filtered[i] = filtered[j];
                filtered[j] = temp;
            }
        }

        _questions = filtered.Take(takeCount).ToArray();

        if (_questions.Length == 0)
        {
            Debug.LogWarning($"No se encontraron Questions para el nivel {currentLevel}. Revisa el campo _levelQuestion en tus assets Question.");
        }
        else if (filtered.Count < 6)
        {
            Debug.LogWarning($"Solo se encontraron {filtered.Count} preguntas para el nivel {currentLevel}. Se usarán todas las disponibles.");
        }
    }

    public void RestartGame()
    {
        DataManager.instance.quizPoints = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CallNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        int level = DataManager.instance.currentLvl;
        int currentLvl = DataManager.instance.currentLvl;
        int points = DataManager.instance.TotalPoints;
        string id = DataManager.instance.userId;
        yield return pointsApi.UpdatePoints(id, currentLvl, points);

        int nextLevel = DataManager.instance.currentLvl + 1;
        SceneManager.LoadScene("Level" + nextLevel);
    }

    public void QuitGame()
    {
        StartCoroutine(QuitWhenApiCallEnds());
    }
    IEnumerator QuitWhenApiCallEnds()
    {
        Debug.Log("Max Points " + DataManager.instance.TotalPoints);
        string id = DataManager.instance.userId;
        int currentLvl = DataManager.instance.currentLvl;
        int points = DataManager.instance.TotalPoints;
        yield return pointsApi.UpdatePoints(id, currentLvl, points);
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (add > 0)
        {
            DataManager.instance.quizPoints += 10;
        }
        else
        {
            DataManager.instance.quizPoints -= 10;
        }
        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
        Debug.Log("Points: " + DataManager.instance.quizPoints);
    }

    #region Getters

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    #endregion
}