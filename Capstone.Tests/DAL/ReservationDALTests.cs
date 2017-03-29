using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Configuration;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL.Tests
{
    [TestClass()]
    public class ReservationDALTests
    {
        private TransactionScope tran;
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        [TestInitialize]
        public void Setup()
        {
            tran = new TransactionScope();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void ReservationDALContainsData()
        {
            //Arrange
            ReservationDAL testDAL = new ReservationDAL(connectionString);
            //Act
            List<Reservation> reservations = testDAL.ViewReservations(1);
            //Assert
            Assert.IsNotNull(reservations);
            Assert.AreNotEqual(0, reservations.Count());
        }

        [TestMethod()]
        public void ReservationDALMakeReservation()
        {
            // Arrange
            ReservationDAL reservationDAL = new ReservationDAL(connectionString);
            // Act
            bool dataWasInserted = reservationDAL.AddReservation("McTesterson Family Reunion", 4, new DateTime(2099,1,1), new DateTime(2099,2,1));
            // Assert
            Assert.IsNotNull(dataWasInserted);
            Assert.AreEqual(true, dataWasInserted);
        }
    }
}

