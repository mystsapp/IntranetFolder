using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.Models_QLTour
{
    public partial class qltourContext : DbContext
    {
        public qltourContext()
        {
        }

        public qltourContext(DbContextOptions<qltourContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booked> Bookeds { get; set; }
        public virtual DbSet<Chiphikhac> Chiphikhacs { get; set; }
        public virtual DbSet<CodeSupplier> CodeSuppliers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Dichvu> Dichvus { get; set; }
        public virtual DbSet<Dieuxe> Dieuxes { get; set; }
        public virtual DbSet<DmLoaiphong> DmLoaiphongs { get; set; }
        public virtual DbSet<Dmchinhanh> Dmchinhanhs { get; set; }
        public virtual DbSet<Dmdaily> Dmdailies { get; set; }
        public virtual DbSet<Dmdiemtq> Dmdiemtqs { get; set; }
        public virtual DbSet<Dmdiemtq1> Dmdiemtqs1 { get; set; }
        public virtual DbSet<Dmdiemtqob> Dmdiemtqobs { get; set; }
        public virtual DbSet<DsLoaixe> DsLoaixes { get; set; }
        public virtual DbSet<Haucan> Haucans { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelDel> HotelDels { get; set; }
        public virtual DbSet<Hoteltemp> Hoteltemps { get; set; }
        public virtual DbSet<Huongdan> Huongdans { get; set; }
        public virtual DbSet<Khachtour> Khachtours { get; set; }
        public virtual DbSet<Loaikhach> Loaikhaches { get; set; }
        public virtual DbSet<Loaivisa> Loaivisas { get; set; }
        public virtual DbSet<Locktourchinhanh> Locktourchinhanhs { get; set; }
        public virtual DbSet<LoginModel> LoginModels { get; set; }
        public virtual DbSet<Ngoaite> Ngoaites { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<Passtype> Passtypes { get; set; }
        public virtual DbSet<Phongban> Phongbans { get; set; }
        public virtual DbSet<Quan> Quans { get; set; }
        public virtual DbSet<Quocgium> Quocgia { get; set; }
        public virtual DbSet<Sightseeing> Sightseeings { get; set; }
        public virtual DbSet<SightseeingDel> SightseeingDels { get; set; }
        public virtual DbSet<SightseeingTemp> SightseeingTemps { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Thanhpho> Thanhphos { get; set; }
        public virtual DbSet<Thongbao> Thongbaos { get; set; }
        public virtual DbSet<TourTempNote> TourTempNotes { get; set; }
        public virtual DbSet<Tourinf> Tourinfs { get; set; }
        public virtual DbSet<Tourkind> Tourkinds { get; set; }
        public virtual DbSet<Tournode> Tournodes { get; set; }
        public virtual DbSet<TournodeLog> TournodeLogs { get; set; }
        public virtual DbSet<Tourprog> Tourprogs { get; set; }
        public virtual DbSet<TourprogDel> TourprogDels { get; set; }
        public virtual DbSet<Tourprogtemp> Tourprogtemps { get; set; }
        public virtual DbSet<Tourtemplate> Tourtemplates { get; set; }
        public virtual DbSet<Trahaucan> Trahaucans { get; set; }
        public virtual DbSet<Tuyentq> Tuyentqs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VChiphiHaucan> VChiphiHaucans { get; set; }
        public virtual DbSet<VDmdiemtq> VDmdiemtqs { get; set; }
        public virtual DbSet<VDmdiemtqob> VDmdiemtqobs { get; set; }
        public virtual DbSet<VListChiphitour> VListChiphitours { get; set; }
        public virtual DbSet<VSupplier> VSuppliers { get; set; }
        public virtual DbSet<VSuppliertp> VSuppliertps { get; set; }
        public virtual DbSet<VTinh> VTinhs { get; set; }
        public virtual DbSet<VTourinf> VTourinfs { get; set; }
        public virtual DbSet<VTrahaucan> VTrahaucans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                optionsBuilder.UseSqlServer("Server=118.68.170.128;database=qltour;Trusted_Connection=true;User Id=vanhong;Password=Hong@2019;Integrated security=false;MultipleActiveResultSets=true");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Booked>(entity =>
            {
                entity.HasKey(e => e.Idbooking)
                    .HasName("PK_Booked_1");

                entity.ToTable("Booked");

                entity.Property(e => e.Idbooking)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Booking).HasMaxLength(10);

                entity.Property(e => e.ConfirmDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sgtcode)
                    .IsRequired()
                    .HasMaxLength(17);

                entity.Property(e => e.SupplierId)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Chiphikhac>(entity =>
            {
                entity.HasKey(e => e.Idorthercost)
                    .HasName("PK_chiphikhac");

                entity.ToTable("Chiphikhac");

                entity.Property(e => e.Idorthercost)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idorthercost");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 1)")
                    .HasColumnName("amount");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Del).HasColumnName("del");

                entity.Property(e => e.Fromdate).HasColumnName("fromdate");

                entity.Property(e => e.Guidedays).HasColumnName("guidedays");

                entity.Property(e => e.Km).HasColumnName("km");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Srvcode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvcode");

                entity.Property(e => e.Srvnode)
                    .HasMaxLength(250)
                    .HasColumnName("srvnode");

                entity.Property(e => e.Srvprofit)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("srvprofit");

                entity.Property(e => e.Srvtype)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvtype");

                entity.Property(e => e.Todate).HasColumnName("todate");

                entity.Property(e => e.TourItem)
                    .HasMaxLength(250)
                    .HasColumnName("tour_item");

                entity.Property(e => e.Unitprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("unitprice");

                entity.Property(e => e.Vatin).HasColumnName("vatin");

                entity.Property(e => e.Vatout).HasColumnName("vatout");
            });

            modelBuilder.Entity<CodeSupplier>(entity =>
            {
                entity.ToTable("CodeSupplier");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Diachi).HasMaxLength(150);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Ghichu).HasMaxLength(250);

                entity.Property(e => e.Lydo).HasMaxLength(250);

                entity.Property(e => e.Masothue).HasMaxLength(50);

                entity.Property(e => e.Nganhnghe).HasMaxLength(50);

                entity.Property(e => e.Ngayyeucau)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nguoilienhe).HasMaxLength(150);

                entity.Property(e => e.Nguoiyeucau).HasMaxLength(50);

                entity.Property(e => e.Quocgia).HasMaxLength(50);

                entity.Property(e => e.Tapdoan).HasMaxLength(50);

                entity.Property(e => e.Tengiaodich).HasMaxLength(50);

                entity.Property(e => e.Tenthuongmai).HasMaxLength(100);

                entity.Property(e => e.Thanhpho).HasMaxLength(50);

                entity.Property(e => e.Tinhtp).HasMaxLength(50);

                entity.Property(e => e.Website).HasMaxLength(200);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("company");

                entity.Property(e => e.CompanyId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("companyId");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .HasColumnName("address");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Contact)
                    .HasColumnType("date")
                    .HasColumnName("contact");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(80)
                    .HasColumnName("fullname");

                entity.Property(e => e.Headoffice)
                    .HasMaxLength(50)
                    .HasColumnName("headoffice");

                entity.Property(e => e.Msthue)
                    .HasMaxLength(50)
                    .HasColumnName("msthue");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .HasColumnName("name");

                entity.Property(e => e.Nation)
                    .HasMaxLength(50)
                    .HasColumnName("nation");

                entity.Property(e => e.Natione)
                    .HasMaxLength(50)
                    .HasColumnName("natione");

                entity.Property(e => e.Nguoidaidien)
                    .HasMaxLength(50)
                    .HasColumnName("nguoidaidien");

                entity.Property(e => e.Nguoilienhe)
                    .HasMaxLength(50)
                    .HasColumnName("nguoilienhe");

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .HasColumnName("tel");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.Property(e => e.Code)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Codealpha).HasMaxLength(10);

                entity.Property(e => e.Continent).HasMaxLength(50);

                entity.Property(e => e.Nation).HasMaxLength(50);

                entity.Property(e => e.Natione).HasMaxLength(50);

                entity.Property(e => e.TelCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Territory).HasMaxLength(50);
            });

            modelBuilder.Entity<Dichvu>(entity =>
            {
                entity.HasKey(e => e.Iddichvu);

                entity.ToTable("Dichvu");

                entity.Property(e => e.Iddichvu)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Tendv).HasMaxLength(50);
            });

            modelBuilder.Entity<Dieuxe>(entity =>
            {
                entity.HasKey(e => e.Idxe);

                entity.ToTable("Dieuxe");

                entity.Property(e => e.Idxe)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Chiphi).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Del).HasColumnName("del");

                entity.Property(e => e.Denngay).HasColumnType("datetime");

                entity.Property(e => e.Diemdon).HasMaxLength(50);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Dongiakm).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Giodon)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Kmnl).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Laixe).HasMaxLength(50);

                entity.Property(e => e.Loaixe).HasMaxLength(50);

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Lotrinh).HasMaxLength(50);

                entity.Property(e => e.Ngaydon).HasColumnType("date");

                entity.Property(e => e.Sgtcode)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.Soxe)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Supplierid)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.YeuCauXe)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DmLoaiphong>(entity =>
            {
                entity.HasKey(e => e.Loaiphong);

                entity.ToTable("dmLoaiphong");

                entity.Property(e => e.Loaiphong)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Dmchinhanh>(entity =>
            {
                entity.ToTable("Dmchinhanh");

                entity.Property(e => e.Diachi).HasMaxLength(100);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(20);

                entity.Property(e => e.Macn)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Masothue).HasMaxLength(20);

                entity.Property(e => e.Tencn).HasMaxLength(50);

                entity.Property(e => e.Thanhpho).HasMaxLength(50);
            });

            modelBuilder.Entity<Dmdaily>(entity =>
            {
                entity.ToTable("Dmdaily");

                entity.Property(e => e.Daily).HasMaxLength(50);

                entity.Property(e => e.Diachi).HasMaxLength(100);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Macn)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Tendaily).HasMaxLength(100);
            });

            modelBuilder.Entity<Dmdiemtq>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Dmdiemtq_1");

                entity.ToTable("Dmdiemtq");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Congno)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Diemtq).HasMaxLength(150);

                entity.Property(e => e.Giatreem).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Giave).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Thanhpho)
                    .HasMaxLength(10)
                    .HasColumnName("thanhpho");

                entity.Property(e => e.Tilelai).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Tinhtp).HasMaxLength(15);
            });

            modelBuilder.Entity<Dmdiemtq1>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Dmdiemtq");

                entity.ToTable("Dmdiemtq_");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Congno)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Diemtq).HasMaxLength(150);

                entity.Property(e => e.Giatreem).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Giave).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Thanhpho)
                    .HasMaxLength(10)
                    .HasColumnName("thanhpho");

                entity.Property(e => e.Tilelai).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Tinhtp).HasMaxLength(15);
            });

            modelBuilder.Entity<Dmdiemtqob>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Dmdiemtqob");

                entity.Property(e => e.Code)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Congno)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Diemtq).HasMaxLength(150);

                entity.Property(e => e.Giatreem).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Giave).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Manuoc).HasMaxLength(15);

                entity.Property(e => e.Matp).HasMaxLength(10);

                entity.Property(e => e.Tilelai).HasColumnType("decimal(10, 0)");
            });

            modelBuilder.Entity<DsLoaixe>(entity =>
            {
                entity.HasKey(e => e.Loaixe);

                entity.ToTable("DsLoaixe");
            });

            modelBuilder.Entity<Haucan>(entity =>
            {
                entity.ToTable("Haucan");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Dongia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Donvitinh).HasMaxLength(10);

                entity.Property(e => e.Ghichu).HasMaxLength(50);

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Mahh).HasMaxLength(15);

                entity.Property(e => e.Maphieudx).HasMaxLength(20);

                entity.Property(e => e.Ngayyeucau).HasColumnType("datetime");

                entity.Property(e => e.Nguoiyeucau).HasMaxLength(50);

                entity.Property(e => e.Sgtcode).HasMaxLength(17);

                entity.Property(e => e.Thanhtien).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Dbl).HasColumnName("dbl");

                entity.Property(e => e.Dblcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("dblcost");

                entity.Property(e => e.Dblfoc).HasColumnName("dblfoc");

                entity.Property(e => e.Dblpax).HasColumnName("dblpax");

                entity.Property(e => e.Extdbl).HasColumnName("extdbl");

                entity.Property(e => e.Extdblcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("extdblcost");

                entity.Property(e => e.Extsgl).HasColumnName("extsgl");

                entity.Property(e => e.Extsglcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("extsglcost");

                entity.Property(e => e.Exttwn).HasColumnName("exttwn");

                entity.Property(e => e.Exttwncost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("exttwncost");

                entity.Property(e => e.Homestay).HasColumnName("homestay");

                entity.Property(e => e.Homestaycost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("homestaycost");

                entity.Property(e => e.Homestaynote)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("homestaynote")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Homestaypax).HasColumnName("homestaypax");

                entity.Property(e => e.Idtourprog)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("idtourprog");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Note)
                    .HasMaxLength(150)
                    .HasColumnName("note");

                entity.Property(e => e.Oth).HasColumnName("oth");

                entity.Property(e => e.Othcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("othcost");

                entity.Property(e => e.Othpax).HasColumnName("othpax");

                entity.Property(e => e.Othtype)
                    .HasMaxLength(50)
                    .HasColumnName("othtype");

                entity.Property(e => e.Sgl).HasColumnName("sgl");

                entity.Property(e => e.Sglcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("sglcost");

                entity.Property(e => e.Sglfoc).HasColumnName("sglfoc");

                entity.Property(e => e.Sglpax).HasColumnName("sglpax");

                entity.Property(e => e.Sgtcode)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Twn).HasColumnName("twn");

                entity.Property(e => e.Twncost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("twncost");

                entity.Property(e => e.Twnfoc).HasColumnName("twnfoc");

                entity.Property(e => e.Twnpax).HasColumnName("twnpax");
            });

            modelBuilder.Entity<HotelDel>(entity =>
            {
                entity.ToTable("Hotel_del");

                entity.Property(e => e.Id).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Dbl).HasColumnName("dbl");

                entity.Property(e => e.Dblcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("dblcost");

                entity.Property(e => e.Dblfoc).HasColumnName("dblfoc");

                entity.Property(e => e.Dblpax).HasColumnName("dblpax");

                entity.Property(e => e.Extdbl).HasColumnName("extdbl");

                entity.Property(e => e.Extdblcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("extdblcost");

                entity.Property(e => e.Extsgl).HasColumnName("extsgl");

                entity.Property(e => e.Extsglcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("extsglcost");

                entity.Property(e => e.Exttwn).HasColumnName("exttwn");

                entity.Property(e => e.Exttwncost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("exttwncost");

                entity.Property(e => e.Homestay).HasColumnName("homestay");

                entity.Property(e => e.Homestaycost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("homestaycost");

                entity.Property(e => e.Homestaynote)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("homestaynote");

                entity.Property(e => e.Homestaypax).HasColumnName("homestaypax");

                entity.Property(e => e.Idtourprog)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("idtourprog");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Note)
                    .HasMaxLength(150)
                    .HasColumnName("note");

                entity.Property(e => e.Oth).HasColumnName("oth");

                entity.Property(e => e.Othcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("othcost");

                entity.Property(e => e.Othpax).HasColumnName("othpax");

                entity.Property(e => e.Othtype)
                    .HasMaxLength(50)
                    .HasColumnName("othtype");

                entity.Property(e => e.Sgl).HasColumnName("sgl");

                entity.Property(e => e.Sglcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("sglcost");

                entity.Property(e => e.Sglfoc).HasColumnName("sglfoc");

                entity.Property(e => e.Sglpax).HasColumnName("sglpax");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Twn).HasColumnName("twn");

                entity.Property(e => e.Twncost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("twncost");

                entity.Property(e => e.Twnfoc).HasColumnName("twnfoc");

                entity.Property(e => e.Twnpax).HasColumnName("twnpax");
            });

            modelBuilder.Entity<Hoteltemp>(entity =>
            {
                entity.ToTable("Hoteltemp");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Dbl).HasColumnName("dbl");

                entity.Property(e => e.Dblcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("dblcost");

                entity.Property(e => e.Dblfoc).HasColumnName("dblfoc");

                entity.Property(e => e.Dblpax).HasColumnName("dblpax");

                entity.Property(e => e.Extdbl).HasColumnName("extdbl");

                entity.Property(e => e.Extdblcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("extdblcost");

                entity.Property(e => e.Extsgl).HasColumnName("extsgl");

                entity.Property(e => e.Extsglcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("extsglcost");

                entity.Property(e => e.Exttwn).HasColumnName("exttwn");

                entity.Property(e => e.Exttwncost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("exttwncost");

                entity.Property(e => e.Homestay).HasColumnName("homestay");

                entity.Property(e => e.Homestaycost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("homestaycost");

                entity.Property(e => e.Homestaynote)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("homestaynote")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Homestaypax).HasColumnName("homestaypax");

                entity.Property(e => e.Idtourprog)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("idtourprog");

                entity.Property(e => e.Note)
                    .HasMaxLength(150)
                    .HasColumnName("note");

                entity.Property(e => e.Oth).HasColumnName("oth");

                entity.Property(e => e.Othcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("othcost");

                entity.Property(e => e.Othpax).HasColumnName("othpax");

                entity.Property(e => e.Othtype)
                    .HasMaxLength(50)
                    .HasColumnName("othtype");

                entity.Property(e => e.Sgl).HasColumnName("sgl");

                entity.Property(e => e.Sglcost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("sglcost");

                entity.Property(e => e.Sglfoc).HasColumnName("sglfoc");

                entity.Property(e => e.Sglpax).HasColumnName("sglpax");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Twn).HasColumnName("twn");

                entity.Property(e => e.Twncost)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("twncost");

                entity.Property(e => e.Twnfoc).HasColumnName("twnfoc");

                entity.Property(e => e.Twnpax).HasColumnName("twnpax");
            });

            modelBuilder.Entity<Huongdan>(entity =>
            {
                entity.HasKey(e => e.IdHuongdan);

                entity.ToTable("Huongdan");

                entity.Property(e => e.IdHuongdan)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Batdau).HasColumnType("datetime");

                entity.Property(e => e.Batdautai).HasMaxLength(20);

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Del).HasColumnName("del");

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Hieuluchc)
                    .HasColumnType("date")
                    .HasColumnName("hieuluchc");

                entity.Property(e => e.Hochieu)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("hochieu");

                entity.Property(e => e.Hopdongcty)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("hopdongcty");

                entity.Property(e => e.Ketthuc).HasColumnType("datetime");

                entity.Property(e => e.Ketthuctai).HasMaxLength(20);

                entity.Property(e => e.Loaitien)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Ndcongviec).HasMaxLength(100);

                entity.Property(e => e.Ngaysinh)
                    .HasColumnType("date")
                    .HasColumnName("ngaysinh");

                entity.Property(e => e.Ngayyeucau)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ngoaingu).HasMaxLength(50);

                entity.Property(e => e.Phai)
                    .IsRequired()
                    .HasColumnName("phai")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Phididoan).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Phidontien).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Phongks)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phongks");

                entity.Property(e => e.Quoctich)
                    .HasMaxLength(30)
                    .HasColumnName("quoctich");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.Tenhd).HasMaxLength(50);

                entity.Property(e => e.Traphi).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Vemaybay).HasColumnName("vemaybay");
            });

            modelBuilder.Entity<Khachtour>(entity =>
            {
                entity.HasKey(e => e.Idkhach)
                    .HasName("PK_khachtour");

                entity.ToTable("Khachtour");

                entity.Property(e => e.Idkhach)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Cmnd)
                    .HasMaxLength(50)
                    .HasColumnName("cmnd")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Del).HasColumnName("del");

                entity.Property(e => e.Diachi)
                    .HasMaxLength(100)
                    .HasColumnName("diachi");

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(50)
                    .HasColumnName("dienthoai");

                entity.Property(e => e.Ghichu)
                    .HasMaxLength(200)
                    .HasColumnName("ghichu");

                entity.Property(e => e.Hieuluchc)
                    .HasColumnType("date")
                    .HasColumnName("hieuluchc");

                entity.Property(e => e.Hochieu)
                    .HasMaxLength(50)
                    .HasColumnName("hochieu");

                entity.Property(e => e.Hoten)
                    .HasMaxLength(50)
                    .HasColumnName("hoten");

                entity.Property(e => e.Loaiphong)
                    .HasMaxLength(50)
                    .HasColumnName("loaiphong");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Makh)
                    .HasMaxLength(50)
                    .HasColumnName("makh");

                entity.Property(e => e.Ngaycaphc)
                    .HasColumnType("date")
                    .HasColumnName("ngaycaphc");

                entity.Property(e => e.Ngaysinh)
                    .HasColumnType("date")
                    .HasColumnName("ngaysinh");

                entity.Property(e => e.Phai).HasColumnName("phai");

                entity.Property(e => e.Prn)
                    .HasMaxLength(20)
                    .HasColumnName("prn");

                entity.Property(e => e.Quoctich)
                    .HasMaxLength(50)
                    .HasColumnName("quoctich");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Visa).HasMaxLength(50);

                entity.Property(e => e.Vmb).HasColumnName("vmb");

                entity.Property(e => e.YeuCauVisa).HasMaxLength(50);
            });

            modelBuilder.Entity<Loaikhach>(entity =>
            {
                entity.ToTable("Loaikhach");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Tenloaikhach)
                    .HasMaxLength(50)
                    .HasColumnName("tenloaikhach");
            });

            modelBuilder.Entity<Loaivisa>(entity =>
            {
                entity.ToTable("Loaivisa");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Visa)
                    .HasMaxLength(50)
                    .HasColumnName("visa");
            });

            modelBuilder.Entity<Locktourchinhanh>(entity =>
            {
                entity.HasKey(e => new { e.Sgtcode, e.Chinhanh });

                entity.ToTable("locktourchinhanh");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Datelock)
                    .HasColumnType("datetime")
                    .HasColumnName("datelock");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Userlock)
                    .HasMaxLength(50)
                    .HasColumnName("userlock");
            });

            modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("LoginModel");

                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Ngoaite>(entity =>
            {
                entity.HasKey(e => e.MaNt);

                entity.ToTable("Ngoaite");

                entity.Property(e => e.MaNt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("MaNT");

                entity.Property(e => e.TenNt)
                    .HasMaxLength(50)
                    .HasColumnName("TenNT");
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasKey(e => new { e.Sgtcode, e.Stt });

                entity.ToTable("Passenger");

                entity.Property(e => e.Sgtcode).HasMaxLength(17);

                entity.Property(e => e.Stt).HasMaxLength(4);

                entity.Property(e => e.HieulucHc).HasColumnName("HieulucHC");

                entity.Property(e => e.Hochieu).HasMaxLength(20);

                entity.Property(e => e.Hoten).HasMaxLength(50);

                entity.Property(e => e.Loginname)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NgaycapHc).HasColumnName("NgaycapHC");

                entity.Property(e => e.Quoctich).HasMaxLength(20);
            });

            modelBuilder.Entity<Passtype>(entity =>
            {
                entity.ToTable("passtype");

                entity.Property(e => e.PasstypeId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("passtypeId");
            });

            modelBuilder.Entity<Phongban>(entity =>
            {
                entity.HasKey(e => e.Maphong);

                entity.ToTable("Phongban");

                entity.Property(e => e.Maphong)
                    .HasMaxLength(5)
                    .HasColumnName("maphong");

                entity.Property(e => e.Macode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("macode");

                entity.Property(e => e.Tenphong)
                    .HasMaxLength(100)
                    .HasColumnName("tenphong");

                entity.Property(e => e.Trangthai).HasColumnName("trangthai");
            });

            modelBuilder.Entity<Quan>(entity =>
            {
                entity.ToTable("Quan");
            });

            modelBuilder.Entity<Quocgium>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Nation).HasMaxLength(50);

                entity.Property(e => e.Natione).HasMaxLength(50);

                entity.Property(e => e.Telcode)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sightseeing>(entity =>
            {
                entity.ToTable("Sightseeing");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Amount).HasColumnType("decimal(12, 1)");

                entity.Property(e => e.ChildernPrice).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Chinhanh).HasMaxLength(3);

                entity.Property(e => e.Codedtq)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Debit)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Httt)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idtourprog)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("idtourprog");

                entity.Property(e => e.PaxPrice).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Serial)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SerialOld).HasMaxLength(10);

                entity.Property(e => e.Sgtcode)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SightseeingDel>(entity =>
            {
                entity.ToTable("Sightseeing_del");

                entity.Property(e => e.Id).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Amount).HasColumnType("decimal(12, 1)");

                entity.Property(e => e.ChildernPrice).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Chinhanh).HasMaxLength(3);

                entity.Property(e => e.Codedtq)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Debit)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Httt)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idtourprog)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("idtourprog");

                entity.Property(e => e.PaxPrice).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.Serial)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SerialOld).HasMaxLength(10);

                entity.Property(e => e.Sgtcode)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SightseeingTemp>(entity =>
            {
                entity.ToTable("SightseeingTemp");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("amount");

                entity.Property(e => e.Childernprice)
                    .HasColumnType("decimal(12, 0)")
                    .HasColumnName("childernprice");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Codedtq)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("codedtq");

                entity.Property(e => e.Debit)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("debit");

                entity.Property(e => e.Httt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("httt");

                entity.Property(e => e.Idtourprogtemp)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("idtourprogtemp");

                entity.Property(e => e.Paxprice)
                    .HasColumnType("decimal(12, 0)")
                    .HasColumnName("paxprice");

                entity.Property(e => e.Serial)
                    .HasMaxLength(50)
                    .HasColumnName("serial");

                entity.Property(e => e.Srvprofit)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("srvprofit");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Vatin).HasColumnName("vatin");

                entity.Property(e => e.Vatout).HasColumnName("vatout");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Supplier");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Codecn).HasMaxLength(5);

                entity.Property(e => e.Diachi).HasMaxLength(150);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Masothue).HasMaxLength(50);

                entity.Property(e => e.Nganhnghe).HasMaxLength(150);

                entity.Property(e => e.Nguoilienhe).HasMaxLength(70);

                entity.Property(e => e.Nguoitao).HasMaxLength(50);

                entity.Property(e => e.Quocgia).HasMaxLength(50);

                entity.Property(e => e.Tengiaodich).HasMaxLength(70);

                entity.Property(e => e.Tenthuongmai).HasMaxLength(70);

                entity.Property(e => e.Thanhpho).HasMaxLength(50);

                entity.Property(e => e.Website).HasMaxLength(100);
            });

            modelBuilder.Entity<Thanhpho>(entity =>
            {
                entity.HasKey(e => e.Matp);

                entity.ToTable("Thanhpho");

                entity.Property(e => e.Matp).HasMaxLength(10);

                entity.Property(e => e.Mien)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Tentp).HasMaxLength(50);
            });

            modelBuilder.Entity<Thongbao>(entity =>
            {
                entity.ToTable("Thongbao");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Diengiai).HasMaxLength(500);

                entity.Property(e => e.IdTourProg).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Iddichvu)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Ngaycapnhat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ngaydv).HasColumnType("datetime");

                entity.Property(e => e.Nguoinhan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nguoinhap)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.SrvcodeNew)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvcodeNew");

                entity.Property(e => e.SrvcodeOld)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvcodeOld");

                entity.Property(e => e.SupplierIdNew)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("supplierIdNew");

                entity.Property(e => e.SupplierIdOld)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("supplierIdOld");
            });

            modelBuilder.Entity<TourTempNote>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_TourTempNote_1");

                entity.ToTable("TourTempNote");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tourinf>(entity =>
            {
                entity.HasKey(e => e.Sgtcode);

                entity.ToTable("tourinf");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Arr)
                    .HasColumnType("datetime")
                    .HasColumnName("arr");

                entity.Property(e => e.Cancel)
                    .HasColumnType("datetime")
                    .HasColumnName("cancel");

                entity.Property(e => e.Cancelnote)
                    .HasMaxLength(150)
                    .HasColumnName("cancelnote");

                entity.Property(e => e.Childern).HasColumnName("childern");

                entity.Property(e => e.Chinhanh)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Chinhanhtao)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanhtao");

                entity.Property(e => e.CompanyId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("companyId");

                entity.Property(e => e.Concernto)
                    .HasMaxLength(50)
                    .HasColumnName("concernto");

                entity.Property(e => e.Createtour)
                    .HasColumnType("datetime")
                    .HasColumnName("createtour");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Dep)
                    .HasColumnType("datetime")
                    .HasColumnName("dep");

                entity.Property(e => e.Departcreate)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("departcreate");

                entity.Property(e => e.Departoperator)
                    .HasMaxLength(50)
                    .HasColumnName("departoperator");

                entity.Property(e => e.Entryby)
                    .HasMaxLength(50)
                    .HasColumnName("entryby");

                entity.Property(e => e.Entryport)
                    .HasMaxLength(50)
                    .HasColumnName("entryport");

                entity.Property(e => e.Khachle).HasColumnName("khachle");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Note)
                    .HasMaxLength(250)
                    .HasColumnName("note");

                entity.Property(e => e.Operators)
                    .HasMaxLength(50)
                    .HasColumnName("operators");

                entity.Property(e => e.PasstypeId)
                    .HasMaxLength(50)
                    .HasColumnName("passtypeId");

                entity.Property(e => e.Pax).HasColumnName("pax");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("rate");

                entity.Property(e => e.Reference)
                    .HasMaxLength(150)
                    .HasColumnName("reference");

                entity.Property(e => e.Revenue)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("revenue");

                entity.Property(e => e.Routing)
                    .HasMaxLength(100)
                    .HasColumnName("routing");

                entity.Property(e => e.TourkindId).HasColumnName("tourkindId");

                entity.Property(e => e.Userlock)
                    .HasMaxLength(50)
                    .HasColumnName("userlock");

                entity.Property(e => e.Visa)
                    .HasMaxLength(50)
                    .HasColumnName("visa")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Tourkind>(entity =>
            {
                entity.ToTable("Tourkind");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TourkindInf).HasMaxLength(50);
            });

            modelBuilder.Entity<Tournode>(entity =>
            {
                entity.HasKey(e => e.Sgtcode);

                entity.ToTable("Tournode");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TournodeLog>(entity =>
            {
                entity.ToTable("Tournode_log");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ngaynhap)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Sgtcode).HasMaxLength(17);
            });

            modelBuilder.Entity<Tourprog>(entity =>
            {
                entity.ToTable("Tourprog");

                entity.HasIndex(e => new { e.Sgtcode, e.Dieuhanh }, "tourProgIndex_sgtcode");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Airtype)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("airtype");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(14, 1)")
                    .HasColumnName("amount");

                entity.Property(e => e.Arr)
                    .HasMaxLength(100)
                    .HasColumnName("arr");

                entity.Property(e => e.Carguide)
                    .HasMaxLength(100)
                    .HasColumnName("carguide");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(50)
                    .HasColumnName("carrier");

                entity.Property(e => e.Childern).HasColumnName("childern");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Dep)
                    .HasMaxLength(100)
                    .HasColumnName("dep");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("deposit");

                entity.Property(e => e.Dieuhanh)
                    .HasMaxLength(50)
                    .HasColumnName("dieuhanh")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Foc).HasColumnName("foc");

                entity.Property(e => e.Infant).HasColumnName("infant");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Lydohuydv)
                    .HasMaxLength(250)
                    .HasColumnName("lydohuydv")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ngayhuydv)
                    .HasColumnType("datetime")
                    .HasColumnName("ngayhuydv");

                entity.Property(e => e.Ngaythang)
                    .HasColumnType("datetime")
                    .HasColumnName("ngaythang");

                entity.Property(e => e.Nguoihuydv)
                    .HasMaxLength(50)
                    .HasColumnName("nguoihuydv");

                entity.Property(e => e.Pax).HasColumnName("pax");

                entity.Property(e => e.Pickuptime)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("pickuptime");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Srvcode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvcode");

                entity.Property(e => e.Srvnode)
                    .HasMaxLength(500)
                    .HasColumnName("srvnode");

                entity.Property(e => e.Srvtype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("srvtype");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Supplierid)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("supplierid");

                entity.Property(e => e.Time)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("time");

                entity.Property(e => e.TourItem)
                    .HasMaxLength(200)
                    .HasColumnName("tour_item");

                entity.Property(e => e.Unitpricea)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricea");

                entity.Property(e => e.Unitpricec)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricec");

                entity.Property(e => e.Unitpricei)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricei");

                entity.Property(e => e.Vatin).HasColumnName("vatin");

                entity.Property(e => e.Vatout).HasColumnName("vatout");

                entity.HasOne(d => d.SgtcodeNavigation)
                    .WithMany(p => p.Tourprogs)
                    .HasForeignKey(d => d.Sgtcode)
                    .HasConstraintName("FK_Tourprog_tourinf");
            });

            modelBuilder.Entity<TourprogDel>(entity =>
            {
                entity.ToTable("Tourprog_del");

                entity.Property(e => e.Id).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Airtype)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("airtype");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(14, 1)")
                    .HasColumnName("amount");

                entity.Property(e => e.Arr)
                    .HasMaxLength(100)
                    .HasColumnName("arr");

                entity.Property(e => e.Carguide)
                    .HasMaxLength(100)
                    .HasColumnName("carguide");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(50)
                    .HasColumnName("carrier");

                entity.Property(e => e.Childern).HasColumnName("childern");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Dep)
                    .HasMaxLength(100)
                    .HasColumnName("dep");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("deposit");

                entity.Property(e => e.Dieuhanh)
                    .HasMaxLength(50)
                    .HasColumnName("dieuhanh");

                entity.Property(e => e.Foc).HasColumnName("foc");

                entity.Property(e => e.Infant).HasColumnName("infant");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Logfile).HasColumnName("logfile");

                entity.Property(e => e.Lydohuydv)
                    .HasMaxLength(250)
                    .HasColumnName("lydohuydv");

                entity.Property(e => e.Ngayhuydv)
                    .HasColumnType("datetime")
                    .HasColumnName("ngayhuydv")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ngaythang)
                    .HasColumnType("datetime")
                    .HasColumnName("ngaythang");

                entity.Property(e => e.Nguoihuydv)
                    .HasMaxLength(50)
                    .HasColumnName("nguoihuydv");

                entity.Property(e => e.Pax).HasColumnName("pax");

                entity.Property(e => e.Pickuptime)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("pickuptime");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Srvcode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvcode");

                entity.Property(e => e.Srvnode)
                    .HasMaxLength(500)
                    .HasColumnName("srvnode");

                entity.Property(e => e.Srvtype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("srvtype");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Supplierid)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("supplierid");

                entity.Property(e => e.Time)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("time");

                entity.Property(e => e.TourItem)
                    .HasMaxLength(200)
                    .HasColumnName("tour_item");

                entity.Property(e => e.Unitpricea)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricea");

                entity.Property(e => e.Unitpricec)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricec");

                entity.Property(e => e.Unitpricei)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricei");

                entity.Property(e => e.Vatin).HasColumnName("vatin");

                entity.Property(e => e.Vatout).HasColumnName("vatout");
            });

            modelBuilder.Entity<Tourprogtemp>(entity =>
            {
                entity.ToTable("Tourprogtemp");

                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Airtype)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("airtype");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(14, 1)")
                    .HasColumnName("amount");

                entity.Property(e => e.Arr)
                    .HasMaxLength(100)
                    .HasColumnName("arr");

                entity.Property(e => e.Carguide)
                    .HasMaxLength(100)
                    .HasColumnName("carguide");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(50)
                    .HasColumnName("carrier");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Dep)
                    .HasMaxLength(100)
                    .HasColumnName("dep");

                entity.Property(e => e.Foc).HasColumnName("foc");

                entity.Property(e => e.Pickuptime)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("pickuptime");

                entity.Property(e => e.Srvcode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvcode");

                entity.Property(e => e.Srvnode)
                    .HasMaxLength(500)
                    .HasColumnName("srvnode");

                entity.Property(e => e.Srvtype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("srvtype");

                entity.Property(e => e.Stt).HasColumnName("stt");

                entity.Property(e => e.Supplierid)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("supplierid");

                entity.Property(e => e.Time)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("time");

                entity.Property(e => e.TourItem)
                    .HasMaxLength(200)
                    .HasColumnName("tour_item");

                entity.Property(e => e.Unitpricea)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricea");

                entity.Property(e => e.Unitpricec)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricec");

                entity.Property(e => e.Unitpricei)
                    .HasColumnType("decimal(12, 1)")
                    .HasColumnName("unitpricei");

                entity.Property(e => e.Vatin).HasColumnName("vatin");

                entity.Property(e => e.Vatout).HasColumnName("vatout");
            });

            modelBuilder.Entity<Tourtemplate>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Tourtemplate");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Chinhanh)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Chudetour).HasMaxLength(50);

                entity.Property(e => e.Ngaytao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nguoitao)
                    .HasMaxLength(50)
                    .HasColumnName("nguoitao");

                entity.Property(e => e.Songay).HasDefaultValueSql("((1))");

                entity.Property(e => e.Tentour).HasMaxLength(150);

                entity.Property(e => e.Tourkind)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Tuyentq).HasMaxLength(500);
            });

            modelBuilder.Entity<Trahaucan>(entity =>
            {
                entity.HasKey(e => e.IdTrahc);

                entity.ToTable("Trahaucan");

                entity.Property(e => e.IdTrahc)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Ghichutra).HasMaxLength(50);

                entity.Property(e => e.Iddexuat).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Mahh).HasMaxLength(15);

                entity.Property(e => e.Maphieutra).HasMaxLength(20);

                entity.Property(e => e.Ngayyeucau)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nguoiyeucau).HasMaxLength(50);

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tuyentq>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Tuyentq");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Tentuyen).HasMaxLength(150);

                entity.Property(e => e.Tuyen).HasMaxLength(120);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Hoten).HasMaxLength(50);

                entity.Property(e => e.Khachdoan).HasColumnName("khachdoan");

                entity.Property(e => e.Khachle).HasColumnName("khachle");

                entity.Property(e => e.Maphong)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<VChiphiHaucan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vChiphiHaucan");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Dongia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Donvitinh).HasMaxLength(10);

                entity.Property(e => e.Ghichu).HasMaxLength(50);

                entity.Property(e => e.Ghichutra).HasMaxLength(50);

                entity.Property(e => e.Id).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdTrahc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Mahh).HasMaxLength(15);

                entity.Property(e => e.Maphieudx).HasMaxLength(20);

                entity.Property(e => e.Maphieutra).HasMaxLength(20);

                entity.Property(e => e.Ngayyeucau).HasColumnType("datetime");

                entity.Property(e => e.Nguoiyeucau).HasMaxLength(50);

                entity.Property(e => e.Sgtcode).HasMaxLength(17);

                entity.Property(e => e.Tenhh)
                    .HasMaxLength(50)
                    .HasColumnName("tenhh");
            });

            modelBuilder.Entity<VDmdiemtq>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vDmdiemtq");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Diemtq).HasMaxLength(150);

                entity.Property(e => e.Tinhtp)
                    .HasMaxLength(50)
                    .HasColumnName("tinhtp");
            });

            modelBuilder.Entity<VDmdiemtqob>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vDmdiemtqob");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Diemtq).HasMaxLength(150);

                entity.Property(e => e.Manuoc)
                    .HasMaxLength(50)
                    .HasColumnName("manuoc");
            });

            modelBuilder.Entity<VListChiphitour>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vListChiphitour_");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Chinhanhdh)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanhdh");

                entity.Property(e => e.Chinhanhtao)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanhtao");

                entity.Property(e => e.Cpusd)
                    .HasColumnType("decimal(38, 1)")
                    .HasColumnName("cpusd");

                entity.Property(e => e.Cpvnd)
                    .HasColumnType("decimal(38, 1)")
                    .HasColumnName("cpvnd");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Diengiai).HasColumnName("diengiai");

                entity.Property(e => e.Dv)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DV");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ngaythang)
                    .HasColumnType("datetime")
                    .HasColumnName("ngaythang");

                entity.Property(e => e.Sgtcode)
                    .HasMaxLength(17)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Srvnode).HasColumnName("srvnode");

                entity.Property(e => e.Srvtype)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("srvtype");

                entity.Property(e => e.Tengiaodich)
                    .HasMaxLength(201)
                    .HasColumnName("tengiaodich");

                entity.Property(e => e.Vatin).HasColumnName("vatin");

                entity.Property(e => e.Vatout).HasColumnName("vatout");
            });

            modelBuilder.Entity<VSupplier>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSupplier");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Diachi).HasMaxLength(150);

                entity.Property(e => e.Dienthoai).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.Masothue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nganhnghe).HasMaxLength(150);

                entity.Property(e => e.Ngayhethan).HasColumnType("date");

                entity.Property(e => e.Ngaytao).HasColumnType("datetime");

                entity.Property(e => e.Nguoilienhe).HasMaxLength(150);

                entity.Property(e => e.Nguoitao).HasMaxLength(50);

                entity.Property(e => e.Quocgia).HasMaxLength(50);

                entity.Property(e => e.Tapdoan).HasMaxLength(50);

                entity.Property(e => e.Tengiaodich).HasMaxLength(100);

                entity.Property(e => e.Tenthuongmai).HasMaxLength(100);

                entity.Property(e => e.Thanhpho).HasMaxLength(50);

                entity.Property(e => e.Tinhtp).HasMaxLength(50);

                entity.Property(e => e.Website).HasMaxLength(200);
            });

            modelBuilder.Entity<VSuppliertp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSuppliertp");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tengiaodich).HasMaxLength(172);
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

            modelBuilder.Entity<VTourinf>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vTourinf");

                entity.Property(e => e.Arr)
                    .HasColumnType("datetime")
                    .HasColumnName("arr");

                entity.Property(e => e.Cancel)
                    .HasColumnType("datetime")
                    .HasColumnName("cancel");

                entity.Property(e => e.Cancelnote)
                    .HasMaxLength(150)
                    .HasColumnName("cancelnote");

                entity.Property(e => e.Childern).HasColumnName("childern");

                entity.Property(e => e.Chinhanh)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanh");

                entity.Property(e => e.Chinhanh2).HasColumnName("chinhanh2");

                entity.Property(e => e.Chinhanhtao)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("chinhanhtao");

                entity.Property(e => e.CompanyId)
                    .HasMaxLength(150)
                    .HasColumnName("companyId")
                    .UseCollation("Latin1_General_CS_AS");

                entity.Property(e => e.Concernto)
                    .HasMaxLength(50)
                    .HasColumnName("concernto");

                entity.Property(e => e.Createtour)
                    .HasColumnType("datetime")
                    .HasColumnName("createtour");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.Dep)
                    .HasColumnType("datetime")
                    .HasColumnName("dep");

                entity.Property(e => e.Departcreate)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("departcreate");

                entity.Property(e => e.Departoperator)
                    .HasMaxLength(50)
                    .HasColumnName("departoperator");

                entity.Property(e => e.Dieuhanh2).HasColumnName("dieuhanh2");

                entity.Property(e => e.Khachle).HasColumnName("khachle");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Logfile)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("logfile");

                entity.Property(e => e.Operators)
                    .HasMaxLength(50)
                    .HasColumnName("operators");

                entity.Property(e => e.PasstypeId)
                    .HasMaxLength(50)
                    .HasColumnName("passtypeId");

                entity.Property(e => e.Pax).HasColumnName("pax");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("rate");

                entity.Property(e => e.Reference)
                    .HasMaxLength(150)
                    .HasColumnName("reference");

                entity.Property(e => e.Revenue)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("revenue");

                entity.Property(e => e.Routing)
                    .HasMaxLength(100)
                    .HasColumnName("routing");

                entity.Property(e => e.Sgtcode)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false)
                    .HasColumnName("sgtcode");

                entity.Property(e => e.Userlock)
                    .HasMaxLength(50)
                    .HasColumnName("userlock");
            });

            modelBuilder.Entity<VTrahaucan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vTrahaucan");

                entity.Property(e => e.Chinhanh)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Dongia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Donvitinh).HasMaxLength(10);

                entity.Property(e => e.Ghichutra).HasMaxLength(50);

                entity.Property(e => e.Id).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdTrahc).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Locktour)
                    .HasColumnType("datetime")
                    .HasColumnName("locktour");

                entity.Property(e => e.Mahh).HasMaxLength(15);

                entity.Property(e => e.Maphieudx).HasMaxLength(20);

                entity.Property(e => e.Maphieutra).HasMaxLength(20);

                entity.Property(e => e.Ngayyeucau).HasColumnType("datetime");

                entity.Property(e => e.Nguoiyeucau).HasMaxLength(50);

                entity.Property(e => e.Sgtcode).HasMaxLength(17);

                entity.Property(e => e.Tenhh)
                    .HasMaxLength(50)
                    .HasColumnName("tenhh");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}