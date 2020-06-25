using System.Threading.Tasks;
using Couchbase.KeyValue;

namespace IntegrationTestPractice
{
    public class DataAccess
    {
        private readonly ICouchbaseCollection _collection;

        public DataAccess(ICouchbaseCollection collection)
        {
            _collection = collection;
        }

        public async Task<Widget> GetWidget(string widgetId)
        {
            var result = await _collection.GetAsync(widgetId);
            return result.ContentAs<Widget>();
        }

        public async Task CreateWidget(string widgetId, Widget widget)
        {
            await _collection.InsertAsync(widgetId, widget);
        }
    }

    public class Widget
    {
        public string Name { get; set; }
    }
}
