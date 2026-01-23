using cityWatch_Project.Data;
using cityWatch_Project.DTOs.Auth;
using cityWatch_Project.DTOs.Users;
using cityWatch_Project.Enums;
using cityWatch_Project.Helpers;
using cityWatch_Project.Models;
using cityWatch_Project.Repositories.Implementations;
using cityWatch_Project.Repositories.Interfaces;
using cityWatch_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cityWatch_Project.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly MainDBContext _dbConetext;
        private readonly UserRepository _userRepo;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenrepo;

        public AuthService(MainDBContext dbConetext, IRefreshTokenRepository refreshTokenRepository ,UserRepository repo, PasswordHasher passwordHasher, JwtTokenGenerator jwtTokenGenerator)
        {
            _dbConetext = dbConetext;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepo = repo;
            _refreshTokenrepo = refreshTokenRepository;
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
                    ErrorMessage = "Cannot find the user",
                    RefreshToken = ""

                };
            }

            if (!_passwordHasher.verify(user, logingDto.Password)) 
            {
                return new LoginServiceResponse
                {
                    Token = string.Empty,
                    Error = true,
                    ErrorMessage = "Wrong credentials",
                    RefreshToken = ""
                    
                };
            }

            await _refreshTokenrepo.DeleteRefreshTokenByUserID(user.UserID);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = _jwtTokenGenerator.GenerateRefreshToken(),
                UserID = user.UserID,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenrepo.AddAsync(newRefreshToken);


            return new LoginServiceResponse
            {
                Token = _jwtTokenGenerator.TokenGenerator(user),
                Error = false,
                ErrorMessage = "",
                RefreshToken = newRefreshToken.Token
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

            //Additional: create the refresh token and save it in the database

            var refreshToken = new RefreshToken
            {
                Token = _jwtTokenGenerator.GenerateRefreshToken(),
                UserID = newUser.UserID,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                Id = Guid.NewGuid()
            };

            await _refreshTokenrepo.AddAsync(refreshToken);

            //6. return the relvant login response
            return new LoginServiceResponse
            {
                Token = _jwtTokenGenerator.TokenGenerator(newUser),
                Error = false,
                ErrorMessage = "",
                RefreshToken = refreshToken.Token
            };

        }

        public async Task<RefreshTokenDto> RefreshTokenHandler(RefreshTokenReqestDto refreshToken)
        {
            RefreshToken exisitngRefreshToken = await _dbConetext.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == refreshToken.RefreshToken);

            if (exisitngRefreshToken is null || exisitngRefreshToken.ExpiresOn < DateTime.UtcNow)
            {
                return null;
            }

            string accessToken = _jwtTokenGenerator.TokenGenerator(exisitngRefreshToken.User!);

            //its a good practice to re makeing the refresh token right after using it 
            exisitngRefreshToken.Token = _jwtTokenGenerator.GenerateRefreshToken();
            exisitngRefreshToken.ExpiresOn = DateTime.UtcNow.AddDays(7);

            //methna AsNoTracking() widiyt aran nathi hinda whenever api _dbContext.SaveChangesasync gahddi badu save wenwa database eke
            await _dbConetext.SaveChangesAsync();

            return new RefreshTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = exisitngRefreshToken.Token
            };
        }
    }
}
