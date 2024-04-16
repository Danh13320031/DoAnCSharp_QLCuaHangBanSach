namespace QLCuaHangBanSach
{
    internal class BookModel
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongTon { get; set; }
        public string MaTheLoai { get; set; }
        public string MaNhaXuatBan { get; set; }
        public string MaTacGia { get; set; }

        public BookModel()
        {
            SoLuongTon = 0;
        }
    }
}
