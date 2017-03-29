using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationDAL
    {
        public string ConnectionString { get; set; }

        public ReservationDAL(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Reservation> ViewReservations(int parkID)
        {
            try
            {
                List<Reservation> reservations = new List<Reservation>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"select reservation.reservation_id, reservation.site_id, reservation.name, reservation.from_date, reservation.to_date, reservation.create_date from reservation join site on site.site_id = reservation.site_id join campground on campground.campground_id = site.campground_id join park on park.park_id = campground.park_id where park.park_id = (@parkid);", conn);
                    cmd.Parameters.AddWithValue("@parkid", parkID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation.ReservationID = Convert.ToInt32(reader["reservation_id"]);
                        reservation.SiteID = Convert.ToInt32(reader["site_id"]);
                        reservation.CustomerName = Convert.ToString(reader["name"]);
                        reservation.ArrivalDate = Convert.ToDateTime(reader["from_date"]);
                        reservation.DepartureDate = Convert.ToDateTime(reader["to_date"]);
                        reservation.CreatedDate = Convert.ToDateTime(reader["create_date"]);

                        reservations.Add(reservation);
                    }
                }
                return reservations;
            }
            catch (SqlException ex)
            {
                throw;
            }

        }

        public bool AddReservation(string personName, int siteID, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"insert into reservation (site_id,name,from_date,to_date,create_date) VALUES((@siteid),(@name),(@startdate),(@enddate),GETDATE());", conn);
                    cmd.Parameters.AddWithValue("@siteid", siteID);
                    cmd.Parameters.AddWithValue("@name", personName);
                    cmd.Parameters.AddWithValue("@startdate", startDate);
                    cmd.Parameters.AddWithValue("@enddate", endDate);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public int GetReservationID(string personName, int siteID, DateTime startDate, DateTime endDate)
        {
            int reservationID = 0;
            try
            {
                List<Reservation> campgrounds = new List<Reservation>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT reservation_ID FROM reservation WHERE site_id = (@siteid) AND name = (@name) AND from_date = (@start_date) AND to_date = (@end_date);", conn);
                    cmd.Parameters.AddWithValue("@siteid", siteID);
                    cmd.Parameters.AddWithValue("@name", personName);
                    cmd.Parameters.AddWithValue("@start_date", startDate);
                    cmd.Parameters.AddWithValue("@end_date", endDate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        reservationID = Convert.ToInt32(reader["reservation_id"]);
                    }
                }
                return reservationID;
            }
            catch (SqlException ex)
            {
                throw;
            }

        }

        public bool CancelReservation(int reservationID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"DELETE FROM reservation WHERE reservation_id = (@reservationID);", conn);
                    cmd.Parameters.AddWithValue("@reservationID", reservationID);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
    }
}

