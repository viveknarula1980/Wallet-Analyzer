using System.Threading.Tasks;

namespace WebScraper
{
    public interface IEtherscanApiServices
    {
        Task<TransactionDetails> GetTransactionDetailsAsync(string txnHash);
    }
}