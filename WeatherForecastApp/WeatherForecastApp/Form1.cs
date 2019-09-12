using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Forms;

namespace WeatherForecastApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            WebRequest request = WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?q=Omsk,ru&APPID=ae00dc4b6dd00a9863e5e712e68387bf");
            request.Method = "POST";
            request.ContentType = "application/x-www-urlencoded";
            WebResponse response = await request.GetResponseAsync();
            string answer = string.Empty;
            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    answer = await reader.ReadToEndAsync();
                }
            }
            response.Close();
            richTextBox1.Text = answer;
            OpenWeather.OpenWeather ow = JsonConvert.DeserializeObject<OpenWeather.OpenWeather>(answer);
            panel1.BackgroundImage = ow.weather[0].Icon;
            label1.Text = ow.weather[0].main;
            label2.Text = ow.weather[0].discription;
            label3.Text = "Средняя температура (C): " + ow.main.temp.ToString("0.##");
            label6.Text = "Скорость (м/с): " + ow.wind.speed.ToString();
            label7.Text = "Направление: " + ow.wind.deg.ToString();
            label4.Text = "Влажность (%): " + ow.main.humidity.ToString();
            label5.Text = "Давление (мм.рт.ст.): " + ((int)ow.main.pressure).ToString();
        }
    }
}
