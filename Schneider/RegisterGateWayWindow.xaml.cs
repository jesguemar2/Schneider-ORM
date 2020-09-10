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
    /// Logic of interaction for RegisterGateWayWindow.xaml
    /// </summary>
    public partial class RegisterGateWayWindow : Window
    {
        private ListView lvGateway;
        public RegisterGateWayWindow(ListView lvGateway)
        {
            InitializeComponent();
            //We save the list view of gateways in this variable
            this.lvGateway = lvGateway;
        }

        //Method that is called when the submit button is clicked
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //Variable to save the numerical value of id
            int id = 0;
            //Variable to save the numerical value of serial number
            int sn = 0;
            //Variable to save the corrected ip
            string ip = "";
            //Variable to save the numerical value of portNumber
            int portNumber = 0;
            //Object to call a function of this class to check the ip format
            FilterIp filterIp = new FilterIp();
            //Variable to check if ip value is correct
            bool ipIsCorrect = true;
            //Variable to check if port value is correct
            bool portIsCorrect = true;
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
                //Check if id and serial number are numerical
                if (!(int.TryParse(Id.Text, out id)) || !(int.TryParse(SerialNumber.Text, out sn)))
                {
                    MessageBox.Show("ERROR! \nId and serial number values have to be numerical");
                }
                else
                {
                    //Check if the ip has a value and if it is correct
                    if (!(Ip.Text == ""))
                    {
                        if (!(filterIp.CheckIp(Ip.Text, out ip)))
                        {
                            ipIsCorrect = false;
                            MessageBox.Show("ERROR!\nIp value isn't correct");
                        }
                    }
                    //Check if port number has a value and it is a number
                    if (!(PortNumber.Text == ""))
                    {
                        if (!(int.TryParse(PortNumber.Text, out portNumber)))
                        {
                            portIsCorrect = false;
                            MessageBox.Show("ERROR!\nPort number value has to be numerical");
                        }
                        //Check if it is a valid port
                        if (portNumber < 0 || portNumber > 65535)
                        {
                            portIsCorrect = false;
                            MessageBox.Show("ERROR!\nPort number isn't a valid port");
                        }
                    }
                    //if the ip and port number values are correct also it can happen that any of these fields are empty
                    //Because ip and port number aren't required parameters
                    if (ipIsCorrect && portIsCorrect)
                    {
                        //Check if the device already exist
                        deviceAlreadyExist = dbOperations.ExistDeviceInDataBase(id, sn, "Gateway");
                        //If id and serial number already exist, we do not insert this values in the database
                        if (deviceAlreadyExist == true)
                        {
                            MessageBox.Show("This device already exist, You can't register a device twice");
                        }
                        else
                        {
                            //Insert the device in the gateway table 
                            dbOperations.InsertGatewayInTable(id, sn, Brand.Text, Model.Text, ip, portNumber);
                            lvGateway.ItemsSource = dbOperations.GetGatewayTableAsList();
                            MessageBox.Show("GOOD!\n You have registered the gateway");
                            //Close the register window
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
    

