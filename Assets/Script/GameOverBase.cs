using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバーの挙動
/// </summary>
public class GameOverBase : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverText;

    /// <summary>
    /// ゲームオーバーになってからの経過時間
    /// </summary>
    private float _gameOverTimer;

    /// <summary>
    /// 最初に呼ばれる
    /// </summary>
    void Start()
    {
        _gameOverText.SetActive(false);
    }

    /// <summary>
    /// 常に呼び出される
    /// </summary>
    void Update()
    {
        //ゲームオーバーテキストが表示されてる場合
        if(_gameOverText.activeSelf)
		{
            _gameOverTimer += Time.deltaTime;
            if(_gameOverTimer >= 5.0f)
			{
                SceneManager.LoadScene("title");
            }
        }
    }

    /// <summary>
    /// ゲームオーバーを開始する
    /// </summary>
    public void GameOverStart()
	{
        _gameOverText.SetActive(true);
    }

}
