using UnityEngine;

/// <summary>
/// Class quản lý khối Tetromino đang rơi
/// </summary>
public class Piece : MonoBehaviour
{
    #region Properties - Thuộc tính
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    [Header("Cấu hình thời gian")]
    [Tooltip("Thời gian giữa các bước rơi tự động")]
    public float thoiGianGiuaCacBuoc = 1f;

    [Tooltip("Thời gian delay giữa các lần di chuyển")]
    public float thoiGianDelayDiChuyen = 0.1f;

    [Tooltip("Thời gian chờ trước khi khóa khối")]
    public float thoiGianDelayKhoa = 0.5f;

    // Các biến thời gian nội bộ
    private float thoiDiemBuocTiepTheo;
    private float thoiDiemDiChuyenTiepTheo;
    private float thoiGianChoKhoa;
    #endregion

    #region Unity Lifecycle - Vòng đời Unity
    private void Update()
    {
        XuLyCapNhatKhoi();
    }
    #endregion

    #region Khởi tạo - Initialization
    /// <summary>
    /// Khởi tạo khối với dữ liệu và vị trí cụ thể
    /// </summary>
    public void KhoiTaoKhoi(Board bangChoi, Vector3Int viTriBatDau, TetrominoData duLieuKhoi)
    {
        this.data = duLieuKhoi;
        this.board = bangChoi;
        this.position = viTriBatDau;

        // Đặt lại các chỉ số
        rotationIndex = 0;
        thoiDiemBuocTiepTheo = Time.time + thoiGianGiuaCacBuoc;
        thoiDiemDiChuyenTiepTheo = Time.time + thoiGianDelayDiChuyen;
        thoiGianChoKhoa = 0f;

        // Khởi tạo mảng cells nếu cần
        if (cells == null)
        {
            cells = new Vector3Int[duLieuKhoi.cells.Length];
        }

        // Sao chép dữ liệu cells từ TetrominoData
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)duLieuKhoi.cells[i];
        }
    }
    #endregion

    #region Cập nhật chính - Main Update
    /// <summary>
    /// Xử lý cập nhật khối mỗi frame
    /// </summary>
    private void XuLyCapNhatKhoi()
    {
        // Xóa khối khỏi bảng để chuẩn bị cập nhật vị trí mới
        board.Clear(this);

        // Tăng thời gian chờ khóa
        thoiGianChoKhoa += Time.deltaTime;

        // Xử lý input xoay
        XuLyInputXoay();

        // Xử lý hard drop
        XuLyInputHardDrop();

        // Xử lý di chuyển nếu đã hết thời gian delay
        if (Time.time > thoiDiemDiChuyenTiepTheo)
        {
            XuLyInputDiChuyen();
        }

        // Tự động rơi xuống theo thời gian
        if (Time.time > thoiDiemBuocTiepTheo)
        {
            ThucHienBuocRoi();
        }

        // Vẽ lại khối ở vị trí mới
        board.Set(this);
    }
    #endregion

    #region Xử lý Input - Input Handling
    /// <summary>
    /// Xử lý input xoay khối
    /// </summary>
    private void XuLyInputXoay()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThucHienXoay(-1); // Xoay trái
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ThucHienXoay(1);  // Xoay phải
        }
    }

    /// <summary>
    /// Xử lý input hard drop (thả nhanh)
    /// </summary>
    private void XuLyInputHardDrop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThucHienHardDrop();
        }
    }

    /// <summary>
    /// Xử lý các input di chuyển (trái, phải, xuống)
    /// </summary>
    private void XuLyInputDiChuyen()
    {
        // Soft drop - rơi nhanh xuống
        if (Input.GetKey(KeyCode.S))
        {
            if (ThucHienDiChuyen(Vector2Int.down))
            {
                // Cập nhật thời gian bước để tránh di chuyển kép
                thoiDiemBuocTiepTheo = Time.time + thoiGianGiuaCacBuoc;
            }
        }

        // Di chuyển trái/phải
        if (Input.GetKey(KeyCode.A))
        {
            ThucHienDiChuyen(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ThucHienDiChuyen(Vector2Int.right);
        }
    }
    #endregion

    #region Di chuyển khối - Piece Movement
    /// <summary>
    /// Thực hiện di chuyển khối theo hướng chỉ định
    /// </summary>
    /// <param name="huongDiChuyen">Vector2Int biểu thị hướng di chuyển</param>
    /// <returns>True nếu di chuyển thành công, False nếu không thể di chuyển</returns>
    private bool ThucHienDiChuyen(Vector2Int huongDiChuyen)
    {
        Vector3Int viTriMoi = position;
        viTriMoi.x += huongDiChuyen.x;
        viTriMoi.y += huongDiChuyen.y;

        bool coTheChuyenDuoc = board.IsValidPosition(this, viTriMoi);

        // Chỉ lưu di chuyển nếu vị trí mới hợp lệ
        if (coTheChuyenDuoc)
        {
            position = viTriMoi;
            thoiDiemDiChuyenTiepTheo = Time.time + thoiGianDelayDiChuyen;
            thoiGianChoKhoa = 0f; // Reset thời gian chờ khóa
        }

        return coTheChuyenDuoc;
    }

    /// <summary>
    /// Thực hiện một bước rơi tự động
    /// </summary>
    private void ThucHienBuocRoi()
    {
        thoiDiemBuocTiepTheo = Time.time + thoiGianGiuaCacBuoc;

        // Thử rơi xuống một hàng
        ThucHienDiChuyen(Vector2Int.down);

        // Nếu khối đã bất động quá lâu thì khóa nó
        if (thoiGianChoKhoa >= thoiGianDelayKhoa)
        {
            ThucHienKhoaKhoi();
        }
    }

    /// <summary>
    /// Thực hiện hard drop - thả khối xuống ngay lập tức
    /// </summary>
    private void ThucHienHardDrop()
    {
        // Tiếp tục di chuyển xuống cho đến khi không thể di chuyển nữa
        while (ThucHienDiChuyen(Vector2Int.down))
        {
            continue;
        }

        // Khóa khối ngay lập tức
        ThucHienKhoaKhoi();
    }

    /// <summary>
    /// Khóa khối tại vị trí hiện tại và tạo khối mới
    /// </summary>
    private void ThucHienKhoaKhoi()
    {
        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }
    #endregion

    #region Xoay khối - Piece Rotation
    /// <summary>
    /// Thực hiện xoay khối theo hướng chỉ định
    /// </summary>
    /// <param name="huongXoay">1 cho xoay phải, -1 cho xoay trái</param>
    private void ThucHienXoay(int huongXoay)
    {
        // Lưu trạng thái xoay hiện tại để có thể hoàn tác nếu cần
        int chiSoXoayGoc = rotationIndex;

        // Xoay tất cả các ô sử dụng ma trận xoay
        rotationIndex = TinhChiSoXoayMoi(rotationIndex + huongXoay, 0, 4);
        ApDungMaTranXoay(huongXoay);

        // Hoàn tác xoay nếu wall kick test thất bại
        if (!KiemTraWallKick(rotationIndex, huongXoay))
        {
            rotationIndex = chiSoXoayGoc;
            ApDungMaTranXoay(-huongXoay);
        }
    }

    /// <summary>
    /// Áp dụng ma trận xoay cho tất cả các ô của khối
    /// </summary>
    /// <param name="huongXoay">Hướng xoay (1 hoặc -1)</param>
    private void ApDungMaTranXoay(int huongXoay)
    {
        float[] maTran = Data.RotationMatrix;

        // Xoay tất cả các ô sử dụng ma trận xoay
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 oHienTai = cells[i];
            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    // Khối "I" và "O" được xoay từ điểm trung tâm offset
                    oHienTai.x -= 0.5f;
                    oHienTai.y -= 0.5f;
                    x = Mathf.CeilToInt((oHienTai.x * maTran[0] * huongXoay) + (oHienTai.y * maTran[1] * huongXoay));
                    y = Mathf.CeilToInt((oHienTai.x * maTran[2] * huongXoay) + (oHienTai.y * maTran[3] * huongXoay));
                    break;

                default:
                    x = Mathf.RoundToInt((oHienTai.x * maTran[0] * huongXoay) + (oHienTai.y * maTran[1] * huongXoay));
                    y = Mathf.RoundToInt((oHienTai.x * maTran[2] * huongXoay) + (oHienTai.y * maTran[3] * huongXoay));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    /// <summary>
    /// Kiểm tra wall kick khi xoay khối
    /// </summary>
    /// <param name="chiSoXoay">Chỉ số xoay hiện tại</param>
    /// <param name="huongXoay">Hướng xoay</param>
    /// <returns>True nếu có thể xoay, False nếu không thể</returns>
    private bool KiemTraWallKick(int chiSoXoay, int huongXoay)
    {
        int chiSoWallKick = TinhChiSoWallKick(chiSoXoay, huongXoay);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int viTriDichChuyen = data.wallKicks[chiSoWallKick, i];

            if (ThucHienDiChuyen(viTriDichChuyen))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Tính chỉ số wall kick dựa trên chỉ số xoay và hướng xoay
    /// </summary>
    /// <param name="chiSoXoay">Chỉ số xoay hiện tại</param>
    /// <param name="huongXoay">Hướng xoay</param>
    /// <returns>Chỉ số wall kick</returns>
    private int TinhChiSoWallKick(int chiSoXoay, int huongXoay)
    {
        int chiSoWallKick = chiSoXoay * 2;

        if (huongXoay < 0)
        {
            chiSoWallKick--;
        }

        return TinhChiSoXoayMoi(chiSoWallKick, 0, data.wallKicks.GetLength(0));
    }
    #endregion

    #region Utility Methods - Phương thức tiện ích
    /// <summary>
    /// Tính chỉ số xoay mới với wrap around
    /// </summary>
    /// <param name="giaTri">Giá trị đầu vào</param>
    /// <param name="min">Giá trị tối thiểu</param>
    /// <param name="max">Giá trị tối đa</param>
    /// <returns>Giá trị sau khi wrap</returns>
    private int TinhChiSoXoayMoi(int giaTri, int min, int max)
    {
        if (giaTri < min)
        {
            return max - (min - giaTri) % (max - min);
        }
        else
        {
            return min + (giaTri - min) % (max - min);
        }
    }
    #endregion

    #region Compatibility Methods - Phương thức tương thích
    /// <summary>
    /// Phương thức tương thích với code cũ
    /// </summary>
    public void Initialize(Board board, Vector3Int position, TetrominoData data) => KhoiTaoKhoi(board, position, data);
    private void HandleMoveInputs() => XuLyInputDiChuyen();
    private void Step() => ThucHienBuocRoi();
    private void HardDrop() => ThucHienHardDrop();
    private void Lock() => ThucHienKhoaKhoi();
    private bool Move(Vector2Int translation) => ThucHienDiChuyen(translation);
    private void Rotate(int direction) => ThucHienXoay(direction);
    private void ApplyRotationMatrix(int direction) => ApDungMaTranXoay(direction);
    private bool TestWallKicks(int rotationIndex, int rotationDirection) => KiemTraWallKick(rotationIndex, rotationDirection);
    private int GetWallKickIndex(int rotationIndex, int rotationDirection) => TinhChiSoWallKick(rotationIndex, rotationDirection);
    private int Wrap(int input, int min, int max) => TinhChiSoXoayMoi(input, min, max);

    // Compatibility properties
    public float stepDelay
    {
        get => thoiGianGiuaCacBuoc;
        set => thoiGianGiuaCacBuoc = value;
    }
    public float moveDelay
    {
        get => thoiGianDelayDiChuyen;
        set => thoiGianDelayDiChuyen = value;
    }
    public float lockDelay
    {
        get => thoiGianDelayKhoa;
        set => thoiGianDelayKhoa = value;
    }
    #endregion
}