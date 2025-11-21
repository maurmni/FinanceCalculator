using Xunit;
using Moq;

namespace FinanceTest
{
    public class UnitTest1
    {
        [Fact]
        public void CreditCalculator_ValidInputs_CalculatesCorrectMonthlyPayment()
        {
            //Arrange
            double loanAmount = 100000;
            int months = 12;
            double annualRate = 12;

            //Act
            double monthlyRate = annualRate / 100 / 12;
            double monthlyPayment = loanAmount *
                                  (monthlyRate * Math.Pow(1 + monthlyRate, months)) /
                                  (Math.Pow(1 + monthlyRate, months) - 1);

            //Assert
            Assert.Equal(8884.88, Math.Round(monthlyPayment, 2));
        }

        [Theory]
        [InlineData(100, "USD", "RUB", 90.0, 9000)]
        [InlineData(1000, "RUB", "USD", 90.0, 11.11)]
        [InlineData(100, "EUR", "RUB", 98.5, 9850)]
        [InlineData(100, "USD", "EUR", 90.0, 98.5, 91.37)]
        public void CurrencyConverter_TheoryTests_ReturnsCorrectValues(
            double amount, string from, string to,
            double usdToRub, double eurToRub, double? expected = null)
        {
            //Arrange & Act
            double result = ConvertCurrencyInternal(amount, from, to, usdToRub, eurToRub);

            //Assert
            if (expected.HasValue)
            {
                Assert.Equal(expected.Value, Math.Round(result, 2));
            }
        }

        [Fact]
        public void DepositCalculator_WithoutCapitalization_CalculatesCorrectIncome()
        {
            //Arrange
            double depositAmount = 100000;
            int months = 12;
            double annualRate = 10;

            //Act
            double income = depositAmount * annualRate * months / 12 / 100;
            double result = depositAmount + income;

            //Assert
            Assert.Equal(10000, income);
            Assert.Equal(110000, result);
        }

        [Fact]
        public void DepositCalculator_WithCapitalization_CalculatesCorrectIncome()
        {
            //Arrange
            double depositAmount = 100000;
            int months = 12;
            double annualRate = 10;

            //Act
            double result = depositAmount * Math.Pow(1 + annualRate / 100 / 12, months);
            double income = result - depositAmount;

            //Assert
            Assert.Equal(10471.31, Math.Round(income, 2));
            Assert.Equal(110471.31, Math.Round(result, 2));
        }
        [Fact]
        public void CreditCalculator_MockConsole_VerifiesOutput()
        {
            //Arrange
            var mockConsole = new Mock<IConsole>();
            var calculator = new FinancialCalculator(mockConsole.Object);
            mockConsole.SetupSequence(c => c.ReadLine())
                .Returns("100000")
                .Returns("12")
                .Returns("12")
                .Returns("");

            //Act
            calculator.CreditCalculator();

            //Assert 
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("≈ÊÂÏÂÒˇ˜Ì˚È ÔÎ‡ÚÂÊ"))), Times.Once);
            mockConsole.Verify(c => c.WriteLine(It.Is<string>(s =>
                s.Contains("8884.88") || s.Contains("8884,88"))), Times.Once);
        }

        private double ConvertCurrencyInternal(double amount, string from, string to, double usdToRub, double eurToRub)
        {
            if (from == to)
                return amount;

            double inRub = from switch
            {
                "RUB" => amount,
                "USD" => amount * usdToRub,
                "EUR" => amount * eurToRub
            };

            return to switch
            {
                "RUB" => inRub,
                "USD" => inRub / usdToRub,
                "EUR" => inRub / eurToRub
            };
        }
    }

    public interface IConsole
    {
        string ReadLine();
        void WriteLine(string value);
        void Write(string value);
        void Clear();
    }

    public class TestConsole : IConsole
    {
        private readonly StringReader _reader;
        private readonly StringWriter _writer;

        public TestConsole(string input)
        {
            _reader = new StringReader(input);
            _writer = new StringWriter();
        }

        public string ReadLine() => _reader.ReadLine();
        public void WriteLine(string value) => _writer.WriteLine(value);
        public void Write(string value) => _writer.Write(value);
        public void Clear() { }

        public string GetOutput() => _writer.ToString();
    }

    public class FinancialCalculator
    {
        private readonly IConsole _console;

        public FinancialCalculator(IConsole console)
        {
            _console = console;
        }

        public void CreditCalculator()
        {
            _console.WriteLine("===  –≈ƒ»“Õ€…  ¿À‹ ”Àﬂ“Œ– ===");
            double loanAmount = double.Parse(_console.ReadLine());
            int months = int.Parse(_console.ReadLine());
            double annualRate = double.Parse(_console.ReadLine());

            double monthlyRate = annualRate / 100 / 12;
            double monthlyPayment = loanAmount *
                                  (monthlyRate * Math.Pow(1 + monthlyRate, months)) /
                                  (Math.Pow(1 + monthlyRate, months) - 1);

            _console.WriteLine($"≈ÊÂÏÂÒˇ˜Ì˚È ÔÎ‡ÚÂÊ: {monthlyPayment:F2} Û·");
        }
    }
}