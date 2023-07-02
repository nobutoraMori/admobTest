using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    /// <summary>
    /// タイトルボタン
    /// </summary>
    [SerializeField] private Button _titleButton;
    // Start is called before the first frame update
    void Start()
    {
        _titleButton.onClick.AddListener(GoTitle);
    }

    private void GoTitle()
    {
        //バナー削除
        AdmobLibrary.DestroyBanner();

        //titleシーンに遷移
        SceneManager.LoadScene("title");
    }
}
