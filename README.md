# pagination
Pagination implementation over big amount of data that requires pagination and sorting


# Sample data
Currently the database has 4 examples on 4 diffeerent tables which one contains a different number of records but shares the same schema. All this data is automatically added to the database on the first run.

```sql
select count(*) from 1_FirstExample; -- 1k 
select count(*) from 2_SecondExample; -- 100k
select count(*) from 3_ThirdExample; -- 1M 
select count(*) from 4_ForthExample; -- 5M
```

# Results
- Each of tests below tries to load the 2 first pages of the dataset.
- Dabase with 2 cores and 8GB ram (docker)
- App runing on a Windows machine, 32gb ram, i5 9th gen


## `CursorPaginationBenchmark` Cursor pagination using ID order only without ordering data

|                                      Method |     Mean |     Error |    StdDev |
|-------------------------------------------- |---------:|----------:|----------:|
|       RunCursorPaginationPagination_Over_1k | 4.580 ms | 0.0464 ms | 0.0434 ms |
|     RunCursorPaginationPagination_Over_100k | 4.085 ms | 0.0783 ms | 0.0870 ms |
| RunCursorPaginationPagination_Over_1million | 4.049 ms | 0.0617 ms | 0.0577 ms |
| RunCursorPaginationPagination_Over_5million | 4.081 ms | 0.0598 ms | 0.0559 ms |

```
Run time: 00:01:04 (64.7 sec), executed benchmarks: 4
Global total time: 00:01:09 (69.45 sec), executed benchmarks: 4

Database CPU usage < 30%
```

## `LimitOffsetPaginationBenchmark` Limit Offset paging without ordering data

|                                 Method |     Mean |     Error |    StdDev |
|--------------------------------------- |---------:|----------:|----------:|
|       RunLimitOffsetPagination_Over_1k | 4.412 ms | 0.0855 ms | 0.0878 ms |
|     RunLimitOffsetPagination_Over_100k | 3.964 ms | 0.0777 ms | 0.0983 ms |
| RunLimitOffsetPagination_Over_1million | 3.970 ms | 0.0760 ms | 0.0813 ms |
| RunLimitOffsetPagination_Over_5million | 4.048 ms | 0.0762 ms | 0.1940 ms |

```
Run time: 00:01:42 (102.49 sec), executed benchmarks: 4
Global total time: 00:01:47 (107.15 sec), executed benchmarks: 4

Database CPU: <30%
```

## `CursorPaginationWithOrderBenchmark`: Cursor with order on `Sorting_FirstName` (contains firstname + cursor unique identifier)

|                                                       Method |         Mean |      Error |     StdDev |
|------------------------------------------------------------- |-------------:|-----------:|-----------:|
|       RunCursorPaginationPagination_OrderByFirstName_Over_1k |     6.344 ms |  0.1158 ms |  0.0967 ms |
|     RunCursorPaginationPagination_OrderByFirstName_Over_100k |   142.488 ms |  2.4281 ms |  2.2712 ms |
| RunCursorPaginationPagination_OrderByFirstName_Over_1million | 1,797.274 ms | 35.7267 ms | 45.1828 ms |
| RunCursorPaginationPagination_OrderByFirstName_Over_5million | 8,408.856 ms | 46.6687 ms | 43.6540 ms |

```
Run time: 00:05:51 (351.37 sec), executed benchmarks: 4
Global total time: 00:05:55 (356 sec), executed benchmarks: 4

Database CPU 100%
```

## `LimitOffsetWithOrderPaginationBenchmark` Limit offset Ordering by `FirstName`

|                                                  Method |         Mean |      Error |     StdDev |
|-------------------------------------------------------- |-------------:|-----------:|-----------:|
|       RunLimitOffsetPagination_OrderByFirstName_Over_1k |     6.223 ms |  0.0610 ms |  0.0571 ms |
|     RunLimitOffsetPagination_OrderByFirstName_Over_100k |   128.780 ms |  1.7591 ms |  1.5594 ms |
| RunLimitOffsetPagination_OrderByFirstName_Over_1million | 1,566.146 ms |  9.9261 ms |  8.7992 ms |
| RunLimitOffsetPagination_OrderByFirstName_Over_5million | 7,613.383 ms | 35.0308 ms | 31.0539 ms |

```
Run time: 00:04:39 (279.97 sec), executed benchmarks: 4
Global total time: 00:04:45 (285.06 sec), executed benchmarks: 4

Database CPU 100%
```

# Test using 100 page size + navigation through 100 pages

comparison only between paginations and no ordering involved

LIMIT OFFSET  - DATABASE GOT 61%+
|                                 Method |     Mean |   Error |   StdDev |
|--------------------------------------- |---------:|--------:|---------:|
|       RunLimitOffsetPagination_Over_1k | 234.5 ms | 4.65 ms | 12.33 ms |
|     RunLimitOffsetPagination_Over_100k | 463.7 ms | 9.20 ms | 14.05 ms |
| RunLimitOffsetPagination_Over_1million | 461.4 ms | 8.93 ms | 11.29 ms |
| RunLimitOffsetPagination_Over_5million | 464.0 ms | 9.01 ms |  8.85 ms |


CURSOR PAGINATION - DATABASE 27%
|                                      Method |      Mean |    Error |    StdDev |    Median |
|-------------------------------------------- |----------:|---------:|----------:|----------:|
|       RunCursorPaginationPagination_Over_1k |  22.98 ms | 0.342 ms |  0.285 ms |  22.92 ms |
|     RunCursorPaginationPagination_Over_100k | 197.84 ms | 3.872 ms | 10.201 ms | 194.55 ms |
| RunCursorPaginationPagination_Over_1million | 192.28 ms | 3.827 ms |  5.109 ms | 193.91 ms |
| RunCursorPaginationPagination_Over_5million | 193.53 ms | 3.861 ms |  5.285 ms | 194.67 ms |

## Running 

Database should be running
```
docker run --cpus=2.000 --memory="8g" --name pagination-mysql8 -e MYSQL_ROOT_PASSWORD="root" -p 3306:3306 -d mysql:8
```

Run migrations
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add Baseline
dotnet ef database update
```

Navigate to `pagination` folder where the csproj is.

Run the project (the first run will take time as it will populate all mock data)
```
dotnet run -c Release
```

It will be prompted all options to run each benchmark separately.


# Legends
```
  Mean   : Arithmetic mean of all measurements
  Error  : Half of 99.9% confidence interval
  StdDev : Standard deviation of all measurements
  Median : Value separating the higher half of all measurements (50th percentile)
  1 ms   : 1 Millisecond (0.001 sec)
```