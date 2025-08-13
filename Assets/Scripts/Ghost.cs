using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class quản lý bóng ma (ghost piece) hiển thị vị trí khối sẽ rơi xuống
/// </summary>
public class Ghost : MonoBehaviour
{
    #region Properties - Thuộc tính
    [Header("Cấu hình bóng ma")]
    [Tooltip("Tile dùng để hiển thị bóng ma")]
    public Tile tileBongMa;

    [Tooltip("Bảng chơi chính")]
    public Board bangChoiChinh;

    [Tooltip("Khối đang theo dõi")]
    public Piece khoiDangTheoDoi;

    /// <summary>
    /// Tilemap để vẽ bóng ma
    /// </summary>
    public Tilemap tilemap { get; private set; }

    /// <summary>
    /// Các ô vuông của bóng ma
    /// </summary>
    public Vector3Int[] cacOVuongBongMa { get; private set; }

    /// <summary>
    /// Vị trí của bóng ma
    /// </summary>
    public Vector3Int viTriBongMa { get; private set; }
    #endregion

    #region Unity Lifecycle - Vòng đời Unity
    private void Awake()
    {
        KhoiTaoThanhPhan();
    }

    private void LateUpdate()
    {
        CapNhatBongMa();
    }
    #endregion

    #region Khởi tạo - Initialization
    /// <summary>
    /// Khởi tạo các thành phần cần thiết
    /// </summary>
    private void KhoiTaoThanhPhan()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        cacOVuongBongMa = new Vector3Int[4]; // Mỗi Tetromino có 4 ô vuông
    }
    #endregion

    #region Cập nhật bóng ma - Ghost Update
    /// <summary>
    /// Cập nhật vị trí và hiển thị bóng ma
    /// </summary>
    private void CapNhatBongMa()
    {
        XoaBongMa();
        SaoChepHinhDangKhoi();
        TinhToanViTriRoi();
        VeBongMa();
    }
    #endregion

    #region Xử lý vẽ bóng ma - Ghost Rendering
    /// <summary>
    /// Xóa bóng ma khỏi tilemap
    /// </summary>
    private void XoaBongMa()
    {
        for (int i = 0; i < cacOVuongBongMa.Length; i++)
        {
            Vector3Int viTriTile = cacOVuongBongMa[i] + viTriBongMa;
            tilemap.SetTile(viTriTile, null);
        }
    }

    /// <summary>
    /// Sao chép hình dạng từ khối đang theo dõi
    /// </summary>
    private void SaoChepHinhDangKhoi()
    {
        for (int i = 0; i < cacOVuongBongMa.Length; i++)
        {
            cacOVuongBongMa[i] = khoiDangTheoDoi.cells[i];
        }
    }

    /// <summary>
    /// Tính toán vị trí thấp nhất mà khối có thể rơi xuống
    /// </summary>
    private void TinhToanViTriRoi()
    {
        Vector3Int viTriHienTai = khoiDangTheoDoi.position;

        int hangHienTai = viTriHienTai.y;
        int hangDay = -bangChoiChinh.boardSize.y / 2 - 1;

        // Tạm thời xóa khối khỏi bảng để kiểm tra vị trí
        bangChoiChinh.Clear(khoiDangTheoDoi);

        // Tìm vị trí thấp nhất có thể
        for (int hang = hangHienTai; hang >= hangDay; hang--)
        {
            viTriHienTai.y = hang;

            if (bangChoiChinh.IsValidPosition(khoiDangTheoDoi, viTriHienTai))
            {
                this.viTriBongMa = viTriHienTai;
            }
            else
            {
                break;
            }
        }

        // Vẽ lại khối lên bảng
        bangChoiChinh.Set(khoiDangTheoDoi);
    }

    /// <summary>
    /// Vẽ bóng ma lên tilemap
    /// </summary>
    private void VeBongMa()
    {
        for (int i = 0; i < cacOVuongBongMa.Length; i++)
        {
            Vector3Int viTriTile = cacOVuongBongMa[i] + viTriBongMa;
            tilemap.SetTile(viTriTile, tileBongMa);
        }
    }
    #endregion

    #region Compatibility Properties - Thuộc tính tương thích
    /// <summary>
    /// Thuộc tính tương thích với code cũ
    /// </summary>
    public Tile tile
    {
        get => tileBongMa;
        set => tileBongMa = value;
    }

    public Board mainBoard
    {
        get => bangChoiChinh;
        set => bangChoiChinh = value;
    }

    public Piece trackingPiece
    {
        get => khoiDangTheoDoi;
        set => khoiDangTheoDoi = value;
    }

    public Vector3Int[] cells => cacOVuongBongMa;
    public Vector3Int position => viTriBongMa;

    /// <summary>
    /// Phương thức tương thích với code cũ
    /// </summary>
    private void Clear() => XoaBongMa();
    private void Copy() => SaoChepHinhDangKhoi();
    private void Drop() => TinhToanViTriRoi();
    private void Set() => VeBongMa();
    #endregion
}