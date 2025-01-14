namespace WebScraper
{
    public class TokenValueInfo
    {
        public double Value { get; set; }
        public bool Inaccurate { get; set; } //booleans shows if value is too big too fit, therefore is incorrect

        public TokenValueInfo(double value, bool inaccurate)
        {
            Value = value;
            Inaccurate = inaccurate;
        }
    }
}
