# pagination
Pagination implementation over big amount of data that requires pagination and sorting

# Results
Each of tests below tries to load the 2 first pages of the dataset.


Limit Offset paging without ordering data
|                                 Method |     Mean |     Error |    StdDev |
|--------------------------------------- |---------:|----------:|----------:|
|       RunLimitOffsetPagination_Over_1k | 4.815 ms | 0.0936 ms | 0.1372 ms |
|     RunLimitOffsetPagination_Over_100k | 4.783 ms | 0.0947 ms | 0.1418 ms |
| RunLimitOffsetPagination_Over_1million | 4.749 ms | 0.0806 ms | 0.0715 ms |
| RunLimitOffsetPagination_Over_5million | 4.750 ms | 0.0874 ms | 0.1007 ms |

Cursor pagination ordered only by ID
|                                      Method |     Mean |     Error |    StdDev |
|-------------------------------------------- |---------:|----------:|----------:|
|       RunCursorPaginationPagination_Over_1k | 5.000 ms | 0.0894 ms | 0.1337 ms |
|     RunCursorPaginationPagination_Over_100k | 4.973 ms | 0.0925 ms | 0.1203 ms |
| RunCursorPaginationPagination_Over_1million | 5.011 ms | 0.0994 ms | 0.1688 ms |
| RunCursorPaginationPagination_Over_5million | 5.035 ms | 0.0991 ms | 0.1836 ms |


Limit offset with order by `FirstName`
|                                                  Method |         Mean |      Error |     StdDev |
|-------------------------------------------------------- |-------------:|-----------:|-----------:|
|       RunLimitOffsetPagination_OrderByFirstName_Over_1k |     7.811 ms |  0.1562 ms |  0.1799 ms |
|     RunLimitOffsetPagination_OrderByFirstName_Over_100k |   159.684 ms |  1.2849 ms |  1.2019 ms |
| RunLimitOffsetPagination_OrderByFirstName_Over_1million | 1,773.151 ms |  5.3438 ms |  4.1721 ms |
| RunLimitOffsetPagination_OrderByFirstName_Over_5million | 8,563.475 ms | 31.6342 ms | 28.0429 ms |

Cursor with order on `Sorting_FirstName`
|                                                       Method |          Mean |      Error |     StdDev |
|------------------------------------------------------------- |--------------:|-----------:|-----------:|
|       RunCursorPaginationPagination_OrderByFirstName_Over_1k |      8.251 ms |  0.1784 ms |  0.5032 ms |
|     RunCursorPaginationPagination_OrderByFirstName_Over_100k |    191.977 ms |  3.5741 ms |  3.3432 ms |
| RunCursorPaginationPagination_OrderByFirstName_Over_1million |  2,236.752 ms | 41.6896 ms | 38.9965 ms |
| RunCursorPaginationPagination_OrderByFirstName_Over_5million | 10,891.878 ms | 57.6054 ms | 48.1031 ms |

## Running 
docker run --name local-mysql8 -e MYSQL_ROOT_PASSWORD="root" -p 3306:3306 -d mysql:8

## Dev
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add Baseline
dotnet ef database update