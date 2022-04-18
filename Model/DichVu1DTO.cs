using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DichVu1DTO
    {
        [Display(Name = "Mã DV")]
        public string MaDv { get; set; }

        [Display(Name = "Tên HĐ")] //[Display(Name = "Tên DV")]
        public string TenHd { get; set; }

        //[Display(Name = "Đơn vị ký kết")]
        //public string DonViKyKet { get; set; }

        //[Display(Name = "Quốc gia")]
        //public string QuocGia { get; set; }

        //[Display(Name = "Miền")]
        //public string Mien { get; set; }

        //[Display(Name = "Tỉnh")]
        //public string Tinh { get; set; }

        //[Display(Name = "Thành phố")]
        //public string ThanhPho { get; set; }

        [Display(Name = "Loại sao")]
        public string LoaiSao { get; set; }

        [Display(Name = "Thông tin LH")]
        public string ThongTinLienHe { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        public string Email { get; set; }

        [Display(Name = "Điện thoại")]
        public string DienThoai { get; set; }

        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; }

        [Display(Name = "Người liên hệ")]
        public string NguoiLienHe { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierId { get; set; }

        public string Website { get; set; }

        [Display(Name = "Đã ký")]
        public bool DaKy { get; set; }

        [Display(Name = "Hoạt động")]
        public bool HoatDong { get; set; }

        [Display(Name = "Tuyến")]
        public string Tuyen { get; set; }

        [Display(Name = "Loại tàu")]
        public string LoaiTau { get; set; }

        [Display(Name = "Loại xe")]
        public string LoaiXe { get; set; }

        [Display(Name = "Đậu xe")]
        public bool DauXe { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Giá HD")]
        public decimal GiaHd { get; set; }

        [Display(Name = "Loại HD")]
        public string LoaiHd { get; set; }

        [Display(Name = "Loại DV")]
        public int LoaiDvid { get; set; }

        public DateTime? NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public DateTime? NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public string LogFile { get; set; }

        [Display(Name = "Thời gian HĐ")]
        public string ThoiGianHd { get; set; }

        [Display(Name = "Bắt đầu HĐ")]
        public DateTime? BatDauHd { get; set; }

        [Display(Name = "Kết thúc HĐ")]
        public DateTime? KetThucHd { get; set; }

        [Display(Name = "Hot deal")]
        public bool HotDeal { get; set; }

        [Display(Name = "Người trình ký")]
        public string NguoiTrinhKy { get; set; }

        [Display(Name = "Ngày trình ký")]
        public DateTime? NgayTrinhKy { get; set; }

        public LoaiDvDTO LoaiDvDTO { get; set; }
        public SupplierDTO SupplierDTO { get; set; }
        public virtual ICollection<HinhAnhDTO> HinhAnhDTOs { get; set; }
        public List<string> ImageUrls { get; set; }
        public string StringImageUrls { get; set; }
    }
}