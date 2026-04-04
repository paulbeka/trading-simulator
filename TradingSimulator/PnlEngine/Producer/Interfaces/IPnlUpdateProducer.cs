using PnlEngine.Models;

namespace PnlEngine.Producer.Interfaces
{
    public interface IPnlUpdateProducer
    {
        Task PublishAsync(PnLUpdate update);
    }
}
