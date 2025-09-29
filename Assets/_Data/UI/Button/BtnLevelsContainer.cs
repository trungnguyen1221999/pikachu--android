using System.Collections.Generic;
using UnityEngine;

public class BtnLevelsContainer : SaiMonoBehaviour
{
    [SerializeField] protected List<BtnLevel> levels = new List<BtnLevel>();

    protected override void Start()
    {
        base.Start();
        HideHighLevelButtons(); // Luôn thực hiện ẩn các level cao (6–9)
    }

    private void HideHighLevelButtons()
    {
        foreach (var btnLevel in levels)
        {
            if (btnLevel.Level > 5 && btnLevel.Level < 10)
            {
                btnLevel.gameObject.SetActive(false);
            }
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadButtons();
    }

    private void LoadButtons()
    {
        if (this.levels.Count > 0) return;
        this.levels.Clear();
        BtnLevel[] btnLevels = GetComponentsInChildren<BtnLevel>();
        foreach (var btnLevel in btnLevels)
        {
            this.levels.Add(btnLevel);
        }
        Debug.LogWarning(transform.name + " LoadButtons", gameObject);
    }
}
