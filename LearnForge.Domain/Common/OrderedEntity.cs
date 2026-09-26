using LearnForge.Domain.Exceptions;

namespace LearnForge.Domain.Common;

public class OrderedEntity : Entity
{
    public int Order { get; private set; }

    protected OrderedEntity() { }
    protected OrderedEntity(string title, int order) : base(title) => Order = ValidateOrder(order);

    internal void ChangeOrder(int order) => Order = ValidateOrder(order);

    private int ValidateOrder(int order)
    {
        return order < 1 ? throw new DomainException($"{EntityLabel} order must be greater than zero.") : order;
    }
}