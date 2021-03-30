using ShopDatabase.Model;
using ShopDatabase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopDatabase
{
    class ShopDbClient
    {
        public static Dictionary<Customer, decimal> GetCustomersWithAllAmountSpent()
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                return uow.GetRepository<ICustomersRepository>().GetCustomersWithAllAmountSpent();
            }    
        }

        public static Dictionary<Category, int> GetProductsCountByCategory()
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                return uow.GetRepository<ICategoryRepository>().GetProductsCountByCategory();
            }
        }

        public static Product GetMostPopularProduct()
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                return uow.GetRepository<IProductRepository>().GetMostPopularProduct();
            }
        }


        public static List<Product> GetProductsList()
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                return uow.GetRepository<IProductRepository>().GetAll().ToList(); ;
            }
        }

        public static List<Customer> GetCustomersList()
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                return uow.GetRepository<ICustomersRepository>().GetAll().ToList(); ;
            }
        }

        public static void AddOrder(DateTime date, Customer customer, List<PositionsOrder> products)
        {
            var order = new Order { Date = date, Customer = customer };
            
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                
                products.Where(p => !productsRepo.IsExist(p.Product))
                .ToList()
                .ForEach(p => throw new ArgumentException($"Продукта {p.Product.Name} нет в БД. Создание заказа невозможно.", nameof(products)));

                var customersRepo = uow.GetRepository<ICustomersRepository>();
                var customerDb = customersRepo.GetCustomer(customer);

                if (Equals(customerDb, null))
                {
                    uow.BeginTransaction();
                    customersRepo.Create(customer);
                }
                else
                {
                    customer = customerDb;
                }

                var ordersRepo = uow.GetRepository<IOrderRepository>();
                ordersRepo.Create(order);
                order.PositionOrder.AddRange(products);
                
                uow.Save();
            }         
        }

        public static void AddProduct(Product product, Category category)
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                var categoriesRepo = uow.GetRepository<ICategoryRepository>();
                var categoryDb = categoriesRepo.GetCategory(category);

                if (Equals(categoryDb, null))
                {
                    uow.BeginTransaction();
                    categoriesRepo.Create(category);
                }
                else
                {
                    category = categoryDb;
                }

                var productsRepo = uow.GetRepository<IProductRepository>();
                productsRepo.Create(product);
                
                uow.Save();
            }
        }

        public static void DeleteProduct(Product product)
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                var productsRepo = uow.GetRepository<IProductRepository>();
                var productDb = productsRepo.GetProduct(product);

                if (productDb == null)
                {
                    throw new ArgumentException($"Продукта {product.Name} нет в БД. Удаление невозможно.", nameof(product));
                }

                productsRepo.Delete(productDb);
                uow.Save();
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            using (var uow = new UnitOfWork(new ShopDbContext()))
            {
                var customersRepo = uow.GetRepository<ICustomersRepository>();
                var customerDb = customersRepo.GetCustomer(customer);

                if (customerDb == null)
                {
                    throw new ArgumentException($"Клиента {customer.FirstName} {customer.MiddleName} {customer.LastName} нет в БД. Редактирование невозможно.", nameof(customer));
                }
                customerDb.Email = customer.Email;
                customerDb.Phone = customer.Phone;

                customersRepo.Update(customerDb);
                uow.Save();
            }
        }     
    }
}
