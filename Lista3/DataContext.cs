using System;
using System.Web;

namespace Lista3
{
    // Mock data access context (simulates DbContext/SqlConnection)
    public class FakeDataContext : IDisposable
    {
        public string Id { get; }
        public FakeDataContext()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        // Simulate a query
        public string QueryInfo() => "FakeDataContext Id=" + Id;

        public void Dispose()
        {
            // release resources here (simulated)
        }
    }

    // Manager providing per-request pseudo-singleton stored in Context.Items
    public static class DataContextManager
    {
        private const string ItemsKey = "PerRequest.DataContext";

        public static FakeDataContext Current
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null) throw new InvalidOperationException("Brak HttpContext.Current");
                var items = ctx.Items;
                var existing = items[ItemsKey] as FakeDataContext;
                if (existing != null) return existing;
                var created = new FakeDataContext();
                items[ItemsKey] = created;
                return created;
            }
        }

        public static void DisposeCurrent()
        {
            var ctx = HttpContext.Current;
            if (ctx == null) return;
            var items = ctx.Items;
            var existing = items[ItemsKey] as IDisposable;
            if (existing != null)
            {
                try { existing.Dispose(); } catch { }
                items[ItemsKey] = null;
            }
        }
    }
}
