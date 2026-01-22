using cityWatch_Project.Data;
using cityWatch_Project.DTOs.Auth;
using cityWatch_Project.DTOs.Users;
using cityWatch_Project.Enums;
using cityWatch_Project.Helpers;
using cityWatch_Project.Models;
using cityWatch_Project.Repositories.Implementations;
using cityWatch_Project.Services.Interfaces;

namespace cityWatch_Project.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly MainDBContext _dbConetext;
        private readonly UserRepository _userRepo;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(MainDBContext dbConetext, UserRepository repo, PasswordHasher passwordHasher, JwtTokenGenerator jwtTokenGenerator)
        {
            _dbConetext = dbConetext;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepo = repo;
        }

        public async Task<LoginServiceResponse> LoginAsync(LoginDto logingDto)
        {
            var user = await _userRepo.FindUserByEmail(logingDto.Email);
            if (user == null)
            {
                return new LoginServiceResponse
                {
                    Token = string.Empty,
                    Error = true,
                    ErrorMessage = "Cannot find the user"

                };
            }

            if (!_passwordHasher.verify(user, logingDto.Password)) 
            {
                return new LoginServiceResponse
                {
                    Token = string.Empty,
                    Error = true,
                    ErrorMessage = "Wrong credentials"
                    
                };
            }

            return new LoginServiceResponse
            {
                Token = _jwtTokenGenerator.TokenGenerator(user),
                Error = false,
                ErrorMessage = ""
            };
        }

        public async Task<LoginServiceResponse> RegisterAsync(RegisterDTO registerDto, Role roleType)
        {
            //1. first check whether the user exists or not
            var user = await _userRepo.FindUserByEmail(registerDto.Email!);
            if (user != null)
            {
                return new LoginServiceResponse
                {
                    Token = string.Empty,
                };
            }

            //2. create the new user 
            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                //Role = Enums.Role.Citizen,
                District = registerDto.District,
                Province = registerDto.Province,
            };

            //3. hash the users password
            newUser.PasswordHash = _passwordHasher.Hash(newUser, registerDto.Password!);

            //4. check what type of user we need to create and acording to that set the user role
            switch (roleType)
            {
                case Enums.Role.Admin:
                    newUser.Role = Enums.Role.Admin;
                    break;

                case Enums.Role.Citizen:
                    newUser.Role = Enums.Role.Citizen;
                    break;

                case Enums.Role.Worker:
                    newUser.Role = Enums.Role.Worker;
                    break;
            }

            //5. save the user in the database
            await _userRepo.AddUserAsync(newUser);

            //6. return the relvant login response
            return new LoginServiceResponse
            {
                Token = _jwtTokenGenerator.TokenGenerator(newUser),
            };

        }
    }
}
