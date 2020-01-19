using P1ReaderApp.Model;
using Serilog;
using System.Threading.Tasks;
using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
using System;
using System.Collections.Generic;

namespace P1ReaderApp.Storage
{
    public class InfluxDbStorage : IStorage
    {
        private readonly LineProtocolClient _client;

        public InfluxDbStorage(string serverAddress, string database, string username, string password)
        {
            _client = new LineProtocolClient(new Uri(serverAddress), database, username, password);
        }

        public async Task SaveP1Measurement(P1Measurements p1Measurements)
        {
            Log.Debug("Saving P1 measurement ({timestamp}) to InfluxDB", p1Measurements.TimeStamp);

            var payload = new LineProtocolPayload();

            payload.Add(new LineProtocolPoint(
                measurement: "p1power",
                fields: new Dictionary<string, object>
                {
                    { "ActualElectricityPowerDelivery", p1Measurements.ActualElectricityPowerDelivery },
                    { "ActualElectricityPowerDraw", p1Measurements.ActualElectricityPowerDraw },
                    { "ElectricityDeliveredByClientTariff1", p1Measurements.ElectricityDeliveredByClientTariff1 },
                    { "ElectricityDeliveredByClientTariff2", p1Measurements.ElectricityDeliveredByClientTariff2 },
                    { "ElectricityDeliveredToClientTariff1", p1Measurements.ElectricityDeliveredToClientTariff1 },
                    { "ElectricityDeliveredToClientTariff2", p1Measurements.ElectricityDeliveredToClientTariff2 },
                    { "InstantaneousActivePowerDeliveryL1", p1Measurements.InstantaneousActivePowerDeliveryL1 },
                    { "InstantaneousActivePowerDeliveryL2", p1Measurements.InstantaneousActivePowerDeliveryL2 },
                    { "InstantaneousActivePowerDeliveryL3", p1Measurements.InstantaneousActivePowerDeliveryL3 },
                    { "InstantaneousActivePowerDrawL1", p1Measurements.InstantaneousActivePowerDrawL1 },
                    { "InstantaneousActivePowerDrawL2", p1Measurements.InstantaneousActivePowerDrawL2 },
                    { "InstantaneousActivePowerDrawL3", p1Measurements.InstantaneousActivePowerDrawL3 },
                    { "InstantaneousCurrentL1", p1Measurements.InstantaneousCurrentL1 },
                    { "InstantaneousCurrentL2", p1Measurements.InstantaneousCurrentL2 },
                    { "InstantaneousCurrentL3", p1Measurements.InstantaneousCurrentL3 },
                    { "InstantaneousVoltageL1", p1Measurements.InstantaneousVoltageL1 },
                    { "InstantaneousVoltageL2", p1Measurements.InstantaneousVoltageL2 },
                    { "InstantaneousVoltageL3", p1Measurements.InstantaneousVoltageL3 }
                },
                tags: new Dictionary<string, string>(),
                utcTimestamp: p1Measurements.TimeStamp.ToUniversalTime()));

            var influxResult = await _client.WriteAsync(payload);

            if (!influxResult.Success)
            {
                Log.Error("Error writing to influxdb: {errorMessage}", influxResult.ErrorMessage);
            }
        }
    }
}