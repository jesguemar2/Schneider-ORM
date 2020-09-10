using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;

namespace Schneider
{
    /// <summary>
    /// Logic of interaction for RegisterWaterMeterWindow.xaml
    /// </summary>
    public partial class RegisterWaterMeterWindow : Window
    {
        ListView lvWaterMeter;

        public RegisterWaterMeterWindow(ListView lvWaterMeter)
        {
            InitializeComponent();
            //Save the list view of water meter in the object variable
            this.lvWaterMeter = lvWaterMeter;
        }

        //Method that is called when the submit button is clicked
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //Variable to save the numeric value of id
            int id = 0;
            //Variable to save the numeric value of serial number
            int sn = 0;
            //Query to insert in the database the device that has been registered
            DataBaseOperations dbOperations = new DataBaseOperations();
            bool deviceAlreadyExist = false;

            //Check if there are values in id and serial number
            if (Id.Text == "" || SerialNumber.Text == "")
            {
                MessageBox.Show("ERROR! \nYou have to insert values of id and serial number");
            }
            else
            {
                //Check if id and serial number are numeric
                if (!(int.TryParse(Id.Text, out id)) || !(int.TryParse(SerialNumber.Text, out sn)))
                {
                    MessageBox.Show("ERROR! \nId and serial number values have to be numeric");
                }
                else
                {
                    deviceAlreadyExist = dbOperations.ExistDeviceInDataBase(id, sn, "WaterMeter");
                    //If id and serial number already exist, we do not insert this values in the database
                    if (deviceAlreadyExist == true)
                    {
                        MessageBox.Show("This device already exist, You can't register a device twice");
                    }
                    else
                    {
                        dbOperations.InsertWaterMeterInTable(id, sn, Brand.Text, Model.Text);
                        lvWaterMeter.ItemsSource = dbOperations.GetWaterMeterTableAsList();
                        MessageBox.Show("GOOD!\nYou have registered the watermeter");
                        //Close the register window
                        this.Close();
                    }
                }
            }
        }
    }
}

