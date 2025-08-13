using UnityEngine;
using System;

/// <summary>
/// Qu?n lý h? th?ng ?i?m s? và level c?a game Tetris
/// </summary>
public class ScoreManager : MonoBehaviour
{
    #region Events - S? ki?n
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnLevelChanged;
    public static event Action<int> OnLinesChanged;
    public static event Action<int> OnHighScoreChanged;
    #endregion

    #region Properties - Thu?c tính
    [Header("C?u hình ?i?m s?")]
    [Tooltip("?i?m c? b?n cho m?i hàng xóa")]
    public int diemCobanMoiHang = 100;

    [Tooltip("?i?m th??ng khi xóa nhi?u hàng cùng lúc")]
    public int[] diemThuongXoaHang = { 0, 40, 100, 300, 1200 }; // 0, 1, 2, 3, 4 hàng

    [Tooltip("S? hàng c?n xóa ?? t?ng level")]
    public int soHangMoiLevel = 10;

    /// <summary>
    /// ?i?m s? hi?n t?i
    /// </summary>
    public int DiemSoHienTai { get; private set; } = 0;

    /// <summary>
    /// Level hi?n t?i
    /// </summary>
    public int LevelHienTai { get; private set; } = 1;

    /// <summary>
    /// S? hàng ?ã xóa
    /// </summary>
    public int SoHangDaXoa { get; private set; } = 0;

    /// <summary>
    /// ?i?m cao nh?t
    /// </summary>
    public int DiemCaoNhat { get; private set; } = 0;
    #endregion

    #region Unity Lifecycle - Vòng ??i Unity
    private void Awake()
    {
        // ??ng ký singleton n?u c?n
        if (FindObjectsOfType<ScoreManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        TaiDiemCaoNhat();
    }

    private void OnEnable()
    {
        // ??ng ký event t? Board khi xóa hàng
        Board.OnLinesCleared += XuLyXoaHang;
    }

    private void OnDisable()
    {
        Board.OnLinesCleared -= XuLyXoaHang;
    }
    #endregion

    #region X? lý ?i?m s? - Score Handling
    /// <summary>
    /// X? lý khi xóa hàng
    /// </summary>
    /// <param name="soHangXoa">S? hàng ?ã xóa</param>
    public void XuLyXoaHang(int soHangXoa)
    {
        if (soHangXoa <= 0) return;

        // Tính ?i?m
        int diemThuong = diemThuongXoaHang[Mathf.Min(soHangXoa, diemThuongXoaHang.Length - 1)];
        int diemNhan = (diemThuong + (soHangXoa * diemCobanMoiHang)) * LevelHienTai;

        // C?p nh?t ?i?m
        DiemSoHienTai += diemNhan;
        SoHangDaXoa += soHangXoa;

        // Ki?m tra t?ng level
        int levelMoi = (SoHangDaXoa / soHangMoiLevel) + 1;
        if (levelMoi > LevelHienTai)
        {
            LevelHienTai = levelMoi;
            OnLevelChanged?.Invoke(LevelHienTai);
        }

        // Ki?m tra ?i?m cao
        if (DiemSoHienTai > DiemCaoNhat)
        {
            DiemCaoNhat = DiemSoHienTai;
            LuuDiemCaoNhat();
            OnHighScoreChanged?.Invoke(DiemCaoNhat); // Gọi sự kiện
        }

        // Thông báo s? ki?n
        OnScoreChanged?.Invoke(DiemSoHienTai);
        OnLinesChanged?.Invoke(SoHangDaXoa);

        Debug.Log($"Xóa {soHangXoa} hàng, nh?n {diemNhan} ?i?m. T?ng: {DiemSoHienTai}");
    }

    /// <summary>
    /// ??t l?i ?i?m s? v? tr?ng thái ban ??u
    /// </summary>
    public void DatLaiDiemSo()
    {
        DiemSoHienTai = 0;
        LevelHienTai = 1;
        SoHangDaXoa = 0;

        OnScoreChanged?.Invoke(DiemSoHienTai);
        OnLevelChanged?.Invoke(LevelHienTai);
        OnLinesChanged?.Invoke(SoHangDaXoa);
    }

    /// <summary>
    /// Tính t?c ?? r?i d?a trên level
    /// </summary>
    /// <returns>Th?i gian gi?a các b??c r?i</returns>
    public float TinhTocDoRoi()
    {
        // Công th?c t?c ??: t?c ?? t?ng theo level
        float tocDoCoban = 1.0f;
        float heSoGiam = 0.1f;
        return Mathf.Max(0.1f, tocDoCoban - ((LevelHienTai - 1) * heSoGiam));
    }
    #endregion

    #region L?u/T?i d? li?u - Save/Load
    /// <summary>
    /// L?u ?i?m cao nh?t
    /// </summary>
    private void LuuDiemCaoNhat()
    {
        PlayerPrefs.SetInt("HighScore", DiemCaoNhat);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// T?i ?i?m cao nh?t
    /// </summary>
    private void TaiDiemCaoNhat()
    {
        DiemCaoNhat = PlayerPrefs.GetInt("HighScore", 0);
    }
    #endregion

    #region Compatibility - T??ng thích
    public static ScoreManager Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Ph??ng th?c t??ng thích
    public void AddScore(int points) => XuLyXoaHang(1);
    public void ResetScore() => DatLaiDiemSo();
    public float GetFallSpeed() => TinhTocDoRoi();
    public int Score => DiemSoHienTai;
    public int Level => LevelHienTai;
    public int Lines => SoHangDaXoa;
    public int HighScore => DiemCaoNhat;
    #endregion
}

/// <summary>
/// Extension cho Board class ?? h? tr? events
/// </summary>
public static class BoardExtensions
{
    public static event Action<int> OnLinesCleared;

    /// <summary>
    /// G?i event khi xóa hàng (c?n ???c g?i t? Board.XuLyXoaHang)
    /// </summary>
    public static void NotifyLinesCleared(int linesCleared)
    {
        OnLinesCleared?.Invoke(linesCleared);
    }
}