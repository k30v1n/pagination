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
    public class LimitOffsetPaginationBenchmark: IDisposable
    {
        const int ITEMS_PER_PAGE = 100;
        const int TOTAL_PAGES_TO_NAVIGATE = 50;

        private readonly DataDbContext _dbContext;
        public LimitOffsetPaginationBenchmark() => _dbContext = new DataDbContext();
        public void Dispose() => _dbContext.Dispose();


        public void RunLimitOffsetPagination(string table)
        {
            var offset = 0;

            for (int i = 0; i < TOTAL_PAGES_TO_NAVIGATE; i++)
            {
                _dbContext.FirstExampleData
                    .FromSqlRaw($"SELECT * FROM pagination.{table} order by FirstName LIMIT {offset}, {ITEMS_PER_PAGE}")
                    .ToList();

                offset += ITEMS_PER_PAGE;
            }
        }

        [Benchmark]
        public void RunLimitOffsetPagination_Over_1k() => RunLimitOffsetPagination("1_FirstExample");

        [Benchmark]
        public void RunLimitOffsetPagination_Over_10k() => RunLimitOffsetPagination("2_SecondExample");

        //[Benchmark]
        //public void RunLimitOffsetPagination_Over_100k() => RunLimitOffsetPagination("3_ThirdExample");

        //[Benchmark]
        //public void RunLimitOffsetPagination_Over_1million() => RunLimitOffsetPagination("4_ForthExample");
    }
}
