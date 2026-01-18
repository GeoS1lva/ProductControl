using ProductControl.Domain.Common;

namespace ProductControl.Domain.Entities
{
    public class Product : EntitiyBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int CurrentQuantity { get; private set; }
        public bool Status { get; private set; }

        public ICollection<StockMovement> StockMovement { get; private set; } = new List<StockMovement>();

        private Product(string name, string description, decimal price, int currentQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            CurrentQuantity = currentQuantity;
            Status = true;
            CreatedAt = DateTime.UtcNow;
        }

        public static ResultPattern<Product> Create(string name, string description, decimal price, int currentQuantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ResultPattern<Product>.Failure("Nome do produto inválido");

            if (price < 0)
                return ResultPattern<Product>.Failure("Preço do produto inválido");

            if (currentQuantity <= 0)
                return ResultPattern<Product>.Failure("Quantidade atual do produto inválida");

            return ResultPattern<Product>.Success(new Product(name, description, price, currentQuantity));
        }

        private Product() { }

        public ResultPattern UpdateProfile(string name, string description, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ResultPattern.Failure("Nome do produto inválido");

            if (price < 0)
                return ResultPattern.Failure("Preço do produto inválido");

            Name = name;
            Description = description;
            Price = price;

            return ResultPattern.Success();
        }

        public ResultPattern DeactivateProduct()
        {
            if (Status is false)
                return ResultPattern.Failure("Esse produto já está desativado.");

            Status = false;
            return ResultPattern.Success();

        }

        public ResultPattern ActivateProduct()
        {
            if (Status is true)
                return ResultPattern.Failure("Esse produto já está ativo.");

            Status = true;
            return ResultPattern.Success();
        }

        public ResultPattern AddProduct(int amount, StockMovement stockMovement)
        {
            if (amount == 0)
                return ResultPattern.Failure("A quantidade a ser adicionada não pode ser zero.");

            CurrentQuantity += amount;
            StockMovement.Add(stockMovement);

            return ResultPattern.Success();
        }

        public ResultPattern RemoveProduct(int amount, StockMovement stockMovement)
        {
            if(amount == 0)
                return ResultPattern.Failure("A quantidade a ser removida não pode ser zero.");

            if(amount > CurrentQuantity)
                return ResultPattern.Failure("Quantidade insuficiente em estoque.");

            CurrentQuantity -= amount;
            StockMovement.Add(stockMovement);

            return ResultPattern.Success();
        }
    }
}
