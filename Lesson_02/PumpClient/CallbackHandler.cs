﻿using PumpClient.PumpServiceReference;
using System;

namespace PumpClient
{
    public class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(StatisticsService statistics)
        {
            Console.Clear();
            Console.WriteLine("Обновление по статистике выполнения скрипта");
            Console.WriteLine($"Всего     тактов: {statistics.AllTacts}");
            Console.WriteLine($"Удачных   тактов: {statistics.SucceesTacts}");
            Console.WriteLine($"Ошибочных тактов: {statistics.ErrorTacts}");
        }
    }
}
