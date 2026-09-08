namespace Products.Application.Contracts
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
    }
}
