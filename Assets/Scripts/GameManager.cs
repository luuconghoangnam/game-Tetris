using UnityEngine;

/// <summary>
/// Qu?n lý tr?ng thái t?ng quát c?a game
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Enums - Li?t kê
    public enum TrangThaiGame
    {
        Menu,
        DangChoi,
        Pause,
        GameOver
    }
    #endregion

    #region Properties - Thu?c tính
    [Header("Tham chi?u components")]
    [Tooltip("Board chính c?a game")]
    public Board boardChinh;

    [Tooltip("UI Manager")]
    public UIManager uiManager;

    [Tooltip("Score Manager")]
    public ScoreManager scoreManager;

    [Tooltip("Piece ?ang ch?i")]
    public Piece pieceChinh;

    /// <summary>
    /// Tr?ng thái hi?n t?i c?a game
    /// </summary>
    public TrangThaiGame TrangThaiHienTai { get; private set; } = TrangThaiGame.Menu;

    /// <summary>
    /// Game ?ang ch?y hay không
    /// </summary>
    public bool GameDangChay => TrangThaiHienTai == TrangThaiGame.DangChoi;
    #endregion

    #region Unity Lifecycle - Vòng ??i Unity
    private void Awake()
    {
        KhoiTaoGameManager();
    }

    private void Start()
    {
        ChuyenTrangThai(TrangThaiGame.Menu);
    }

    private void Update()
    {
        XuLyInputGame();
    }
    #endregion

    #region Kh?i t?o - Initialization
    /// <summary>
    /// Kh?i t?o GameManager
    /// </summary>
    private void KhoiTaoGameManager()
    {
        // T? ??ng tìm các component n?u ch?a ???c gán
        if (!boardChinh) boardChinh = FindObjectOfType<Board>();
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();
        if (!scoreManager) scoreManager = FindObjectOfType<ScoreManager>();
        if (!pieceChinh) pieceChinh = FindObjectOfType<Piece>();

        // ??m b?o framerate ?n ??nh
        Application.targetFrameRate = 60;
    }
    #endregion

    #region Qu?n lý tr?ng thái - State Management
    /// <summary>
    /// Chuy?n tr?ng thái game
    /// </summary>
    public void ChuyenTrangThai(TrangThaiGame trangThaiMoi)
    {
        TrangThaiGame trangThaiCu = TrangThaiHienTai;
        TrangThaiHienTai = trangThaiMoi;

        // X? lý chuy?n tr?ng thái
        switch (trangThaiMoi)
        {
            case TrangThaiGame.Menu:
                XuLyChuyenVeMenu();
                break;
            case TrangThaiGame.DangChoi:
                XuLyBatDauChoi();
                break;
            case TrangThaiGame.Pause:
                XuLyPause();
                break;
            case TrangThaiGame.GameOver:
                XuLyGameOver();
                break;
        }

        Debug.Log($"Chuy?n tr?ng thái t? {trangThaiCu} sang {trangThaiMoi}");
    }

    /// <summary>
    /// X? lý chuy?n v? menu
    /// </summary>
    private void XuLyChuyenVeMenu()
    {
        Time.timeScale = 1f;
        if (boardChinh) boardChinh.tilemap.ClearAllTiles();
    }

    /// <summary>
    /// X? lý b?t ??u ch?i
    /// </summary>
    private void XuLyBatDauChoi()
    {
        Time.timeScale = 1f;
        if (scoreManager) scoreManager.DatLaiDiemSo();
        if (boardChinh) 
        {
            boardChinh.tilemap.ClearAllTiles();
            boardChinh.TaoKhoiMoi();
        }
    }

    /// <summary>
    /// X? lý pause
    /// </summary>
    private void XuLyPause()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// X? lý game over
    /// </summary>
    private void XuLyGameOver()
    {
        if (uiManager) uiManager.HienThiGameOver();
        Debug.Log("Game Over!");
    }
    #endregion

    #region Ph??ng th?c công khai - Public Methods
    /// <summary>
    /// B?t ??u game m?i
    /// </summary>
    public void BatDauGameMoi()
    {
        ChuyenTrangThai(TrangThaiGame.DangChoi);
    }

    /// <summary>
    /// Kh?i ??ng l?i game
    /// </summary>
    public void KhoiDongLaiGame()
    {
        ChuyenTrangThai(TrangThaiGame.DangChoi);
    }

    /// <summary>
    /// Pause/Unpause game
    /// </summary>
    public void ChuyenDoiPause()
    {
        if (TrangThaiHienTai == TrangThaiGame.DangChoi)
        {
            ChuyenTrangThai(TrangThaiGame.Pause);
        }
        else if (TrangThaiHienTai == TrangThaiGame.Pause)
        {
            ChuyenTrangThai(TrangThaiGame.DangChoi);
        }
    }

    /// <summary>
    /// K?t thúc game
    /// </summary>
    public void KetThucGame()
    {
        ChuyenTrangThai(TrangThaiGame.GameOver);
    }

    /// <summary>
    /// V? menu chính
    /// </summary>
    public void VeMenuChinh()
    {
        ChuyenTrangThai(TrangThaiGame.Menu);
    }
    #endregion

    #region Input handling - X? lý input
    /// <summary>
    /// X? lý input chung c?a game
    /// </summary>
    private void XuLyInputGame()
    {
        // Ch? x? lý input khi game ?ang ch?y
        if (!GameDangChay) return;

        // C?p nh?t t?c ?? r?i theo level
        if (pieceChinh && scoreManager)
        {
            float tocDoMoi = scoreManager.TinhTocDoRoi();
            pieceChinh.thoiGianGiuaCacBuoc = tocDoMoi;
        }
    }
    #endregion

    #region Compatibility - T??ng thích
    public void StartNewGame() => BatDauGameMoi();
    public void RestartGame() => KhoiDongLaiGame();
    public void TogglePause() => ChuyenDoiPause();
    public void EndGame() => KetThucGame();
    public void BackToMenu() => VeMenuChinh();
    public bool IsGameRunning => GameDangChay;
    public TrangThaiGame CurrentState => TrangThaiHienTai;
    #endregion
}