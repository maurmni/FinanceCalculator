namespace FinanceCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ФИНАНСОВЫЙ КАЛЬКУЛЯТОР ===");
                Console.WriteLine("1. Расчет кредита");
                Console.WriteLine("2. Конвертер валют");
                Console.WriteLine("3. Калькулятор вкладов");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreditCalculator();
                        break;
                    case "2":
                        ConvertCurrency();
                        break;
                    case "3":
                        DepositCalculator();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод! Нажмите Enter");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void CreditCalculator()
        {
            Console.WriteLine("=== КРЕДИТНЫЙ КАЛЬКУЛЯТОР ===");

            double loanAmount = ReadSum("Введите сумму кредита: ");
            int months = ReadInt("Введите срок кредита (месяцев): ");
            double annualRate = ReadRate("Введите процентную ставку (% годовых): "); 

            var sw = System.Diagnostics.Stopwatch.StartNew();

            double monthlyRate = annualRate / 100 / 12;
            double monthlyPayment = loanAmount *
                                    (monthlyRate * Math.Pow(1 + monthlyRate, months)) /
                                    (Math.Pow(1 + monthlyRate, months) - 1);

            double totalPayment = monthlyPayment * months;
            double overpayment = totalPayment - loanAmount;

            Console.WriteLine($"\nЕжемесячный платеж: {monthlyPayment:F2} руб");
            Console.WriteLine($"Общая сумма выплат: {totalPayment:F2} руб");
            Console.WriteLine($"Переплата по кредиту: {overpayment:F2} руб");

            sw.Stop();
            Console.WriteLine($"Время выполнения: {sw.Elapsed.Milliseconds} миллисекунд");

            Pause();
        }

        static void ConvertCurrency()
        {
            Console.WriteLine("=== КОНВЕРТЕР ВАЛЮТ ===");

            double usdToRub = 90.0;
            double eurToRub = 98.5;
            double eurToUsd = 1.09;
            string from;
            while (true)
            {
                Console.Write("Исходная валюта (RUB, USD, EUR): ");
                from = Console.ReadLine().ToUpper();

                if (from == "RUB" || from == "USD" || from == "EUR")
                    break;

                Console.WriteLine("Ошибка: неизвестная валюта! Повторите ввод");
            }

            string to;

            while (true)
            {
                Console.Write("Целевая валюта (RUB, USD, EUR): ");
                to = Console.ReadLine().ToUpper();

                if (to == "RUB" || to == "USD" || to == "EUR")
                    break;

                Console.WriteLine("Ошибка: неизвестная валюта! Повторите ввод.\n");
            }

            double amount;

            while (true)
            {
                Console.Write("Введите сумму: ");
                if (double.TryParse(Console.ReadLine(), out amount) && amount >= 0)
                    break;

                Console.WriteLine("Ошибка: введите корректное число.\n");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();

            if (from == to)
            {
                Console.WriteLine($"\nРезультат: {amount:F2} {to}");
                Pause();
                return;
            }

            double inRub = from switch
            {
                "RUB" => amount,
                "USD" => amount * usdToRub,
                "EUR" => amount * eurToRub
            };

            double result = to switch
            {
                "RUB" => inRub,
                "USD" => inRub / usdToRub,
                "EUR" => inRub / eurToRub
            };
            Console.WriteLine($"\nРезультат конвертации:");
            Console.WriteLine($"{amount:F2} {from} = {result:F2} {to}");

            sw.Stop();
            Console.WriteLine($"Время выполнения: {sw.Elapsed.Milliseconds} миллисекунд");

            Pause();
        }

        static void DepositCalculator()
        {
            Console.WriteLine("=== КАЛЬКУЛЯТОР ВКЛАДОВ ===");
            double depositAmount = ReadSum("Введите сумму вклада: ");
            int months = ReadInt("Введите срок (месяцев): ");
            double annualRate = ReadRate("Введите процентную ставку (% годовых): ");

            string type;
            while (true)
            {
                Console.Write("Тип вклада (1 - с капитализацией, 2 - без): ");
                type = Console.ReadLine();
                if (type == "1" || type == "2")
                    break;
                Console.WriteLine("Ошибка: выберите 1 или 2!");
            }

            double result, income;

            var sw = System.Diagnostics.Stopwatch.StartNew();

            if (type == "2")
            {
                income = depositAmount * annualRate * months / 12 / 100;
                result = depositAmount + income;
            }
            else
            {
                result = depositAmount * Math.Pow(1 + annualRate / 100 / 12, months);
                income = result - depositAmount;
            }

            Console.WriteLine($"\nДоход по вкладу: {income:F2} руб");
            Console.WriteLine($"Итоговая сумма: {result:F2} руб");

            sw.Stop();
            Console.WriteLine($"Время выполнения: {sw.Elapsed.Milliseconds} миллисекунд");

            Pause();
        }

        static double ReadRate(string message)
        {
            double value;
            while (true)
            {
                Console.Write(message);
                if (double.TryParse(Console.ReadLine(), out value) && value >= 0.1 && value <= 99.9)
                    return value;
                Console.WriteLine("Ошибка! Введите корректное число");
            }
        }

        static double ReadSum(string message)
        {
            double value;
            while (true)
            {
                Console.Write(message);
                if (double.TryParse(Console.ReadLine(), out value) && value >= 1000 && value<=10000000)
                    return value;
                Console.WriteLine("Ошибка! Введите корректное число");
            }
        }

        static int ReadInt(string message)
        {
            int value;
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out value) && value > 0 && value <= 360)
                    return value;
                Console.WriteLine("Ошибка! Введите корректное число");
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nНажмите Enter для продолжения");
            Console.ReadLine();
        }
    }
}
