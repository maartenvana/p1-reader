using FluentAssertions;
using P1ReaderApp.Model;
using P1ReaderApp.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace P1ReaderApp.Tests
{
    public class MessageParserTests
    {
        [Fact]
        public async Task SagemcomT210DESMR53Fase_ParseSuccess()
        {
            // Arrange
            var expectedTimestamp = DateTime.UtcNow;
            var messageBuffer = new MessageBuffer<P1Measurements>();
            var parser = new MessageParser(messageBuffer);

            // Act
            var measurements = await parser.ParseSerialMessages(new P1MessageCollection
            {
                Messages = TestP1Messages.SagemcomT210DESMR53Fase,
                ReceivedUtc = expectedTimestamp
            });

            // Assert
            measurements.Should().NotBeNull();

            measurements.ActualElectricityPowerDelivery.Should().Be(0.001M);
            measurements.ActualElectricityPowerDraw.Should().Be(0.134M);
            measurements.InstantaneousActivePowerDeliveryL1.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDeliveryL2.Should().Be(0.002M);
            measurements.InstantaneousActivePowerDeliveryL3.Should().Be(0.003M);
            measurements.ElectricityDeliveredByClientTariff1.Should().Be(0.084M);
            measurements.ElectricityDeliveredByClientTariff2.Should().Be(0M);
            measurements.ElectricityDeliveredToClientTariff1.Should().Be(0.855M);
            measurements.ElectricityDeliveredToClientTariff2.Should().Be(0.693M);
            measurements.EquipmentIdentifier.Should().Be("345083459863498657345968");
            measurements.InstantaneousActivePowerDrawL1.Should().Be(0.094M);
            measurements.InstantaneousActivePowerDrawL2.Should().Be(0.040M);
            measurements.InstantaneousActivePowerDrawL3.Should().Be(0.003M);
            measurements.InstantaneousCurrentL1.Should().Be(1);
            measurements.InstantaneousCurrentL2.Should().Be(2);
            measurements.InstantaneousCurrentL3.Should().Be(3);
            measurements.InstantaneousVoltageL1.Should().Be(229.0M);
            measurements.InstantaneousVoltageL2.Should().Be(226.0M);
            measurements.InstantaneousVoltageL3.Should().Be(229.0M);
            measurements.LongPowerFailuresInAnyPhase.Should().Be(4);
            measurements.NetActualElectricityPower.Should().Be(0.134M - 0.001M);
            measurements.PowerFailuresInAnyPhase.Should().Be(8);
            measurements.Tariff.Should().Be(2);
            measurements.TimeStamp.Should().Be(expectedTimestamp);
            measurements.TotalInstantaneousCurrent.Should().Be(1+2+3);
            measurements.TotalInstantaneousVoltage.Should().Be(229.0M + 226.0M + 229.0M);
            measurements.Version.Should().Be("50");
        }

        [Fact]
        public async Task LandisGyrE350ZCF1001FaseKleinverbruik_ParseSuccess()
        {
            // Arrange
            var expectedTimestamp = DateTime.UtcNow;
            var messageBuffer = new MessageBuffer<P1Measurements>();
            var parser = new MessageParser(messageBuffer);

            // Act
            var measurements = await parser.ParseSerialMessages(new P1MessageCollection
            {
                Messages = TestP1Messages.LandisGyrE350ZCF1001FaseKleinverbruik,
                ReceivedUtc = expectedTimestamp
            });

            // Assert
            measurements.Should().NotBeNull();

            measurements.ActualElectricityPowerDelivery.Should().Be(0.001M);
            measurements.ActualElectricityPowerDraw.Should().Be(0.494M);
            measurements.InstantaneousActivePowerDeliveryL1.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDeliveryL2.Should().Be(0M);
            measurements.InstantaneousActivePowerDeliveryL3.Should().Be(0M);
            measurements.ElectricityDeliveredByClientTariff1.Should().Be(10.981M);
            measurements.ElectricityDeliveredByClientTariff2.Should().Be(28.031M);
            measurements.ElectricityDeliveredToClientTariff1.Should().Be(2074.842M);
            measurements.ElectricityDeliveredToClientTariff2.Should().Be(881.383M);
            measurements.EquipmentIdentifier.Should().Be("4530303331303033303031363939353135");
            measurements.InstantaneousActivePowerDrawL1.Should().Be(0.494M);
            measurements.InstantaneousActivePowerDrawL2.Should().Be(0);
            measurements.InstantaneousActivePowerDrawL3.Should().Be(0);
            measurements.InstantaneousCurrentL1.Should().Be(3);
            measurements.InstantaneousCurrentL2.Should().Be(0);
            measurements.InstantaneousCurrentL3.Should().Be(0);
            measurements.InstantaneousVoltageL1.Should().Be(0);
            measurements.InstantaneousVoltageL2.Should().Be(0);
            measurements.InstantaneousVoltageL3.Should().Be(0);
            measurements.LongPowerFailuresInAnyPhase.Should().Be(3);
            measurements.NetActualElectricityPower.Should().Be(0.494M - 0.001M);
            measurements.PowerFailuresInAnyPhase.Should().Be(4);
            measurements.Tariff.Should().Be(1);
            measurements.TimeStamp.Should().Be(expectedTimestamp);
            measurements.TotalInstantaneousCurrent.Should().Be(3);
            measurements.TotalInstantaneousVoltage.Should().Be(0);
            measurements.Version.Should().Be("42");
        }

        [Fact]
        public async Task KaifaE0003Telegram_ParseSuccess()
        {
            // Arrange
            var expectedTimestamp = DateTime.UtcNow;
            var messageBuffer = new MessageBuffer<P1Measurements>();
            var parser = new MessageParser(messageBuffer);

            // Act
            var measurements = await parser.ParseSerialMessages(new P1MessageCollection
            {
                Messages = TestP1Messages.KaifaE0003Telegram,
                ReceivedUtc = expectedTimestamp
            });

            // Assert
            measurements.Should().NotBeNull();

            measurements.ActualElectricityPowerDelivery.Should().Be(1.869M);
            measurements.ActualElectricityPowerDraw.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDeliveryL1.Should().Be(0.688M);
            measurements.InstantaneousActivePowerDeliveryL2.Should().Be(0.778M);
            measurements.InstantaneousActivePowerDeliveryL3.Should().Be(0.403M);
            measurements.ElectricityDeliveredByClientTariff1.Should().Be(3284.320M);
            measurements.ElectricityDeliveredByClientTariff2.Should().Be(7764.691M);
            measurements.ElectricityDeliveredToClientTariff1.Should().Be(4726.494M);
            measurements.ElectricityDeliveredToClientTariff2.Should().Be(4844.281M);
            measurements.EquipmentIdentifier.Should().Be("4530303033303030303032313234383133");
            measurements.InstantaneousActivePowerDrawL1.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDrawL2.Should().Be(0.003M);
            measurements.InstantaneousActivePowerDrawL3.Should().Be(0.002M);
            measurements.InstantaneousCurrentL1.Should().Be(3);
            measurements.InstantaneousCurrentL2.Should().Be(4);
            measurements.InstantaneousCurrentL3.Should().Be(2);
            measurements.InstantaneousVoltageL1.Should().Be(0);
            measurements.InstantaneousVoltageL2.Should().Be(0);
            measurements.InstantaneousVoltageL3.Should().Be(0);
            measurements.LongPowerFailuresInAnyPhase.Should().Be(7);
            measurements.NetActualElectricityPower.Should().Be(0.001M - 1.869M);
            measurements.PowerFailuresInAnyPhase.Should().Be(13);
            measurements.Tariff.Should().Be(2);
            measurements.TimeStamp.Should().Be(expectedTimestamp);
            measurements.TotalInstantaneousCurrent.Should().Be(3+4+2);
            measurements.TotalInstantaneousVoltage.Should().Be(0);
            measurements.Version.Should().Be("42");
        }

        [Fact]
        public async Task ESMR50ISKRAAM550_ParseSuccess()
        {
            // Arrange
            var expectedTimestamp = DateTime.UtcNow;
            var messageBuffer = new MessageBuffer<P1Measurements>();
            var parser = new MessageParser(messageBuffer);

            // Act
            var measurements = await parser.ParseSerialMessages(new P1MessageCollection
            {
                Messages = TestP1Messages.ESMR50ISKRAAM550,
                ReceivedUtc = expectedTimestamp
            });

            // Assert
            measurements.Should().NotBeNull();

            measurements.ActualElectricityPowerDelivery.Should().Be(0.498M);
            measurements.ActualElectricityPowerDraw.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDeliveryL1.Should().Be(0.676M);
            measurements.InstantaneousActivePowerDeliveryL2.Should().Be(0.002M);
            measurements.InstantaneousActivePowerDeliveryL3.Should().Be(0.003M);
            measurements.ElectricityDeliveredByClientTariff1.Should().Be(1285.951M);
            measurements.ElectricityDeliveredByClientTariff2.Should().Be(2876.514M);
            measurements.ElectricityDeliveredToClientTariff1.Should().Be(3808.351M);
            measurements.ElectricityDeliveredToClientTariff2.Should().Be(2948.827M);
            measurements.EquipmentIdentifier.Should().Be("4530303334303036383130353136343136");
            measurements.InstantaneousActivePowerDrawL1.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDrawL2.Should().Be(0.033M);
            measurements.InstantaneousActivePowerDrawL3.Should().Be(0.132M);
            measurements.InstantaneousCurrentL1.Should().Be(2);
            measurements.InstantaneousCurrentL2.Should().Be(5);
            measurements.InstantaneousCurrentL3.Should().Be(9);
            measurements.InstantaneousVoltageL1.Should().Be(236.0M);
            measurements.InstantaneousVoltageL2.Should().Be(232.6M);
            measurements.InstantaneousVoltageL3.Should().Be(235.1M);
            measurements.LongPowerFailuresInAnyPhase.Should().Be(3);
            measurements.NetActualElectricityPower.Should().Be(0.001M - 0.498M);
            measurements.PowerFailuresInAnyPhase.Should().Be(6);
            measurements.Tariff.Should().Be(2);
            measurements.TimeStamp.Should().Be(expectedTimestamp);
            measurements.TotalInstantaneousCurrent.Should().Be(2+5+9);
            measurements.TotalInstantaneousVoltage.Should().Be(236.0M+232.6M+235.1M);
            measurements.Version.Should().Be("50");
        }

        [Fact]
        public async Task DSRM40Telegram_ParseSuccess()
        {
            // Arrange
            var expectedTimestamp = DateTime.UtcNow;
            var messageBuffer = new MessageBuffer<P1Measurements>();
            var parser = new MessageParser(messageBuffer);

            // Act
            var measurements = await parser.ParseSerialMessages(new P1MessageCollection
            {
                Messages = TestP1Messages.DSRM40Telegram,
                ReceivedUtc = expectedTimestamp
            });

            // Assert
            measurements.Should().NotBeNull();

            measurements.ActualElectricityPowerDelivery.Should().Be(0.001M);
            measurements.ActualElectricityPowerDraw.Should().Be(1.193M);
            measurements.InstantaneousActivePowerDeliveryL1.Should().Be(0);
            measurements.InstantaneousActivePowerDeliveryL2.Should().Be(0);
            measurements.InstantaneousActivePowerDeliveryL3.Should().Be(0);
            measurements.ElectricityDeliveredByClientTariff1.Should().Be(123459.789M);
            measurements.ElectricityDeliveredByClientTariff2.Should().Be(123460.789M);
            measurements.ElectricityDeliveredToClientTariff1.Should().Be(123457.789M);
            measurements.ElectricityDeliveredToClientTariff2.Should().Be(123458.789M);
            measurements.EquipmentIdentifier.Should().Be("4B384547303034303436333935353037");
            measurements.InstantaneousActivePowerDrawL1.Should().Be(0);
            measurements.InstantaneousActivePowerDrawL2.Should().Be(0);
            measurements.InstantaneousActivePowerDrawL3.Should().Be(0);
            measurements.InstantaneousCurrentL1.Should().Be(0);
            measurements.InstantaneousCurrentL2.Should().Be(0);
            measurements.InstantaneousCurrentL3.Should().Be(0);
            measurements.InstantaneousVoltageL1.Should().Be(0);
            measurements.InstantaneousVoltageL2.Should().Be(0);
            measurements.InstantaneousVoltageL3.Should().Be(0);
            measurements.LongPowerFailuresInAnyPhase.Should().Be(2);
            measurements.NetActualElectricityPower.Should().Be(1.193M - 0.001M);
            measurements.PowerFailuresInAnyPhase.Should().Be(4);
            measurements.Tariff.Should().Be(2);
            measurements.TimeStamp.Should().Be(expectedTimestamp);
            measurements.TotalInstantaneousCurrent.Should().Be(0);
            measurements.TotalInstantaneousVoltage.Should().Be(0);
            measurements.Version.Should().Be("40");
        }

        [Fact]
        public async Task MessageVariant1_ParseSucces()
        {
            // Arrange
            var expectedTimestamp = DateTime.UtcNow;
            var messageBuffer = new MessageBuffer<P1Measurements>();
            var parser = new MessageParser(messageBuffer);

            // Act
            var measurements = await parser.ParseSerialMessages(new P1MessageCollection
            {
                Messages = TestP1Messages.MessageVariant1,
                ReceivedUtc = expectedTimestamp
            });

            // Assert
            measurements.Should().NotBeNull();

            measurements.ActualElectricityPowerDelivery.Should().Be(0.005M);
            measurements.ActualElectricityPowerDraw.Should().Be(0.484M);
            measurements.InstantaneousActivePowerDeliveryL1.Should().Be(0.001M);
            measurements.InstantaneousActivePowerDeliveryL2.Should().Be(0.002M);
            measurements.InstantaneousActivePowerDeliveryL3.Should().Be(0.003M);
            measurements.ElectricityDeliveredByClientTariff1.Should().Be(507.529M);
            measurements.ElectricityDeliveredByClientTariff2.Should().Be(1416.610M);
            measurements.ElectricityDeliveredToClientTariff1.Should().Be(9532.622M);
            measurements.ElectricityDeliveredToClientTariff2.Should().Be(5254.129M);
            measurements.EquipmentIdentifier.Should().Be("5179164565876186628967151365498132");
            measurements.InstantaneousActivePowerDrawL1.Should().Be(0.396M);
            measurements.InstantaneousActivePowerDrawL2.Should().Be(0.078M);
            measurements.InstantaneousActivePowerDrawL3.Should().Be(0.009M);
            measurements.InstantaneousCurrentL1.Should().Be(3);
            measurements.InstantaneousCurrentL2.Should().Be(4);
            measurements.InstantaneousCurrentL3.Should().Be(10);
            measurements.InstantaneousVoltageL1.Should().Be(0);
            measurements.InstantaneousVoltageL2.Should().Be(0);
            measurements.InstantaneousVoltageL3.Should().Be(0);
            measurements.LongPowerFailuresInAnyPhase.Should().Be(6);
            measurements.NetActualElectricityPower.Should().Be(0.484M - 0.005M);
            measurements.PowerFailuresInAnyPhase.Should().Be(12);
            measurements.Tariff.Should().Be(1);
            measurements.TimeStamp.Should().Be(expectedTimestamp);
            measurements.TotalInstantaneousCurrent.Should().Be(3 + 4 + 10);
            measurements.TotalInstantaneousVoltage.Should().Be(0);
            measurements.Version.Should().Be("42");
        }
    }
}