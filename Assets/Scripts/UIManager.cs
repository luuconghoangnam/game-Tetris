using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps; // Đảm bảo đã using

/// <summary>
/// Qu?n lý UI c?a game Tetris
/// </summary>
public class UIManager : MonoBehaviour
{
    #region UI Elements - Thành ph?n UI
    [Header("Text hi?n th?")]
    [Tooltip("Text hi?n th? ?i?m s?")]
    public TextMeshProUGUI textDiemSo;

    [Tooltip("Text hi?n th? level")]
    public TextMeshProUGUI textLevel;

    [Tooltip("Text hi?n th? s? hàng ?ã xóa")]
    public TextMeshProUGUI textSoHang;

    [Tooltip("Text hi?n th? ?i?m cao nh?t")]
    public TextMeshProUGUI textDiemCao;

    [Header("Menu Panel High Score")]
    [Tooltip("Text hi?n th? high score ? menu chính")]
    public TextMeshProUGUI textHighScoreMenu;

    [Header("Game Over Panel Texts")]
    [Tooltip("Text hiển thị điểm cuối cùng")]
    public TextMeshProUGUI textFinalScore;
    [Tooltip("Text hiển thị high score ở game over")]
    public TextMeshProUGUI textHighScoreGameOver;
    [Tooltip("Text hiển thị thông báo kỷ lục mới")]
    public TextMeshProUGUI textNewRecord;

    [Header("Panels")]
    [Tooltip("Panel game over")]
    public GameObject panelGameOver;

    [Tooltip("Panel pause")]
    public GameObject panelPause;

    [Tooltip("Panel menu chính")]
    public GameObject panelMenuChinh;

    [Header("Buttons")]
    [Tooltip("Nút b?t ??u game")]
    public Button nutBatDau;

    [Tooltip("Nút pause")]
    public Button nutPause;

    [Tooltip("Nút restart")]
    public Button nutRestart;

    [Tooltip("Nút menu")]
    public Button nutMenu;

    [Tooltip("Nút ti?p t?c")]
    public Button nutTiepTuc;

    [Header("Next Block Display")]
    [Tooltip("Tilemap dùng để hiển thị block tiếp theo")]
    public Tilemap nextBlockTilemap;
    #endregion

    #region Unity Lifecycle - Vòng ??i Unity
    private void Awake()
    {
        KhoiTaoUI();
    }

    private void OnEnable()
    {
        DangKyEvents();
    }

    private void OnDisable()
    {
        HuyDangKyEvents();
    }
    #endregion

    #region Kh?i t?o - Initialization
    /// <summary>
    /// Kh?i t?o UI ban ??u
    /// </summary>
    private void KhoiTaoUI()
    {
        // Thi?t l?p tr?ng thái ban ??u
        if (panelGameOver) panelGameOver.SetActive(false);
        if (panelPause) panelPause.SetActive(false);
        if (panelMenuChinh) panelMenuChinh.SetActive(true);

        // Gán s? ki?n cho các nút
        if (nutBatDau) nutBatDau.onClick.AddListener(BatDauGame);
        if (nutPause) nutPause.onClick.AddListener(PauseGame);
        if (nutRestart) nutRestart.onClick.AddListener(KhoiDongLaiGame);
        if (nutMenu) nutMenu.onClick.AddListener(VeMenu);
        if (nutTiepTuc) nutTiepTuc.onClick.AddListener(TiepTucGame);
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager)
            CapNhatDiemCao(scoreManager.DiemCaoNhat);
    }

    /// <summary>
    /// ??ng ký các s? ki?n t? ScoreManager
    /// </summary>
    private void DangKyEvents()
    {
        ScoreManager.OnScoreChanged += CapNhatDiemSo;
        ScoreManager.OnLevelChanged += CapNhatLevel;
        ScoreManager.OnLinesChanged += CapNhatSoHang;
        ScoreManager.OnHighScoreChanged += CapNhatDiemCao;
    }

    /// <summary>
    /// H?y ??ng ký các s? ki?n
    /// </summary>
    private void HuyDangKyEvents()
    {
        ScoreManager.OnScoreChanged -= CapNhatDiemSo;
        ScoreManager.OnLevelChanged -= CapNhatLevel;
        ScoreManager.OnLinesChanged -= CapNhatSoHang;
        ScoreManager.OnHighScoreChanged -= CapNhatDiemCao;
    }
    #endregion

    #region C?p nh?t UI - UI Updates
    /// <summary>
    /// C?p nh?t hi?n th? ?i?m s?
    /// </summary>
    private void CapNhatDiemSo(int diemSo)
    {
        if (textDiemSo) textDiemSo.text = $"Score: {diemSo:N0}";
    }

    /// <summary>
    /// C?p nh?t hi?n th? level
    /// </summary>
    private void CapNhatLevel(int level)
    {
        if (textLevel) textLevel.text = $"Level: {level}";
    }

    /// <summary>
    /// C?p nh?t hi?n th? s? hàng ?ã xóa
    /// </summary>
    private void CapNhatSoHang(int soHang)
    {
        if (textSoHang) textSoHang.text = $"Lines: {soHang}";
    }

    /// <summary>
    /// C?p nh?t ?i?m cao nh?t
    /// </summary>
    public void CapNhatDiemCao(int diemCao)
    {
        if (textDiemCao) textDiemCao.text = $"High Score: {diemCao:N0}";
    }
    #endregion

    #region X? lý nút b?m - Button Handlers
    /// <summary>
    /// B?t ??u game m?i
    /// </summary>
    public void BatDauGame()
    {
        if (panelMenuChinh) panelMenuChinh.SetActive(false);
        
        // Tìm GameManager và b?t ??u game
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager)
        {
            gameManager.BatDauGameMoi();
        }
    }

    /// <summary>
    /// Pause game
    /// </summary>
    public void PauseGame()
    {
        if (panelPause) panelPause.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Ti?p t?c game
    /// </summary>
    public void TiepTucGame()
    {
        if (panelPause) panelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Kh?i ??ng l?i game
    /// </summary>
    public void KhoiDongLaiGame()
    {
        Time.timeScale = 1f;
        if (panelGameOver) panelGameOver.SetActive(false);
        
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager)
        {
            gameManager.KhoiDongLaiGame();
        }
    }

    /// <summary>
    /// V? menu chính
    /// </summary>
    public void VeMenu()
    {
        Time.timeScale = 1f;
        if (panelGameOver) panelGameOver.SetActive(false);
        if (panelPause) panelPause.SetActive(false);
        if (panelMenuChinh) panelMenuChinh.SetActive(true);

        // Cập nhật high score ở menu
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager && textHighScoreMenu)
        {
            textHighScoreMenu.text = $"High Score: {scoreManager.DiemCaoNhat:N0}";
        }
    }

    /// <summary>
    /// Hi?n th? màn hình game over
    /// </summary>
    public void HienThiGameOver()
    {
        if (panelGameOver) panelGameOver.SetActive(true);

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager)
        {
            // Hiển thị điểm cuối cùng
            if (textFinalScore)
                textFinalScore.text = $"Final Score: {scoreManager.DiemSoHienTai:N0}";

            // Hiển thị high score
            if (textHighScoreGameOver)
                textHighScoreGameOver.text = $"High Score: {scoreManager.DiemCaoNhat:N0}";

            // Hiển thị "New Record" nếu đạt kỷ lục mới
            if (textNewRecord)
                textNewRecord.gameObject.SetActive(scoreManager.DiemSoHienTai >= scoreManager.DiemCaoNhat && scoreManager.DiemSoHienTai > 0);
        }
    }

    /// <summary>
    /// Thoát game (dùng cho nút Exit)
    /// </summary>
    public void ThoatGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    #region Input handling
    private void Update()
    {
        // X? lý phím ESC ?? pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelPause && panelPause.activeInHierarchy)
            {
                TiepTucGame();
            }
            else if (panelMenuChinh && !panelMenuChinh.activeInHierarchy)
            {
                PauseGame();
            }
        }
    }
    #endregion

    #region Compatibility - T??ng thích
    public void UpdateScore(int score) => CapNhatDiemSo(score);
    public void UpdateLevel(int level) => CapNhatLevel(level);
    public void UpdateLines(int lines) => CapNhatSoHang(lines);
    public void ShowGameOver() => HienThiGameOver();
    public void StartGame() => BatDauGame();
    public void RestartGame() => KhoiDongLaiGame();
    public void BackToMenu() => VeMenu();
    #endregion

    #region Next Block Display
    public void HienThiBlockTiepTheo(TetrominoData data)
    {
        if (nextBlockTilemap == null) return;
        nextBlockTilemap.ClearAllTiles();

        // Tính offset để block nằm giữa panel nhỏ (tùy chỉnh cho đẹp)
        Vector3Int offset = new Vector3Int(2, 2, 0);

        foreach (var cell in data.cells)
        {
            var pos = (Vector3Int)cell + offset;
            nextBlockTilemap.SetTile(pos, data.tile);
        }
    }
    #endregion
}