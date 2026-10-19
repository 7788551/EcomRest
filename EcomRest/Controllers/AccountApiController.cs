using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using EcomRest.Models;

namespace EcomRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(User obj)
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            string query = @"INSERT INTO Users 
                             (Name,Email,Password,RoleId) 
                             VALUES
                             (@Name,@Email,@Password,@RoleId)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {

            }
                return Ok(new
                {
                    success = true,
                    message = "Registration successful!"
                });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login obj)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            string query = @"SELECT UserId,Name,Email,RoleId
                             FROM Users
                             WHERE Email = @Email 
                            AND Password = @Password";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", obj.Email);
                    cmd.Parameters.AddWithValue("@Password", obj.Password);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            HttpContext.Session.SetString(
                                "UserId", dr["UserId"].ToString());

                            HttpContext.Session.SetString(
                                "Name",
                                dr["Name"].ToString());
                            HttpContext.Session.SetString(
                                "RoleId",
                                dr["RoleId"].ToString());

                            int roleId = Convert.ToInt32(dr["RoleId"]);

                            string redirectUrl;

                            if(roleId == 1)
                            {
                                redirectUrl = "/Dashboard/AdminDashboard";
                            }
                            else
                            {
                                redirectUrl = "/Dashboard/UserDashboard";
                            }

                            return Ok(new
                            {
                                success = true,
                                message = "Login Successful!",
                                redirectUrl = redirectUrl
                            });

                        }
                    }
                }
            }

            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password !"
            });
        }

    }
}
