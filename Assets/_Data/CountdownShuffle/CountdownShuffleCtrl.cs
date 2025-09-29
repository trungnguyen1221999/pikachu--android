using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownShuffleCtrl : SaiMonoBehaviour
{
    private static CountdownShuffleCtrl instance;
    public static CountdownShuffleCtrl Instance => instance;

    [SerializeField] private Image foreground;
    [SerializeField] private GameObject linearHolder;
    [SerializeField] private float timeRemaining;
    [SerializeField] private float maxTimer;
    private bool isCountingDown;

    protected override void Awake()
    {
        base.Awake();
        if (CountdownShuffleCtrl.instance != null) Debug.LogError("Only 1 CountdownShuffleCtrl allow to exist");
        CountdownShuffleCtrl.instance = this;
    }

    protected override void Reset()
    {
        base.Reset();
        maxTimer = 300;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLinearHolder();
        this.LoadForeground();
    }

    private void LoadLinearHolder()
    {
        if (this.linearHolder != null) return;
        this.linearHolder = transform.Find("LinearHolder").gameObject;
        Debug.LogWarning(transform.name + ": LoadLinearHolder", gameObject);
    }

    private void LoadForeground()
    {
        if (this.foreground != null) return;
        this.foreground = linearHolder.transform.Find("Foreground")?.GetComponent<Image>();
        Debug.LogWarning(transform.name + ": LoadForeground", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        timeRemaining = maxTimer;

        // Bỏ điều kiện debug, luôn đếm ngược khi game start
        isCountingDown = true;

        // Ẩn UI nếu cần (tuỳ ý)
        ShowingUI(false);
    }

    protected virtual void Update()
    {
        CountdownShuffle();
    }

    private void CountdownShuffle()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        if (!isCountingDown) return;

        ShowingUI(true);

        if (GameManager.Instance.RemainShuffle <= 0) timeRemaining = maxTimer;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            foreground.fillAmount = timeRemaining / maxTimer;
            return;
        }

        // Khi hết thời gian
        // Kiểm tra nếu chưa thắng thì chuyển GameOver
        if (GridManagerCtrl.Instance.gridSystem.blocksRemain > 0)
        {
            // Chưa thắng mà hết thời gian -> Game Over
            GameManager.Instance.ChangeState(GameState.GameOver);
            isCountingDown = false;  // dừng đếm
            ShowingUI(false);
            return;
        }

        // Nếu thắng rồi hoặc không còn block nào thì shuffle như bình thường
        ShuffleTrigger();
        timeRemaining = maxTimer;
    }


    private static void ShuffleTrigger()
    {
        GridManagerCtrl.Instance.blockAuto.ShuffleBlocks();
        SoundManager.Instance.PlaySound(SoundManager.Sound.win);
    }

    public bool IsCountingDown() => isCountingDown;

    public void SetShouldCountingDown() => isCountingDown = true;

    private void ShowingUI(bool active)
    {
        this.linearHolder.SetActive(active);
    }
}
