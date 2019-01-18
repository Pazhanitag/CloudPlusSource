using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;

namespace CloudPlus.Test.Helpers
{
    public static class MockSetGenerator
    {
        public static DbSet<T> CreateMockSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);
            mockSet.Setup(m => m.AsNoTracking()).Returns(mockSet.Object);
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => data.Add(s));
            mockSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(s => data.AddRange(s));
            mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => data.Remove(s));
            return mockSet.Object;
        }

        public static DbSet<T> CreateAsyncMockSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IDbAsyncEnumerable<T>>()
                    .Setup(m => m.GetAsyncEnumerator())
                    .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(s => data.Add(s));
            mockSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(s => data.AddRange(s));
            mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => data.Remove(s));

            return mockSet.Object;
        }
    }
}
