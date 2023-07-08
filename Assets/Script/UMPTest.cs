using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMPTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        AdmobUMP.OnShowStart = () =>
        {
            Debug.Log("Show");
           // _view.gameObject.SetActive(false);
        };
        AdmobUMP.OnShowEnd = () =>
        {
            Debug.Log("Show End ");
          //  _view.gameObject.SetActive(true);
        };
        AdmobUMP.FirstSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
