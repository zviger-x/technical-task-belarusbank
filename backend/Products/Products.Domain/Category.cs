using Shared.Entities;

namespace Products.Domain
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
