using P1ReaderApp.Model;
using System;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public class ConsoleStatusPrintService
    {
        public Task PrintP1Measurement(P1Measurements measurements)
        {
            Console.Clear();

            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"EquipmentIdentifier: {measurements.EquipmentIdentifier}");
            Console.WriteLine($"Version: {measurements.Version}");
            Console.WriteLine($"TimeStamp: {measurements.TimeStamp}");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"LongPowerFailuresInAnyPhase: {measurements.LongPowerFailuresInAnyPhase}");
            Console.WriteLine($"PowerFailuresInAnyPhase: {measurements.PowerFailuresInAnyPhase}");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"Tariff: {measurements.Tariff}");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"----------------------------------------------------------------------");
            Console.WriteLine($"ActualElectricityPowerDraw: {measurements.ActualElectricityPowerDraw} kW");
            Console.WriteLine($"ActualElectricityPowerDelivery: {measurements.ActualElectricityPowerDelivery} kW");
            Console.WriteLine($"ElectricityDeliveredByClientTariff1: {measurements.ElectricityDeliveredByClientTariff1} kWh");
            Console.WriteLine($"ElectricityDeliveredByClientTariff2: {measurements.ElectricityDeliveredByClientTariff2} kWh");
            Console.WriteLine($"ElectricityDeliveredToClientTariff1: {measurements.ElectricityDeliveredToClientTariff1} kWh");
            Console.WriteLine($"ElectricityDeliveredToClientTariff2: {measurements.ElectricityDeliveredToClientTariff2} kWh");
            Console.WriteLine($"InstantaneousActivePowerDeliveryL1: {measurements.InstantaneousActivePowerDeliveryL1} kW");
            Console.WriteLine($"InstantaneousActivePowerDeliveryL2: {measurements.InstantaneousActivePowerDeliveryL2} kW");
            Console.WriteLine($"InstantaneousActivePowerDeliveryL3: {measurements.InstantaneousActivePowerDeliveryL3} kW");
            Console.WriteLine($"InstantaneousActivePowerDrawL1: {measurements.InstantaneousActivePowerDrawL1} kW");
            Console.WriteLine($"InstantaneousActivePowerDrawL2: {measurements.InstantaneousActivePowerDrawL2} kW");
            Console.WriteLine($"InstantaneousActivePowerDrawL3: {measurements.InstantaneousActivePowerDrawL3} kW");
            Console.WriteLine($"InstantaneousCurrentL1: {measurements.InstantaneousCurrentL1} A");
            Console.WriteLine($"InstantaneousCurrentL2: {measurements.InstantaneousCurrentL2} A");
            Console.WriteLine($"InstantaneousCurrentL3: {measurements.InstantaneousCurrentL3} A");
            Console.WriteLine($"InstantaneousVoltageL1: {measurements.InstantaneousVoltageL1} V");
            Console.WriteLine($"InstantaneousVoltageL2: {measurements.InstantaneousVoltageL2} V");
            Console.WriteLine($"InstantaneousVoltageL3: {measurements.InstantaneousVoltageL3} V");

            return Task.CompletedTask;
        }
    }
}