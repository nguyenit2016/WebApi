# WebApi
# Setup dự án
1: Mở project bằng visual studio

2: Thay đổi Server name của SQL server:
	Trong file appsettings.json, tìm key: OrderDbConnection và thay đổi Server="Server name trên SSMS"
	
3: Migration database: tại visual studio: Chọn Tools => NuGet Package Manager => Package Manager Console => nhập lệnh: "dotnet ef database update" và enter

4: Chạy dự án OrderServices

Tạo Migration mới(mỗi khi thay đổi database thì cần phải tạo mới 1 migration): dotnet ef migrations add InitialCreate
Migration lên database (update database khi có thay đổi): dotnet ef database update