using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class chứa tất cả dữ liệu tĩnh cho game Tetris
/// </summary>
public static class DuLieuTetris
{
    #region Ma trận xoay - Rotation Matrix
    /// <summary>
    /// Cos của góc 90 độ
    /// </summary>
    public static readonly float cosGoc90 = Mathf.Cos(Mathf.PI / 2f);

    /// <summary>
    /// Sin của góc 90 độ
    /// </summary>
    public static readonly float sinGoc90 = Mathf.Sin(Mathf.PI / 2f);

    /// <summary>
    /// Ma trận xoay 90 độ dùng cho việc xoay các khối Tetromino
    /// </summary>
    public static readonly float[] MaTranXoay = new float[] { cosGoc90, sinGoc90, -sinGoc90, cosGoc90 };
    #endregion

    #region Định nghĩa hình dạng khối - Shape Definitions
    /// <summary>
    /// Dictionary chứa định nghĩa hình dạng của từng loại Tetromino
    /// Mỗi khối được định nghĩa bằng 4 Vector2Int biểu thị vị trí các ô vuông
    /// </summary>
    public static readonly Dictionary<Tetromino, Vector2Int[]> HinhDangCacKhoi = new Dictionary<Tetromino, Vector2Int[]>()
    {
        // Khối I - hình thẳng dài
        { Tetromino.I, new Vector2Int[] {
            new Vector2Int(-1, 1), new Vector2Int( 0, 1),
            new Vector2Int( 1, 1), new Vector2Int( 2, 1)
        }},

        // Khối J - hình chữ J
        { Tetromino.J, new Vector2Int[] {
            new Vector2Int(-1, 1), new Vector2Int(-1, 0),
            new Vector2Int( 0, 0), new Vector2Int( 1, 0)
        }},

        // Khối L - hình chữ L
        { Tetromino.L, new Vector2Int[] {
            new Vector2Int( 1, 1), new Vector2Int(-1, 0),
            new Vector2Int( 0, 0), new Vector2Int( 1, 0)
        }},

        // Khối O - hình vuông
        { Tetromino.O, new Vector2Int[] {
            new Vector2Int( 0, 1), new Vector2Int( 1, 1),
            new Vector2Int( 0, 0), new Vector2Int( 1, 0)
        }},

        // Khối S - hình chữ S
        { Tetromino.S, new Vector2Int[] {
            new Vector2Int( 0, 1), new Vector2Int( 1, 1),
            new Vector2Int(-1, 0), new Vector2Int( 0, 0)
        }},

        // Khối T - hình chữ T
        { Tetromino.T, new Vector2Int[] {
            new Vector2Int( 0, 1), new Vector2Int(-1, 0),
            new Vector2Int( 0, 0), new Vector2Int( 1, 0)
        }},

        // Khối Z - hình chữ Z
        { Tetromino.Z, new Vector2Int[] {
            new Vector2Int(-1, 1), new Vector2Int( 0, 1),
            new Vector2Int( 0, 0), new Vector2Int( 1, 0)
        }},
    };
    #endregion

    #region Dữ liệu Wall Kick - Wall Kick Data
    /// <summary>
    /// Dữ liệu wall kick riêng cho khối I (có cách xử lý khác biệt)
    /// </summary>
    private static readonly Vector2Int[,] WallKickChoKhoiI = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

    /// <summary>
    /// Dữ liệu wall kick cho các khối J, L, O, S, T, Z (có cách xử lý giống nhau)
    /// </summary>
    private static readonly Vector2Int[,] WallKickChoCacKhoiKhac = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };

    /// <summary>
    /// Dictionary ánh xạ loại Tetromino với dữ liệu wall kick tương ứng
    /// </summary>
    public static readonly Dictionary<Tetromino, Vector2Int[,]> DuLieuWallKickTheoLoai = new Dictionary<Tetromino, Vector2Int[,]>()
    {
        { Tetromino.I, WallKickChoKhoiI },
        { Tetromino.J, WallKickChoCacKhoiKhac },
        { Tetromino.L, WallKickChoCacKhoiKhac },
        { Tetromino.O, WallKickChoCacKhoiKhac },
        { Tetromino.S, WallKickChoCacKhoiKhac },
        { Tetromino.T, WallKickChoCacKhoiKhac },
        { Tetromino.Z, WallKickChoCacKhoiKhac },
    };
    #endregion

    #region Phương thức truy cập - Access Methods
    /// <summary>
    /// Lấy danh sách các ô vuông tạo thành khối theo loại Tetromino
    /// </summary>
    public static Vector2Int[] LayDanhSachOVuong(Tetromino loaiKhoi)
    {
        return HinhDangCacKhoi[loaiKhoi];
    }

    /// <summary>
    /// Lấy dữ liệu wall kick theo loại Tetromino
    /// </summary>
    public static Vector2Int[,] LayDuLieuWallKick(Tetromino loaiKhoi)
    {
        return DuLieuWallKickTheoLoai[loaiKhoi];
    }
    #endregion

    #region Compatibility Properties - Thuộc tính tương thích
    /// <summary>
    /// Thuộc tính tương thích với code cũ
    /// </summary>
    public static float cos => cosGoc90;
    public static float sin => sinGoc90;
    public static float[] RotationMatrix => MaTranXoay;
    public static Dictionary<Tetromino, Vector2Int[]> Cells => HinhDangCacKhoi;
    public static Dictionary<Tetromino, Vector2Int[,]> WallKicks => DuLieuWallKickTheoLoai;
    #endregion
}

/// <summary>
/// Alias cho tương thích ngược
/// </summary>
public static class Data
{
    public static float cos => DuLieuTetris.cos;
    public static float sin => DuLieuTetris.sin;
    public static float[] RotationMatrix => DuLieuTetris.RotationMatrix;
    public static Dictionary<Tetromino, Vector2Int[]> Cells => DuLieuTetris.Cells;
    public static Dictionary<Tetromino, Vector2Int[,]> WallKicks => DuLieuTetris.WallKicks;
}