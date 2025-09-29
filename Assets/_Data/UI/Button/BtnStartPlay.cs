using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnStartPlay : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.ResetGameOverState();
        GameManager.Instance.StartNewGame();
        StartCoroutine(LoadSceneAndStartGame(1));
    }

    private IEnumerator LoadSceneAndStartGame(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Đợi load xong scene
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Scene đã load xong, bắt đầu game
        GameManager.Instance.StartNewGame();
    }
}
