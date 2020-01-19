using P1ReaderApp.Attributes;
using P1ReaderApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public class MessageParser : IMessageParser
    {
        private readonly IMessageBuffer<P1Measurements> _measurementsBuffer;
        private IDictionary<string, OBISField> _fields;

        public MessageParser(IMessageBuffer<P1Measurements> measurementsBuffer)
        {
            _measurementsBuffer = measurementsBuffer;
            CreateFieldDictionary();
        }

        public async Task<P1Measurements> ParseSerialMessages(List<string> messages)
        {
            var measurements = new P1Measurements
            {
                ActualElectricityPowerDelivery = GetDecimalField(nameof(P1Measurements.ActualElectricityPowerDelivery), messages),
                ActualElectricityPowerDraw = GetDecimalField(nameof(P1Measurements.ActualElectricityPowerDraw), messages),
                ElectricityDeliveredByClientTariff1 = GetDecimalField(nameof(P1Measurements.ElectricityDeliveredByClientTariff1), messages),
                ElectricityDeliveredByClientTariff2 = GetDecimalField(nameof(P1Measurements.ElectricityDeliveredByClientTariff2), messages),
                ElectricityDeliveredToClientTariff1 = GetDecimalField(nameof(P1Measurements.ElectricityDeliveredToClientTariff1), messages),
                ElectricityDeliveredToClientTariff2 = GetDecimalField(nameof(P1Measurements.ElectricityDeliveredToClientTariff2), messages),
                EquipmentIdentifier = GetStringField(nameof(P1Measurements.EquipmentIdentifier), messages),
                InstantaneousActivePowerDeliveryL1 = GetDecimalField(nameof(P1Measurements.InstantaneousActivePowerDeliveryL1), messages),
                InstantaneousActivePowerDeliveryL2 = GetDecimalField(nameof(P1Measurements.InstantaneousActivePowerDeliveryL2), messages),
                InstantaneousActivePowerDeliveryL3 = GetDecimalField(nameof(P1Measurements.InstantaneousActivePowerDeliveryL3), messages),
                InstantaneousActivePowerDrawL1 = GetDecimalField(nameof(P1Measurements.InstantaneousActivePowerDrawL1), messages),
                InstantaneousActivePowerDrawL2 = GetDecimalField(nameof(P1Measurements.InstantaneousActivePowerDrawL2), messages),
                InstantaneousActivePowerDrawL3 = GetDecimalField(nameof(P1Measurements.InstantaneousActivePowerDrawL3), messages),
                InstantaneousCurrentL1 = GetIntegerField(nameof(P1Measurements.InstantaneousCurrentL1), messages),
                InstantaneousCurrentL2 = GetIntegerField(nameof(P1Measurements.InstantaneousCurrentL2), messages),
                InstantaneousCurrentL3 = GetIntegerField(nameof(P1Measurements.InstantaneousCurrentL3), messages),
                InstantaneousVoltageL1 = GetIntegerField(nameof(P1Measurements.InstantaneousVoltageL1), messages),
                InstantaneousVoltageL2 = GetIntegerField(nameof(P1Measurements.InstantaneousVoltageL2), messages),
                InstantaneousVoltageL3 = GetIntegerField(nameof(P1Measurements.InstantaneousVoltageL3), messages),
                LongPowerFailuresInAnyPhase = GetIntegerField(nameof(P1Measurements.LongPowerFailuresInAnyPhase), messages),
                PowerFailuresInAnyPhase = GetIntegerField(nameof(P1Measurements.PowerFailuresInAnyPhase), messages),
                Tariff = GetIntegerField(nameof(P1Measurements.Tariff), messages),
                TimeStamp = GetDateTimeField(nameof(P1Measurements.TimeStamp), messages),
                Version = GetStringField(nameof(P1Measurements.Version), messages)
            };

            await _measurementsBuffer.QueueMessage(measurements, CancellationToken.None);

            return measurements;
        }

        private void CreateFieldDictionary()
        {
            _fields = new Dictionary<string, OBISField>();

            PropertyInfo[] properties = typeof(P1Measurements).GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is OBISField obisField)
                    {
                        _fields.Add(property.Name, obisField);
                    }
                }
            }
        }

        private DateTime GetDateTimeField(string fieldName, List<string> messages)
        {
            var (obisField, fieldValue) = GetField(fieldName, messages);

            return string.IsNullOrWhiteSpace(fieldValue) ? new DateTime(1970, 1, 1) :
                DateTime.ParseExact(fieldValue, obisField.Format, CultureInfo.InvariantCulture);
        }

        private decimal GetDecimalField(string fieldName, List<string> messages)
        {
            var (_, fieldValue) = GetField(fieldName, messages);

            return string.IsNullOrWhiteSpace(fieldValue) ? 0M :
                decimal.Parse(fieldValue, CultureInfo.InvariantCulture);
        }

        private (OBISField, string) GetField(string fieldName, List<string> messages)
        {
            var obisField = _fields[fieldName];

            foreach (var message in messages)
            {
                if (message.StartsWith(obisField.Reference))
                {
                    var match = Regex.Match(message, obisField.ValueRegex);

                    if (match.Success)
                    {
                        var matchGroup = match.Groups[1];

                        return (obisField, matchGroup.Value);
                    }
                }
            }

            return (obisField, string.Empty);
        }

        private int GetIntegerField(string fieldName, List<string> messages)
        {
            var (_, fieldValue) = GetField(fieldName, messages);

            return string.IsNullOrWhiteSpace(fieldValue) ? 0 :
                int.Parse(fieldValue, CultureInfo.InvariantCulture);
        }

        private string GetStringField(string fieldName, List<string> messages)
        {
            var (_, fieldValue) = GetField(fieldName, messages);

            return fieldValue;
        }
    }
}