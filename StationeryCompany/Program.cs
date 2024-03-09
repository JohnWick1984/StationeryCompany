using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Data Source=EUGENE1984; Initial Catalog=StationeryCompany; Integrated Security=True;";

    static void Main()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Подключение к базе данных успешно.");

                CreateViewsProceduresAndTriggers(connection);

                DisplayAllProducts(connection);
                DisplayProductTypes(connection);
                DisplaySalesManagers(connection);
                DisplayProductsMaxQuantity(connection);
                DisplayProductsMinQuantity(connection);
                DisplayProductsMinCost(connection);
                DisplayProductsMaxCost(connection);
                DisplayProductsOfType(connection, "Writing");
                DisplayProductsSoldByManager(connection, 1);
                DisplayProductsBoughtByCompany(connection, "ABC Company");
                DisplayLatestSaleInformation(connection);
                DisplayAverageQuantityByProductType(connection);

                // Ваш код для выполнения других операций может быть добавлен здесь

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        Console.WriteLine("Нажмите Enter для отключения от базы данных...");
        Console.ReadLine();
    }
    static void CreateViewsProceduresAndTriggers(SqlConnection connection)
    {
        // Создание представления 'ProductsInfo'
        using (SqlCommand command = new SqlCommand("CREATE VIEW ProductsInfo AS SELECT * FROM Products", connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Создано представление 'ProductsInfo'");
        }

        // Создание хранимой процедуры 'GetProductTypes'
        using (SqlCommand command = new SqlCommand("CREATE PROCEDURE GetProductTypes AS SELECT DISTINCT Product_Type FROM Products", connection))
        {
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            Console.WriteLine("Создана хранимая процедура 'GetProductTypes'");
        }

        // Создание триггера 'UpdateQuantityOnSale'
        using (SqlCommand command = new SqlCommand("CREATE TRIGGER UpdateQuantityOnSale ON Sales AFTER INSERT AS BEGIN UPDATE Products SET Quantity = Quantity - (SELECT Quantity_Sold FROM INSERTED) WHERE Product_ID = (SELECT Product_ID FROM INSERTED) END", connection))
        {
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            Console.WriteLine("Создан триггер 'UpdateQuantityOnSale'");
        }
    }

    static void DisplayAllProducts(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT * FROM Products", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Вся информация о канцтоварах:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Product_Name: {reader["Product_Name"]}, Product_Type: {reader["Product_Type"]}, Quantity: {reader["Quantity"]}, Cost_Price: {reader["Cost_Price"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductTypes(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT DISTINCT Product_Type FROM Products", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Все типы канцтоваров:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_Type: {reader["Product_Type"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplaySalesManagers(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT * FROM Managers", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Менеджеры по продажам:");
                while (reader.Read())
                {
                    Console.WriteLine($"Manager_ID: {reader["Manager_ID"]}, FirstName: {reader["FirstName"]}, LastName: {reader["LastName"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsMaxQuantity(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Quantity = (SELECT MAX(Quantity) FROM Products)", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Канцтовары с максимальным количеством единиц:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Product_Name: {reader["Product_Name"]}, Product_Type: {reader["Product_Type"]}, Quantity: {reader["Quantity"]}, Cost_Price: {reader["Cost_Price"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsMinQuantity(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Quantity = (SELECT MIN(Quantity) FROM Products)", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Канцтовары с минимальным количеством единиц:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Product_Name: {reader["Product_Name"]}, Product_Type: {reader["Product_Type"]}, Quantity: {reader["Quantity"]}, Cost_Price: {reader["Cost_Price"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsMinCost(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Cost_Price = (SELECT MIN(Cost_Price) FROM Products)", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Канцтовары с минимальной себестоимостью единицы:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Product_Name: {reader["Product_Name"]}, Product_Type: {reader["Product_Type"]}, Quantity: {reader["Quantity"]}, Cost_Price: {reader["Cost_Price"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsMaxCost(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE Cost_Price = (SELECT MAX(Cost_Price) FROM Products)", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Канцтовары с максимальной себестоимостью единицы:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Product_Name: {reader["Product_Name"]}, Product_Type: {reader["Product_Type"]}, Quantity: {reader["Quantity"]}, Cost_Price: {reader["Cost_Price"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsOfType(SqlConnection connection, string productType)
    {
        using (SqlCommand command = new SqlCommand($"SELECT * FROM Products WHERE Product_Type = '{productType}'", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine($"Канцтовары типа '{productType}':");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Product_Name: {reader["Product_Name"]}, Product_Type: {reader["Product_Type"]}, Quantity: {reader["Quantity"]}, Cost_Price: {reader["Cost_Price"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsSoldByManager(SqlConnection connection, int managerId)
    {
        using (SqlCommand command = new SqlCommand($"SELECT * FROM Sales WHERE Manager_ID = {managerId}", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine($"Канцтовары, проданные менеджером с ID {managerId}:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Quantity_Sold: {reader["Quantity_Sold"]}, Unit_Price: {reader["Unit_Price"]}, Sale_Date: {reader["Sale_Date"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayProductsBoughtByCompany(SqlConnection connection, string buyerCompanyName)
    {
        using (SqlCommand command = new SqlCommand($"SELECT * FROM Sales WHERE Buyer_Company_Name = '{buyerCompanyName}'", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine($"Канцтовары, купленные фирмой '{buyerCompanyName}':");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_ID: {reader["Product_ID"]}, Quantity_Sold: {reader["Quantity_Sold"]}, Unit_Price: {reader["Unit_Price"]}, Sale_Date: {reader["Sale_Date"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayLatestSaleInformation(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Sales ORDER BY Sale_Date DESC", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Информация о самой недавней продаже:");
                while (reader.Read())
                {
                    Console.WriteLine($"Sale_ID: {reader["Sale_ID"]}, Product_ID: {reader["Product_ID"]}, Manager_ID: {reader["Manager_ID"]}, Buyer_Company_Name: {reader["Buyer_Company_Name"]}, Quantity_Sold: {reader["Quantity_Sold"]}, Unit_Price: {reader["Unit_Price"]}, Sale_Date: {reader["Sale_Date"]}");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayAverageQuantityByProductType(SqlConnection connection)
    {
        using (SqlCommand command = new SqlCommand("SELECT Product_Type, AVG(Quantity) AS AverageQuantity FROM Products GROUP BY Product_Type", connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Среднее количество товаров по каждому типу канцтоваров:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product_Type: {reader["Product_Type"]}, AverageQuantity: {reader["AverageQuantity"]}");
                }
            }
            Console.WriteLine();
        }
    }
}
