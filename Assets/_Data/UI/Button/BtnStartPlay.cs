using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnStartPlay : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.ResetGameOverState();
        GameManager.Instance.StartNewGame();
    }
}
