using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data;
using System.Windows.Media;
using System.Diagnostics;

namespace FlightSystem {
    static class InitialUICreation {

        public static clsDataAccess dataAccess = new clsDataAccess();

        /// <summary>
        /// Calls rest of UI Creation methods using numFlights passed from UI to ensure it doesn't attempt to create
        /// a UI from a flight in the DB but not in the UI.
        /// </summary>
        /// <param name="FlightDropdown"></param>
        /// <param name="PassengerDropdown"></param>
        /// <param name="FlightStacks"></param>
        /// <param name="numFlights"></param>
        public static void CreateUI(ComboBox FlightDropdown, ComboBox PassengerDropdown, StackPanel FlightsStack, int numFlights) {

            PopulateFlightDropdown(FlightDropdown, numFlights);
            PopulatePassengerDropdown(PassengerDropdown);
            PopulateFlightSeats(FlightsStack, GetSeatsTaken(numFlights));

        }

        public static int[][] GetSeatsTaken(int numFlights) {
            // store each flights seats taken in jagged array
            int[][] flightSeatsTaken = new int[numFlights][];

            // for each flight
            for (int flightNum = 1; flightNum <= numFlights; flightNum++) {
                
                // get total seats taken for array size
                int returnVals = 0;
                DataSet dataSet = dataAccess.ExecuteSQLStatement("Select * FROM Flight_Passenger_Link WHERE Flight_ID = " + flightNum, ref returnVals);
                flightSeatsTaken[flightNum - 1] = new int[returnVals]; // intialize size of jagged array at flight index
                
                // add taken seat to jagged array for each one taken
                int seatsTakenIndex = 0;
                foreach (DataRow row in dataSet.Tables[0].Rows) {
                    flightSeatsTaken[flightNum - 1][seatsTakenIndex++] = Int32.Parse(row.ItemArray[2].ToString());
                }
            }
            return flightSeatsTaken;
        }

        /// <summary>
        /// Returns number of flights listed in the DB
        /// </summary>
        /// <returns></returns>
        public static int GetNumFlights() {
            return Int32.Parse(dataAccess.ExecuteScalarSQL("SELECT Count(Flight_ID) FROM Flight"));
        }

        /// <summary>
        /// Populates flight dropdown based on how many flights have a UI set up first, then how many are in the DB.
        /// If another flight is added to DB but has no corresponding UIStack it will not attempt to display.
        /// </summary>
        /// <param name="FlightDropdown"></param>
        /// <param name="numFlights"></param>
        public static void PopulateFlightDropdown(ComboBox FlightDropdown, int numFlights) {
            
            // add flight to dropdown from DB
            int returnVals = 0;
            DataSet dataSet = dataAccess.ExecuteSQLStatement("Select * FROM Flight", ref returnVals);
            for (int i = 0; i < numFlights; i++) {
                FlightDropdown.Items.Add(dataSet.Tables[0].Rows[i].ItemArray[2].ToString() + ": " + dataSet.Tables[0].Rows[i].ItemArray[1].ToString());
            }
        }

        /// <summary>
        /// Adds every passenger to the passenger dropdown
        /// </summary>
        /// <param name="PassengerDropdown"></param>
        public static void PopulatePassengerDropdown(ComboBox PassengerDropdown) {
            // populate passenger dropdown on UI
            int returnVals = 0;
            DataSet dataSet = dataAccess.ExecuteSQLStatement("Select * FROM Passenger", ref returnVals);
            for (int i = 0; i < returnVals; i++) {
                PassengerDropdown.Items.Add(dataSet.Tables[0].Rows[i].ItemArray[1].ToString() + " " + dataSet.Tables[0].Rows[i].ItemArray[2].ToString());
            }
        }

        /// <summary>
        /// Populates flight seats on UI based on whats in the DB - Easily scalable to more flights
        /// </summary>
        /// <param name="FlightStacks"></param>
        /// <param name="numFlights"></param>
        public static void PopulateFlightSeats(StackPanel FlightsStack, int[][] flightSeatsTaken) {

            int flightNum = 0;
            // for each flight
            foreach (StackPanel flightStack in FlightsStack.Children) {

                int seatNum = 1;
                // for each row in the flight
                foreach (StackPanel stack in flightStack.Children) {
                    // for each seat button
                    foreach (Button seatButton in stack.Children) {

                        // if seat is taken
                        if (flightSeatsTaken[flightNum].Contains(seatNum)) {
                            seatButton.Background = (Brush)(new BrushConverter().ConvertFrom("#FFF35454"));
                            seatButton.IsEnabled = false;
                            seatButton.Opacity = 1;
                        }
                        seatButton.Content = seatNum++; // assign seat number
                    }
                }

                flightNum++;
            }
        }
    }
}
