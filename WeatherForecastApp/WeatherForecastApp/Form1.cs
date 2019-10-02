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
        private string url = "https://api.openweathermap.org/data/2.5/forecast?q=";
        private string units = "metric";
        public string City { get; set; }

        public Form1()
        {
            InitializeComponent();
            cBCity.SelectedIndexChanged += cBCity_SelectedIndexChanged;
            City = cBCity.Text;
        }

        private async void Request(string URL, string town, string Unit)
        {
            WebRequest request = WebRequest.Create(URL + town + ",ru&cnt=4&units=" + Unit + "&APPID=ae00dc4b6dd00a9863e5e712e68387bf");
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
            panel1.BackgroundImage = ow.list[0].weather[0].Icon;
            panel2.BackgroundImage = ow.list[1].weather[0].Icon;
            panel3.BackgroundImage = ow.list[2].weather[0].Icon;
            label1.Text = ow.list[0].weather[0].main;
            switch (Unit)
            {
                case "metric":
                    {
                        label3.Text = ow.list[0].main.temp.ToString("0.##") + " ⁰C";
                        label8.Text = ow.list[1].main.temp.ToString("0.##") + " ⁰C";
                        label9.Text = "Ветер: " + ow.list[1].wind.speed.ToString() + " метр/сек";
                        label6.Text = "Скорость (метр/сек): " + ow.list[0].wind.speed.ToString();
                        label10.Text = ow.list[2].main.temp.ToString("0.##") + " ⁰C";
                        label11.Text = "Ветер: " + ow.list[2].wind.speed.ToString() + " метр/сек";
                        break;
                    }
                case "imperial":
                    {
                        label3.Text = ow.list[0].main.temp.ToString("0.##") + " F";
                        label8.Text = ow.list[1].main.temp.ToString("0.##") + " F";
                        label9.Text = "Ветер: " + ow.list[1].wind.speed.ToString() + " миль/час";
                        label6.Text = "Скорость (миль/час): " + ow.list[0].wind.speed.ToString();
                        label10.Text = ow.list[2].main.temp.ToString("0.##") + " F";
                        label11.Text = "Ветер: " + ow.list[2].wind.speed.ToString() + " миль/час";
                        break;
                    }
            }
            label7.Text = "Направление: " + ow.list[0].wind.deg.ToString();
            label4.Text = "Влажность (%): " + ow.list[0].main.humidity.ToString();
            label5.Text = "Давление (мм.рт.ст.): " + ((int)ow.list[0].main.pressure).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Request(url, City, units);
        }

        private void cBCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            City =  cBCity.SelectedItem.ToString();
            Request(url, City, units);
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                if (radioButton.Text == "⁰C") units = "metric";
                if (radioButton.Text == "F") units = "imperial";
                Request(url, City, units);
            }
        }
    }
}
