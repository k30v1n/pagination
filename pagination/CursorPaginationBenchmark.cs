﻿using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination
{
    public class CursorPaginationBenchmark
    {
        const int ITEMS_PER_PAGE = 100;
        const int TOTAL_PAGES_TO_NAVIGATE = 2;

        private readonly DataDbContext _dbContext;
        public CursorPaginationBenchmark() => _dbContext = new DataDbContext();
        public void Dispose() => _dbContext.Dispose();


        public List<T>? RunCursorPaginationPagination<T>(string table, DbSet<T> dbSet) where T : UserBase
        {
            string cursor = string.Empty;

            List<T>? lastPage = null;

            for (int i = 0; i < TOTAL_PAGES_TO_NAVIGATE; i++)
            {
                lastPage = dbSet
                    .FromSqlRaw($"SELECT Id, FirstName, LastName, GrossAmount, DateOfBirth, Sorting_FirstName FROM pagination.{table} where Id > '{cursor}' order by Id asc LIMIT {ITEMS_PER_PAGE}")
                    .AsNoTracking()
                    .ToList();

                if (lastPage.Count == 0) break;

                cursor = lastPage.Last()?.Id.ToString() ?? string.Empty;
            }

            return lastPage;
        }

        [Benchmark]
        public List<FirstExampleData>? RunCursorPaginationPagination_Over_1k() => RunCursorPaginationPagination("1_FirstExample", _dbContext.FirstExampleData);

        [Benchmark]
        public List<SecondExampleData>? RunCursorPaginationPagination_Over_100k() => RunCursorPaginationPagination("2_SecondExample", _dbContext.SecondExampleData);

        [Benchmark]
        public List<ThirdExampleData>? RunCursorPaginationPagination_Over_1million() => RunCursorPaginationPagination("3_ThirdExample", _dbContext.ThirdExampleData);

        [Benchmark]
        public List<ForthExampleData>? RunCursorPaginationPagination_Over_5million() => RunCursorPaginationPagination("4_ForthExample", _dbContext.ForthExampleData);
    }
}
