using NUnit.Framework;
using System;
namespace test
{
    [TestFixture]
    public class ReservationManagerTests
    {
        private ReservationManager manager;

        [SetUp]
        public void Setup()
        {
            manager = new ReservationManager();
            manager.AddRestaurant("A", 10);
            manager.AddRestaurant("B", 5);
        }

        [Test]
        public void BookTable_ValidBooking_ReturnsTrue()
        {
            DateTime date = new DateTime(2023, 12, 25);
            bool result = manager.BookTable("A", date, 3);

            Assert.IsTrue(result);
        }

        [Test]
        public void BookTable_InvalidRestaurant_ThrowsException()
        {
            DateTime date = new DateTime(2023, 12, 25);

            Assert.Throws<Exception>(() => manager.BookTable("C", date, 3));
        }

        [Test]
        public void BookTable_InvalidTableNumber_ThrowsException()
        {
            DateTime date = new DateTime(2023, 12, 25);

            Assert.Throws<Exception>(() => manager.BookTable("A", date, 15));
        }

        [Test]
        public void GetAllFreeTables_NoBookings_ReturnsAllTables()
        {
            DateTime date = new DateTime(2023, 12, 25);
            var freeTables = manager.GetAllFreeTables(date);

            Assert.AreEqual(15, freeTables.Count);
        }

        [Test]
        public void SortRestAvail_SortedRestaurants_ReturnsSortedRestaurants()
        {
            DateTime date = new DateTime(2023, 12, 25);

            manager.BookTable("A", date, 3); // Book some tables to create different availability

            manager.SortRestAvail(date);

            var sortedRestaurants = manager.res;

            Assert.AreEqual("B", sortedRestaurants[0].name);
        }
    }
}
