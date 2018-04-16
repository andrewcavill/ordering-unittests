
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Repositories.Interfaces
{
    public interface IEmailRepository
    {
        void SendOrderCommitEmail(Order order);
    }
}
