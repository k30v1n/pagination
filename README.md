# pagination
Pagination implementation over big amount of data that requires pagination and sorting

## Running 
docker run --name local-mysql8 -e MYSQL_ROOT_PASSWORD="root" -p 3306:3306 -d mysql:8





## Dev
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update