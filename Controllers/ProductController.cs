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
                string sqlSelectProducts = "SELECT * FROM Products";
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
                string sqlAddProducts = "INSERT INTO Products (ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued) VALUES (@ProductName,@SupplierID,@CategoryID,@QuantityPerUnit,@UnitPrice,@UnitsInStock,@UnitsOnOrder,@ReorderLevel,@Discontinued)";

                //Burada sorgumuzu parametreleri ile çalıştırıyoruz.
                var result = cnn.Execute(sqlAddProducts, new
                {
                    ProductName = product.ProductName,
                    SupplierID = product.SupplierId,
                    CategoryID = product.CategoryId,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder,
                    ReorderLevel = product.ReorderLevel,
                    Discontinued = product.Discontinued
                });

                return Ok("Ürün Eklendi");
            }
            return BadRequest();
        }
    }
}