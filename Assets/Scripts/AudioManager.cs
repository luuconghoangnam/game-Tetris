using UnityEngine;

/// <summary>
/// Qu?n l� �m thanh c?a game
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Properties - Thu?c t�nh
    [Header("Audio Sources")]
    [Tooltip("Audio source cho nh?c n?n")]
    public AudioSource audioSourceNhacNen;

    [Tooltip("Audio source cho hi?u ?ng �m thanh")]
    public AudioSource audioSourceHieuUng;

    [Header("Audio Clips")]
    [Tooltip("Nh?c n?n ch�nh")]
    public AudioClip nhacNenChinh;

    [Tooltip("�m thanh khi x�a h�ng")]
    public AudioClip amThanhXoaHang;

    [Tooltip("�m thanh khi di chuy?n")]
    public AudioClip amThanhDiChuyen;

    [Tooltip("�m thanh khi xoay")]
    public AudioClip amThanhXoay;

    [Tooltip("�m thanh game over")]
    public AudioClip amThanhGameOver;
    #endregion

    #region Unity Lifecycle - V�ng ??i Unity
    private void Awake()
    {
        // ??m b?o ch? c� m?t AudioManager
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
        // ??ng k� events
        if (FindObjectOfType<ScoreManager>())
        {
            ScoreManager.OnLevelChanged += PhatAmThanhLevelUp;
        }
    }
    #endregion

    #region Ph�t �m thanh - Audio Playback
    /// <summary>
    /// Ph�t nh?c n?n
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
    /// Ph�t hi?u ?ng �m thanh
    /// </summary>
    public void PhatHieuUngAmThanh(AudioClip clip)
    {
        if (audioSourceHieuUng && clip)
        {
            audioSourceHieuUng.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Ph�t �m thanh x�a h�ng
    /// </summary>
    public void PhatAmThanhXoaHang()
    {
        PhatHieuUngAmThanh(amThanhXoaHang);
    }

    /// <summary>
    /// Ph�t �m thanh di chuy?n
    /// </summary>
    public void PhatAmThanhDiChuyen()
    {
        PhatHieuUngAmThanh(amThanhDiChuyen);
    }

    /// <summary>
    /// Ph�t �m thanh xoay
    /// </summary>
    public void PhatAmThanhXoay()
    {
        PhatHieuUngAmThanh(amThanhXoay);
    }

    /// <summary>
    /// Ph�t �m thanh game over
    /// </summary>
    public void PhatAmThanhGameOver()
    {
        PhatHieuUngAmThanh(amThanhGameOver);
    }

    /// <summary>
    /// Ph�t �m thanh level up
    /// </summary>
    private void PhatAmThanhLevelUp(int level)
    {
        // C� th? th�m �m thanh ri�ng cho level up
        PhatAmThanhXoaHang();
    }
    #endregion

    #region ?i?u khi?n �m l??ng - Volume Control
    /// <summary>
    /// ??t �m l??ng nh?c n?n
    /// </summary>
    public void DatAmLuongNhacNen(float amLuong)
    {
        if (audioSourceNhacNen)
        {
            audioSourceNhacNen.volume = Mathf.Clamp01(amLuong);
        }
    }

    /// <summary>
    /// ??t �m l??ng hi?u ?ng
    /// </summary>
    public void DatAmLuongHieuUng(float amLuong)
    {
        if (audioSourceHieuUng)
        {
            audioSourceHieuUng.volume = Mathf.Clamp01(amLuong);
        }
    }

    /// <summary>
    /// T?t/b?t �m thanh
    /// </summary>
    public void ChuyenDoiAmThanh()
    {
        if (audioSourceNhacNen) audioSourceNhacNen.mute = !audioSourceNhacNen.mute;
        if (audioSourceHieuUng) audioSourceHieuUng.mute = !audioSourceHieuUng.mute;
    }
    #endregion

    #region Compatibility - T??ng th�ch
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