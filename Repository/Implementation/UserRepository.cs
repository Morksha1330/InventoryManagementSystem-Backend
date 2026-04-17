using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

       

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            var user =_context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new KeyNotFoundException("User Not Fount");
                   
            return user;
            
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }


    }
}
