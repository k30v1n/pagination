using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination
{
    public class LimitOffsetWithOrderPaginationBenchmark: IDisposable
    {
        const int ITEMS_PER_PAGE = 100;
        const int TOTAL_PAGES_TO_NAVIGATE = 2;

        private readonly DataDbContext _dbContext;
        public LimitOffsetWithOrderPaginationBenchmark() => _dbContext = new DataDbContext();
        public void Dispose() => _dbContext.Dispose();


        public void RunLimitOffsetPagination<T>(string table, DbSet<T> dbSet) where T: class
        {
            var offset = 0;

            for (int i = 0; i < TOTAL_PAGES_TO_NAVIGATE; i++)
            {
                dbSet.FromSqlRaw($"SELECT * FROM pagination.{table} order by FirstName LIMIT {offset}, {ITEMS_PER_PAGE}")
                    .ToList();

                offset += ITEMS_PER_PAGE;
            }
        }

        [Benchmark]
        public void RunLimitOffsetPagination_OrderByFirstName_Over_1k() => RunLimitOffsetPagination("1_FirstExample", _dbContext.FirstExampleData);

        [Benchmark]
        public void RunLimitOffsetPagination_OrderByFirstName_Over_100k() => RunLimitOffsetPagination("2_SecondExample", _dbContext.SecondExampleData);

        [Benchmark]
        public void RunLimitOffsetPagination_OrderByFirstName_Over_1million() => RunLimitOffsetPagination("3_ThirdExample", _dbContext.ThirdExampleData);

        [Benchmark]
        public void RunLimitOffsetPagination_OrderByFirstName_Over_5million() => RunLimitOffsetPagination("4_ForthExample", _dbContext.ForthExampleData);
    }
}
