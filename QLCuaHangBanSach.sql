use [SQLQuery3.sql];
--1. Thêm bảng nhà xuất bản
CREATE TABLE NhaXuatBan (
    MaNhaXuatBan NVARCHAR(20) PRIMARY KEY,
    TenNhaXuatBan NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    DienThoai NVARCHAR(20)
);

--2. Thêm bảng tác giả
CREATE TABLE TacGia (
    MaTacGia NVARCHAR(20) PRIMARY KEY,
    TenTacGia NVARCHAR(255) NOT NULL,
    LienLac NVARCHAR(255)
);
select * from TacGia;

--3. Thêm bảng thể loại
CREATE TABLE TheLoai (
    MaTheLoai NVARCHAR(20) PRIMARY KEY,
    TenTheLoai NVARCHAR(255) NOT NULL
);

--4. Thêm bảng Sách
CREATE TABLE Sach (
    MaSach NVARCHAR(20) PRIMARY KEY,
    TenSach NVARCHAR(255) NOT NULL,
    SoLuongTon INT DEFAULT 0,
    MaTheLoai NVARCHAR(20),
    MaNhaXuatBan NVARCHAR(20),
    MaTacGia NVARCHAR(20),
    FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai),
    FOREIGN KEY (MaNhaXuatBan) REFERENCES NhaXuatBan(MaNhaXuatBan),
    FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia)
);

--5. Thêm bảng phiếu nhập
CREATE TABLE PhieuNhap (
    SoPhieuNhap NVARCHAR(20) PRIMARY KEY,
    NgayNhap DATE NOT NULL,
    MaNhaXuatBan NVARCHAR(20),
    FOREIGN KEY (MaNhaXuatBan) REFERENCES NhaXuatBan(MaNhaXuatBan)
);
select * from PhieuNhap;

--6. Thêm bảng hóa đơn
CREATE TABLE HoaDon (
    SoHoaDon NVARCHAR(20) PRIMARY KEY,
    NgayBan DATE NOT NULL
);

--7. Thêm bảng chi tiết phiếu nhập
CREATE TABLE ChiTietPhieuNhap (
    MaSach NVARCHAR(20),
    SoPhieuNhap NVARCHAR(20),
    SoLuongNhap INT NOT NULL,
    GiaNhap DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (MaSach, SoPhieuNhap),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    FOREIGN KEY (SoPhieuNhap) REFERENCES PhieuNhap(SoPhieuNhap)
);
select * from ChiTietPhieuNhap;

select PhieuNhap.MaNhaXuatBan, PhieuNhap.NgayNhap, PhieuNhap.SoPhieuNhap,
ChiTietPhieuNhap.MaSach, ChiTietPhieuNhap.SoLuongNhap, ChiTietPhieuNhap.GiaNhap
from PhieuNhap join ChiTietPhieuNhap on PhieuNhap.SoPhieuNhap = ChiTietPhieuNhap.SoPhieuNhap;

--8. Thêm bảng chi tiết hóa đơn
CREATE TABLE ChiTietHoaDon (
    MaSach NVARCHAR(20),
    SoHoaDon NVARCHAR(20),
    SoLuongBan INT NOT NULL,
    GiaBan DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (MaSach, SoHoaDon),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach),
    FOREIGN KEY (SoHoaDon) REFERENCES HoaDon(SoHoaDon),
);

-- Thêm bảng users
CREATE TABLE users (
	id int identity(1, 1) primary key not null,
    username varchar(80) not null,
	password varchar(80) not null,
);
insert into users(username, password) values ('Danh13320031', 'DDaannhh133');
select * from users;
delete from users where users.id = 2 or users.id = 3 or users.id = 4;
SELECT TOP(1) users.username FROM users WHERE username='Danh' AND password='danhhung2003'
drop table users;

select top(1) * from users where users.username = 'Danh133200' and users.password = 'DDaannhh1';

-- 1. Thêm dữ liệu nhà xuất bản
insert into [dbo].[NhaXuatBan] ([MaNhaXuatBan],[TenNhaXuatBan],[DiaChi],[DienThoai]) values 
('NXB1',N'Nhà xuất bản Kim Đồng',N'55 Quang Trung, Hà Nội, Việt Nam ','(024) 39434730'),
('NXB2',N'Nhà xuất bản Tổng hợp thành phố Hồ Chí Minh',N'62 Nguyễn Thị Minh Khai, Phường Đa Kao, Quận 1, TP. HCM ','(028) 38 256 804'),
('NXB3',N'Nhà xuất bản Hội Nhà văn ',N'số 65 Nguyễn Du, Quận Hai Bà Trưng, Hà Nội ','(024) 3822 2135 '),
('NXB4',N'Nhà xuất bản Chính trị quốc gia Sự thật ',N'6/86 Duy tân, Cầu Giấy, Hà Nội  ','024 3822 1581'),
('NXB5',N'Nhà xuất bản Phụ nữ Việt Nam',N'39 Hàng Chuối, Quận Hai Bà Trưng, Hà Nội ','(024) 39710717 '),
('NXB6',N'Nhà xuất bản Lao Động',N'175 Giảng Võ, Đống Đa, Hà Nội ','03435445465'),
('NXB7',N'Nhã Nam ',N'59 Đỗ Quang, Cầu Giấy, Hà Nội ','0903244248'),
('NXB8',N'Đinh Tị Books',N'Nhà NV22 – Khu 12 – Ngõ 13 Lĩnh Nam – P. Mai Động – Q. Hoàng Mai – TP. Hà Nội','0247.309.3388'),
('NXB9',N'Đông A',N'113 Đông Các, P. Ô Chợ Dừa, Q. Đống Đa, Hà Nội ','(024) 3856 9381'),
('NXBa1',N'Nhà xuất bản Trẻ',N'161B Lý Chính Thắng, phường Võ Thị Sáu, Quận 3, TP. Hồ Chí Minh','(84.028) 39316289');
select * from [dbo].[NhaXuatBan];

--2.  Thêm dữ liệu thể loại
 insert into [dbo].[TheLoai] ([MaTheLoai],[TenTheLoai]) values
 ('TL1',N'Sách thiếu nhi'),
 ('TL2',N'Sách tâm lý, tình cảm'),
 ('TL3',N'Sách tôn giáo'),
 ('TL4',N'Sách văn hoá xã hội'),
 ('TL5',N'Sách lịch sử'),
 ('TL6',N' Sách văn học viễn tưởng'),
 ('TL7',N'Sách tiểu sử, tự truyện'),
 ('TL8',N'Sách kinh dị, bí ẩn'),
 ('TL9',N'Sách dạy nấu ăn'),
 ('TLa1',N'Sách khoa học công nghệ'),
 ('TLa2',N'Sách truyền cảm hứng');
 select * from TheLoai;

 --3.  Thêm dữ liệu tác giả
 insert into [dbo].[TacGia] ([MaTacGia],[TenTacGia],[LienLac]) values 
('TG1',N'Tô Hoài',N'Việt Nam'),
('TG2',N'Đoàn Giỏi',N'Việt Nam'),
('TG3',N'Trần Đăng Khoa',N'Việt Nam'),
('TG4',N'Nguyễn Nhật Ánh',N'Việt Nam'),
('TG5',N' Rolf Kalmuczak',N'Đức'),
('TG6',N'Sơn Tùng',N'Việt Nam'),
('TG7',N'Fujiko F. Fujio',N'Nhật Bản'),
('TG8',N' Đặng Thùy Trâm',N'Việt Nam'),
('TG9',N'Yann Martel',N'Cannada'),
('TGa1',N' Murakami Haruki',N'Nhật Bản'),
('TGa2',N'Antoine de Saint-Exupéry',N'Pháp'),
('TGa3',N' William Golding',N'Anh'),
('TGa4',N'Shin Kyung-sook ',N'Hàn Quốc');
select * from TacGia;

-- Lấy tất cả Tác giả có liên lạc là ở Việt Nam
select * from TacGia where TacGia.LienLac = N'Việt Nam';

--4.  Thêm dữ liệu sách
insert into [dbo].[Sach]([MaSach],[TenSach],[SoLuongTon],[MaTheLoai],[MaNhaXuatBan],[MaTacGia]) values
('S1',N'Dế mè phiêu lưu ký',56,'TL1','NXB1','TG1'),
('S2',N'Đất rừng phương Nam',35,'TL1','NXB1','TG2'),
('S3',N'Góc sân và khoảng trời',40,'TL1','NXB1','TG3'),
('S4',N'Kính vạn hoa',59,'TL1','NXB1','TG4'),
('S5',N'Tứ quái TKKG',90,'TL1','NXB1','TG5'),
('S6',N'Búp sen xanh',56,'TL4','NXB1','TG6'),
('S7',N'Doraemon',60,'TL1','NXB1','TG7'),
('S8',N'Nhật ký Đặng Thùy Trâm',56,'TL7','NXB7','TG8'),
('S9',N'Cuộc đời của Pi',57,'TL7','NXB7','TG9'),
('Sa1',N'Rừng Na Uy',58,'TL7','NXB7','TGa1'),
('Sa2',N'Hoàng tử bé',59,'TL7','NXB7','TGa2'),
('Sa3',N'Chúa Ruồi',60,'TLa2','NXB7','TGa3'),
('Sa4',N'Hãy chăm sóc mẹ',61,'TL1','NXB7','TGa4');

-- Lấy tất cả dữ liệu từ bảng sách
select * from Sach;
select top(1) Sach.MaSach from Sach where Sach.MaSach = 'S1'
delete from Sach where Sach.MaSach = '';

update Sach
set Sach.TenSach = 'Kimetsu No Zaiba'
where Sach.MaSach = 'Sa12';

select LEN(Sach.TenSach) from Sach;
select count(Sach.SoLuongTon) from Sach;

select Sach.TenSach, Sach.SoLuongTon 
from Sach 
where Sach.SoLuongTon >= 30 and Sach.SoLuongTon <= 60;

select Sach.MaNhaXuatBan as mxb, Sach.MaSach as id
from Sach;

select Sach.TenSach, 
TheLoai.MaTheLoai, TheLoai.TenTheLoai, 
TacGia.MaTacGia, TacGia.TenTacGia,
NhaXuatBan.MaNhaXuatBan, NhaXuatBan.TenNhaXuatBan
from Sach
inner join TheLoai on Sach.MaTheLoai = TheLoai.MaTheLoai
inner join TacGia on Sach.MaTacGia = TacGia.MaTacGia
inner join NhaXuatBan on Sach.MaNhaXuatBan = NhaXuatBan.MaNhaXuatBan;

select Sach.TenSach, Sach.SoLuongTon
from Sach
where Sach.SoLuongTon >
(
	select AVG(Sach.SoLuongTon)
	from Sach
);

select count(Sach.TenSach)
from Sach
group by Sach.SoLuongTon;

select count(Sach.TenSach), Sach.SoLuongTon
from Sach
group by Sach.SoLuongTon
having Sach.SoLuongTon > 55

select * from Sach where Sach.TenSach like N'N%';

--5.  Thêm dữ liệu phiếu nhập
insert into [dbo].[PhieuNhap]([SoPhieuNhap],[NgayNhap],[MaNhaXuatBan]) values
('P1',N'02-05-2023','NXB1'),
('P2',N'02-06-2023','NXB2'),
('P3',N'02-07-2023','NXB3'),
('P4',N'03-05-2023','NXB4'),
('P5',N'04-05-2023','NXB1'),
('P6',N'09-09-2022','NXB5'),
('P7',N'07-10-2023','NXB8'),
('P8',N'02-11-2023','NXB9'),
('P9',N'04-08-2023','NXBa1'),
('Pa1',N'02-12-2023','NXB4');
select * from PhieuNhap;

delete from PhieuNhap where PhieuNhap.SoPhieuNhap = 'P236';

select month(PhieuNhap.NgayNhap) from PhieuNhap;
select AVG(DISTINCT Sach.SoLuongTon) from Sach;
select MIN(DISTINCT Sach.SoLuongTon) from Sach;
select VAR(DISTINCT Sach.SoLuongTon) from Sach;
select COUNT(40) from Sach;
select STDEV(Sach.SoLuongTon) from Sach;
select LEN(Sach.TenSach) from Sach;
select RIGHT(Sach.TenSach, 5) from Sach;
select LOWER(Sach.TenSach) from Sach;
select CONCAT(Sach.MaTheLoai, ' ', Sach.MaNhaXuatBan) from Sach;
select REPLACE(Sach.MaNhaXuatBan, 'NXB1', 'NXB400') from Sach;
select SUBSTRING(Sach.TenSach, 5, LEN(Sach.TenSach)) from Sach; 
select CEILING(3.2);
select SUM(EXP(Sach.SoLuongTon)) from Sach;


--6.  Thêm dữ liệu chi tiết phiếu nhập
insert into [dbo].[ChiTietPhieuNhap]([MaSach],[SoPhieuNhap],[SoLuongNhap],[GiaNhap])values
('S1','P1', 60, 80000),
('S2','P2', 65, 85000),
('S3','P3', 69, 100000),
('S4','P4', 90, 80000),
('S5','P5', 65, 88000),
('S6','P6', 90, 81300),
('S7','P7', 60, 101400),
('S8','P8', 100, 106500),
('S9','P9', 55, 89200),
('Sa1','Pa1', 20, 200000);
select * from ChiTietPhieuNhap;

SELECT Sach.MaSach, Sach.TenSach, PhieuNhap.NgayNhap, ChiTietPhieuNhap.SoLuongNhap, ChiTietPhieuNhap.GiaNhap 
FROM ChiTietPhieuNhap 
JOIN Sach ON Sach.MaSach = ChiTietPhieuNhap.MaSach 
JOIN PhieuNhap ON PhieuNhap.SoPhieuNhap = ChiTietPhieuNhap.SoPhieuNhap 
WHERE ChiTietPhieuNhap.SoPhieuNhap = PhieuNhap.SoPhieuNhap;

--7. Thêm dữ liệu hóa đơn
insert into [dbo].[HoaDon]([SoHoaDon], [NgayBan])values
('HD001', '2024-01-01'),
('HD002', '2024-01-02'),
('HD003', '2024-01-03'),
('HD004', '2024-01-04'),
('HD005', '2024-01-05'),
('HD006', '2024-01-06'),
('HD007', '2024-01-07'),
('HD008', '2024-01-08'),
('HD009', '2024-01-09'),
('HD010', '2024-01-10');
select * from HoaDon;


--8. Thêm dữ liệu chi tiết hóa đơn
insert into [dbo].[ChiTietHoaDon]([MaSach],[SoHoaDon],[SoLuongBan],[GiaBan])values
('S1', 'HD001', 5, 120000.50),
('S2', 'HD002', 7, 150000.75),
('S3', 'HD003', 3, 95000.25),
('S4', 'HD004', 8, 110000.00),
('S5', 'HD005', 6, 135000.80),
('S6', 'HD006', 4, 102000.35),
('S7', 'HD007', 9, 125000.20),
('S8', 'HD008', 2, 98000.75),
('S9', 'HD009', 6, 145000.50),
('Sa1', 'HD010', 3, 115000.60);
select * from ChiTietHoaDon;

select ChiTietHoaDon.MaSach, ChiTietHoaDon.SoHoaDon, 
ChiTietHoaDon.SoLuongBan, ChiTietHoaDon.GiaBan, HoaDon.NgayBan 
from ChiTietHoaDon 
join HoaDon on ChiTietHoaDon.SoHoaDon = HoaDon.SoHoaDon;

-- Kết nối dữ liệu 2 bảng Sách & Chi tiết hóa đơn bằng left join
select ChiTietHoaDon.SoHoaDon, Sach.TenSach, Sach.MaSach
from ChiTietHoaDon
full join Sach on ChiTietHoaDon.MaSach = Sach.MaSach;

-- Kết nối dữ liệu 2 bảng Phiếu nhập & Chi tiết phiếu nhập bằng full join
select PhieuNhap.SoPhieuNhap, ChiTietPhieuNhap.SoLuongNhap, ChiTietPhieuNhap.GiaNhap
from PhieuNhap
full join ChiTietPhieuNhap on PhieuNhap.SoPhieuNhap = ChiTietPhieuNhap.SoPhieuNhap;

-- Nhóm các hàng ở bảng Sách bằng group by kết hợp having và order by
select count(Sach.MaSach), Sach.SoLuongTon
from Sach
group by Sach.SoLuongTon
having Sach.SoLuongTon > 45
order by count(Sach.MaSach) DESC;