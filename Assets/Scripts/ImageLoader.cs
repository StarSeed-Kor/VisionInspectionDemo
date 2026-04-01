using UnityEngine;
using System.IO;

/// <summary>
/// 이미지 로더: 절대 경로 → Texture2D
/// </summary>
public class ImageLoader : MonoBehaviour
{
    public Texture2D Load(string absolutePath)
    {
        if (string.IsNullOrEmpty(absolutePath) || !File.Exists(absolutePath))
        {
            Debug.LogWarning($"[ImageLoader] 파일 없음: {absolutePath}");
            return null;
        }

        try
        {
            byte[]    bytes = File.ReadAllBytes(absolutePath);
            Texture2D tex   = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            if (!tex.LoadImage(bytes))
            {
                Debug.LogError($"[ImageLoader] LoadImage 실패: {absolutePath}");
                Destroy(tex);
                return null;
            }

            tex.name = Path.GetFileName(absolutePath);
            Debug.Log($"[ImageLoader] 로드: {tex.name} ({tex.width}x{tex.height})");
            return tex;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ImageLoader] 예외: {e.Message}");
            return null;
        }
    }
}
