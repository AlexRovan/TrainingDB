using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ADOTask
{
    internal class ShopDbClient
    {
        private readonly string _connectionString;

        public ShopDbClient(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException("Не передана строка подключения к БД", nameof(connectionString));
        }

        public int GetCountProducts()
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            const string sql = "SELECT COUNT(*) FROM dbo.Products";

            using var command = new SqlCommand(sql, connection);
            return (int)command.ExecuteScalar();
        }

        private int GetCategoryId(string category)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            const string sql = "SELECT Id FROM dbo.Categories WHERE Name = @category";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@category", category) { SqlDbType = SqlDbType.NVarChar });

            return (int?)command.ExecuteScalar() ?? -1;
        }

        public void AddCategory(string name)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                const string sql = "INSERT INTO dbo.Categories ([Name]) VALUES (@name)";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@name", name) { SqlDbType = SqlDbType.NVarChar });
                command.Transaction = transaction;

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
            }
        }

        public void AddProduct(string name, decimal price, string category)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var categoryId = GetCategoryId(category);

                if (categoryId == -1)
                {
                    throw new ArgumentException($"Неизвестная категория {category}", nameof(category));
                }

                const string sql = "INSERT INTO dbo.Products ([Name], [Price], [Category_id]) VALUES (@name, @price, @categoryId)";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@name", name) { SqlDbType = SqlDbType.NVarChar });
                command.Parameters.Add(new SqlParameter("@price", price) { SqlDbType = SqlDbType.Decimal });
                command.Parameters.Add(new SqlParameter("@categoryId", categoryId) { SqlDbType = SqlDbType.Int });
                command.Transaction = transaction;

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
            }
        }

        public void UpdateProduct(string name, decimal price, string category)
        {
            CheckProductExists(name);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                var categoryId = GetCategoryId(category);

                if (categoryId == -1)
                {
                    throw new ArgumentException($"Неизвестная категория {category}", nameof(category));
                }

                const string sql = "UPDATE [dbo].[Products] SET [Price] = @price,[Category_id] = @categoryId WHERE [Name] = @name";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@name", name) { SqlDbType = SqlDbType.NVarChar });
                command.Parameters.Add(new SqlParameter("@price", price) { SqlDbType = SqlDbType.NVarChar });
                command.Parameters.Add(new SqlParameter("@categoryId", categoryId) { SqlDbType = SqlDbType.Int });
                command.Transaction = transaction;

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
            }
        }

        public void DeleteProduct(string name)
        {
            CheckProductExists(name);

            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            var transaction = connection.BeginTransaction();
            try
            {
                const string sql = "DELETE FROM [dbo].[Products] WHERE Name = @name";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("@name", name) { SqlDbType = SqlDbType.NVarChar });
                command.Transaction = transaction;

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
            }
        }

        public void PrintAllProducts()
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            const string sql = "SELECT p.[Name], " +
                               "p.[Price], " +
                               "c.[Name] Category " +
                               "FROM [BD_Shop].[dbo].[Products] p, " +
                               "[BD_Shop].[dbo].Categories c " +
                               "WHERE p.Category_id = c.Id";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} {reader[1]} {reader[2]}");
            }
        }

        public DataTable GetDataTableProducts()
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            const string sql = "SELECT p.[Name], " +
                               "p.[Price], " +
                               "c.[Name] Category " +
                               "FROM [BD_Shop].[dbo].[Products] p, " +
                               "[BD_Shop].[dbo].Categories c " +
                               "WHERE p.Category_id = c.Id";

            var adapter = new SqlDataAdapter(sql, connection);

            var dataSet = new DataSet();
            adapter.Fill(dataSet);

            return dataSet.Tables[0];
        }

        private void CheckProductExists(string name)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            var sql = $"SELECT COUNT(*) FROM dbo.Products WHERE Name=@name";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@name", name) { SqlDbType = SqlDbType.NVarChar });

            if ((int)command.ExecuteScalar() == 0)
            {
                throw new ArgumentException($"Продукт с именем {name} отсуствует в таблице.", nameof(name));
            }
        }
    }
}