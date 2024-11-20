using System.Text;

namespace HW_ASP_4._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.MapRazorPages();

            app.Run();
        }
    }

    // User model
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    // Repository Interface
    public interface IUserRepository
    {
        void Add(User user);
        void Remove(int id);
        User? GetById(int id);
        IEnumerable<User> GetAll();
        void Update(User user);
    }

    // Repository implementation
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private int _currentId = 1;

        public void Add(User user)
        {
            user.Id = _currentId++;
            _users.Add(user);
        }

        public void Remove(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null) _users.Remove(user);
            else throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public IEnumerable<User> GetAll() => _users;

        public void Update(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
            }
            else throw new KeyNotFoundException($"User with ID {user.Id} not found.");
        }
    }
}
