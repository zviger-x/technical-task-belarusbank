using Microsoft.EntityFrameworkCore;
using Products.Domain;
using Products.Infrastructure.Contexts;

namespace Products.Infrastructure.Initialization
{
    public sealed class DatabaseInitializer
    {
        private readonly ProductsDbContext _context;

        public DatabaseInitializer(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.MigrateAsync(cancellationToken);

            await SeedDemoData(cancellationToken);
        }

        private async Task SeedDemoData(CancellationToken cancellationToken)
        {
            if (await _context.Categories.AnyAsync(cancellationToken))
                return;

            var food = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Еда"
            };

            var sweets = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Вкусности"
            };

            var water = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Вода"
            };

            var drinks = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Напитки"
            };

            var snacks = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Закуски"
            };

            var categories = new[]
            {
                food,
                sweets,
                water,
                drinks,
                snacks
            };

            await _context.Categories.AddRangeAsync(categories, cancellationToken);

            var products = new[]
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Селедка",
                    CategoryId = food.Id,
                    Description = "Селедка соленая",
                    Price = 10.00m,
                    GeneralNote = "Акция",
                    SpecialNote = "Пересоленая"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Тушенка",
                    CategoryId = food.Id,
                    Description = "Тушенка говяжья",
                    Price = 20.00m,
                    GeneralNote = "Вкусная",
                    SpecialNote = "Жилы"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Колбаса",
                    CategoryId = food.Id,
                    Description = "Колбаса вареная",
                    Price = 25.00m,
                    GeneralNote = "Свежая",
                    SpecialNote = "Охлажденная"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Сало",
                    CategoryId = food.Id,
                    Description = "Сало соленое",
                    Price = 18.00m,
                    GeneralNote = "С чесноком",
                    SpecialNote = "Нарезанное"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Пельмени",
                    CategoryId = food.Id,
                    Description = "Пельмени с мясом",
                    Price = 22.00m,
                    GeneralNote = "Полуфабрикат",
                    SpecialNote = "С говядиной и свининой"
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Сгущенка",
                    CategoryId = sweets.Id,
                    Description = "Молоко сгущенное с сахаром",
                    Price = 30.00m,
                    GeneralNote = "В жестяной банке",
                    SpecialNote = "С ключом"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Шоколад",
                    CategoryId = sweets.Id,
                    Description = "Шоколад молочный",
                    Price = 12.00m,
                    GeneralNote = "С орехами",
                    SpecialNote = "Молочный"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Пряники",
                    CategoryId = sweets.Id,
                    Description = "Пряники с начинкой",
                    Price = 8.00m,
                    GeneralNote = "С глазурью",
                    SpecialNote = "Со сгущенным молоком"
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Квас",
                    CategoryId = water.Id,
                    Description = "Квас хлебный",
                    Price = 15.00m,
                    GeneralNote = "Вятский",
                    SpecialNote = "Газированный"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Вода",
                    CategoryId = water.Id,
                    Description = "Вода минеральная",
                    Price = 7.00m,
                    GeneralNote = "Без газа",
                    SpecialNote = "Питьевая"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Минералка",
                    CategoryId = water.Id,
                    Description = "Вода минеральная газированная",
                    Price = 9.00m,
                    GeneralNote = "С газом",
                    SpecialNote = "Сильногазированная"
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Компот",
                    CategoryId = drinks.Id,
                    Description = "Компот из сухофруктов",
                    Price = 13.00m,
                    GeneralNote = "Без консервантов",
                    SpecialNote = "Без сахара"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Лимонад",
                    CategoryId = drinks.Id,
                    Description = "Лимонад газированный",
                    Price = 16.00m,
                    GeneralNote = "Лимонный",
                    SpecialNote = "Сильногазированный"
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Сухарики",
                    CategoryId = snacks.Id,
                    Description = "Сухарики со вкусом чеснока",
                    Price = 6.00m,
                    GeneralNote = "Пшеничные",
                    SpecialNote = "Со вкусом чеснока"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Чипсы",
                    CategoryId = snacks.Id,
                    Description = "Картофельные чипсы",
                    Price = 14.00m,
                    GeneralNote = "С паприкой",
                    SpecialNote = "Без ароматизаторов"
                }
            };

            await _context.Products.AddRangeAsync(products, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
