using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Google.Apis.Auth;
using InfiniShopAPI.DTOs;
using InfiniShopAPI.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly string _googleClientID;



        public AccountController(DataContext context, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _googleClientID = configuration["AppSettings:GoogleClientID"].ToString();
        }


        [HttpPost("authenticate")]
        public async Task<ActionResult<UserDTO>> Authenticate(SocialAuthDTO request)
        {
            
            
            //Validation to check if user exists
            //Generate UserDTO with email and token
            if (await UserExists(request.email) == true)
            {
                System.Diagnostics.Debug.WriteLine("User exists, returning token.");
                return Ok(new UserDTO
                {

                    Email = request.email,
                    Token = new { AuthToken = TokenService.CreateSocialAuthToken(request.email) }.AuthToken
                });

            }
            else 
            {
                //Create user in DB
                var user = new AppUser
                {
                    Email = request.email.ToLower(),
                    Name = request.firstName,
                    LastName = request.lastName,
                };
                //load to db
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("User saved correctly, generating token...");


                return Ok(new UserDTO
                {

                    Email =  request.email,
                    Token = new { AuthToken = TokenService.CreateSocialAuthToken(request.email) }.AuthToken
                });
            }

            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            //Validation to check if user exists
            if (await UserExists(registerDTO.email) == true)
            {
                return BadRequest("Email is taken.");
            }
            //encrypt the password
            PasswordUtils.CreatePasswordHash(registerDTO.password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new AppUser
            {
                Email = registerDTO.email.ToLower(),
                Name = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            //load to db
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new UserDTO{

                Email = user.Email,
                Token = _tokenService.CreateToken(user)

            });

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {

            //Loads the user if email is found
            //NEVER FORGET THE AWAIT IF IS AN ASYNC
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);

            //validation to check if user exists or email is valid
            if (user == null)
            {
                return Unauthorized("Invalid email.");
            }
            else
            {
                if (user.PasswordHash == null || user.PasswordSalt == null) {
                    return Unauthorized("Email asociated with a social network account.");
                }
                if (!PasswordUtils.VerifyPasswordHash(loginDTO.Password, user.PasswordHash, user.PasswordSalt)) { 
                    return Unauthorized("Invalid password."); 
                }
                
            }
            //If everything ok return user
            return Ok(new UserDTO{

                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
           

        }

        //PETITION TO DELETE BY ID
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(int id)
        {

            //find first the user
            var userItem = await _context.Users.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            //Remove from DB and save the changes
            _context.Users.Remove(userItem);
            await _context.SaveChangesAsync();

            //Just return a not content after delete
            return NoContent();

        }

        //Search if UserExist by its email
        private async Task<bool> UserExists(string email)
        {

            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }


        /*
        //Generate Google payload
        private GoogleJsonWebSignature.Payload  GenerateGooglePayload(string token) {
            
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string>() { _googleClientID };
            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(token, settings).Result;
            

            return payload;

        }
        */
    }
}