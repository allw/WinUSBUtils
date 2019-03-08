using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WinUSBUtils.Tests
{
    [TestClass]
    public class GetSerialNumberTests
    {
        [TestMethod]
        [ExpectedException(typeof(EmptySerialNumberException))]
        public void GetSerialNumber_PNPDeviceIDwithEmptySeriaNumberShouldThrowEmtySerialNumberException()
        {
            var deviceFinder = new DeviceFinder(string.Empty, string.Empty);

            deviceFinder.GetSerialNumber(string.Empty);
        }

        [TestMethod]
        public void GetSerialNumber_shouldReturnValidSerialNumberAccordingToPNPDeviceIDProvided()
        {
            var deviceFinder = new DeviceFinder(string.Empty, string.Empty);

            var serialNumber = "412312312344";
            var pnpDeviceID = $@"USB\VID_0D8C&PID_000C\{serialNumber}";

            var result = deviceFinder.GetSerialNumber(pnpDeviceID);

            Assert.AreEqual(serialNumber, result);
        }
    }
}
