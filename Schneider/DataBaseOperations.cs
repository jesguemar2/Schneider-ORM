using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Infrastructure;

namespace Schneider
{
    class DataBaseOperations
    {
		/// <summary>
		/// Get the water meter database table as list
		/// </summary>
		/// <returns>List of water meters objects</returns>
		public List<WaterMeter> GetWaterMeterTableAsList()
        {
			List<WaterMeter> listWaterMeter = new List<WaterMeter>();

			using (DevicesEntities db = new DevicesEntities())
            {
				listWaterMeter = db.WaterMeter.ToList();
            }
			return listWaterMeter;
        }
		/// <summary>
		/// Get the light meter database table as list
		/// </summary>
		/// <returns>List of light meter objects</returns>
		public List<LightMeter> GetLightMeterTableAsList()
        {
			List<LightMeter> listLightMeter = new List<LightMeter>();

			using (DevicesEntities db = new DevicesEntities())
			{
				listLightMeter = db.LightMeter.ToList();
			}
			return listLightMeter;
		}
		/// <summary>
		/// Get the gateway database table as list
		/// </summary>
		/// <returns>List of gateway objects</returns>
		public List<Gateway> GetGatewayTableAsList()
		{
			List<Gateway> listGateway = new List<Gateway>();

			using (DevicesEntities db = new DevicesEntities())
			{
				listGateway = db.Gateway.ToList();
			}
			return listGateway;
		}
		/// <summary>
		/// Check if a device exist in the current database table
		/// </summary>
		/// <returns>true if the device exist, but false</returns>
		public bool ExistDeviceInDataBase(int id, int serialNumber, string typeDevice)
        {
			bool existDevice = false;

            switch (typeDevice)
            {
				case "WaterMeter":
					List<WaterMeter> listWaterMeter = new List<WaterMeter>();
					listWaterMeter = GetWaterMeterTableAsList();
					foreach(WaterMeter wm in listWaterMeter)
                    {
						if(id == wm.Id && serialNumber == wm.SerialNumber)
                        {
							existDevice = true;
                        }
                    }
					break;
				case "LightMeter":
					List<LightMeter> listLightMeter = new List<LightMeter>();
					listLightMeter = GetLightMeterTableAsList();
					foreach (LightMeter lm in listLightMeter)
					{
						if (id == lm.Id && serialNumber == lm.SerialNumber)
						{
							existDevice = true;
						}
					}
					break;
				case "Gateway":
					List<Gateway> listGateways = new List<Gateway>();
					listGateways = GetGatewayTableAsList();
					foreach (Gateway gw in listGateways)
					{
						if (id == gw.Id && serialNumber == gw.SerialNumber)
						{
							existDevice = true;
						}
					}
					break;
				default:
					break;
            }
			return existDevice;
        }
		/// <summary>
		/// Add a water meter to the water meter database table
		/// </summary>
		public void InsertWaterMeterInTable(int id, int serialNumber, string brand, string model)
		{
			WaterMeter waterMeter = new WaterMeter
			{
				Id = id,
				SerialNumber = serialNumber,
				Brand = brand,
				Model = model
			};
			try
            {
                using (DevicesEntities db = new DevicesEntities())
                {
                    db.WaterMeter.Add(waterMeter);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException e) { Console.WriteLine(e); }
		}
		/// <summary>
		/// Add a light meter to the light meter database table
		/// </summary>
		public void InsertLightMeterInTable(int id, int serialNumber, string brand, string model)
		{
			LightMeter lightMeter = new LightMeter
			{
				Id = id, 
				SerialNumber = serialNumber,
				Brand = brand, 
				Model = model
			};
			try
            {
                using (DevicesEntities db = new DevicesEntities())
                {
                    db.LightMeter.Add(lightMeter);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException e) { Console.WriteLine(e); }
		}
		/// <summary>
		/// Add a gateway to the gateway database table
		/// </summary>
		public void InsertGatewayInTable(int id, int serialNumber, string brand, string model, string ip, int portNumber)
		{
			Gateway gateway = new Gateway
			{
				Id = id,
				SerialNumber = serialNumber,
				Brand = brand,
				Model = model,
				Ip = ip,
				PortNumber = portNumber
			};
			try
			{
				using (DevicesEntities db = new DevicesEntities())
				{
					db.Gateway.Add(gateway);
					db.SaveChanges();
				}
			}
			catch (DbUpdateException e){ Console.WriteLine(e); }
		}
		/// <summary>
		/// Update the brand and model of a specific water meter in the database table
		/// </summary>
		public void UpdateBrandModelWaterMeterInTable(int id, int serialNumber, string brand, string model)
        {
			try
			{
				using (DevicesEntities db = new DevicesEntities())
				{
					WaterMeter waterMeter = db.WaterMeter.Find(id, serialNumber);
					waterMeter.Brand = brand;
					waterMeter.Model = model;
					db.SaveChanges();
				}
			}
			catch(DbUpdateException e){ Console.WriteLine(e); }
		}
		/// <summary>
		/// Update the brand and model of a specific light meter in the database table
		/// </summary>
		public void UpdateBrandModelLightMeterInTable(int id, int serialNumber, string brand, string model)
		{
			try
			{
				using (DevicesEntities db = new DevicesEntities())
				{
					LightMeter lightMeter = db.LightMeter.Find(id, serialNumber);
					lightMeter.Brand = brand;
					lightMeter.Model = model;
					db.SaveChanges();
				}
			}
			catch(DbUpdateException e){	Console.WriteLine(e); }
		}
		/// <summary>
		/// Update the brand and model of a specific water meter in the database table
		/// </summary>
		public void UpdateBrandModelGatewayInTable(int id, int serialNumber, string brand, string model)
		{
			try
			{
				using (DevicesEntities db = new DevicesEntities())
				{
					Gateway gateway = db.Gateway.Find(id, serialNumber);
					gateway.Brand = brand;
					gateway.Model = model;
					db.SaveChanges();
				}
			}
			catch (DbUpdateException e){ Console.WriteLine(e); }
		}
		/// <summary>
		/// Update the brand and model of a specific gateway in the database table
		/// </summary>
		public void UpdateIpPortGatewayInTable(int id, int serialNumber, string ip, int portNumber)
		{
			Gateway gateway = new Gateway();
			try
			{
				using (DevicesEntities db = new DevicesEntities())
				{
					gateway = db.Gateway.Find(id, serialNumber);
					gateway.Ip = ip;
					gateway.PortNumber = portNumber;
					db.SaveChanges();
				}
			}
			catch (DbUpdateException e){ Console.WriteLine(e); }
		}
		/// <summary>
		/// Delete a specific water meter in the database table
		/// </summary>
		public void DeleteWaterMeterFromTable(int id, int serialNumber)
        {
			WaterMeter waterMeter = new WaterMeter();

			try
			{
				using (DevicesEntities db = new DevicesEntities())
				{
					waterMeter = db.WaterMeter.Find(id, serialNumber);
					db.WaterMeter.Remove(waterMeter);
					db.SaveChanges();
				}
			}
			catch (DbUpdateException e){ Console.WriteLine(e); }
		}
		/// <summary>
		/// Delete a specific light meter in the light meter database table
		/// </summary>
		public void DeleteLightMeterFromTable(int id, int serialNumber)
		{
			LightMeter lightMeter = new LightMeter();

			try
            {
                using (DevicesEntities db = new DevicesEntities())
                {
                    lightMeter = db.LightMeter.Find(id, serialNumber);
                    db.LightMeter.Remove(lightMeter);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException e) { Console.WriteLine(e); }
		}
		/// <summary>
		/// Delete a specific device in the gateway database table
		/// </summary>
		public void DeleteGatewayFromTable(int id, int serialNumber)
		{
			Gateway gateway = new Gateway();

			try
            {
                using (DevicesEntities db = new DevicesEntities())
                {
                    gateway = db.Gateway.Find(id, serialNumber);
                    db.Gateway.Remove(gateway);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException e) { Console.WriteLine(e); }
		}
		/// <summary>
		/// Search a specific water meter in the water meter database table
		/// </summary>
		/// <returns>Water meter Object found</returns>
		public WaterMeter SearchWaterMeter(int id, int serialNumber)
        {
			WaterMeter waterMeter = new WaterMeter();

            using (DevicesEntities db = new DevicesEntities())
            {
				waterMeter = db.WaterMeter.Find(id, serialNumber);
            }
			return waterMeter;
        }
		/// <summary>
		/// Search a specific light meter in the light meter database table
		/// </summary>
		/// <returns>Light meter Object found</returns>
		public LightMeter SearchLightMeter(int id, int serialNumber)
		{
			LightMeter lightMeter = new LightMeter();

			using (DevicesEntities db = new DevicesEntities())
			{
				lightMeter = db.LightMeter.Find(id, serialNumber);
			}
			return lightMeter;
		}
		/// <summary>
		/// Search a specific gateway in the gateway database table
		/// </summary>
		/// <returns>Gateway Object found</returns>
		public Gateway SearchGateway(int id, int serialNumber)
		{
			Gateway gateway = new Gateway();

			using (DevicesEntities db = new DevicesEntities())
			{
				gateway = db.Gateway.Find(id, serialNumber);
			}
			return gateway;
		}
	}
}
