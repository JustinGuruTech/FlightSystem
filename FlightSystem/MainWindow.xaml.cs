using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Diagnostics;

namespace FlightSystem {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        LogicHandler logicHandler = new LogicHandler();
        public MainWindow() {
            InitializeComponent();

            // calling CreateUI like this with FlightStacks.Length prevents trying to display a UI for a flight in DB but without a UIStack
            InitialUICreation.CreateUI(FlightDropdown, PassengerDropdown, FlightsStack, FlightsStack.Children.Count);
            FlightDropdown.SelectedIndex = 0;
            
        }

        /// <summary>
        /// Shrinks all flights not selected and expands flight selected. Scalable to more than 2 flights.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlightDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int flightStackNum = 0;
            foreach (StackPanel flightStack in FlightsStack.Children) {
                // selected flight
                if (flightStackNum++ == (sender as ComboBox).SelectedIndex) {
                    flightStack.Visibility = Visibility.Visible;
                    flightStack.Height = 320;
                } 
                // not selected flight
                else {
                    flightStack.Visibility = Visibility.Hidden;
                    flightStack.Height = 0;
                }
            }

            // update title of flight
            FlightName.Content = FlightDropdown.SelectedValue;
          
        }
    }
}
