using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーダー用アイコンの角度を調整
/// </summary>
public class RaderIcon : MonoBehaviour
{
	void Update()
	{
		//親オブジェクトの角度
		Vector3 parentAngle = transform.parent.transform.localRotation.eulerAngles;
		//角度修正
		gameObject.transform.rotation = Quaternion.Euler(90, parentAngle.y, parentAngle.z);
	}
}
