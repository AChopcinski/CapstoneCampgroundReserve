using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        public string ConnectionString { get; set; }

        public CampgroundDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Campground> GetAllCampgrounds(int parkNum)
        {
            try
            {
                List<Campground> campgrounds = new List<Campground>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT * FROM campground WHERE park_id = (@parkid);", conn);
                    cmd.Parameters.AddWithValue("@parkid", parkNum);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        campground.ParkID = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
                        campground.CloseMonth = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDouble(reader["daily_fee"]);
                        
                        campgrounds.Add(campground);
                    }
                }
                return campgrounds;
            }
            catch (SqlException ex)
            {
                throw;
            }

        }
    }
}
