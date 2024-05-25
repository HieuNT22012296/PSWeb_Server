using PSWeb_Server.Connection;
using PSWeb_Server.DTOs.UserDTOs;
using Microsoft.Data.SqlClient;
using System.Data;
namespace PSWeb_Server.Repository
{
    public class UserRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;
        public UserRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<CreateUserResponseDTO> AddUserAsync(CreateUserDTO user)
        {
            CreateUserResponseDTO createUserResponseDTO = new CreateUserResponseDTO();
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("spAddUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Email", user.Email);
            // Convert password to hash here 
            command.Parameters.AddWithValue("@PasswordHash", user.Password);
            command.Parameters.AddWithValue("@Type", "User");
            command.Parameters.AddWithValue("@Status", "Active");
            command.Parameters.AddWithValue("@Fund", 0);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            var userIdParam = new SqlParameter("@ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(userIdParam);
            command.Parameters.Add(errorMessageParam);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var UserId = (int)userIdParam.Value;
            if (UserId != -1)
            {
                createUserResponseDTO.ID = UserId;
                createUserResponseDTO.IsCreated = true;
                createUserResponseDTO.Message = "User Created Successfully";
                return createUserResponseDTO;
            }
            var message = errorMessageParam.Value?.ToString();
            createUserResponseDTO.IsCreated = false;
            createUserResponseDTO.Message = message ?? "An unknown error occurred while creating the user.";
            return createUserResponseDTO;
        }

        public async Task<List<UserResponseDTO>> ListAllUsersAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("spListAllUsers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var users = new List<UserResponseDTO>();
            while (reader.Read())
            {
                users.Add(new UserResponseDTO
                {
                    ID = reader.GetInt32("ID"),
                    Email = reader.GetString("Email"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Type = reader.GetString("Type"),
                    Status = reader.GetString("Status"),
                    Fund = reader.GetDecimal("Fund"),
                    CreatedOn = reader.GetDateTime("CreatedOn"),
                });
            }
            return users;
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("spGetUserByID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@ID", userId);
            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
            command.Parameters.Add(errorMessageParam);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (!reader.Read())
            {
                return null;
            }
            var user = new UserResponseDTO
            {
                ID = reader.GetInt32("ID"),
                Email = reader.GetString("Email"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Type = reader.GetString("Type"),
                Status = reader.GetString("Status"),
                Fund = reader.GetDecimal("Fund"),
                CreatedOn = reader.GetDateTime("CreatedOn"),
            };
            return user;
        }

        public async Task<UpdateUserResponseDTO> UpdateUserAsync(UpdateUserDTO user)
        {
            UpdateUserResponseDTO updateUserResponseDTO = new UpdateUserResponseDTO()
            {
                userID = user.ID
            };
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("spUpdateUserInformation", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@ID", user.ID);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Fund", user.Fund);
            command.Parameters.AddWithValue("@Status", user.Status);
            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(errorMessageParam);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var message = errorMessageParam.Value?.ToString();
            if (string.IsNullOrEmpty(message))
            {
                updateUserResponseDTO.Message = "User Information Updated.";
                updateUserResponseDTO.IsUpdated = true;
            }
            else
            {
                updateUserResponseDTO.Message = message;
                updateUserResponseDTO.IsUpdated = false;
            }
            return updateUserResponseDTO;
        }

        public async Task<DeleteUserResponseDTO> DeleteUserAsync(int userId)
        {
            DeleteUserResponseDTO deleteUserResponseDTO = new DeleteUserResponseDTO();
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("spDeleteUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@ID", userId);
            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(errorMessageParam);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var message = errorMessageParam.Value?.ToString();
            if (!string.IsNullOrEmpty(message))
            {
                deleteUserResponseDTO.Message = message;
                deleteUserResponseDTO.IsDeleted = false;
            }
            else
            {
                deleteUserResponseDTO.Message = "User Deleted.";
                deleteUserResponseDTO.IsDeleted = true;
            }
            return deleteUserResponseDTO;
        }
        public async Task<LoginUserResponseDTO> LoginUserAsync(LoginUserDTO login)
        {
            LoginUserResponseDTO userLoginResponseDTO = new LoginUserResponseDTO();
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("spLoginUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Email", login.Email);
            command.Parameters.AddWithValue("@PasswordHash", login.Password); // Ensure password is hashed
            var userIdParam = new SqlParameter("@ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(userIdParam);
            command.Parameters.Add(errorMessageParam);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var success = userIdParam.Value != DBNull.Value && (int)userIdParam.Value > 0;
            if (success)
            {
                var userId = Convert.ToInt32(userIdParam.Value);
                userLoginResponseDTO.UserId = userId;
                userLoginResponseDTO.IsLogin = true;
                userLoginResponseDTO.Message = "Login Successful";
                return userLoginResponseDTO;
            }
            var message = errorMessageParam.Value?.ToString();
            userLoginResponseDTO.IsLogin = false;
            userLoginResponseDTO.Message = message;
            return userLoginResponseDTO;
        }
    }
}