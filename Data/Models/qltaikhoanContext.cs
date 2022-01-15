﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.Models
{
    public partial class qltaikhoanContext : DbContext
    {
        public qltaikhoanContext()
        {
        }

        public qltaikhoanContext(DbContextOptions<qltaikhoanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<City> Citys { get; set; }
        public virtual DbSet<DanhGiaCamLao> DanhGiaCamLaos { get; set; }
        public virtual DbSet<DanhGiaDiemThamQuan> DanhGiaDiemThamQuans { get; set; }
        public virtual DbSet<DanhGiaKhachSan> DanhGiaKhachSans { get; set; }
        public virtual DbSet<DanhGiaLandtour> DanhGiaLandtours { get; set; }
        public virtual DbSet<DanhGiaNhaHang> DanhGiaNhaHangs { get; set; }
        public virtual DbSet<DanhGiaVanChuyen> DanhGiaVanChuyens { get; set; }
        public virtual DbSet<DichVu1> DichVus1 { get; set; }
        public virtual DbSet<Dichvu> Dichvus { get; set; }
        public virtual DbSet<Dmchinhanh> Dmchinhanhs { get; set; }
        public virtual DbSet<Dmdaily> Dmdailies { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<FolderUser> FolderUsers { get; set; }
        public virtual DbSet<HinhAnh> HinhAnhs { get; set; }
        public virtual DbSet<LoaiDv> LoaiDvs { get; set; }
        public virtual DbSet<LoginModel> LoginModels { get; set; }
        public virtual DbSet<Mien> Miens { get; set; }
        public virtual DbSet<National> Nationals { get; set; }
        public virtual DbSet<Phongban> Phongbans { get; set; }
        public virtual DbSet<Quan> Quans { get; set; }
        public virtual DbSet<Quocgium> Quocgia { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Supplierob> Supplierobs { get; set; }
        public virtual DbSet<Thanhpho> Thanhphos { get; set; }
        public virtual DbSet<Thanhpho1> Thanhpho1s { get; set; }
        public virtual DbSet<Tinh> Tinhs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VPhongban> VPhongbans { get; set; }
        public virtual DbSet<VSupplierob> VSupplierobs { get; set; }
        public virtual DbSet<VTinh> VTinhs { get; set; }
        public virtual DbSet<VUserHoadon> VUserHoadons { get; set; }
        public virtual DbSet<Vungmien> Vungmiens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                optionsBuilder.UseSqlServer("Server=118.68.170.128;database=qltaikhoan;Trusted_Connection=true;User Id=vanhong;Password=Hong@2019;Integrated security=false;MultipleActiveResultSets=true");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUser");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Chuongtrinh)
                    .HasMaxLength(70)
                    .HasColumnName("chuongtrinh");

                entity.Property(e => e.Mact)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("mact");

                entity.Property(e => e.Mota)
                    .HasMaxLength(100)
                    .HasColumnName("mota");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.Mact)
                    .HasName("PK_chuongtrinh");

                entity.ToTable("Application");

                entity.Property(e => e.Mact)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("mact");

                entity.Property(e => e.Chuongtrinh)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("chuongtrinh");

                entity.Property(e => e.Link)
                    .HasMaxLength(50)
                    .HasColumnName("link");

                entity.Property(e => e.Mota)
                    .HasMaxLength(150)
                    .HasColumnName("mota");
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.Mact });

                entity.ToTable("ApplicationUser");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Mact)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("mact");

                entity.HasOne(d => d.MactNavigation)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.Mact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUser_Application");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityCode);

                entity.Property(e => e.CityCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.Property(e => e.NationCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DanhGiaCamLao>(entity =>
            {
                entity.ToTable("DanhGiaCamLao");

                entity.Property(e => e.CldvvaHdvtiengViet)
                    .HasMaxLength(150)
                    .HasColumnName("CLDVVaHDVTiengViet");

                entity.Property(e => e.DaCoKhaoSatThucTe).HasMaxLength(250);

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DoiXeOrLoaiXe)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DongYduaVaoDsncu).HasColumnName("DongYDuaVaoDSNCU");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GiaCa).HasMaxLength(150);

                entity.Property(e => e.KhaNangHuyDong).HasMaxLength(150);

                entity.Property(e => e.KinhNghiem).HasMaxLength(250);

                entity.Property(e => e.KnngheNghiep)
                    .HasMaxLength(250)
                    .HasColumnName("KNNgheNghiep");

                entity.Property(e => e.KntaiThiTruongVn)
                    .HasMaxLength(150)
                    .HasColumnName("KNTaiThiTruongVN");

                entity.Property(e => e.Kqdat).HasColumnName("KQDat");

                entity.Property(e => e.KqkhaoSatThem).HasColumnName("KQKhaoSatThem");

                entity.Property(e => e.LanDauOrTaiKy).HasMaxLength(50);

                entity.Property(e => e.LoaiDvid).HasColumnName("LoaiDVId");

                entity.Property(e => e.MucDoHtxuLySuCo)
                    .HasMaxLength(150)
                    .HasColumnName("MucDoHTXuLySuCo");

                entity.Property(e => e.MucDoKipThoiTrongGd)
                    .HasMaxLength(150)
                    .HasColumnName("MucDoKipThoiTrongGD");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NguoiSua)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NguoiTao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NlkhaiThacDvtaiDiaPhuong)
                    .HasMaxLength(150)
                    .HasColumnName("NLKhaiThacDVTaiDiaPhuong");

                entity.Property(e => e.PhapNhan).HasMaxLength(150);

                entity.Property(e => e.SanPham).HasMaxLength(150);

                entity.Property(e => e.SoXeChinhThuc).HasMaxLength(50);

                entity.Property(e => e.TenNcu)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("TenNCU");
            });

            modelBuilder.Entity<DanhGiaDiemThamQuan>(entity =>
            {
                entity.ToTable("DanhGiaDiemThamQuan");

                entity.Property(e => e.CoBaiDoXe).HasMaxLength(150);

                entity.Property(e => e.CoGpkd).HasColumnName("CoGPKD");

                entity.Property(e => e.CoHdvat).HasColumnName("CoHDVAT");

                entity.Property(e => e.CoNhaHang).HasMaxLength(50);

                entity.Property(e => e.CoNhaVeSinh).HasMaxLength(50);

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DongYduaVaoDsncu).HasColumnName("DongYDuaVaoDSNCU");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoaiDvid).HasColumnName("LoaiDVId");

                entity.Property(e => e.MucDoHapDan).HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NguoiSua)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NguoiTao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhuongTienPvvuiChoi)
                    .HasMaxLength(50)
                    .HasColumnName("PhuongTienPVVuiChoi");

                entity.Property(e => e.SucChuaToiDa).HasMaxLength(150);

                entity.Property(e => e.TenNcu)
                    .HasMaxLength(150)
                    .HasColumnName("TenNCU");

                entity.Property(e => e.ThaiDoPvcuaNv)
                    .HasMaxLength(50)
                    .HasColumnName("ThaiDoPVCuaNV");

                entity.Property(e => e.ViTri).HasMaxLength(150);

                entity.Property(e => e.Website)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DanhGiaKhachSan>(entity =>
            {
                entity.ToTable("DanhGiaKhachSan");

                entity.Property(e => e.CoBaiDoXe).HasMaxLength(150);

                entity.Property(e => e.CoBoTriPhongChoNb)
                    .HasMaxLength(150)
                    .HasColumnName("CoBoTriPhongChoNB");

                entity.Property(e => e.CoNhaHang).HasMaxLength(150);

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DongYduaVaoDsncu).HasColumnName("DongYDuaVaoDSNCU");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gpkd).HasColumnName("GPKD");

                entity.Property(e => e.KqKhaoSatThem).HasMaxLength(150);

                entity.Property(e => e.LoaiDvid).HasColumnName("LoaiDVId");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NguoiSua)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NguoiTao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SucChuaToiDa).HasMaxLength(150);

                entity.Property(e => e.TenNcu)
                    .HasMaxLength(150)
                    .HasColumnName("TenNCU");

                entity.Property(e => e.ThaiDoPvcuaNv)
                    .HasMaxLength(150)
                    .HasColumnName("ThaiDoPVCuaNV");

                entity.Property(e => e.Vat).HasColumnName("VAT");

                entity.Property(e => e.ViTri).HasMaxLength(150);

                entity.Property(e => e.Website)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DanhGiaLandtour>(entity =>
            {
                entity.ToTable("DanhGiaLandtour");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CldvvaHdv)
                    .HasMaxLength(150)
                    .HasColumnName("CLDVVaHDV");

                entity.Property(e => e.CoGpkd).HasColumnName("CoGPKD");

                entity.Property(e => e.CoHdvat).HasColumnName("CoHDVAT");

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DongYduaVaoDsncu).HasColumnName("DongYDuaVaoDSNCU");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GiaCa).HasMaxLength(50);

                entity.Property(e => e.KinhNghiemThiTruongNd)
                    .HasMaxLength(150)
                    .HasColumnName("KinhNghiemThiTruongND");

                entity.Property(e => e.LoaiDvid).HasColumnName("LoaiDVId");

                entity.Property(e => e.MucDoHoTroXuLySuCo).HasMaxLength(50);

                entity.Property(e => e.MucDoKipThoiTrongGd)
                    .HasMaxLength(50)
                    .HasColumnName("MucDoKipThoiTrongGD");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NguoiSua)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NguoiTao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NlkhaiThacDvdiaPhuong)
                    .HasMaxLength(150)
                    .HasColumnName("NLKhaiThacDVDiaPhuong");

                entity.Property(e => e.SanPham).HasMaxLength(50);

                entity.Property(e => e.TenNcu)
                    .HasMaxLength(150)
                    .HasColumnName("TenNCU");

                entity.Property(e => e.Website)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DanhGiaNhaHang>(entity =>
            {
                entity.ToTable("DanhGiaNhaHang");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChatLuong).HasMaxLength(50);

                entity.Property(e => e.CoBaiDoXe).HasMaxLength(150);

                entity.Property(e => e.CoGpkd).HasColumnName("CoGPKD");

                entity.Property(e => e.CoHdvat).HasColumnName("CoHDVAT");

                entity.Property(e => e.CoPvmienPhiNoiBo)
                    .HasMaxLength(150)
                    .HasColumnName("CoPVMienPhiNoiBo");

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DinhLuong).HasMaxLength(50);

                entity.Property(e => e.DongYduaVaoDsncu).HasColumnName("DongYDuaVaoDSNCU");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoaiDvid).HasColumnName("LoaiDVId");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NguoiSua)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NguoiTao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NhaVeSinh).HasMaxLength(50);

                entity.Property(e => e.SucChuaToiDa).HasMaxLength(150);

                entity.Property(e => e.TenNcu)
                    .HasMaxLength(150)
                    .HasColumnName("TenNCU");

                entity.Property(e => e.ThaiDoPvcuaNv)
                    .HasMaxLength(50)
                    .HasColumnName("ThaiDoPVCuaNV");

                entity.Property(e => e.ViTri).HasMaxLength(150);

                entity.Property(e => e.Website)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DanhGiaVanChuyen>(entity =>
            {
                entity.ToTable("DanhGiaVanChuyen");

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DoiXeOrLoaiXe).HasMaxLength(150);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.KhaNangHuyDong).HasMaxLength(150);

                entity.Property(e => e.KinhNghiem).HasMaxLength(50);

                entity.Property(e => e.LoaiDvid).HasColumnName("LoaiDVId");

                entity.Property(e => e.PhapNhan).HasMaxLength(150);

                entity.Property(e => e.SoXeChinhThuc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenNcu)
                    .HasMaxLength(150)
                    .HasColumnName("TenNCU");

                entity.Property(e => e.Website)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DichVu1>(entity =>
            {
                entity.HasKey(e => e.MaDv);

                entity.ToTable("DichVus");

                entity.Property(e => e.MaDv)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaDV");

                entity.Property(e => e.DiaChi).HasMaxLength(250);

                entity.Property(e => e.DienThoai).HasMaxLength(50);

                entity.Property(e => e.DonViKyKet).HasMaxLength(150);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GhiChu).HasMaxLength(250);

                entity.Property(e => e.GiaHd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("GiaHD");

                entity.Property(e => e.LoaiHd)
                    .HasMaxLength(50)
                    .HasColumnName("LoaiHD");

                entity.Property(e => e.LoaiSao).HasMaxLength(50);

                entity.Property(e => e.LoaiTau).HasMaxLength(150);

                entity.Property(e => e.Mien).HasMaxLength(100);

                entity.Property(e => e.NguoiLienHe).HasMaxLength(150);

                entity.Property(e => e.QuocGia).HasMaxLength(100);

                entity.Property(e => e.SupplierId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TenDv)
                    .HasMaxLength(150)
                    .HasColumnName("TenDV");

                entity.Property(e => e.ThanhPho).HasMaxLength(50);

                entity.Property(e => e.Tinh).HasMaxLength(100);

                entity.Property(e => e.Tuyen).HasMaxLength(250);

                entity.Property(e => e.Website).HasMaxLength(150);
            });

            modelBuilder.Entity<Dichvu>(entity =>
            {
                entity.HasKey(e => e.Iddichvu);

                entity.ToTable("dichvu");

                entity.Property(e => e.Iddichvu)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("iddichvu");

                entity.Property(e => e.Tendv)
                    .HasMaxLength(50)
                    .HasColumnName("tendv");

                entity.Property(e => e.Trangthai)
                    .IsRequired()
                    .HasColumnName("trangthai")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Dmchinhanh>(entity =>
            {
                entity.ToTable("dmchinhanh");

                entity.Property(e => e.Diachi)
                    .HasMaxLength(200)
                    .HasColumnName("diachi");

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dienthoai");

                entity.Property(e => e.Fax)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fax");

                entity.Property(e => e.Macn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("macn");

                entity.Property(e => e.Masothue)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("masothue");

                entity.Property(e => e.Tencn)
                    .HasMaxLength(100)
                    .HasColumnName("tencn");

                entity.Property(e => e.Thanhpho)
                    .HasMaxLength(50)
                    .HasColumnName("thanhpho");

                entity.Property(e => e.Trangthai)
                    .IsRequired()
                    .HasColumnName("trangthai")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Dmdaily>(entity =>
            {
                entity.ToTable("dmdaily");

                entity.Property(e => e.Daily)
                    .HasMaxLength(25)
                    .HasColumnName("daily");

                entity.Property(e => e.Diachi)
                    .HasMaxLength(100)
                    .HasColumnName("diachi");

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(50)
                    .HasColumnName("dienthoai");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");

                entity.Property(e => e.Macn)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("macn");

                entity.Property(e => e.Tendaily)
                    .HasMaxLength(100)
                    .HasColumnName("tendaily");

                entity.Property(e => e.Trangthai).HasColumnName("trangthai");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("ErrorLog");

                entity.Property(e => e.InnerMessage).HasMaxLength(300);

                entity.Property(e => e.MaCn)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasMaxLength(300);
            });

            modelBuilder.Entity<FolderUser>(entity =>
            {
                entity.ToTable("FolderUser");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Path)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HinhAnh>(entity =>
            {
                entity.Property(e => e.DichVuId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LoaiDv>(entity =>
            {
                entity.ToTable("LoaiDVs");

                entity.Property(e => e.GhiChu).HasMaxLength(150);

                entity.Property(e => e.MaLoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenLoai).HasMaxLength(50);
            });

            modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("LoginModel");

                entity.Property(e => e.Diachi)
                    .HasMaxLength(200)
                    .HasColumnName("diachi");

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(50)
                    .HasColumnName("dienthoai");

                entity.Property(e => e.Doimk).HasColumnName("doimk");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Hoten)
                    .HasMaxLength(50)
                    .HasColumnName("hoten");

                entity.Property(e => e.Macn)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("macn");

                entity.Property(e => e.Macode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("macode");

                entity.Property(e => e.Mact)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("mact");

                entity.Property(e => e.Maphong)
                    .HasMaxLength(50)
                    .HasColumnName("maphong");

                entity.Property(e => e.Masothue)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("masothue");

                entity.Property(e => e.Ngaydoimk)
                    .HasColumnType("date")
                    .HasColumnName("ngaydoimk");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("roleId");

                entity.Property(e => e.Tencn)
                    .HasMaxLength(100)
                    .HasColumnName("tencn");

                entity.Property(e => e.Trangthai).HasColumnName("trangthai");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Mien>(entity =>
            {
                entity.ToTable("Mien");

                entity.Property(e => e.MienId)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TenMien).HasMaxLength(50);
            });

            modelBuilder.Entity<National>(entity =>
            {
                entity.HasKey(e => e.NationCode);

                entity.Property(e => e.NationCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Continent).HasMaxLength(50);

                entity.Property(e => e.NationName).HasMaxLength(50);

                entity.Property(e => e.Territory).HasMaxLength(50);
            });

            modelBuilder.Entity<Phongban>(entity =>
            {
                entity.HasKey(e => e.Maphong);

                entity.ToTable("phongban");

                entity.Property(e => e.Maphong)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("maphong");

                entity.Property(e => e.Hdvat).HasColumnName("hdvat");

                entity.Property(e => e.Macode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("macode");

                entity.Property(e => e.Tenphong)
                    .HasMaxLength(50)
                    .HasColumnName("tenphong");

                entity.Property(e => e.Trangthai).HasColumnName("trangthai");
            });

            modelBuilder.Entity<Quan>(entity =>
            {
                entity.ToTable("quan");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Tenquan)
                    .HasMaxLength(50)
                    .HasColumnName("tenquan");

                entity.Property(e => e.Tentp)
                    .HasMaxLength(50)
                    .HasColumnName("tentp");
            });

            modelBuilder.Entity<Quocgium>(entity =>
            {
                entity.Property(e => e.TenNuoc).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("roleName");

                entity.Property(e => e.Trangthai)
                    .IsRequired()
                    .HasColumnName("trangthai")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_supplier_1");

                entity.ToTable("supplier");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Diachi).HasMaxLength(250);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Masothue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nganhnghe).HasMaxLength(300);

                entity.Property(e => e.Ngayhethan).HasColumnType("date");

                entity.Property(e => e.Ngaytao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nguoilienhe).HasMaxLength(200);

                entity.Property(e => e.Nguoitao).HasMaxLength(50);

                entity.Property(e => e.Quocgia).HasMaxLength(50);

                entity.Property(e => e.Tapdoan).HasMaxLength(50);

                entity.Property(e => e.Tengiaodich).HasMaxLength(100);

                entity.Property(e => e.Tennganhang).HasMaxLength(50);

                entity.Property(e => e.Tenthuongmai).HasMaxLength(100);

                entity.Property(e => e.Thanhpho).HasMaxLength(50);

                entity.Property(e => e.Tinhtp).HasMaxLength(50);

                entity.Property(e => e.Tknganhang).HasMaxLength(50);

                entity.Property(e => e.Tour)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("tour");

                entity.Property(e => e.Website).HasMaxLength(200);
            });

            modelBuilder.Entity<Supplierob>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("supplierob");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Diachi).HasMaxLength(250);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Masothue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nganhnghe).HasMaxLength(300);

                entity.Property(e => e.Ngayhethan).HasColumnType("date");

                entity.Property(e => e.Ngaytao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nguoilienhe).HasMaxLength(100);

                entity.Property(e => e.Nguoitao).HasMaxLength(50);

                entity.Property(e => e.Quocgia).HasMaxLength(50);

                entity.Property(e => e.Tapdoan).HasMaxLength(50);

                entity.Property(e => e.Tengiaodich).HasMaxLength(100);

                entity.Property(e => e.Tennganhang).HasMaxLength(50);

                entity.Property(e => e.Tenthuongmai).HasMaxLength(100);

                entity.Property(e => e.Thanhpho).HasMaxLength(50);

                entity.Property(e => e.Tinhtp).HasMaxLength(50);

                entity.Property(e => e.Tknganhang).HasMaxLength(50);

                entity.Property(e => e.Website).HasMaxLength(150);
            });

            modelBuilder.Entity<Thanhpho>(entity =>
            {
                entity.HasKey(e => e.Matp);

                entity.ToTable("thanhpho");

                entity.Property(e => e.Matp)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("matp");

                entity.Property(e => e.Mien)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("mien");

                entity.Property(e => e.Tentp)
                    .HasMaxLength(50)
                    .HasColumnName("tentp");
            });

            modelBuilder.Entity<Thanhpho1>(entity =>
            {
                entity.HasKey(e => e.Matp)
                    .HasName("PK_thanhpho1_1");

                entity.ToTable("thanhpho1");

                entity.Property(e => e.Matp).HasMaxLength(6);

                entity.Property(e => e.Matinh)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Tentp).HasMaxLength(50);
            });

            modelBuilder.Entity<Tinh>(entity =>
            {
                entity.HasKey(e => e.Matinh);

                entity.ToTable("Tinh");

                entity.Property(e => e.Matinh).HasMaxLength(3);

                entity.Property(e => e.MienId)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Tentinh).HasMaxLength(50);

                entity.Property(e => e.VungId)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("users");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(50)
                    .HasColumnName("dienthoai");

                entity.Property(e => e.Doimk).HasColumnName("doimk");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Hoten)
                    .HasMaxLength(50)
                    .HasColumnName("hoten");

                entity.Property(e => e.Macn)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("macn");

                entity.Property(e => e.Maphong)
                    .HasMaxLength(50)
                    .HasColumnName("maphong");

                entity.Property(e => e.Ngaydoimk)
                    .HasColumnType("date")
                    .HasColumnName("ngaydoimk")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ngaytao)
                    .HasColumnType("datetime")
                    .HasColumnName("ngaytao")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nguoitao)
                    .HasMaxLength(50)
                    .HasColumnName("nguoitao");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("roleId");

                entity.Property(e => e.Trangthai).HasColumnName("trangthai");
            });

            modelBuilder.Entity<VPhongban>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vPhongban");

                entity.Property(e => e.Maphong)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("maphong");

                entity.Property(e => e.Tenphong)
                    .HasMaxLength(50)
                    .HasColumnName("tenphong");
            });

            modelBuilder.Entity<VSupplierob>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSupplierob");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tengiaodich)
                    .HasMaxLength(166)
                    .HasColumnName("tengiaodich");

                entity.Property(e => e.Tour)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("tour");
            });

            modelBuilder.Entity<VTinh>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vTinh");

                entity.Property(e => e.Matinh)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Mien)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("mien");

                entity.Property(e => e.TenMien).HasMaxLength(50);

                entity.Property(e => e.TenVung).HasMaxLength(50);

                entity.Property(e => e.Tentinh).HasMaxLength(50);

                entity.Property(e => e.VungId)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("vungId");
            });

            modelBuilder.Entity<VUserHoadon>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vUserHoadon");

                entity.Property(e => e.Accounthddt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("accounthddt");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.Kyhieuhd)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("kyhieuhd");

                entity.Property(e => e.Mausohd)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("mausohd");

                entity.Property(e => e.Maviettat)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("maviettat");

                entity.Property(e => e.Passwordhddt)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("passwordhddt");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Vungmien>(entity =>
            {
                entity.HasKey(e => e.VungId);

                entity.ToTable("vungmien");

                entity.Property(e => e.VungId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("vungId");

                entity.Property(e => e.Mien)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("mien");

                entity.Property(e => e.TenVung).HasMaxLength(50);
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}