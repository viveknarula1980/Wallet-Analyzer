namespace WebScraper
{
    public class EtherscanTransactionByHashSuccessfulResponse
    {
        public TransactionDetails Result { get; set; }
    }
    public class EtherscanTransactionByHashFailedResponse
    {
        public string Result { get; set; }
    }
}
