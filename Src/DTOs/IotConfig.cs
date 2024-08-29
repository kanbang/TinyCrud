
namespace Tiny.DTOs
{
    public class IotConfig
    {
        public List<DayAgeWeatherCurve>? DayAgeWeatherCurve { get; set; }
        public List<DayAgeRespiratoryCapacityCurve>? DayAgeRespiratoryCapacityCurve { get; set; }
        public List<DayAgeWeightCurve>? DayAgeWeightCurve { get; set; }
        public WetCurtainConfig? WetCurtainConfig { get; set; }
        public ApparentTemperatureConfig? ApparentTemperatureConfig { get; set; }
        public HeatStressIndexConfig? HeatStressIndexConfig { get; set; }
        public WaterLineConfig? WaterLineConfig { get; set; }
        public FeedConfig? FeedConfig { get; set; }
    }

    public class DayAgeWeatherCurve
    {
        public int DayAge { get; set; }
        public double HumidityHeatCompCoefficient { get; set; }
        public double AirCoolingCompCoefficient { get; set; }
        public int CO2UpperLimit { get; set; }
        public int CO2LowerLimit { get; set; }
        public int NH3UpperLimit { get; set; }
        public double HeatStressIndex { get; set; }
        public double TargetHumidity { get; set; }
    }

    public class DayAgeRespiratoryCapacityCurve
    {
        public int DayAge { get; set; }
        public List<RespiratoryCapacity> List { get; set; }
    }

    public class RespiratoryCapacity
    {
        public int OutdoorTemp { get; set; }
        public double? TargetValue { get; set; }
    }

    public class DayAgeWeightCurve
    {
        public int DayAge { get; set; }
        public double Weight { get; set; }
    }

    public class WetCurtainConfig
    {
        public int CycleTime { get; set; }
        public List<WetCurtainConfigInfo> WetCurtainConfigInfoList { get; set; }
    }

    public class WetCurtainConfigInfo
    {
        public string LevelName { get; set; }
        public int Sort { get; set; }
        public double TemperatureDifference { get; set; }
        public int StartDayAge { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public double MaxHumidityLimit { get; set; }
        public int MinWindSpeedLimit { get; set; }
        public double MinOutdoorTempLimit { get; set; }
        public int OpenDuration { get; set; }
        public int CloseDuration { get; set; }
        public List<WetCurtain> WetCurtainList { get; set; }
    }

    public class WetCurtain
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class ApparentTemperatureConfig
    {
        public HumidityHeatCompConfig HumidityHeatCompConfig { get; set; }
        public AirCooledCompConfig AirCooledCompConfig { get; set; }
    }

    public class HumidityHeatCompConfig
    {
        public bool IsActive { get; set; }
        public int StartDayAge { get; set; }
        public int StopDayAge { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public double CompHumidityUpperLimit { get; set; }
        public double CompHumidityLowerLimit { get; set; }
        public double HumidityHysteresis { get; set; }
        public int ReactionTime { get; set; }
        public double MaxCompensationTemp { get; set; }
    }

    public class AirCooledCompConfig
    {
        public bool IsActive { get; set; }
        public bool IsTunnelVentilationActive { get; set; }
        public int StartDayAge { get; set; }
        public int StopDayAge { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
    }

    public class HeatStressIndexConfig
    {
        public bool IsActive { get; set; }
        public int AlarmValue { get; set; }
    }

    public class WaterLineConfig
    {
        public double WaterShortageAlarm { get; set; }
        public double WaterLeakageAlarm { get; set; }
        public double LightOffWaterLevel { get; set; }
    }

    public class FeedConfig
    {
        public double SupplementaryFeedSurplus { get; set; }
        public double AlarmSurplus { get; set; }
        public bool IsActiveFeedSignal { get; set; }
    }

}
