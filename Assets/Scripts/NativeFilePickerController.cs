using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

/// <summary>
/// OS 네이티브 파일 선택 창 호출 컨트롤러
/// 배치 위치: Panel_Main 오브젝트 (UIController와 같은 오브젝트 권장)
///
/// ── 방식 선택 ───────────────────────────────────────────
///  [권장] SimpleFileBrowser (무료 에셋)
///        Asset Store: "Runtime File Browser" by Süleyman Yasir KULA
///        https://assetstore.unity.com/packages/tools/gui/runtime-file-browser-113006
///
///  [대안] SFB (StandaloneFileBrowser, GitHub 오픈소스)
///        https://github.com/gkngkc/UnityStandaloneFileBrowser
///
///  [에디터 전용] EditorUtility.OpenFilePanel
///        빌드에서는 동작하지 않으므로 데모/개발 시에만 사용
/// ──────────────────────────────────────────────────────
/// </summary>
public class NativeFilePickerController : MonoBehaviour
{
    [Header("참조")]
    public InspectionManager inspectionManager;

    [Header("파일 필터")]
    public string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp" };

    // ──────────────────────────────────────────
    /// <summary>
    /// [버튼 OnClick 연결용]
    /// OS 네이티브 파일 선택 창 열기
    /// </summary>
    public void OpenFilePicker()
    {
        // ── NativeFilePicker 에셋 사용 시 ──────────────────
        // Asset Store에서 "NativeFilePicker" 임포트 후 아래 주석 해제

        NativeFilePicker.PickFile(
            (path) =>
            {
                if (!string.IsNullOrEmpty(path))
                    inspectionManager.RunInspection(path);
            },
            new string[] { "image/*" }
        );

        // ── ① SimpleFileBrowser 에셋 사용 시 ──────────────────
        // Asset Store에서 "Runtime File Browser" 임포트 후 아래 주석 해제
        /*
                SimpleFileBrowser.FileBrowser.SetFilters(true,
                    new SimpleFileBrowser.FileBrowser.Filter("Images", allowedExtensions));
                SimpleFileBrowser.FileBrowser.SetDefaultFilter(".png");

                SimpleFileBrowser.FileBrowser.ShowLoadDialog(
                    onSuccess: (paths) =>
                    {
                        if (paths != null && paths.Length > 0)
                            inspectionManager.RunInspection(paths[0]);
                    },
                    onCancel: () => Debug.Log("[FilePicker] 취소"),
                    pickMode: SimpleFileBrowser.FileBrowser.PickMode.Files,
                    allowMultiSelection: false,
                    title: "이미지 파일 선택",
                    loadButtonText: "선택"
                );
        */

        // ── ② StandaloneFileBrowser (SFB) 사용 시 ─────────────
        // GitHub에서 SFB 임포트 후 아래 주석 해제
        /*
                var extensions = new SFB.ExtensionFilter[]
                {
                    new SFB.ExtensionFilter("Image Files", "png", "jpg", "jpeg", "bmp")
                };

                string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel("이미지 파일 선택", "", extensions, false);

                if (paths != null && paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
                    inspectionManager.RunInspection(paths[0]);
        */

        // ── ③ 에디터 전용 (빌드 불가, 개발/데모용) ────────────
        /*
        #if UNITY_EDITOR
                string path = UnityEditor.EditorUtility.OpenFilePanelWithFilters(
                    "이미지 파일 선택",
                    "",
                    new string[] { "Image files", "png,jpg,jpeg,bmp", "All files", "*" }
                );

                if (!string.IsNullOrEmpty(path))
                    inspectionManager.RunInspection(path);
        #else
                // 빌드 환경: 위 ① 또는 ② 방식 중 하나를 선택 후 주석 해제
                Debug.LogWarning("[FilePicker] 빌드 환경에서는 SimpleFileBrowser 또는 SFB를 사용하세요.");
        #endif
        */
    }
}
