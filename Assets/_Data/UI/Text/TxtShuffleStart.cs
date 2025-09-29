using System.Collections;
using UnityEngine;

public class TxtShuffleStart : BaseText
{
    protected override void Start()
    {
        // Chế độ mặc định: hiển thị số lượt shuffle còn lại
        text.text = "x" + GameManager.Instance.RemainShuffle;
    }
}
