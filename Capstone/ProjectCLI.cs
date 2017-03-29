using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Capstone
{
    public class ProjectCLI
    {
        const string Command_Quit = "q";
        const string Command_Return = "r";
        const string Command_ViewAllParks = "1";
        const string Command_DeleteRSVP = "2";

        static string DatabaseConnection = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

        public void RunCLI()
        {
            DisplayMainMenu();

            while (true)
            {
                string command = Console.ReadLine();
                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_ViewAllParks:
                        ViewAllParks();
                        break;
                    case Command_DeleteRSVP:
                        CancelReservation();
                        break;
                    case Command_Quit:
                        Console.WriteLine("Thank you for using the national parks reservation system!");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }
                Console.Clear();
                DisplayMainMenu();
            }
        }

        private void CancelReservation()
        {
            while (true)
            {
                ReservationDAL reservationDAL = new DAL.ReservationDAL(DatabaseConnection);
                int reservationID = CLIHelper.GetInteger("Enter Reservation ID to delete (enter 0 to go back): ");

                if (reservationID == 0)
                {
                    break;
                }

                try
                {
                    if (reservationDAL.CancelReservation(reservationID))
                    {
                        Console.WriteLine("Successfully Cancelled");
                        Console.ReadLine();
                        break;
                    }

                }
                catch
                {
                    Console.WriteLine("Unable to Find Reservation to Cancel"); ;
                    break;
                }
            }
        }

        private void ViewAllParks()
        {
            while (true)
            {
                ParkDAL parkDAL = new DAL.ParkDAL(DatabaseConnection);
                List<Park> parks = parkDAL.GetAllParks();
                Console.Clear();
                Console.WriteLine("Choose a Park:");
                Console.WriteLine("--------------");

                if (parks.Count() > 0)
                {
                    for (int i = 0; i < parks.Count(); i++)
                    {
                        Console.WriteLine((i + 1) + ": " + parks[i].Name + " National Park");
                    }
                    Console.WriteLine("\nR: Return to Previous Menu");
                }

                string command = Console.ReadLine();
                Console.Clear();

                if (command.ToLower() == Command_Return)
                {
                    return;
                }
                else if (command.Length == 1 && Convert.ToInt32(command.ToLower()) <= parks.Count() && Convert.ToInt32(command.ToLower()) >= 0)
                {
                    ViewParkInformation(command);
                }
                else
                {
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                }
            }
        }

        private void ViewParkInformation(string choice)
        {
            while (true)
            {
                ParkDAL parkDAL = new DAL.ParkDAL(DatabaseConnection);
                List<Park> parks = parkDAL.GetAllParks();
                Park park = parks[Convert.ToInt32(choice) - 1];

                DisplayParkInformation(park);
                DisplayParkMenu();

                string command = Console.ReadLine();
                Console.Clear();

                if (command.ToLower() == Command_Return)
                {
                    return;
                }
                else if (command == "1")
                {
                    ViewCampgroundsAtPark(park.ParkID, park.Name);
                }
                else if (command == "2")
                {
                    ViewAllReservations(park.ParkID, park.Name);
                }
                else
                {
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                }
            }
        }


        private static void DisplayParkInformation(Park park)
        {
            Console.Clear();
            Console.WriteLine(park.Name + " National Park");
            Console.WriteLine("----------------------");
            Console.WriteLine("Location: ".PadRight(20) + park.Location);
            Console.WriteLine("Established: ".PadRight(20) + park.EstablishedDate.ToString("yyyy-MM-dd"));
            Console.WriteLine("Area: ".PadRight(20) + park.Area.ToString("N0") + " Square Miles");
            Console.WriteLine("Yearly Visitors: ".PadRight(20) + park.Visitors.ToString("N0"));
            Console.WriteLine();
            WordWrapParkDescription(park);
        }

        private static void WordWrapParkDescription(Park park)
        {
            string[] words = park.Description.Split(' ');
            List<string> lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
            {
                if (l.Last().Length + w.Length >= 80)
                    l.Add(w);
                else
                    l[l.Count - 1] += " " + w;
                return l;
            });

            for (int i = 0; i < lines.Count(); i++)
            {
                Console.WriteLine(lines[i]);
            }
        }

        private void ViewAllReservations(int parkID, string parkName)
        {
            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            List<Reservation> upcomingReservations = reservationDAL.ViewReservations(parkID);

            Console.Clear();
            Console.WriteLine(parkName + " National Park - Upcoming Reservations");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("RSVP#".PadRight(6) + "Site ID".PadRight(6) + "Name".PadRight(40) + "From Date".PadRight(15) + "To Date".PadRight(15) + "Created Date");
            for (int i = 0; i < upcomingReservations.Count(); i++)
            {
                Console.WriteLine(upcomingReservations[i].ReservationID.ToString().PadRight(6) + upcomingReservations[i].SiteID.ToString().PadRight(6) + upcomingReservations[i].CustomerName.PadRight(40) + upcomingReservations[i].ArrivalDate.ToString("yyyy-MM-dd").PadRight(15) + upcomingReservations[i].DepartureDate.ToString("yyyy-MM-dd").PadRight(15) + upcomingReservations[i].CreatedDate.ToString("yyyy-MM-dd").PadRight(6));
            }
            Console.ReadLine();
        }

        private void ViewCampgroundsAtPark(int parkID, string parkName)
        {
            while (true)
            {
                CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
                List<Campground> camps = campgroundDAL.GetAllCampgrounds(parkID);
                Console.Clear();
                Console.WriteLine(parkName + " National Park Campgrounds");
                Console.WriteLine("-------------------------------------");

                if (camps.Count() > 0)
                {
                    Console.WriteLine("# ".PadRight(6) + "Name".PadRight(35) + "Open".PadRight(10) + "Close".PadRight(10) + "Daily Fee");
                    for (int i = 0; i < camps.Count(); i++)
                    {
                        Console.WriteLine((i + 1) + ": ".PadRight(5) + camps[i].Name.PadRight(35) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].OpenMonth).PadRight(10) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].CloseMonth).PadRight(10) + camps[i].DailyFee.ToString("C"));
                    }
                    Console.WriteLine();
                    Console.WriteLine("1: Search for Available Reservations");
                    Console.WriteLine("\nR: Return to Previous Menu");
                }
                string command = Console.ReadLine();
                Console.Clear();

                if (command.ToLower() == Command_Return)
                {
                    return;
                }
                else if (command == "1")
                {
                    SearchReservationsAtCampground(parkID);

                }
                else
                {
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                }
            }
        }

        private void SearchReservationsAtCampground(int parkID)
        {
            while (true)
            {
                CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
                List<Campground> camps = campgroundDAL.GetAllCampgrounds(parkID);
                Console.Clear();
                Console.WriteLine("Choose a Campground to Reserve:");
                Console.WriteLine("--------------------");

                if (camps.Count() > 0)
                {
                    Console.WriteLine("# ".PadRight(6) + "Name".PadRight(35) + "Open".PadRight(10) + "Close".PadRight(10) + "Daily Fee");
                    for (int i = 0; i < camps.Count(); i++)
                    {
                        Console.WriteLine((i + 1) + ": ".PadRight(5) + camps[i].Name.PadRight(35) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].OpenMonth).PadRight(10) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].CloseMonth).PadRight(10) + camps[i].DailyFee.ToString("C"));
                    }
                    Console.Write("\nWhich Campground (enter 0 to cancel):");
                    string choice = Console.ReadLine();
                    if (choice == "0")
                    {
                        break;
                    }

                    DateTime arrival = (CLIHelper.GetDateTime("Please enter your desired arrival date (yyyy/mm/dd): "));
                    DateTime departure = (CLIHelper.GetDateTime("Please enter your desired departure date (yyyy/mm/dd): "));
                    SiteDAL sitedal = new SiteDAL(DatabaseConnection);
                    List<Site> sites = sitedal.GetAvailableSites(Convert.ToInt32(choice), arrival, departure);

                    double costOfStay = (departure - arrival).TotalDays * camps[Convert.ToInt32(choice) - 1].DailyFee;
                    if (sites.Count() > 0)
                    {
                        List<int> siteIds = new List<int>();

                        Console.WriteLine("ID".PadRight(5) + "Max Occ.".PadRight(10) + "RV Hookup".PadRight(10) + "Max RV Length".PadRight(15) + "Wheelchair OK".PadRight(15) + "Cost");
                        for (int i = 0; i < 5; i++)
                        {
                            Console.WriteLine(sites[i].SiteID.ToString().PadRight(5) + sites[i].MaxOccupancy.ToString().PadRight(10) + sites[i].HasRVHookup.ToString().PadRight(10) + sites[i].MaxRVLength.ToString().PadRight(15) + sites[i].IsWheelChairAccessible.ToString().PadRight(10) + costOfStay.ToString("C"));
                            siteIds.Add(sites[i].SiteID);
                        }

                        string siteChoice = CLIHelper.GetString("Which site ID would you like to reserve? (enter 0 to cancel):");
                        if (siteChoice == "0")
                        {
                            break;
                        }
                        else if (siteIds.Contains(Convert.ToInt32(siteChoice)))
                        {
                            AddReservationToSite(sites[Convert.ToInt32(siteChoice) - 1].SiteID, arrival, departure);
                        }
                    }
                }
            }
        }

        private void AddReservationToSite(int siteID, DateTime arrival, DateTime departure)
        {
            while (true)
            {
                ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
                string reservationName = CLIHelper.GetString("Please Enter Reservation Name: ");
                reservationDAL.AddReservation(reservationName, siteID, arrival, departure);

                Console.WriteLine("Site Successfully Reserved. Reservation ID: " + reservationDAL.GetReservationID(reservationName, siteID, arrival, departure));
                Console.ReadLine();
                break;
            }
        }


        private void DisplayMainMenu()
        {
            Console.WriteLine(" Parks and Recreation - Main Menu");
            Console.WriteLine(" --------------------------------");
            Console.WriteLine(" 1 - Show All Parks");
            Console.WriteLine(" 2 - Delete Existing Reservation");
            Console.WriteLine("\n Q - Quit");
            Console.WriteLine();
        }

        private static void DisplayParkMenu()
        {
            Console.WriteLine("\nSelect an Option:");
            Console.WriteLine("1: View Campgrounds");
            Console.WriteLine("2: Display Upcoming Reservations");
            Console.WriteLine("\nR: Return to Previous Menu");
        }



    }
}
