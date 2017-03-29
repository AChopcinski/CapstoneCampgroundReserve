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
    public class CampgroundDALTests
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
        public void CampgroundDALIsStuffHereTest()
        {
            //Arrange
            CampgroundDAL testDAL = new CampgroundDAL(connectionString);
            //Act
            List<Campground> campgrounds = testDAL.GetAllCampgrounds(1);
            //Assert
            Assert.IsNotNull(campgrounds);
            Assert.AreNotEqual(0, campgrounds.Count());
        }

    }
}