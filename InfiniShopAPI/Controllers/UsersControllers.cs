using API.Data;
using API.Entities;
using InfiniShopAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{


    public class UsersControllers : BaseApiController
    {

        private readonly DataContext _dataContext;
        public UsersControllers(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //We make the methods async to do the request multithread
        //
        // GET PETITONS
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _dataContext.Users.ToListAsync();

        }

        //Get user by id - 
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            //Function to get entity by ID
            var userItem = await _dataContext.Users.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }
            return userItem;
        }


        // POST PETITION
        [HttpPost]
        public async Task<ActionResult<AppUser>> AddUser(AppUser appUser)
        {

            //Add and save
            await _dataContext.Users.AddAsync(appUser);
            await _dataContext.SaveChangesAsync();

            //This returns a StatusCodes.Status201Created response
            return CreatedAtAction(nameof(GetUser), new { id = appUser.Id }, appUser);

        }


        // PUT PETITION
        // With PUT method we have to send again all the properties of the entity
        // If we just want to update an specific property we have to user PATCH method
        [HttpPut]
        public async Task<ActionResult<AppUser>> UpdateUser(AppUser appUser)
        {
            var userItem = await _dataContext.Users.FindAsync(appUser.Id);
            if (userItem == null)
            {
                return NotFound();
            }

            //update and change entity state
            _dataContext.Entry(appUser).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            //Just return a not content after update
            return NoContent();

        }


        [HttpPatch("updateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfile userProfile)
        {

            var currentUser = await _dataContext.Users.FindAsync(userProfile.Id);
            if (currentUser == null)
            {
                return NotFound();
            }

            currentUser.Name = userProfile.Name;
            currentUser.LastName = userProfile.Lastname;
            currentUser.PhoneNumber = userProfile.Phone;

            _dataContext.Users.Update(currentUser);

            await _dataContext.SaveChangesAsync();
            return Ok();

        }

        //PETITION TO DELETE BY ID
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {

            //find first the user
            var userItem = await _dataContext.Users.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            //Remove from DB and save the changes
            _dataContext.Users.Remove(userItem);
            await _dataContext.SaveChangesAsync();

            //Just return a not content after delete
            return NoContent();

        }

        [AllowAnonymous]
        [HttpGet("getUserByEmail/{email}")]
        public async Task<ActionResult<AppUser>> GetUserByEmail(string email)
        {
            //Function to get entity by ID
            var userItem = await _dataContext.Users.Where(u => u.Email == email).SingleAsync();
            if (userItem == null)
            {
                return NotFound();
            }
            return userItem;
        }

    }
}