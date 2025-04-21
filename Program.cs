using LibreHardwareMonitor.Hardware;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Computer computer = new Computer
{
    IsCpuEnabled = true,
    IsGpuEnabled = true,
    IsMemoryEnabled = true,
    IsMotherboardEnabled = true,
    IsControllerEnabled = true,
    IsNetworkEnabled = true,
    IsStorageEnabled = true
};


string getMetrics() {
    computer.Open();
    computer.Accept(new UpdateVisitor());
    var metrics = new List<string>();
    foreach (IHardware hardware in computer.Hardware)
    {
        foreach (ISensor sensor in hardware.Sensors)
        {
            metrics.Add(formatSensorData(sensor, hardware.Name));
        }
    }
    computer.Close();
    return string.Join("\n", metrics);
}

static string formatSensorData(ISensor sensor, string hardwareName)
    {
        string metricName = "unknown";
        switch (sensor.SensorType)
        {
            case SensorType.Voltage:
                metricName = "voltage";
				break;
            case SensorType.Current:
                metricName = "current";
				break;
            case SensorType.Power:
                metricName = "power";
				break;
            case SensorType.Clock:
                metricName = "clock";
				break;
            case SensorType.Temperature:
                metricName = "temperature";
				break;
            case SensorType.Load:
                metricName = "load";
				break;
            case SensorType.Frequency:
                metricName = "frequency";
				break;
            case SensorType.Fan:
                metricName = "fan";
				break;
            case SensorType.Flow:
                metricName = "flow";
				break;
            case SensorType.Control:
                metricName = "control";
				break;
            case SensorType.Level:
                metricName = "level";
				break;
            case SensorType.Factor:
                metricName = "factor";
				break;
            case SensorType.Data:
                metricName = "data";
				break;
            case SensorType.SmallData:
                metricName = "smallData";
				break;
            case SensorType.Throughput:
                metricName = "throughput";
				break;
            case SensorType.TimeSpan:
                metricName = "timeSpan";
				break;
            case SensorType.Energy:
                metricName = "energy";
				break;
            case SensorType.Noise:
                metricName = "noise";
				break;
            case SensorType.Conductivity:
                metricName = "conductivity";
				break;
            case SensorType.Humidity:
                metricName = "humidity";
				break;
        }

        metricName = "librehardwaremonitor_" + metricName;
        string label = $"{sensor.Name.Replace(" ", "_").Replace("/", "_").Replace("#", "").Replace(")", "").Replace("(", "").ToLower()}";
        
        return $"{metricName}{{hardware=\"{hardwareName}\", sensor=\"{label}\"}} {sensor.Value.GetValueOrDefault()}";
    }

app.MapGet("/", () => "OK");
app.MapGet("/metrics", getMetrics);

app.Run("http://0.0.0.0:9100");


class UpdateVisitor : IVisitor
{
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }
    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();
        foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
    }
    public void VisitSensor(ISensor sensor) { }
    public void VisitParameter(IParameter parameter) { }
}

