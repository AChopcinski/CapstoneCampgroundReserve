using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteDAL
    {
        public string ConnectionString { get; set; }

        public SiteDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }


        public List<Site> GetAvailableSites(int campgroundID, DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Site> sites = new List<Site>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT TOP 5 * FROM site WHERE campground_id = (@campgroundid) AND site_id NOT IN (SELECT site.site_id FROM site join reservation on reservation.site_id = site.site_id WHERE((@startdate) >= from_date AND (@startdate) <= to_date) OR((@enddate) >= from_date AND (@enddate) <= to_date) OR((@startdate) <= from_date AND (@enddate) >= to_date));",conn);
                    cmd.Parameters.AddWithValue("@campgroundid", campgroundID);
                    cmd.Parameters.AddWithValue("@startdate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.SiteID = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.IsWheelChairAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.SiteNumberInCampground = Convert.ToInt32(reader["site_number"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.HasRVHookup= Convert.ToBoolean(reader["utilities"]);

                        sites.Add(site);
                    }
                }
                return sites;
            }
            catch (SqlException ex)
            {
                throw;
            }

        }
    }
}
