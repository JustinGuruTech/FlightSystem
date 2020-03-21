using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightSystem {
    class LogicHandler {

        public void UpdateFlight(ComboBox FlightDropdown, int flightIndex) {
            FlightDropdown.SelectedIndex = flightIndex;
        }
    }
}
