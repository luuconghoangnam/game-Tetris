using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    #region Properties - Thuộc tính
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    [Header("Cấu hình bảng chơi")]
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    /// <summary>
    /// Vùng giới hạn của bảng chơi
    /// </summary>
    public RectInt VungGioiHan
    {
        get
        {
            Vector2Int viTriTamGiac = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(viTriTamGiac, boardSize);
        }
    }
    #endregion

    #region Unity Lifecycle - Vòng đời Unity
    private void Awake()
    {
        KhoiTaoThanhPhan();
        KhoiTaoDuLieuTetromino();
    }

    private void Start()
    {
        TaoKhoiMoi();
    }
    #endregion

    #region Khởi tạo - Initialization
    /// <summary>
    /// Khởi tạo các thành phần cần thiết
    /// </summary>
    private void KhoiTaoThanhPhan()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
    }

    /// <summary>
    /// Khởi tạo dữ liệu cho tất cả loại Tetromino
    /// </summary>
    private void KhoiTaoDuLieuTetromino()
    {
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }
    #endregion

    #region Quản lý Piece - Piece Management
    /// <summary>
    /// Tạo một khối Tetromino mới ngẫu nhiên
    /// </summary>
    public void TaoKhoiMoi()
    {
        int chiSoNgauNhien = Random.Range(0, tetrominoes.Length);
        TetrominoData duLieuKhoi = tetrominoes[chiSoNgauNhien];

        activePiece.Initialize(this, spawnPosition, duLieuKhoi);

        if (KiemTraViTriHopLe(activePiece, spawnPosition))
        {
            VeKhoiLenBang(activePiece);
        }
        else
        {
            XuLyKetThucGame();
        }
    }

    /// <summary>
    /// Xử lý khi game kết thúc
    /// </summary>
    public void XuLyKetThucGame()
    {
        tilemap.ClearAllTiles();
        Debug.Log("Game Over - Trò chơi kết thúc!");
    }
    #endregion

    #region Vẽ và xóa Piece - Drawing and Clearing
    /// <summary>
    /// Vẽ khối lên bảng chơi
    /// </summary>
    public void VeKhoiLenBang(Piece khoiCanVe)
    {
        for (int i = 0; i < khoiCanVe.cells.Length; i++)
        {
            Vector3Int viTriTile = khoiCanVe.cells[i] + khoiCanVe.position;
            tilemap.SetTile(viTriTile, khoiCanVe.data.tile);
        }
    }

    /// <summary>
    /// Xóa khối khỏi bảng chơi
    /// </summary>
    public void XoaKhoiKhoiBang(Piece khoiCanXoa)
    {
        for (int i = 0; i < khoiCanXoa.cells.Length; i++)
        {
            Vector3Int viTriTile = khoiCanXoa.cells[i] + khoiCanXoa.position;
            tilemap.SetTile(viTriTile, null);
        }
    }
    #endregion

    #region Kiểm tra vị trí - Position Validation
    /// <summary>
    /// Kiểm tra xem vị trí có hợp lệ cho khối hay không
    /// </summary>
    public bool KiemTraViTriHopLe(Piece khoi, Vector3Int viTriMoi)
    {
        RectInt vungGioiHan = VungGioiHan;

        for (int i = 0; i < khoi.cells.Length; i++)
        {
            Vector3Int viTriTile = khoi.cells[i] + viTriMoi;

            if (!vungGioiHan.Contains((Vector2Int)viTriTile))
            {
                return false;
            }

            if (tilemap.HasTile(viTriTile))
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Xử lý hàng đầy - Line Clearing System
    /// <summary>
    /// Xóa tất cả các hàng đã đầy
    /// </summary>
    public void XuLyXoaHang()
    {
        RectInt vungGioiHan = VungGioiHan;
        int hangHienTai = vungGioiHan.yMin;

        while (hangHienTai < vungGioiHan.yMax)
        {
            if (KiemTraHangDay(hangHienTai))
            {
                ThucHienXoaHang(hangHienTai);
            }
            else
            {
                hangHienTai++;
            }
        }
    }

    /// <summary>
    /// Kiểm tra xem một hàng có đầy hay không
    /// </summary>
    public bool KiemTraHangDay(int soHang)
    {
        RectInt vungGioiHan = VungGioiHan;

        for (int cot = vungGioiHan.xMin; cot < vungGioiHan.xMax; cot++)
        {
            Vector3Int viTri = new Vector3Int(cot, soHang, 0);

            if (!tilemap.HasTile(viTri))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Thực hiện xóa một hàng và đẩy các hàng trên xuống
    /// </summary>
    public void ThucHienXoaHang(int soHang)
    {
        RectInt vungGioiHan = VungGioiHan;

        for (int cot = vungGioiHan.xMin; cot < vungGioiHan.xMax; cot++)
        {
            Vector3Int viTri = new Vector3Int(cot, soHang, 0);
            tilemap.SetTile(viTri, null);
        }

        while (soHang < vungGioiHan.yMax)
        {
            for (int cot = vungGioiHan.xMin; cot < vungGioiHan.xMax; cot++)
            {
                Vector3Int viTriTren = new Vector3Int(cot, soHang + 1, 0);
                TileBase tileTren = tilemap.GetTile(viTriTren);

                Vector3Int viTriDuoi = new Vector3Int(cot, soHang, 0);
                tilemap.SetTile(viTriDuoi, tileTren);
            }

            soHang++;
        }
    }
    #endregion

    #region Compatibility Methods - Phương thức tương thích
    public void SpawnPiece() => TaoKhoiMoi();
    public void GameOver() => XuLyKetThucGame();
    public void Set(Piece piece) => VeKhoiLenBang(piece);
    public void Clear(Piece piece) => XoaKhoiKhoiBang(piece);
    public bool IsValidPosition(Piece piece, Vector3Int position) => KiemTraViTriHopLe(piece, position);
    public void ClearLines() => XuLyXoaHang();
    public bool IsLineFull(int row) => KiemTraHangDay(row);
    public void LineClear(int row) => ThucHienXoaHang(row);
    public RectInt Bounds => VungGioiHan;
    #endregion
}