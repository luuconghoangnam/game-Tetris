using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Enum định nghĩa các loại khối Tetris chuẩn
/// </summary>
public enum Tetromino
{
    I, // Khối thẳng dài
    J, // Khối chữ J
    L, // Khối chữ L
    O, // Khối vuông
    S, // Khối chữ S
    T, // Khối chữ T
    Z  // Khối chữ Z
}

/// <summary>
/// Cấu trúc dữ liệu chứa thông tin của một khối Tetromino
/// </summary>
[System.Serializable]
public struct TetrominoData
{
    [Header("Cấu hình hiển thị")]
    [Tooltip("Tile dùng để hiển thị khối")]
    public Tile tile;

    [Tooltip("Loại Tetromino")]
    public Tetromino tetromino;

    /// <summary>
    /// Mảng các ô tạo thành khối
    /// </summary>
    public Vector2Int[] cacOVuong { get; private set; }

    /// <summary>
    /// Dữ liệu wall kick cho việc xoay khối
    /// </summary>
    public Vector2Int[,] duLieuWallKick { get; private set; }

    /// <summary>
    /// Khởi tạo dữ liệu cho khối Tetromino từ class DuLieuTetris
    /// </summary>
    public void KhoiTaoDuLieu()
    {
        cacOVuong = DuLieuTetris.LayDanhSachOVuong(tetromino);
        duLieuWallKick = DuLieuTetris.LayDuLieuWallKick(tetromino);
    }

    #region Compatibility Properties - Thuộc tính tương thích
    /// <summary>
    /// Thuộc tính tương thích với code cũ
    /// </summary>
    public Vector2Int[] cells => cacOVuong;

    /// <summary>
    /// Thuộc tính tương thích với code cũ
    /// </summary>
    public Vector2Int[,] wallKicks => duLieuWallKick;

    /// <summary>
    /// Phương thức tương thích với code cũ
    /// </summary>
    public void Initialize() => KhoiTaoDuLieu();
    #endregion
}