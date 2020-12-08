using System;
using System.Collections.Generic;
using System.Management;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("WinUSBUtils.Tests")]

namespace WinUSBUtils
{
    public class DeviceFinder
    {
        public string VendorID { get; private set; }

        public string ProductID { get; private set; }

        public DeviceFinder(string vendorID, string productID)
        {
            VendorID = vendorID;
            ProductID = productID;
        }

        public IEnumerable<USBDeviceInfo> GetAll()
        {
            List<USBDeviceInfo> result = new List<USBDeviceInfo>();

            try
            {
                var deviceIDpattern = $@"USB%%VID_{VendorID}&PID_{ProductID}%";
                var query = $@"SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE ""{deviceIDpattern}""";
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    ManagementObjectCollection collection;

                    collection = searcher.Get();

                    foreach (var device in collection)
                    {
                        try
                        {
                            result.Add(ExtractDevice(device));
                        }
                        catch (EmptySerialNumberException)
                        {
                            continue;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }

            return result;
        }

        public IEnumerable<USBDeviceInfo> FindBySerialNumberPrefix(string serialNumberPrefix)
        {
            List<USBDeviceInfo> result = new List<USBDeviceInfo>();

            try
            {
                var deviceIDpattern = $@"USB%%VID_{VendorID}&PID_{ProductID}\\%%{serialNumberPrefix}%";
                var query = $@"SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE ""{deviceIDpattern}""";
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    ManagementObjectCollection collection;

                    collection = searcher.Get();

                    foreach(var device in collection)
                    {
                        try
                        {
                            result.Add(ExtractDevice(device));
                        }
                        catch(EmptySerialNumberException)
                        {
                            continue;
                        }
                        catch(Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }

            return result;
        }

        private USBDeviceInfo ExtractDevice(ManagementBaseObject device)
        {
            string deviceID = (string)device.GetPropertyValue("DeviceID");
            string pnpDeviceID = (string)device.GetPropertyValue("PNPDeviceID");
            string description = (string)device.GetPropertyValue("Description");
            string serialNumber = GetSerialNumber(pnpDeviceID);

            return new USBDeviceInfo(deviceID, pnpDeviceID, description, serialNumber);
        }

        internal string GetSerialNumber(string pnpDeviceID)
        {
            string serialNumber = string.Empty;
            // The serial number isn´t a property of the Win32_PnPEntity class, therefore it must be extracted from the PNODeviceID field
            // PNPDeviceID format: USB\VID_0D8C&PID_000C\000000004321
            string[] serialNumberData = pnpDeviceID.Split('\\');
            if (serialNumberData.Length >= 3)
            {
                serialNumber = serialNumberData[2];
            }
            if(string.IsNullOrWhiteSpace(serialNumber))
            {
                throw new EmptySerialNumberException($"The device with PNPDeviceID {pnpDeviceID} doesn´t have a serial number.");
            }
            return serialNumber;
        }
    }
}
