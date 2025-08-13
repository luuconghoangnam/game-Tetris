using UnityEngine;

/// <summary>
/// Qu?n lý âm thanh c?a game
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Properties - Thu?c tính
    [Header("Audio Sources")]
    [Tooltip("Audio source cho nh?c n?n")]
    public AudioSource audioSourceNhacNen;

    [Tooltip("Audio source cho hi?u ?ng âm thanh")]
    public AudioSource audioSourceHieuUng;

    [Header("Audio Clips")]
    [Tooltip("Nh?c n?n chính")]
    public AudioClip nhacNenChinh;

    [Tooltip("Âm thanh khi xóa hàng")]
    public AudioClip amThanhXoaHang;

    [Tooltip("Âm thanh khi di chuy?n")]
    public AudioClip amThanhDiChuyen;

    [Tooltip("Âm thanh khi xoay")]
    public AudioClip amThanhXoay;

    [Tooltip("Âm thanh game over")]
    public AudioClip amThanhGameOver;
    #endregion

    #region Unity Lifecycle - Vòng ??i Unity
    private void Awake()
    {
        // ??m b?o ch? có m?t AudioManager
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PhatNhacNen();
    }

    private void OnEnable()
    {
        // ??ng ký events
        if (FindObjectOfType<ScoreManager>())
        {
            ScoreManager.OnLevelChanged += PhatAmThanhLevelUp;
        }
    }
    #endregion

    #region Phát âm thanh - Audio Playback
    /// <summary>
    /// Phát nh?c n?n
    /// </summary>
    public void PhatNhacNen()
    {
        if (audioSourceNhacNen && nhacNenChinh)
        {
            audioSourceNhacNen.clip = nhacNenChinh;
            audioSourceNhacNen.loop = true;
            audioSourceNhacNen.Play();
        }
    }

    /// <summary>
    /// Phát hi?u ?ng âm thanh
    /// </summary>
    public void PhatHieuUngAmThanh(AudioClip clip)
    {
        if (audioSourceHieuUng && clip)
        {
            audioSourceHieuUng.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Phát âm thanh xóa hàng
    /// </summary>
    public void PhatAmThanhXoaHang()
    {
        PhatHieuUngAmThanh(amThanhXoaHang);
    }

    /// <summary>
    /// Phát âm thanh di chuy?n
    /// </summary>
    public void PhatAmThanhDiChuyen()
    {
        PhatHieuUngAmThanh(amThanhDiChuyen);
    }

    /// <summary>
    /// Phát âm thanh xoay
    /// </summary>
    public void PhatAmThanhXoay()
    {
        PhatHieuUngAmThanh(amThanhXoay);
    }

    /// <summary>
    /// Phát âm thanh game over
    /// </summary>
    public void PhatAmThanhGameOver()
    {
        PhatHieuUngAmThanh(amThanhGameOver);
    }

    /// <summary>
    /// Phát âm thanh level up
    /// </summary>
    private void PhatAmThanhLevelUp(int level)
    {
        // Có th? thêm âm thanh riêng cho level up
        PhatAmThanhXoaHang();
    }
    #endregion

    #region ?i?u khi?n âm l??ng - Volume Control
    /// <summary>
    /// ??t âm l??ng nh?c n?n
    /// </summary>
    public void DatAmLuongNhacNen(float amLuong)
    {
        if (audioSourceNhacNen)
        {
            audioSourceNhacNen.volume = Mathf.Clamp01(amLuong);
        }
    }

    /// <summary>
    /// ??t âm l??ng hi?u ?ng
    /// </summary>
    public void DatAmLuongHieuUng(float amLuong)
    {
        if (audioSourceHieuUng)
        {
            audioSourceHieuUng.volume = Mathf.Clamp01(amLuong);
        }
    }

    /// <summary>
    /// T?t/b?t âm thanh
    /// </summary>
    public void ChuyenDoiAmThanh()
    {
        if (audioSourceNhacNen) audioSourceNhacNen.mute = !audioSourceNhacNen.mute;
        if (audioSourceHieuUng) audioSourceHieuUng.mute = !audioSourceHieuUng.mute;
    }
    #endregion

    #region Compatibility - T??ng thích
    public void PlayBackgroundMusic() => PhatNhacNen();
    public void PlaySoundEffect(AudioClip clip) => PhatHieuUngAmThanh(clip);
    public void PlayLineClearSound() => PhatAmThanhXoaHang();
    public void PlayMoveSound() => PhatAmThanhDiChuyen();
    public void PlayRotateSound() => PhatAmThanhXoay();
    public void PlayGameOverSound() => PhatAmThanhGameOver();
    public void SetMusicVolume(float volume) => DatAmLuongNhacNen(volume);
    public void SetSFXVolume(float volume) => DatAmLuongHieuUng(volume);
    public void ToggleSound() => ChuyenDoiAmThanh();
    #endregion
}