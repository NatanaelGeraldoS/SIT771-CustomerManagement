using MySql.Data.MySqlClient;
using CustomerManagement.Models;

// dotnet add package MySql.Data


namespace CustomerManagement.Data
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Customer> GetAll()
        {
            var customers = new List<Customer>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            try
            {
                // Select all user data and append the information to the model before it returned.
                var cmd = new MySqlCommand("SELECT * FROM customers", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email")
                    });
                }
                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Error to gather user data", ex.Message);
                return [];
            }
        }

        public bool Add(Customer customer)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            // Begin transaction so if there is any error, we can cancel all transaction that happen after this transaction started
            var trans = conn.BeginTransaction();

            try
            {
                // Insert the new data to the database
                var cmd = new MySqlCommand("INSERT INTO customers (name, email) VALUES (@name, @email)", conn);
                cmd.Parameters.AddWithValue("@name", customer.Name);
                cmd.Parameters.AddWithValue("@email", customer.Email);
                cmd.ExecuteNonQuery();
                // Save the transaction
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Error to Insert new User", ex.Message);
                // If error rollback all transaction
                trans.Rollback();
                return false;
            }
        }

        public bool Update(Customer customer)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            // Begin transaction so if there is any error, we can cancel all transaction that happen after this transaction started
            var trans = conn.BeginTransaction();

            try
            {
                // Update the new data to the database
                var cmd = new MySqlCommand("UPDATE customers SET name = @name, email = @email WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", customer.Id);
                cmd.Parameters.AddWithValue("@name", customer.Name);
                cmd.Parameters.AddWithValue("@email", customer.Email);
                cmd.ExecuteNonQuery();
                // Save the transaction
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Error to Update user information", ex.Message);
                // If error rollback all transaction
                trans.Rollback();
                return false;
            }
        }

        public bool Delete(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            // Begin transaction so if there is any error, we can cancel all transaction that happen after this transaction started
            var trans = conn.BeginTransaction();

            try
            {
                // Delete the data to the database
                var cmd = new MySqlCommand("DELETE FROM customers WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                // Save the transaction
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Error to Delete user", ex.Message);
                // If error rollback all transaction
                trans.Rollback();
                return false;
            }
        }
    }
}
