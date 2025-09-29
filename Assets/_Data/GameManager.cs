using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    GameOver,
    Victory
}

public class GameManager : SaiSingleton<GameManager>
{
    protected GameState currentState;
    public GameState CurrentState => currentState;

    #region Game Logic Variables
    private bool isCountdownShuffle = false;

    [SerializeField] protected int maxLevel = 10; // Tổng 10 màn
    [SerializeField] protected int gameLevel = 1;
    [SerializeField] protected int remainShuffle = 9;
    public int RemainShuffle => remainShuffle;

    [SerializeField] protected int remainHint = 9;
    public int RemainHint => remainHint;

    public int CurrentLevel => gameLevel;
    #endregion

    // Events
    public event Action OnGameOver;
    public event Action OnFinishGame;
    public event Action<GameState> OnGameStateChanged;

    protected override void Start()
    {
        base.Start();
        SetInitialState();
    }

    protected virtual void FixedUpdate()
    {
        if (currentState != GameState.Playing) return;

        UpdateGameplay();
    }

    protected virtual void UpdateGameplay()
    {
        CheckGameStatus();
        CheckShouldCountdownShuffle();
    }

    #region Game State Handlers
    protected virtual void SetInitialState()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.ToLower().Contains("mainmenu"))
        {
            ChangeState(GameState.MainMenu);
        }
        else
        {
            StartNewGame();
        }
    }

    public virtual void ChangeState(GameState newState)
    {
        if (currentState == newState) return;

        ExitCurrentState();
        currentState = newState;
        EnterNewState();

        OnGameStateChanged?.Invoke(currentState);
    }

    protected virtual void ExitCurrentState()
    {
        if (currentState == GameState.Playing)
        {
            // Cleanup if needed
        }
    }

    protected virtual void EnterNewState()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
        }
    }
    #endregion

    public virtual void StartNewGame()
    {
        StartCoroutine(WaitForGameSceneLoad());
    }

    protected virtual IEnumerator WaitForGameSceneLoad()
    {
        SceneManager.LoadScene("game");

        yield return null;

        while (GridManagerCtrl.Instance == null || GridManagerCtrl.Instance.gridSystem.blocksRemain == 0)
        {
            yield return null;
        }

        InitializeData();
        ChangeState(GameState.Playing);
    }

    protected virtual void CheckShouldCountdownShuffle()
    {
        if (isCountdownShuffle) return;
        if (!IsDebugTouch()) return;
        if (CountdownShuffleCtrl.Instance == null) return;
        if (CountdownShuffleCtrl.Instance.IsCountingDown()) return;

        CountdownShuffleCtrl.Instance.SetShouldCountingDown();
        isCountdownShuffle = true;
    }

    /// <summary>
    /// Kiểm tra người chơi có tương tác debug: click chuột (PC) hoặc tap màn hình (mobile).
    /// </summary>
    protected virtual bool IsDebugTouch()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0); // Click chuột trái
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began; // Tap trên mobile
#endif
    }

    [ProButton]
    public virtual void NextLevel()
    {
        gameLevel++;

        if (gameLevel > maxLevel)
        {
            gameLevel = 1;
        }

        StartCoroutine(WaitForGameSceneLoad());
    }

    public virtual void UseHint()
    {
        remainHint--;
        if (remainHint < 0) remainHint = 0;
    }

    public virtual void UseShuffle()
    {
        remainShuffle--;
        if (remainShuffle < 0) remainShuffle = 0;
    }

    public virtual void AddMoreHint(int hintNum)
    {
        remainHint += hintNum;
    }

    public virtual void AddMoreShuffle(int shuffleNum)
    {
        remainShuffle += shuffleNum;
    }

    protected virtual void CheckGameStatus()
    {
        if (GridManagerCtrl.Instance?.gridSystem == null) return;

        int blocksRemain = GridManagerCtrl.Instance.gridSystem.blocksRemain;
        bool noMovesLeft = remainShuffle <= 0 && !GridManagerCtrl.Instance.blockAuto.isNextBlockExist;

        if (blocksRemain == 0)
        {
            ChangeState(GameState.Victory);
        }
        else if (noMovesLeft && blocksRemain > 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    protected virtual void HandleGameOver()
    {
        OnGameOver?.Invoke();
        SoundManager.Instance?.PlaySound(SoundManager.Sound.no_move);
    }

    protected virtual void HandleVictory()
    {
        if (gameLevel == maxLevel)
        {
            OnFinishGame?.Invoke();
            return;
        }

        SoundManager.Instance?.PlaySound(SoundManager.Sound.win);
    }

    public virtual void ResetGameOverState()
    {
        remainShuffle = 9;
        remainHint = 9;
        gameLevel = 1;

        OnGameOver = null;
        OnFinishGame = null;
    }

    protected virtual void InitializeData()
    {
        LoadMaxLevel();
        isCountdownShuffle = false;
    }

    protected virtual void LoadMaxLevel()
    {
        maxLevel = 10; // Đặt cố định 10 màn
    }

    #region Event Registration
    protected override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    protected virtual void OnSceneUnloaded(Scene scene)
    {
        OnGameOver = null;
        OnFinishGame = null;
    }
    #endregion
}
