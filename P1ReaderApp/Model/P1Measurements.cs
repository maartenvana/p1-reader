using P1ReaderApp.Attributes;
using System;
using System.Collections.Generic;

namespace P1ReaderApp.Model
{
    public class P1Measurements
    {
        [OBISField("1-0:2.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal ActualElectricityPowerDelivery { get; set; }

        [OBISField("1-0:1.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal ActualElectricityPowerDraw { get; set; }

        [OBISField("0-n:24.1.0")]
        public List<P1AddonDeviceMeasurement> AddonMeasurements { get; set; } = new List<P1AddonDeviceMeasurement>();

        [OBISField("1-0:2.8.1", ValueRegex = @"[\(](.*?)(\*kWh)[\)]")]
        public decimal ElectricityDeliveredByClientTariff1 { get; set; }

        [OBISField("1-0:2.8.2", ValueRegex = @"[\(](.*?)(\*kWh)[\)]")]
        public decimal ElectricityDeliveredByClientTariff2 { get; set; }

        [OBISField("1-0:1.8.1", ValueRegex = @"[\(](.*?)(\*kWh)[\)]")]
        public decimal ElectricityDeliveredToClientTariff1 { get; set; }

        [OBISField("1-0:1.8.2", ValueRegex = @"[\(](.*?)(\*kWh)[\)]")]
        public decimal ElectricityDeliveredToClientTariff2 { get; set; }

        [OBISField("0-0:96.1.1")]
        public string EquipmentIdentifier { get; set; }

        [OBISField("1-0:22.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal InstantaneousActivePowerDeliveryL1 { get; set; }

        [OBISField("1-0:42.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal InstantaneousActivePowerDeliveryL2 { get; set; }

        [OBISField("1-0:62.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal InstantaneousActivePowerDeliveryL3 { get; set; }

        [OBISField("1-0:21.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal InstantaneousActivePowerDrawL1 { get; set; }

        [OBISField("1-0:41.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal InstantaneousActivePowerDrawL2 { get; set; }

        [OBISField("1-0:61.7.0", ValueRegex = @"[\(](.*?)(\*kW)[\)]")]
        public decimal InstantaneousActivePowerDrawL3 { get; set; }

        [OBISField("1-0:31.7.0", ValueRegex = @"[\(](.*?)(\*A)[\)]")]
        public int InstantaneousCurrentL1 { get; set; }

        [OBISField("1-0:51.7.0", ValueRegex = @"[\(](.*?)(\*A)[\)]")]
        public int InstantaneousCurrentL2 { get; set; }

        [OBISField("1-0:71.7.0", ValueRegex = @"[\(](.*?)(\*A)[\)]")]
        public int InstantaneousCurrentL3 { get; set; }

        [OBISField("1-0:32.7.0", ValueRegex = @"[\(](.*?)(\*V)[\)]")]
        public decimal InstantaneousVoltageL1 { get; set; }

        [OBISField("1-0:52.7.0", ValueRegex = @"[\(](.*?)(\*V)[\)]")]
        public decimal InstantaneousVoltageL2 { get; set; }

        [OBISField("1-0:72.7.0", ValueRegex = @"[\(](.*?)(\*V)[\)]")]
        public decimal InstantaneousVoltageL3 { get; set; }

        [OBISField("0-0:96.7.9")]
        public int LongPowerFailuresInAnyPhase { get; set; }

        public decimal NetActualElectricityPower => ActualElectricityPowerDraw - ActualElectricityPowerDelivery;

        [OBISField("0-0:96.7.21")]
        public int PowerFailuresInAnyPhase { get; set; }

        [OBISField("0-0:96.14.0")]
        public int Tariff { get; set; }

        [OBISField("0-0:1.0.0", Format = "yyMMddHHmmss", ValueRegex = @"[\(](.*?)(W)[\)]")]
        public DateTime TimeStamp { get; set; }

        public int TotalInstantaneousCurrent => InstantaneousCurrentL1 + InstantaneousCurrentL2 + InstantaneousCurrentL3;

        public decimal TotalInstantaneousVoltage => InstantaneousVoltageL1 + InstantaneousVoltageL2 + InstantaneousVoltageL3;

        [OBISField("1-3:0.2.8")]
        public string Version { get; set; }
    }
}