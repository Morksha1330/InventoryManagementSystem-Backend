using Azure;
using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class HomeController : ControllerBase

    {

        private readonly ApplicationDbContext _database;

        public HomeController(ApplicationDbContext database)
        {
            _database = database;
        }


        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return Ok();
        }

        [HttpGet("Users")]
        public IActionResult GetUserList()                  // FETCHING ALL USERS
        {
            var result = _database.Users.ToList();
            return Ok(result);
        }

        [HttpPost("User")]
        public IActionResult GetUser([FromBody] customUser userr)           // FETCHING USER BY USERNAME OR PASSWORD
        {
            var user = _database.Users.FirstOrDefault(x => x.Username == userr.Username || x.EPF_No == userr.Username);
            return Ok(user);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] RegisterDto userData)        //  UPDATING USER DETAILS BY USER HIMSELF
        {
            var response = new HttpResponseData<object>();

            var user = _database.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return NotFound(response);
            }

            // Update fields
            user.Username = userData.Username=="string"? user.Username: userData.Username;
            user.Name = userData.Name == "string" ? user.Name: userData.Name;
            user.Email = userData.Email == "string" ? user.Email: userData.Email;

            _database.Update(user);
            _database.SaveChanges();

            response.Success = true;
            response.Message = "User updated successfully";
            response.Result = user;

            return Ok(response);
        }

        [HttpGet("ProductList")]                    
        public IActionResult GetProducts()                  // full product list eka fetch kranna
        {
            var user = _database.Products.ToList();
            return Ok(user);
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromBody] ProductDTO product)          //   STORING PRODUCT NEW ONE
        {
            var response = new HttpResponseData<object>();
            var userId = Convert.ToInt32(User.FindFirst("id")?.Value);

            var productExist = _database.Products.FirstOrDefault(x => x.ProductName == product.ProductName);
            var lastProduct = _database.Products.OrderByDescending(x => x.Id).FirstOrDefault();
            if (productExist == null)
            {
                var addProduct = new Product()
                {

                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    SKU = product.SKU,
                    UnitPrice = product.UnitPrice,
                    Active = product.Active,
                    CreatedDate = DateTime.Now,
                    CreatedUser = userId
                };
                _database.Add(addProduct);
                _database.SaveChanges();

                response.Success = true;
                response.Message = "Product Insert Success!!";
                response.ResponsCode = 200;

            }
                response.Success = false;
                response.Message = "Product Insert Error.Please contact Admin!!";
                response.ResponsCode = 500;

            return Ok(response);

        }

        [HttpPost("CustomProduct")]                                 
        public IActionResult CustomProduct([FromBody] CustomProduct product)                        // product ekak search karanna
        {
            ////var userlist = _database.Users.Select(u => u.Username == user.Username || u.Email == user.Username);
            var customProduct = (from a in _database.Products
                     join b in _database.Categories on a.CategoryId equals b.Id
                     where (a.SKU == product.ProductCode || a.ProductName == product.ProName)
                     select new ProductDTO
                     {
                         ProductName = a.ProductName,
                         CategoryName = b.CategoryName,
                         SKU = a.SKU,
                         UnitPrice = a.UnitPrice,
                         Active = a.Active,
                         CategoryId = a.CategoryId,
                         Id = a.Id

                     }).ToList();
            return Ok(customProduct);

        }

        [HttpGet("CategoryList")]
        public IActionResult GetCategory()                  //   FETCHING ALL CATEGORIES
        {
            var user = _database.Categories.ToList();
            return Ok(user);
        }

        [HttpPost("AddCategory")]           
        public IActionResult AddCategory([FromBody] Category category)                  // category add karanna method eka
        {
            var response = new HttpResponseData<object>();
            var userId = Convert.ToInt32(User.FindFirst("id")?.Value);

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                return Ok(string.Empty);
            // split the sentence in to array
            var words = category.CategoryName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //generate the catCode
            var code = string.Concat(words.Select(w => char.ToUpper(w[0])));                                        
            // catetgory exsist
            var CatExist = _database.Categories.FirstOrDefault(x => x.CategoryCode == category.CategoryCode || x.CategoryName == category.CategoryName || x.CategoryCode == code);
            
            if(CatExist == null)
            {
                var Category = new Category()
                {
                    CategoryName = category.CategoryName,
                    CategoryCode = code,
                    Description = category.Description,
                    Active = category.Active
                };
                _database.Add(Category);
                _database.SaveChanges();

                response.Success = true;
                response.Message = "Category Insert Success!!";
                response.ResponsCode = 200;
            }
            response.Success = false;
            response.Message = "Category Insert Error.!!";
            response.ResponsCode = 500;
            
            return Ok(response);

        }               

        [HttpPost("CustomCategory")]   
        public IActionResult CustomCategory([FromBody] Category category)               // category searching part eka
        {
            var customCategory = (from a in _database.Categories 
                                 where ( a.CategoryCode == category.CategoryCode)
                                 select new Category
                                 {
                                     CategoryCode = a.CategoryCode,
                                     CategoryName = a.CategoryName,
                                     Description = a.Description,
                                     Active = a.Active,
                                     Id = a.Id

                                 }).ToList();
            return Ok(customCategory);

        }

    }
}
