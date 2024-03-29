﻿using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination
{
    public class LimitOffsetPaginationBenchmark: IDisposable
    {
        const int ITEMS_PER_PAGE = 100;
        const int TOTAL_PAGES_TO_NAVIGATE = 2;

        private readonly DataDbContext _dbContext;
        public LimitOffsetPaginationBenchmark() => _dbContext = new DataDbContext();
        public void Dispose() => _dbContext.Dispose();


        public List<T>? RunLimitOffsetPagination<T>(string table, DbSet<T> dbSet) where T : class
        {
            var offset = 0;

            List<T>? lastPage = null;

            for (int i = 0; i < TOTAL_PAGES_TO_NAVIGATE; i++)
            {
                lastPage = dbSet
                    .FromSqlRaw($"SELECT Id, FirstName, LastName, GrossAmount, DateOfBirth, Sorting_FirstName FROM pagination.{table} LIMIT {offset}, {ITEMS_PER_PAGE}")
                    .AsNoTracking()
                    .ToList();

                offset += ITEMS_PER_PAGE;
            }

            return lastPage;
        }

        [Benchmark]
        public List<FirstExampleData>? RunLimitOffsetPagination_Over_1k() => RunLimitOffsetPagination("1_FirstExample", _dbContext.FirstExampleData);

        [Benchmark]
        public List<SecondExampleData>? RunLimitOffsetPagination_Over_100k() => RunLimitOffsetPagination("2_SecondExample", _dbContext.SecondExampleData);

        [Benchmark]
        public List<ThirdExampleData>? RunLimitOffsetPagination_Over_1million() => RunLimitOffsetPagination("3_ThirdExample", _dbContext.ThirdExampleData);

        [Benchmark]
        public List<ForthExampleData>? RunLimitOffsetPagination_Over_5million() => RunLimitOffsetPagination("4_ForthExample", _dbContext.ForthExampleData);
    }
}
