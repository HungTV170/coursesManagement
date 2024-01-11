# Sử dụng một hình ảnh có SDK .NET 7.0 để xây dựng ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Sao chép tất cả các tệp vào thư mục làm việc
COPY . .

# Thực hiện quá trình restore
RUN dotnet restore

# Thực hiện quá trình xây dựng
RUN dotnet build -c Release

RUN dotnet publish -c Release
# Giai đoạn 2: Xuất bản ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app

# Sao chép tất cả từ giai đoạn xây dựng (build stage) vào thư mục làm việc
COPY --from=build /app/bin/Release/net7.0/publish .

# Sau các dòng COPY để xây dựng ứng dụng, thêm dòng sau để sao chép thư mục wwwroot
COPY wwwroot /app/wwwroot

# Chỉ định cổng mà ứng dụng sẽ lắng nghe
EXPOSE 80

# Lệnh để chạy ứng dụng khi container khởi chạy
CMD ["dotnet", "CourseManagement.dll"]
