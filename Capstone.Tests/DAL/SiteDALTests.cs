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
    public class SiteDALTests
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
        public void SiteDALContainsData()
        {
            //Arrange
            SiteDAL testDAL = new SiteDAL(connectionString);
            //Act
            List<Site> sites = testDAL.GetAvailableSites(1, new DateTime(2017, 2, 15), new DateTime(2017, 2, 17));
            //Assert
            Assert.IsNotNull(sites);
            Assert.AreNotEqual(0, sites.Count());
        }

    }
}
