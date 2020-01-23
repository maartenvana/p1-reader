using P1ReaderApp.Model;
using System;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public class ConsoleStatusPrintService : IStatusPrintService
    {
        private P1Measurements _lastMeasurements;
        private P1MessageCollection _lastMessage;

        public Task UpdateP1Measurements(P1Measurements measurements)
        {
            _lastMeasurements = measurements;
            Redraw();

            return Task.CompletedTask;
        }

        public Task UpdateRawData(P1MessageCollection message)
        {
            _lastMessage = message;
            Redraw();

            return Task.CompletedTask;
        }

        private void Redraw()
        {
            Console.Clear();
            WriteRawMessages();
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"----------------------------------------------------------------------");
            WriteMeasurements();
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"----------------------------------------------------------------------");
            WriteCalculatedMeasurements();
        }

        private void WriteCalculatedMeasurements()
        {
            if (_lastMeasurements == null)
            {
                Console.WriteLine("No calculated measurements (yet)");

                return;
            }
            Console.WriteLine($"{nameof(P1Measurements.NetActualElectricityPower)}: {_lastMeasurements.NetActualElectricityPower} kW");
            Console.WriteLine($"{nameof(P1Measurements.TotalInstantaneousCurrent)}: {_lastMeasurements.TotalInstantaneousCurrent} A");
            Console.WriteLine($"{nameof(P1Measurements.TotalInstantaneousVoltage)}: {_lastMeasurements.TotalInstantaneousVoltage} V");
        }

        private void WriteMeasurements()
        {
            if (_lastMeasurements == null)
            {
                Console.WriteLine("No measurements (yet)");

                return;
            }

            Console.WriteLine($"{nameof(P1Measurements.EquipmentIdentifier)}: {_lastMeasurements.EquipmentIdentifier}");
            Console.WriteLine($"{nameof(P1Measurements.Version)}: {_lastMeasurements.Version}");
            Console.WriteLine($"{nameof(P1Measurements.TimeStamp)}: {_lastMeasurements.TimeStamp} | {_lastMeasurements.TimeStamp.Kind} ");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"{nameof(P1Measurements.LongPowerFailuresInAnyPhase)}: {_lastMeasurements.LongPowerFailuresInAnyPhase}");
            Console.WriteLine($"{nameof(P1Measurements.PowerFailuresInAnyPhase)}: {_lastMeasurements.PowerFailuresInAnyPhase}");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"{nameof(P1Measurements.Tariff)}: {_lastMeasurements.Tariff}");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"{nameof(P1Measurements.ActualElectricityPowerDraw)}: {_lastMeasurements.ActualElectricityPowerDraw} kW");
            Console.WriteLine($"{nameof(P1Measurements.ActualElectricityPowerDelivery)}: {_lastMeasurements.ActualElectricityPowerDelivery} kW");
            Console.WriteLine($"{nameof(P1Measurements.ElectricityDeliveredByClientTariff1)}: {_lastMeasurements.ElectricityDeliveredByClientTariff1} kWh");
            Console.WriteLine($"{nameof(P1Measurements.ElectricityDeliveredByClientTariff2)}: {_lastMeasurements.ElectricityDeliveredByClientTariff2} kWh");
            Console.WriteLine($"{nameof(P1Measurements.ElectricityDeliveredToClientTariff1)}: {_lastMeasurements.ElectricityDeliveredToClientTariff1} kWh");
            Console.WriteLine($"{nameof(P1Measurements.ElectricityDeliveredToClientTariff2)}: {_lastMeasurements.ElectricityDeliveredToClientTariff2} kWh");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousActivePowerDeliveryL1)}: {_lastMeasurements.InstantaneousActivePowerDeliveryL1} kW");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousActivePowerDeliveryL2)}: {_lastMeasurements.InstantaneousActivePowerDeliveryL2} kW");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousActivePowerDeliveryL3)}: {_lastMeasurements.InstantaneousActivePowerDeliveryL3} kW");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousActivePowerDrawL1)}: {_lastMeasurements.InstantaneousActivePowerDrawL1} kW");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousActivePowerDrawL2)}: {_lastMeasurements.InstantaneousActivePowerDrawL2} kW");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousActivePowerDrawL3)}: {_lastMeasurements.InstantaneousActivePowerDrawL3} kW");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousCurrentL1)}: {_lastMeasurements.InstantaneousCurrentL1} A");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousCurrentL2)}: {_lastMeasurements.InstantaneousCurrentL2} A");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousCurrentL3)}: {_lastMeasurements.InstantaneousCurrentL3} A");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousVoltageL1)}: {_lastMeasurements.InstantaneousVoltageL1} V");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousVoltageL2)}: {_lastMeasurements.InstantaneousVoltageL2} V");
            Console.WriteLine($"{nameof(P1Measurements.InstantaneousVoltageL3)}: {_lastMeasurements.InstantaneousVoltageL3} V");
        }

        private void WriteRawMessages()
        {
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"Raw messages received at {_lastMessage.ReceivedUtc} UTC:");
            foreach (var message in _lastMessage.Messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}