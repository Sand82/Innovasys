using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

using Innovasys_App.Models.DTOs;
using Innovasys_App.Models.Views;
using Innovasys_App.Data;
using Innovasys_App.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Innovasys_App.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ApiService apiService;
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IDbConnection dbConnection;

        public UserService(
            ApiService apiService, 
            AppDbContext dbContext, 
            IConfiguration configuration, 
            IDbConnection dbConnection)
        {
            this.apiService = apiService;
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.dbConnection = dbConnection;
        }

        public async Task LoadData()
        {
            if (!this.dbContext.Users.Any())
            {
                var data = await apiService.LoadData();

                await AddData(data);
            }
        }

        public List<UserViewModel> GetData()
        {
            var sql = @"
                 SELECT 
                 u.Id, u.Name, u.NotUsername, u.Email, u.Phone, u.Website, u.Note, u.IsActive, u.CreatedAt,
                 a.Id AS AddressId, a.Street, a.Suite, a.City, a.ZipCode, a.Lat, a.Lng, a.UserId
                 FROM Users u
                 LEFT JOIN Addresses a ON a.UserId = u.Id";

            var userDictionary = new Dictionary<int, UserViewModel>();

            var users = dbConnection.Query<UserViewModel, AddressViewModel, UserViewModel>(
                sql,
                (user, address) =>
                {
                    if (!userDictionary.TryGetValue(user.Id, out var userEntry))
                    {
                        userEntry = user;
                        userDictionary.Add(userEntry.Id, userEntry);
                    }

                    userEntry.Address = address;
                    return userEntry;
                },
                splitOn: "AddressId"
            ).Distinct().ToList();

            return users;
        }

        public async Task<(bool Success, string Message)> EditData(List<UserViewModel> model)
        {
            TruncateDb();
            try
            {
                foreach (var user in model)
                {
                    var currUser = new User
                    {
                        Name = user.Name,
                        NotUsername = user.NotUsername,
                        Phone = user.Phone,
                        Email = user.Email,
                        Website = user.Website,
                        CreatedAt = DateTime.UtcNow,
                        Note = user.Note,
                        IsActive = user.IsActive,
                    };

                    var currAddress = new Address
                    {
                        Street = user.Address!.Street,
                        Suite = user.Address.Suite,
                        City = user.Address.City,
                        ZipCode = user.Address.ZipCode,
                        Lat = user.Address.Lat,
                        Lng = user.Address.Lng,
                    };

                    await AddToDB(currUser, currAddress);
                }
                return (true, "Data saved successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }

            
        }
        private async Task<(bool Success, string Message)> AddData(List<UserDTO> data)
        {
            try
            {
                foreach (var user in data)
                {
                    var currUser = new User
                    {
                        Name = user.Name,
                        NotUsername = user.Username,
                        Phone = user.Phone,
                        Email = user.Email,
                        Website = user.Website,
                        CreatedAt = DateTime.UtcNow,
                        Note = "",
                        IsActive = true,
                    };

                    var currAddress = new Address
                    {
                        Street = user.Address!.Street,
                        Suite = user.Address.Suite,
                        City = user.Address.City,
                        ZipCode = user.Address.ZipCode,
                        Lat = double.Parse(user.Address.Geo!.Lat!),
                        Lng = double.Parse(user.Address.Geo.Lng!),
                    };

                    await AddToDB(currUser, currAddress);
                }
                return (true, "Data saved successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }


        private async Task AddToDB(User currUser, Address currAddress)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertUserQuery = @"
                            INSERT INTO Users (Name, NotUsername, Phone, Email, Website, CreatedAt, Note, IsActive)
                            VALUES (@Name, @NotUsername, @Phone, @Email, @Website, @CreatedAt, @Note, @IsActive);
                            SELECT CAST(SCOPE_IDENTITY() as int);
                        ";

                        var userId = await connection.ExecuteScalarAsync<int>(insertUserQuery, currUser, transaction);

                        var insertAddressQuery = @"
                            INSERT INTO Addresses (Street, Suite, City, ZipCode, Lat, Lng, UserId)
                            VALUES (@Street, @Suite, @City, @ZipCode, @Lat, @Lng, @UserId);
                        ";

                        currAddress.UserId = userId;

                        await connection.ExecuteAsync(insertAddressQuery, currAddress, transaction);

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                       
                        throw new Exception("Error while saving data to the database.", ex);
                    }
                }
            }
        }

        private void TruncateDb()
        {
            using (var connection = new SqlConnection(dbConnection.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute("DELETE FROM Addresses", transaction: transaction);

                    connection.Execute("DELETE FROM Users", transaction: transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
