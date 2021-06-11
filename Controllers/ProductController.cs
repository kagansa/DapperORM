using Dapper;
using DapperORM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperORM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //appsettings içerisindeki connection stringimizi alıyoruz.
        private readonly IConfiguration _configuration;
        private string conn;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = configuration.GetConnectionString("SqlConnection");
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            using (IDbConnection cnn = new SqlConnection(conn))
            {
                string sqlSelectProducts = "SELECT ProductId, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel FROM Products";
                var result = cnn.Query<Product>(sqlSelectProducts).ToList();
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            using (IDbConnection cnn = new SqlConnection(conn))
            {
                DynamicParameters parameter = new DynamicParameters();

                //Parametrelerimizi oluşturuyoruz.
                parameter.Add("@ProductName", product.ProductName, DbType.String);
                parameter.Add("@QuantityPerUnit", product.QuantityPerUnit, DbType.String);
                parameter.Add("@UnitPrice", product.UnitPrice, DbType.Decimal);
                parameter.Add("@UnitsInStock", product.UnitsInStock, DbType.Int32);
                parameter.Add("@UnitsOnOrder", product.UnitsOnOrder, DbType.Int32);
                parameter.Add("@ReorderLevel", product.ReorderLevel, DbType.Int32);

                string sqlAddProducts = "INSERT INTO Products (ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel) VALUES (@ProductName, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel);";

                //Burada sorgumuzu parametreleri ile çalıştırıyoruz.
                var result = cnn.Query<Product>(sqlAddProducts,parameter).ToList();

                return Ok("Ürün Eklendi");
            }
            return BadRequest();
        }
    }
}