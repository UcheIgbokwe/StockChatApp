using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ChatDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public UserService(ChatDbContext dbContext, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.UserName == model.UserName);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new InvalidCredentialsException("Username or password is incorrect");
            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _tokenService.GenerateToken(user);
            return response;
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users;
        }

        public User GetById(int Id)
        {
            return GetUser(Id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_dbContext.Users.Any(x => x.Email == model.Email))
                throw new AppException("Email address already exist");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            //user.PasswordHash = model.Password;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        private User GetUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}