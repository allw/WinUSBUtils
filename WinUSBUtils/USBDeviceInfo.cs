namespace WinUSBUtils
{
    public class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description, string serialNumber)
        {
            DeviceID = deviceID;
            PNPDeviceID = pnpDeviceID;
            Description = description;
            SerialNumber = serialNumber;
        }
        public string DeviceID { get; private set; }

        public string PNPDeviceID { get; private set; }

        public string Description { get; private set; }

        public string SerialNumber { get; private set; }
}
}
