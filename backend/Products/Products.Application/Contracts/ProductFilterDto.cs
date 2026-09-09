namespace Products.Application.Contracts
{
    public class ProductFilterDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
