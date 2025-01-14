﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Linq;
using System.Data;
using System.Globalization;

namespace CV19Console
{
    public class Progtam
    { 
        private const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
       
        private static async Task<Stream> GetDataStream() //формирует поток данных в байтах
        {
            var client = new HttpClient();
            var response = await client.GetAsync(data_url, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();

        }
        private static  IEnumerable<string> GetDataLines()//разбиывает поток на последовательность строк
        {
            using var data_stream =  GetDataStream().Result;
            using var data_reader = new StreamReader(data_stream);

            while(!data_reader.EndOfStream)
            {
                var line = data_reader.ReadLine();
                if(string.IsNullOrWhiteSpace(line)) continue; 

                yield return line.Replace("Korea,","Korea -");
            }
        }
        private static DateTime[] GetDates()=>GetDataLines() //метод извлекает все даты
            .First()
            .Split(',')
            .Skip(4)
            .Select(s=>DateTime.Parse(s,CultureInfo.InvariantCulture))
            .ToArray();

        private static IEnumerable<(string Contry,string Province, int[] Counts)> GetData()
        {
            var lines = GetDataLines()
                .Skip(1)
                .Select(line => line.Split(','));
            foreach(var row in lines)
            {

                var province = row[0].Trim();
                var country_name = row[1].Trim(' ','"');
                var counts = row.Skip(5).Select(int.Parse).ToArray();
                yield return (country_name, province, counts);
            }    

        }

        
        public static void Main()
        {
            //var web_client = new WebClient();
            
           // var client = new HttpClient();
            // var items = new string[10];
            //var last_item = items[^1];
           // var response= client.GetAsync(data_url).Result;
           // var csv_str = response.Content.ReadAsStringAsync().Result;
          // foreach (var data_line in GetDataLines())
            //{
              //  Console.WriteLine(data_line);
            //}

            //var dates = GetDates(); 
            //Console.WriteLine(string.Join("\r\n",dates));
            var russia_data = GetData().First(v=>v.Contry.Equals("Russia",StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (data, count) => $"{data:dd:MM} - {count}")));
            Console.ReadLine();

            

        }
    }
}
