[
    {
        "path": "/dayAgeWeatherCurve/0/dayAge",
        "op": "replace",
        "value": 22
    }
]


[
    {
        "path": "dayAgeWeatherCurve",
        "op": "replace",
        "value": [
            {
                "DayAge": 1,
                "HumidityHeatCompCoefficient": 0.1,
                "AirCoolingCompCoefficient": 0.12,
                "CO2UpperLimit": 2000,
                "CO2LowerLimit": 400,
                "NH3UpperLimit": 1500,
                "HeatStressIndex": 1.01,
                "TargetHumidity": 50.1
            },
            {
                "DayAge": 2,
                "HumidityHeatCompCoefficient": 0.12,
                "AirCoolingCompCoefficient": 0.21,
                "CO2UpperLimit": 2100,
                "CO2LowerLimit": 420,
                "NH3UpperLimit": 1530,
                "HeatStressIndex": 1.21,
                "TargetHumidity": 49.2
            }
        ]
    }
]



{
    "DayAgeWeatherCurve": [
        {
            "DayAge": 2,
            "HumidityHeatCompCoefficient": 0.1,
            "AirCoolingCompCoefficient": 0.12,
            "CO2UpperLimit": 2000,
            "CO2LowerLimit": 400,
            "NH3UpperLimit": 1500,
            "HeatStressIndex": 1.02,
            "TargetHumidity": 50.1
        },
        {
            "DayAge": 2,
            "HumidityHeatCompCoefficient": 0.12,
            "AirCoolingCompCoefficient": 0.21,
            "CO2UpperLimit": 2100,
            "CO2LowerLimit": 420,
            "NH3UpperLimit": 1530,
            "HeatStressIndex": 1.21,
            "TargetHumidity": 49.2
        }
    ],
    "DayAgeRespiratoryCapacityCurve": [
        {
            "DayAge": 1,
            "List": [
                {
                    "OutdoorTemp": -20,
                    "TargetValue": 0.21
                },
                {
                    "OutdoorTemp": -10,
                    "TargetValue": 0.3
                },
                {
                    "OutdoorTemp": 0,
                    "TargetValue": 0.41
                },
                {
                    "OutdoorTemp": 10,
                    "TargetValue": 0.52
                },
                {
                    "OutdoorTemp": 20,
                    "TargetValue": 0.66
                },
                {
                    "OutdoorTemp": 30,
                    "TargetValue": 0.68
                }
            ]
        }
    ],
    "DayAgeWeightCurve": [
        {
            "DayAge": 2,
            "Weight": 5.0
        },
        {
            "DayAge": 4,
            "Weight": 6.0
        }
    ],
    "wetCurtainConfig": null,
    "apparentTemperatureConfig": null,
    "heatStressIndexConfig": null,
    "waterLineConfig": null,
    "feedConfig": null
}