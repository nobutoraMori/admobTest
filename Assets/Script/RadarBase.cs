using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーダーの動き
/// 敵の位置表示
/// </summary>
public class RadarBase : MonoBehaviour
{
    //プレイヤーのアイコン
    [SerializeField] private GameObject _playerIcon;

    /// <summary>
    /// 敵マーカーのオリジナル
    /// </summary>
    [SerializeField] private GameObject _enemyMarkerOriginal;

    /// <summary>
    /// 敵管理システム
    /// </summary>
    [SerializeField] private EnemyManager _enemyManager;

    /// <summary>
    /// 敵のマーカーリスト
    /// </summary>
    private List<GameObject> _enemyMarkerList;
    /// <summary>
    /// 敵の射撃マーカーリスト
    /// </summary>
    private List<GameObject> _enemyShotMarkerList;

    /// <summary>
    /// 最初に呼ばれる
    /// </summary>
    void Start()
    {
        _enemyMarkerList = new List<GameObject>();
        _enemyShotMarkerList = new List<GameObject>();

        //2回呼ぶ
        for(int f = 0; f < 2; f++)
		{
            float scale = 1.0f;
            if(f == 1)
			{
                //射撃の作成の場合は小さくする
                scale = 0.5f;
			}
            //20回呼ぶ
            for (int i = 0; i < 20; i++)
            {
                //マーカーの作成
                GameObject marker = Instantiate(_enemyMarkerOriginal);
                //親オブジェクトの変更
                marker.transform.parent = transform;
                marker.transform.localPosition = Vector3.zero;
                marker.transform.localScale = new Vector3(scale, scale, scale);
                marker.transform.eulerAngles = Vector3.zero;
                //非表示にする
                marker.gameObject.SetActive(false);
                if (f == 0)
                {
                    //敵キャラとして追加
                    _enemyMarkerList.Add(marker);
                }
                else if(f == 1)
				{
                    //敵射撃として追加
                    _enemyShotMarkerList.Add(marker);
				}
            }
		}
    }

    /// <summary>
    /// 常に呼ばれる
    /// </summary>
    void Update()
    {
        //敵マーカーの更新
        for (int f = 0; f < 2; f++)
        {
            //参照する最大数
            int max = 0;
            if(f == 0)
			{
                max = _enemyManager.EnemyList.Count;
            }
            else if(f == 1)
			{
                max = _enemyManager.EnemyShotList.Count;
            }
            //表示を更新
            for (int i = 0; i < 20; i++)
            {
                //参照しているマーカーオブジェクト
                GameObject markerObject = null;
                if (f == 0)
                {
                    markerObject = _enemyMarkerList[i];
                }
                else if (f == 1)
                {
                    markerObject = _enemyShotMarkerList[i];
                }

                if (i < max)
                {
                    //位置を更新
                    Vector3 position = Vector3.zero;
                    if(f == 0)
					{
                        position = _enemyManager.EnemyList[i].transform.localPosition;
                    }
                    else if(f == 1)
					{
                        position = _enemyManager.EnemyShotList[i].transform.localPosition;
                    }
                    //座標を設定
                    RectTransform markerPosition = markerObject.GetComponent<RectTransform>();
                    //実際の座標÷3
                    markerPosition.localPosition = new Vector3(position.x / 3, position.z / 3, 0) ;
                    //表示する
                    markerObject.SetActive(true);
                }
                else
                {
                    //非表示にする
                    markerObject.SetActive(false);
                }
            }
        }


        //プレイヤーの向いてる角度
        float angle = PlayerBase.GetInstance().transform.localEulerAngles.y;
        //アイコンのほうに向きのほうを設定
        _playerIcon.transform.localEulerAngles = new Vector3(0, 0, angle * -1);
    }
}
