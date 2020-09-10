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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Windows.Media.Media3D;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity;

namespace Schneider
{
	/// <summary>
	/// Logic interaction for MainWindow.axml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			//Object to call a method to update the listviews 
			DataBaseOperations dbOperations = new DataBaseOperations();
			//Update the listviews to show the current registered devices
			lvWaterMeter.ItemsSource = dbOperations.GetWaterMeterTableAsList();
			lvLightMeter.ItemsSource = dbOperations.GetLightMeterTableAsList();
			lvGateway.ItemsSource = dbOperations.GetGatewayTableAsList();
		}
		//Method that is called when is clicked prev button
		//It show us the previous tabitem
		private void btnPreviousTab_Click(object sender, RoutedEventArgs e)
		{
			int newIndex = tcSample.SelectedIndex - 1;
			if (newIndex < 0)
				newIndex = tcSample.Items.Count - 1;
			tcSample.SelectedIndex = newIndex;
		}

		//Method that is called when is clicked next button
		//It show us the next tabitem
		private void btnNextTab_Click(object sender, RoutedEventArgs e)
		{
			int newIndex = tcSample.SelectedIndex + 1;
			if (newIndex >= tcSample.Items.Count)
				newIndex = 0;
			tcSample.SelectedIndex = newIndex;
		}

		//Method that is called when is clicked exit button
		//It close the application
		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		//Method that is called when submit of search a device is clicked
		//Search a specific device from the devices tables. it depends on 
		//the radiobutton that is checked, to choose what type of device has to search
		private void SearchDevice_Click(object sender, RoutedEventArgs e)
		{
			//Variable to save the numerical value of id
			int id = 0;
			//Variable to save the numerical value of serial number
			int serialNumber = 0;
			//Obejct to call database operations to update a register of the table 
			DataBaseOperations dbOperations = new DataBaseOperations();
			//Check if there are empty fields, when submit button is clicked
			if (IdSearch.Text == "" || SerialNumberSearch.Text == "")
			{
				MessageBox.Show("ERROR! \nThere are empty fields");
			}
			else
			{
				//Check if id and serial number are numerical
				if (!(int.TryParse(IdSearch.Text, out id)) || !(int.TryParse(SerialNumberSearch.Text, out serialNumber)))
				{
					MessageBox.Show("ERROR! \nId and serial number values have to be numerical");
				}
				else
				{
					if (WaterMeterSearchOption.IsChecked == true)
					{
						//Check if the id and serial number correspond to a register water meter
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "WaterMeter"))
						{
							//Call this method to search the specific device and it return a water meter object
							WaterMeter wmFound = dbOperations.SearchWaterMeter(id, serialNumber);
							MessageBox.Show("WATERMETER FOUND!" +
								"\nId: " + wmFound.Id + "\nSerial number: " + wmFound.SerialNumber +
								"\nBrand: " + wmFound.Brand + "\nModel: " + wmFound.Model);
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else if (LightMeterSearchOption.IsChecked == true)
					{
						//Check if the id and serial number correspond to a register light meter
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "LightMeter"))
						{
							//Call this method to search the specific device and it return a light meter object
							LightMeter lmFound = dbOperations.SearchLightMeter(id, serialNumber);
							MessageBox.Show("LIGHTMETER FOUND!" +
								"\nId: " + lmFound.Id + "\nSerial number: " + lmFound.SerialNumber +
								"\nBrand: " + lmFound.Brand + "\nModel: " + lmFound.Model);
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else if (GatewaySearchOption.IsChecked == true)
					{
						//Check if the id and serial number correspond to a register gateway
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "Gateway"))
						{
							//Call this method to search the specific device and it return a gateway object
							Gateway gwFound = dbOperations.SearchGateway(id, serialNumber);
							MessageBox.Show("GATEWAY FOUND!" +
								"\nId: " + gwFound.Id + "\nSerial number: " + gwFound.SerialNumber +
								"\nBrand: " + gwFound.Brand + "\nModel: " + gwFound.Model + "\nIp: " + gwFound.Ip +
								"\nPort number: " + gwFound.PortNumber);
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else
					{
						MessageBox.Show("ERROR!\n You have to select a type of device");
					}
				}
				//Set again the fields empty
				IdSearch.Text = "";
				SerialNumberSearch.Text = "";
			}
		}

		//Method that is called when register device button is clicked.
		//it open a new window to register a device. it dependes on the radiobutton
		//that is checked, to choose what window will open
		private void RegisterDevice_Click(object sender, RoutedEventArgs e)
		{
			Window win;

			if (WaterMeterRegisterOption.IsChecked == true)
			{
				win = new RegisterWaterMeterWindow(lvWaterMeter);
				win.Show();
			}
			else if (LightMeterRegisterOption.IsChecked == true)
			{
				win = new RegisterLightMeterWindow(lvLightMeter);
				win.Show();
			}
			else if (GatewayRegisterOption.IsChecked == true)
			{
				win = new RegisterGateWayWindow(lvGateway);
				win.Show();
			}
			else
			{
				MessageBox.Show("ERROR!\n You have to select a type of device");
			}
		}

		//Method that is called when delete device button is clicked
		//delete a device from the registered devices table. it depends on 
		//the radiobutton that is checked, to choose what type of device has to delete
		private void DeleteDevice_Click(object sender, RoutedEventArgs e)
		{
			//Variable to save the numerical value of id
			int id = 0;
			//Variable to save the numerical value of serial number
			int serialNumber = 0;
			//Obejct to call database operations to update a register of the table 
			DataBaseOperations dbOperations = new DataBaseOperations();
			//Check if there are empty fields, when submit button is clicked
			if (IdDelete.Text == "" || SerialNumberDelete.Text == "")
			{
				MessageBox.Show("ERROR! \nThere are empty fields");
			}
			else
			{
				//Check if id and serial number are numerical
				if (!(int.TryParse(IdDelete.Text, out id)) || !(int.TryParse(SerialNumberDelete.Text, out serialNumber)))
				{
					MessageBox.Show("ERROR! \nId and serial number values have to be numerical");
				}
				else
				{
					if (WaterMeterOptionDelete.IsChecked == true)
					{
						//Check if the id and serial number correspond to a register water meter
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "WaterMeter"))
						{
							//Call to that method to delete the water meter chosen 
							dbOperations.DeleteWaterMeterFromTable(id, serialNumber);
							//Update the list view to show the change
							lvWaterMeter.ItemsSource = dbOperations.GetWaterMeterTableAsList();
							MessageBox.Show("GOOD!\nYou have deleted the water meter");
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else if (LightMeterOptionDelete.IsChecked == true)
					{
						//Check if the id and serial number correspont to a register light meter
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "LightMeter"))
						{
							//Call to that method to delete the light meter chosen
							dbOperations.DeleteLightMeterFromTable(id, serialNumber);
							//Update the list view to show the change
							lvLightMeter.ItemsSource = dbOperations.GetLightMeterTableAsList();
							MessageBox.Show("GOOD!\nYou have deleted the light meter");
						}
						else
						{
							MessageBox.Show("ERRROR!\nThis device doesn't exist");
						}
					}
					else if (GatewayOptionDelete.IsChecked == true)
					{
						//Check if the id and serial number correspont to a register gateway
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "Gateway"))
						{
							//Call to that method to delete the gateway chosen
							dbOperations.DeleteGatewayFromTable(id, serialNumber);
							//Update the list view to show the change
							lvGateway.ItemsSource = dbOperations.GetGatewayTableAsList();
							MessageBox.Show("GOOD!\nYou have deleted the gateway");
						}
						else
						{
							MessageBox.Show("ERROR\nThis device doesn't exist");
						}
					}
					else
					{
						MessageBox.Show("ERROR!\n You have to select a type of device");
					}
				}
				//Set again the fields empty
				IdDelete.Text = "";
				SerialNumberDelete.Text = "";
			}
		}

		//Method that is called when submit of modify brand and model is clicked
		//modify the brand and model of a device from the registered devices table. it depends on 
		//the radiobutton that is checked, to choose what type of device has to delete
		private void SubmitModifyBM_Click(object sender, RoutedEventArgs e)
		{
			//Variable to save the numerical value of id
			int id = 0;
			//Variable to save the numerical value of serial number
			int serialNumber = 0;
			//Obejct to call database operations to update a register of the table 
			DataBaseOperations dbOperations = new DataBaseOperations();
			//Check if there are empty fields, when submit button is clicked
			if (IdModifyBM.Text == "" || SerialNumberModifyBM.Text == "" || BrandModifyBM.Text == "" || ModelModifyBM.Text == "")
			{
				MessageBox.Show("ERROR! \nThere are empty fields");
			}
			else
			{
				//Check if id and serial number are numerical
				if (!(int.TryParse(IdModifyBM.Text, out id)) || !(int.TryParse(SerialNumberModifyBM.Text, out serialNumber)))
				{
					MessageBox.Show("ERROR! \nId and serial number values have to be numerical");
				}
				else
				{
					if (WaterMeterOptionModifyBM.IsChecked == true)
					{
						//Check if the id and serial number correspont to a register water meter
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "WaterMeter"))
						{
							//Call this method to change brand and model of the chosen water meter
							dbOperations.UpdateBrandModelWaterMeterInTable(id, serialNumber, BrandModifyBM.Text,
								ModelModifyBM.Text);
							//Update the list view to show the change
							lvWaterMeter.ItemsSource = dbOperations.GetWaterMeterTableAsList();
							MessageBox.Show("GOOD!\nYou have modified brand and model of the device");
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else if (LightMeterOptionModifyBM.IsChecked == true)
					{
						//Check if the id and serial number correspond to a register light meter
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "LightMeter"))
						{
							//Call this method to change brand and model of the chosen light meter
							dbOperations.UpdateBrandModelLightMeterInTable(id, serialNumber, BrandModifyBM.Text,
							ModelModifyBM.Text);
							//Update the list view to show the change
							lvLightMeter.ItemsSource = dbOperations.GetLightMeterTableAsList();
							MessageBox.Show("GOOD!\nYou have modified brand and model of the device");
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else if (GatewayOptionModifyBM.IsChecked == true)
					{
						//Check if the id and serial number correspond to a register gateway
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "Gateway"))
						{
							//Call this method to change brand and model of the chosen gateway
							dbOperations.UpdateBrandModelGatewayInTable(id, serialNumber, BrandModifyBM.Text,
							ModelModifyBM.Text);
							//Update the list view to show the change
							lvGateway.ItemsSource = dbOperations.GetGatewayTableAsList();
							MessageBox.Show("GOOD!\nYou have modified brand and model of the device");
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
					}
					else
					{
						MessageBox.Show("ERROR!\n You have to select a type of device");
					}
				}
				//Set again the fields empty
				IdModifyBM.Text = "";
				SerialNumberModifyBM.Text = "";
				BrandModifyBM.Text = "";
				ModelModifyBM.Text = "";
			}
		}

		//Method that is called when submit of modify ip and port is clicked
		//modify the ip and port number of a gateway from the registered gateways table.
		private void SubmitModifyIPPN_Click(object sender, RoutedEventArgs e)
		{
			//Variable to save the numerical value of id
			int id = 0;
			//Variable to save the numerical value of serial number
			int serialNumber = 0;
			//Variable to save the corrected ip
			string ip = "";
			//Variable to save the numerical value of portNumber
			int portNumber = 0;
			//Variable to check if the ip is correct
			bool ipIsCorrect = true;
			//Variable to check if the port is correct
			bool portIsCorrect = true;
			//Object to call a method of this class to check the ip format
			FilterIp filterIp = new FilterIp();
			//Obejct to call database operations to update a register of the table 
			DataBaseOperations dbOperations = new DataBaseOperations();
			//Check if there are empty fields, when submit button is clicked
			if (IdModifyIPPN.Text == "" || SerialNumberModifyIPPN.Text == "" || IpModifyIPPN.Text == "" || PortNumberModifyIPPN.Text == "")
			{
				MessageBox.Show("ERROR! \nThere are empty fields");
			}
			else
			{
				//Check if id and serial number are numeric
				if (!(int.TryParse(IdModifyIPPN.Text, out id)) || !(int.TryParse(SerialNumberModifyIPPN.Text, out serialNumber)))
				{
					MessageBox.Show("ERROR! \nId and serial number values have to be numerical");
				}
				else
				{
					//Check if the ip has a value and if it is correct
					if (!(filterIp.CheckIp(IpModifyIPPN.Text, out ip)))
					{
						ipIsCorrect = false;
						MessageBox.Show("ERROR!\nIp value isn't correct");
					}
					//Check if port number has a value and it is a number
					if (!(int.TryParse(PortNumberModifyIPPN.Text, out portNumber)))
					{
						portIsCorrect = false;
						MessageBox.Show("ERROR!\nPort number value has to be numerical");
						if(portNumber < 0 || portNumber > 65535)
                        {
							portIsCorrect = false;
							MessageBox.Show("ERROR!\nPort number isn't a valid port");
						}
					}
					//if the ip and port number are right, we check if the id and serial number 
					//correspond to a register device 
					if (ipIsCorrect && portIsCorrect)
					{
						//Call to this method to check if the gateway exist
						if (dbOperations.ExistDeviceInDataBase(id, serialNumber, "Gateway"))
						{
							//We update the gateway table with the inserted values
							dbOperations.UpdateIpPortGatewayInTable(id, serialNumber, ip,
							portNumber);
							//We update the list view with the modification that we have just made
							lvGateway.ItemsSource = dbOperations.GetGatewayTableAsList();
							MessageBox.Show("GOOD!\nYou have modified ip and port number of the device");
						}
						else
						{
							MessageBox.Show("ERROR!\nThis device doesn't exist");
						}
						//Set again the fields empty
						IdModifyIPPN.Text = "";
						SerialNumberModifyIPPN.Text = "";
						IpModifyIPPN.Text = "";
						PortNumberModifyIPPN.Text = "";
					}
				}
			}
		}
    }
}
