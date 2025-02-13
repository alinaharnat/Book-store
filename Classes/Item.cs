using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Bookstore.Classes
{
    public class Item
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }

        //
        //
        //
        public string BookTitle {  get; set; }
        public decimal BookPrice { get; set; }

        public static DataTable GetItems(SqlConnection connection)
        {
            string query = @"
           SELECT 
           i.item_id ,
           i.order_id AS 'Номер замовлення', 
           i.book_id ,
           b.book_title AS 'Назва книги', 
           i.quantity AS 'Доступна кількість', 
           i.item_price AS 'Ціна'
           FROM Items i
           JOIN Books b ON i.book_id = b.book_id;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();

                    dataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }
        public static bool InsertItem(int orderId, int bookId, int quantity, SqlConnection connection)
        {
            string getBookDetailsQuery = @"
        SELECT stock_quantity, book_price 
        FROM Books 
        WHERE book_id = @BookId";

            string insertItemQuery = @"
        INSERT INTO Items (order_id, book_id, quantity, item_price)
        VALUES (@OrderId, @BookId, @Quantity, @ItemPrice)";

            using (SqlCommand getBookDetailsCommand = new SqlCommand(getBookDetailsQuery, connection))
            {
                getBookDetailsCommand.Parameters.AddWithValue("@BookId", bookId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlDataReader reader = getBookDetailsCommand.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new Exception("Книга з вказаним ID не знайдена.");
                    }

                    int stockQuantity = reader.GetInt32(0);
                    decimal bookPrice = reader.GetDecimal(1);

                    if (quantity > stockQuantity)
                    {
                        
                       // MessageBox.Show("Недостатня кількість доступних книг для цієї позиції.");
                        return false;
                        
                    }

                    decimal itemPrice = quantity * bookPrice;
                    reader.Close();

                    using (SqlCommand insertItemCommand = new SqlCommand(insertItemQuery, connection))
                    {
                        insertItemCommand.Parameters.AddWithValue("@OrderId", orderId);
                        insertItemCommand.Parameters.AddWithValue("@BookId", bookId);
                        insertItemCommand.Parameters.AddWithValue("@Quantity", quantity);
                        insertItemCommand.Parameters.AddWithValue("@ItemPrice", itemPrice);

                        insertItemCommand.ExecuteNonQuery();
                    }

                    UpdateAvailableQuantityAfterAdding(bookId, quantity, connection);   
                }
            }
            
            UpdateOrderTotalPrice(orderId, connection);
            return true;
        }

        public static void UpdateItem(int itemId, int newQuantity, SqlConnection connection)
        {
            string getItemDetailsQuery = @"
        SELECT i.quantity, b.stock_quantity, b.book_price, i.order_id
        FROM Items i
        JOIN Books b ON i.book_id = b.book_id
        WHERE i.item_id = @ItemId";

            string updateItemQuery = @"
        UPDATE Items 
        SET quantity = @NewQuantity, item_price = @NewItemPrice
        WHERE item_id = @ItemId";

            using (SqlCommand getItemDetailsCommand = new SqlCommand(getItemDetailsQuery, connection))
            {
                getItemDetailsCommand.Parameters.AddWithValue("@ItemId", itemId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlDataReader reader = getItemDetailsCommand.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new Exception("Запис з вказаним ID не знайдено.");
                    }

                    int currentQuantity = reader.GetInt32(0);
                    int availableQuantity = reader.GetInt32(1);
                    decimal bookPrice = reader.GetDecimal(2);
                    int orderId = reader.GetInt32(3);

                    int quantityDifference = newQuantity - currentQuantity;

                    if (availableQuantity - quantityDifference < 0)
                    {
                        MessageBox.Show("Недостатня кількість доступних книг для оновлення позиції.");
                    }

                    reader.Close();

                    decimal newItemPrice = newQuantity * bookPrice;

                    using (SqlCommand updateItemCommand = new SqlCommand(updateItemQuery, connection))
                    {
                        updateItemCommand.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        updateItemCommand.Parameters.AddWithValue("@NewItemPrice", newItemPrice);
                        updateItemCommand.Parameters.AddWithValue("@ItemId", itemId);

                        updateItemCommand.ExecuteNonQuery();
                    }

                    UpdateAvailableQuantityForEdit(itemId, quantityDifference, connection);

                    UpdateOrderTotalPrice(orderId, connection);
                }
            }
        }


        public static void UpdateAvailableQuantityAfterAdding(int bookId, int quantityToSubtract, SqlConnection connection)
        {
            string updateQuantityQuery = @"
    UPDATE Books 
    SET stock_quantity = stock_quantity - @QuantityToSubtract, 
        book_status = CASE 
                        WHEN stock_quantity - @QuantityToSubtract > 0 THEN 'В наявності' 
                        ELSE 'Немає в наявності' 
                    END
    WHERE book_id = @BookId";

            try
            {
                using (SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, connection))
                {
                    updateQuantityCommand.Parameters.AddWithValue("@QuantityToSubtract", quantityToSubtract);
                    updateQuantityCommand.Parameters.AddWithValue("@BookId", bookId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    updateQuantityCommand.ExecuteNonQuery();
                }
            }
            catch (InvalidOperationException ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Open();
                }

                throw new InvalidOperationException("Помилка при оновленні доступної кількості: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public static void UpdateAvailableQuantityAfterDeleting(int bookId, int quantityToSubtract, SqlConnection connection, SqlTransaction transaction)
        {
            string updateQuantityQuery = @"
    UPDATE Books
    SET 
        stock_quantity = stock_quantity - @QuantityToSubtract,
        book_status = CASE 
                        WHEN stock_quantity - @QuantityToSubtract > 0 THEN 'В наявності'
                        ELSE 'Немає в наявності'
                    END
    WHERE book_id = @BookId";

            try
            {
                using (SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, connection, transaction))
                {
                    updateQuantityCommand.Parameters.AddWithValue("@QuantityToSubtract", quantityToSubtract);
                    updateQuantityCommand.Parameters.AddWithValue("@BookId", bookId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    updateQuantityCommand.ExecuteNonQuery();
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Помилка при оновленні доступної кількості: " + ex.Message, ex);
            }
        }



        private static void UpdateAvailableQuantityForEdit(int itemId, int quantityDifference, SqlConnection connection)
        {
            string updateQuantityQuery = @"
UPDATE Books 
SET stock_quantity = stock_quantity - @QuantityDifference, 
    book_status = CASE WHEN stock_quantity - @QuantityDifference > 0 THEN 'В наявності' ELSE 'Немає в наявності' END
WHERE book_id = (SELECT book_id FROM Items WHERE item_id = @ItemId)";

            using (SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, connection))
            {
                updateQuantityCommand.Parameters.AddWithValue("@QuantityDifference", quantityDifference);
                updateQuantityCommand.Parameters.AddWithValue("@ItemId", itemId);

                updateQuantityCommand.ExecuteNonQuery();
            }
        }

        //public static void DeleteItem(int itemId, string connectionString)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        string getItemInfoQuery = @"
        //SELECT i.order_id, i.book_id, i.quantity, o.order_status
        //FROM Items i
        //INNER JOIN Orders o ON i.order_id = o.order_id
        //WHERE i.item_id = @ItemId";

        //        int orderId = 0;
        //        int bookId = 0;
        //        int quantity = 0;
        //        string orderStatus = null;

        //        using (SqlCommand getItemInfoCommand = new SqlCommand(getItemInfoQuery, connection))
        //        {
        //            getItemInfoCommand.Parameters.AddWithValue("@ItemId", itemId);

        //            using (SqlDataReader reader = getItemInfoCommand.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    orderId = reader.GetInt32(0);
        //                    bookId = reader.GetInt32(1);
        //                    quantity = reader.GetInt32(2);
        //                    orderStatus = reader.GetString(3);
        //                }
        //                else
        //                {
        //                    throw new InvalidOperationException("Позиція не знайдена.");
        //                }
        //            }
        //        }

        //        if (orderStatus == "Нове" || orderStatus == "Прийняте" || orderStatus == "Зібране")
        //        {
        //            using (SqlTransaction transaction = connection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    string deleteItemQuery = "DELETE FROM Items WHERE item_id = @ItemId";

        //                    using (SqlCommand deleteCommand = new SqlCommand(deleteItemQuery, connection, transaction))
        //                    {
        //                        deleteCommand.Parameters.AddWithValue("@ItemId", itemId);
        //                        deleteCommand.ExecuteNonQuery();
        //                    }

        //                    string updateBookQuantityQuery = @"
        //            UPDATE Books
        //            SET stock_quantity = stock_quantity + @Quantity,
        //                book_status = CASE WHEN stock_quantity + @Quantity > 0 THEN 'В наявності' ELSE 'Немає в наявності' END
        //            WHERE book_id = @BookId";

        //                    using (SqlCommand updateCommand = new SqlCommand(updateBookQuantityQuery, connection, transaction))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@Quantity", quantity);
        //                        updateCommand.Parameters.AddWithValue("@BookId", bookId);
        //                        updateCommand.ExecuteNonQuery();
        //                    }

        //                    UpdateOrderTotalPrice(orderId, connection);

        //                    transaction.Commit();
        //                    Console.WriteLine("Позиція успішно видалена");
        //                }
        //                catch
        //                {
        //                    transaction.Rollback();
        //                    throw new Exception("Сталася помилка при видаленні позиції.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Видалення заборонено для замовлень із поточним статусом.");
        //        }
        //    }
        //}
        public static void DeleteItemById(int itemId, SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            string getItemInfoQuery = @"
    SELECT  i.book_id, i.quantity
    FROM Items i
    WHERE i.item_id = @ItemId";

            int bookId = 0;
            int quantity = 0;

            using (SqlCommand getItemInfoCommand = new SqlCommand(getItemInfoQuery, connection))
            {
                getItemInfoCommand.Parameters.AddWithValue("@ItemId", itemId);

                using (SqlDataReader reader = getItemInfoCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bookId = reader.GetInt32(0);
                        quantity = reader.GetInt32(1);
                    }
                    else
                    {
                        throw new InvalidOperationException("Позиція не знайдена.");
                    }
                }
            }

          
            
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string deleteItemQuery = "DELETE FROM Items WHERE item_id = @ItemId";

                        using (SqlCommand deleteCommand = new SqlCommand(deleteItemQuery, connection, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@ItemId", itemId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        string updateBookQuantityQuery = @"
                UPDATE Books
                SET stock_quantity = stock_quantity + @Quantity,
                    book_status = CASE WHEN stock_quantity + @Quantity > 0 THEN 'В наявності' ELSE 'Немає в наявності' END
                WHERE book_id = @BookId";

                        using (SqlCommand updateCommand = new SqlCommand(updateBookQuantityQuery, connection, transaction))
                        {
                            updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                            updateCommand.Parameters.AddWithValue("@BookId", bookId);
                            updateCommand.ExecuteNonQuery();
                        }


                        transaction.Commit();
                        Console.WriteLine("Позиція успішно видалена");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw new Exception("Сталася помилка при видаленні позиції.");
                    }
                }
            }
           
        

        private static void UpdateOrderTotalPrice(int orderId, SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string calculateTotalPriceQuery = @"
            SELECT COALESCE(SUM(item_price), 0)
            FROM Items
            WHERE order_id = @OrderId";

                    string updateOrderQuery = @"
            UPDATE Orders
            SET total_price = @TotalPrice
            WHERE order_id = @OrderId";

                    decimal totalPrice;
                    using (SqlCommand calculateTotalCommand = new SqlCommand(calculateTotalPriceQuery, connection, transaction))
                    {
                        calculateTotalCommand.Parameters.AddWithValue("@OrderId", orderId);
                        totalPrice = (decimal)(calculateTotalCommand.ExecuteScalar() ?? 0);
                    }

                    using (SqlCommand updateOrderCommand = new SqlCommand(updateOrderQuery, connection, transaction))
                    {
                        updateOrderCommand.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        updateOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                        updateOrderCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new Exception("Сталася помилка при оновленні загальної суми замовлення.");
                }
            }
        }
        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetItems(connection);
            dataGridView.DataSource = dataTable;
        }
        public static DataTable GetOrderItems(SqlConnection connection, int orderId, DataGridView dataGridView)
        {
            string queryString = @"
    SELECT 
        oi.item_id AS ID,
        oi.book_id,
        b.book_title AS 'Назва книги',
        oi.quantity AS Кількість,
        b.book_price AS 'Ціна за одиницю',
        oi.item_price  AS 'Загальна вартість'
    FROM Items oi
    JOIN Books b ON oi.book_id = b.book_id
    WHERE oi.order_id = @OrderId; 
";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    
                 
                   
                    return dataTable;
                }
            }
            
        }
        public static List<int> GetItemIdsForOrder(int orderId, SqlConnection connection)
        {
            List<int> itemIds = new List<int>();

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = "SELECT item_id FROM Items WHERE order_id = @orderId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itemIds.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при отриманні item_id: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return itemIds;
        }
        public static void ProcessItems(List<int> itemIds, SqlConnection connection)
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                foreach (int itemId in itemIds)
                {
                    Item.DeleteItemById(itemId, connection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при обробці елементів: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

    }
}
