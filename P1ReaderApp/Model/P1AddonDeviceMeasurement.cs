using P1ReaderApp.Attributes;

namespace P1ReaderApp.Model
{
    public class P1AddonDeviceMeasurement
    {
        [OBISField("0-n:24.1.0")]
        public string DeviceType { get; set; }

        [OBISField("0-n:96.1.0")]
        public string EquipmentIdentifier { get; set; }

        [OBISField("0-n:24.2.1")]
        public string Last5MinuteValue { get; set; }
    }
}