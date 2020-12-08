# WinUSBUtils
Util classes for working with USB devices in windows applications.

DeviceFinder is able to identify devices from a serial number prefix, given you know its VendorID and ProductID.

```
var finder = new DeviceFinder("0D8C", "000C");
var devices = finder.FindBySerialNumberPrefix("PREF");
```
