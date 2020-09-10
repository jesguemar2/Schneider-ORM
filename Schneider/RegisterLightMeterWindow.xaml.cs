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
using System.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Windows.Media.Media3D;

namespace Schneider
{
    /// <summary>
    /// Logic of interaction for RegisterLightMeterWindow.xaml
    /// </summary>

    public partial class RegisterLightMeterWindow : Window
    {
        private ListView lvLightMeter;
        public RegisterLightMeterWindow(ListView lvLightMeter)
        {
            InitializeComponent();
            //Save the light meter list view in this variable
            this.lvLightMeter = lvLightMeter;
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
                    deviceAlreadyExist = dbOperations.ExistDeviceInDataBase(id, sn, "LightMeter");
                    //If id and serial number already exist, we do not insert this values in the database
                    if (deviceAlreadyExist == true)
                    {
                        MessageBox.Show("This device already exist, You can't register a device twice");
                    }
                    else
                    {
                        dbOperations.InsertLightMeterInTable(id, sn, Brand.Text, Model.Text);
                        //We update the tables of the lists view
                        lvLightMeter.ItemsSource = dbOperations.GetLightMeterTableAsList();
                        MessageBox.Show("GOOD!\nYou have registered the light meter");
                        //Close the register window
                        this.Close();
                    }
                }
            }
        }
    }
}
