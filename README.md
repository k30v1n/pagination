# pagination
Pagination implementation over big amount of data that requires pagination and sorting

# Results

Without ordering data
|                                 Method |     Mean |     Error |    StdDev |
|--------------------------------------- |---------:|----------:|----------:|
|       RunLimitOffsetPagination_Over_1k | 4.815 ms | 0.0936 ms | 0.1372 ms |
|     RunLimitOffsetPagination_Over_100k | 4.783 ms | 0.0947 ms | 0.1418 ms |
| RunLimitOffsetPagination_Over_1million | 4.749 ms | 0.0806 ms | 0.0715 ms |
| RunLimitOffsetPagination_Over_5million | 4.750 ms | 0.0874 ms | 0.1007 ms |

Ordering data
|                                                  Method |         Mean |      Error |     StdDev |
|-------------------------------------------------------- |-------------:|-----------:|-----------:|
|       RunLimitOffsetPagination_OrderByFirstName_Over_1k |     7.811 ms |  0.1562 ms |  0.1799 ms |
|     RunLimitOffsetPagination_OrderByFirstName_Over_100k |   159.684 ms |  1.2849 ms |  1.2019 ms |
| RunLimitOffsetPagination_OrderByFirstName_Over_1million | 1,773.151 ms |  5.3438 ms |  4.1721 ms |
| RunLimitOffsetPagination_OrderByFirstName_Over_5million | 8,563.475 ms | 31.6342 ms | 28.0429 ms |


## Running 
docker run --name local-mysql8 -e MYSQL_ROOT_PASSWORD="root" -p 3306:3306 -d mysql:8





## Dev
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update