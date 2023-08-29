using System.Collections;
using Unity.Advertisement.IosSupport;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public sealed class IDFA : MonoBehaviour
{
    private IEnumerator Start()
    {
#if UNITY_IOS
        // まだ許可ダイアログを表示したことがない場合
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
             ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            // 許可ダイアログを表示します
            ATTrackingStatusBinding.RequestAuthorizationTracking();
 
            // 許可ダイアログで「App にトラッキングしないように要求」か
            // 「トラッキングを許可」のどちらかが選択されるまで待機します
            while (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                    ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                yield return null;
            }
        }
 
        // IDFA（広告 ID）をログ出力します
        // トラッキングが許可されている場合は IDFA が文字列で出力されます
        // 許可されていない場合は「00000000-0000-0000-0000-000000000000」が出力されます  
        Debug.Log(Device.advertisingIdentifier);
#else
        yield return null;
#endif
    }
}
