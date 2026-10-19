using EcomRest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EcomRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT ProductId,ProductName,Price ,
                                 Quantity,CreatedDate FROM Products";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    };
                    products.Add(product);
                }
            }

            return Ok(products);
        }



        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            Product product = null;

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT ProductId,ProductName, Price,Quantity,CreatedDate
                                 FROM Products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    };
                }

                if (product == null)
                {
                    return NotFound(new {
                        message = "Product not found"
                    });


                }
                return Ok(product);

            }


        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Products (ProductName, Price,Quantity)
                                 VALUES (@ProductName,@Price,@Quantity)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                con.Open();

                cmd.ExecuteNonQuery();
            }
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id,Product product)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Products 
                      SET ProductName = @ProductName,
                           Price = @Price,
                           Quantity = @Quantity
                           WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query,con);

                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity",product.Quantity);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if(rowsAffected == 0)
                {
                    return NotFound(new
                    {
                        message = "Product not found"
                    });
                }
            }
            return Ok(new
            {
                message = "Product Added Successfully "
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query,con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound(new
                    {
                        message = "Product not Found"
                    });
                }
            }
                return Ok( new
                {
                    message ="Product deleted Successfully"
                });
        }
    }
    
}
