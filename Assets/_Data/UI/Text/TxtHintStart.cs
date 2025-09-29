using System.Collections;
using UnityEngine;

public class TxtHintStart : BaseText
{
    protected override void Start()
    {
        // Hiển thị số lượt gợi ý còn lại
        text.text = "x" + GameManager.Instance.RemainHint;
    }
}
