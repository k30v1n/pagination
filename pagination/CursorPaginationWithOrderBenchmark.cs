using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination
{
    public class CursorPaginationWithOrderBenchmark
    {
        const int ITEMS_PER_PAGE = 100;
        const int TOTAL_PAGES_TO_NAVIGATE = 2;

        private readonly DataDbContext _dbContext;
        public CursorPaginationWithOrderBenchmark() => _dbContext = new DataDbContext();
        public void Dispose() => _dbContext.Dispose();


        public void RunCursorPaginationPagination<T>(string table, DbSet<T> dbSet) where T : UserBase
        {
            string cursor = string.Empty;

            for (int i = 0; i < TOTAL_PAGES_TO_NAVIGATE; i++)
            {
                var result = dbSet
                    .FromSqlRaw($"SELECT * FROM pagination.{table} where Sorting_FirstName > '{cursor}' order by Sorting_FirstName asc LIMIT {ITEMS_PER_PAGE}")
                    .ToList();

                cursor = result.Last()?.Sorting_FirstName?.ToString() ?? string.Empty;
            }
        }

        [Benchmark]
        public void RunCursorPaginationPagination_OrderByFirstName_Over_1k() => RunCursorPaginationPagination("1_FirstExample", _dbContext.FirstExampleData);

        [Benchmark]
        public void RunCursorPaginationPagination_OrderByFirstName_Over_100k() => RunCursorPaginationPagination("2_SecondExample", _dbContext.SecondExampleData);

        [Benchmark]
        public void RunCursorPaginationPagination_OrderByFirstName_Over_1million() => RunCursorPaginationPagination("3_ThirdExample", _dbContext.ThirdExampleData);

        [Benchmark]
        public void RunCursorPaginationPagination_OrderByFirstName_Over_5million() => RunCursorPaginationPagination("4_ForthExample", _dbContext.ForthExampleData);
    }
}
