using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Management.Buckets;
using NUnit.Framework;

namespace IntegrationTestPractice.Tests
{
    [Category("SkipWhenLiveUnitTesting")]
    [TestFixture]
    public class Tests
    {
        private ICouchbaseCollection _collection;
        private DataAccess _dal;
        private ICluster _cluster;
        private List<string> _documentsToDelete;

        [SetUp]
        public async Task Setup()
        {
            var connectionString = Environment.GetEnvironmentVariable("COUCHBASE_CONNECTION_STRING") ?? "couchbase://localhost";
            var username = Environment.GetEnvironmentVariable("COUCHBASE_USERNAME") ?? "Administrator";
            var password = Environment.GetEnvironmentVariable("COUCHBASE_PASSWORD") ?? "password";
            var bucketName = Environment.GetEnvironmentVariable("COUCHBASE_BUCKET_NAME") ?? "tests";

            _cluster = await Cluster.ConnectAsync(connectionString, username, password);

            var bucket = await _cluster.BucketAsync(bucketName);

            _collection = bucket.DefaultCollection();

            _dal = new DataAccess(_collection);

            _documentsToDelete = new List<string>();
        }

        [Test]
        public async Task Does_data_access_retrieve_the_document()
        {
            // arrange - put a document in the database that can be retrieved
            var id = "widget::" + Guid.NewGuid();
            var expectedWidget = new Widget {Name = "Matt's widget " + Guid.NewGuid()};
            await _collection.InsertAsync(id, expectedWidget);
            _documentsToDelete.Add(id);

            // act
            var actualWidget = await _dal.GetWidget(id);

            // assert
            Assert.That(actualWidget.Name, Is.EqualTo(expectedWidget.Name));
        }

        [TearDown]
        public async Task Teardown()
        {
            foreach (var key in _documentsToDelete)
                await _collection.RemoveAsync(key);

            await _cluster.DisposeAsync();
        }
    }
}