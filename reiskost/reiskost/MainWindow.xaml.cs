using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace reiskost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            basicFlightTextBox.Text = "";
            basicPricePerDayTextBox.Text = "";
            destinationTextBox.Text = "";
            discountPercentageTextBox.Text = "";
            flightClassTextBox.Text = "";
            numberOfDaysTextBox.Text = "";
            numberOfPersonsTextBox.Text = "";
            resultTextBox.Text = "REISKOST VOLGENS BESTEMMING: ";
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            string destination = destinationTextBox.Text;

            bool isBasicPrice = double.TryParse(basicFlightTextBox.Text, out double basicPrice);
            bool isFlightClass = int.TryParse(flightClassTextBox.Text, out int flightClass);
            bool isBasicPricePerDay = double.TryParse(basicPricePerDayTextBox.Text, out double basicPricePerDay);
            bool isNumberOfDays = int.TryParse(numberOfDaysTextBox.Text, out int numberOfDays);
            bool isNumberOfPersons = int.TryParse(numberOfPersonsTextBox.Text, out int numberOfPersons);
            bool isDiscountPercentage = int.TryParse(discountPercentageTextBox.Text, out int discountPercentage);

            if(isBasicPrice && isFlightClass && isBasicPricePerDay && isNumberOfDays && isNumberOfPersons && isDiscountPercentage)
            {
                if (flightClass == 1)
                {
                    basicPrice = basicPrice * 1.3;
                }
                else if(flightClass == 2)
                {
                    //basicPrice wordt niet aangepast;
                }
                else if(flightClass == 3)
                {
                    basicPrice = basicPrice * 0.8;
                }
                else
                {
                    MessageBox.Show("Verkeerde keuze", "Verkeerde ingave");

                    flightClassTextBox.Focus();
                    flightClassTextBox.SelectAll();

                    return;
                }

                double totalFlightPrice = basicPrice * numberOfPersons;

                int index = 1;
                double totalPricePerDay = 0;
                do
                {
                    if (index <= 2)
                    {
                        totalPricePerDay += basicPricePerDay;
                    } 
                    else if(index == 3)
                    {
                        totalPricePerDay += basicPricePerDay * 0.5;
                    }
                    else if (index>3)
                    {
                        totalPricePerDay += basicPricePerDay * 0.3;
                    }
                    index++;
                }
                while (index <= numberOfPersons);

                double totalHotelPrice = Math.Round(totalPricePerDay * numberOfDays,2);

                double totalTravelPrice = Math.Round(totalHotelPrice + totalFlightPrice,2);

                double discount = Math.Round(totalTravelPrice * discountPercentage/100,2);

                double toPay = Math.Round(totalTravelPrice - discount,2);

                resultTextBox.Text = resultTextBox.Text + $" {destination}\n\n" 
                                                        + $"Totale vluchtprijs: € {totalFlightPrice}\n" 
                                                        + $"Totale verblijfprijs: € {totalHotelPrice}\n"
                                                        + $"Totale reisprijs: € {totalTravelPrice}\n"
                                                        + $"Korting: € {discount}\n\n"
                                                        + $"Te betalen: € {toPay}";            
            }
            else
            {
                MessageBox.Show("Alle gegevens moeten ingevuld worden", "Foutieve ingave");
                return;
            }

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            flightClassTextBox.ToolTip = "1=Businessclass\n2=Standaard lijnvlucht\n3=Chartervlucht";
        }

        
    }
}