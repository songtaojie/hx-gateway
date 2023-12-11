// Apache-2.0 License
// Copyright (c) 2021-2022 
// ����:songtaojie
// �绰/΢�ţ�stjworkemail@163.com

namespace Hx.Gateway.PayApi;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
