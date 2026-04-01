using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이미지 처리 모듈: Grayscale 변환 → Threshold 이진화
/// </summary>
public class ImageProcessor : MonoBehaviour
{
    [Header("Threshold 설정")]
    [Range(0f, 1f)]
    [Tooltip("이 값보다 밝은 픽셀 → White / 어두운 픽셀 → Black")]
    public float threshold = 0.5f;
    public Slider threshold_Slider;
    public TextMeshProUGUI threshold_Text;

    [Header("리사이즈 최적화")]
    public bool enableResize   = false;
    public int  resizeMaxWidth = 512;

    public void OnValueChanged_Threshold(float _value)
    {
        threshold = _value;
        threshold_Text.text = _value.ToString("N2");
    }

    private void Start()
    {
        threshold_Slider.value = threshold;
        OnValueChanged_Threshold(threshold);
    }

    public Texture2D Process(Texture2D source)
    {
        if (source == null) return null;

        Texture2D target = enableResize ? ResizeTexture(source, resizeMaxWidth) : source;

        int      w      = target.width;
        int      h      = target.height;
        Color32[] src   = target.GetPixels32();
        Color32[] dst   = new Color32[src.Length];

        for (int i = 0; i < src.Length; i++)
        {
            // NTSC Luminance: 인간 시각 민감도 기반 Grayscale
            float gray = (src[i].r * 0.299f +
                          src[i].g * 0.587f +
                          src[i].b * 0.114f) / 255f;

            byte val = gray > threshold ? (byte)255 : (byte)0;
            dst[i]   = new Color32(val, val, val, 255);
        }

        Texture2D result = new Texture2D(w, h, TextureFormat.RGBA32, false);
        result.SetPixels32(dst);
        result.Apply();
        return result;
    }

    private Texture2D ResizeTexture(Texture2D src, int maxWidth)
    {
        if (src.width <= maxWidth) return src;

        float ratio = (float)maxWidth / src.width;
        int   newW  = maxWidth;
        int   newH  = Mathf.RoundToInt(src.height * ratio);

        RenderTexture rt = RenderTexture.GetTemporary(newW, newH, 0, RenderTextureFormat.ARGB32);
        RenderTexture.active = rt;
        Graphics.Blit(src, rt);

        Texture2D resized = new Texture2D(newW, newH, TextureFormat.RGBA32, false);
        resized.ReadPixels(new Rect(0, 0, newW, newH), 0, 0);
        resized.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return resized;
    }
}
