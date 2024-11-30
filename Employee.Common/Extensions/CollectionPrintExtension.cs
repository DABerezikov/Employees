namespace Employees.Common.Extensions
{
    public static class CollectionPrintExtension
    {
        private static Action<object>? _strategy;
        public static void Print<T>(this IEnumerable<T> collection, Action<object>? strategy = null)
        {
            _strategy = strategy;
            foreach (var item in collection)
            {
                if (item != null) _strategy?.Invoke(item);
            }
        }
    }
}
