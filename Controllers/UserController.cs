using Microsoft.AspNetCore.Mvc;
using PSWeb_Server.Models;
using PSWeb_Server.DTOs.UserDTOs;
using PSWeb_Server.Repository;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSWeb_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("AddUser")]
        public async Task<APIResponse<CreateUserResponseDTO>> AddUser(CreateUserDTO createUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return new APIResponse<CreateUserResponseDTO>(HttpStatusCode.BadRequest, "Invalid Data in the Requrest Body");
            }
            try
            {
                var response = await _userRepository.AddUserAsync(createUserDTO);
                if (response.IsCreated)
                {
                    return new APIResponse<CreateUserResponseDTO>(response, response.Message);
                }
                return new APIResponse<CreateUserResponseDTO>(HttpStatusCode.BadRequest, response.Message);
            }
            catch (Exception ex)
            {
                return new APIResponse<CreateUserResponseDTO>(HttpStatusCode.InternalServerError, "Registration Failed.", ex.Message);
            }
        }
        [HttpGet("AllUsers")]
        public async Task<APIResponse<List<UserResponseDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.ListAllUsersAsync();
                return new APIResponse<List<UserResponseDTO>>(users, "Retrieved all Users Successfully.");
            }
            catch (Exception ex)
            {
                return new APIResponse<List<UserResponseDTO>>(HttpStatusCode.InternalServerError, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("GetUser/{userId}")]
        public async Task<APIResponse<UserResponseDTO>> GetUserById(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new APIResponse<UserResponseDTO>(HttpStatusCode.NotFound, "User not found.");
                }
                return new APIResponse<UserResponseDTO>(user, "User fetched successfully.");
            }
            catch (Exception ex)
            {
                return new APIResponse<UserResponseDTO>(HttpStatusCode.InternalServerError, "Error fetching user.", ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<APIResponse<UpdateUserResponseDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return new APIResponse<UpdateUserResponseDTO>(HttpStatusCode.BadRequest, "Invalid Request Body");
            }
            if (id != updateUserDTO.ID)
            {
                return new APIResponse<UpdateUserResponseDTO>(HttpStatusCode.BadRequest, "Mismatched User ID.");
            }
            try
            {
                var response = await _userRepository.UpdateUserAsync(updateUserDTO);
                if (response.IsUpdated)
                {
                    return new APIResponse<UpdateUserResponseDTO>(response, response.Message);
                }
                return new APIResponse<UpdateUserResponseDTO>(HttpStatusCode.BadRequest, response.Message);
            }
            catch (Exception ex)
            {
                return new APIResponse<UpdateUserResponseDTO>(HttpStatusCode.InternalServerError, "Update Failed.", ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<APIResponse<DeleteUserResponseDTO>> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return new APIResponse<DeleteUserResponseDTO>(HttpStatusCode.NotFound, "User not found.");
                }
                var response = await _userRepository.DeleteUserAsync(id);
                if (response.IsDeleted)
                {
                    return new APIResponse<DeleteUserResponseDTO>(response, response.Message);
                }
                return new APIResponse<DeleteUserResponseDTO>(HttpStatusCode.BadRequest, response.Message);
            }
            catch (Exception ex)
            {
                return new APIResponse<DeleteUserResponseDTO>(HttpStatusCode.InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<APIResponse<LoginUserResponseDTO>> LoginUser([FromBody] LoginUserDTO loginUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return new APIResponse<LoginUserResponseDTO>(HttpStatusCode.BadRequest, "Invalid Data in the Requrest Body");
            }
            try
            {
                var response = await _userRepository.LoginUserAsync(loginUserDTO);
                if (response.IsLogin)
                {
                    return new APIResponse<LoginUserResponseDTO>(response, response.Message);
                }
                return new APIResponse<LoginUserResponseDTO>(HttpStatusCode.BadRequest, response.Message);
            }
            catch (Exception ex)
            {
                return new APIResponse<LoginUserResponseDTO>(HttpStatusCode.InternalServerError, "Login failed.", ex.Message);
            }
        }
    }
}

