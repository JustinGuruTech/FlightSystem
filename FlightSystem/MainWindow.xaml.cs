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

            // variables for calling UI Creation
            int numFlights = InitialUICreation.GetNumFlights();
            StackPanel[] FlightStacks = new StackPanel[numFlights];
            FlightStacks[0] = Flight1Stack;
            FlightStacks[1] = Flight2Stack;

            // calling CreateUI like this with FlightStacks.Length prevents trying to display a UI for a flight in DB but without a UIStack
            InitialUICreation.CreateUI(FlightDropdown, PassengerDropdown, FlightStacks, FlightStacks.Length);

            FlightDropdown.SelectedIndex = 0;
            
        }

        private void FlightDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ((sender as ComboBox).SelectedIndex == 0) {
                Flight1Stack.Visibility = Visibility.Visible;
                Flight2Stack.Visibility = Visibility.Hidden;
            } else {
                Flight1Stack.Visibility = Visibility.Hidden;
                Flight2Stack.Visibility = Visibility.Visible;
            }
        }
    }
}
