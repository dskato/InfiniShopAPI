using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using InfiniShopAPI.DTOs;
using InfiniShopAPI.Entities;
using InfiniShopAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Diagnostics;

namespace InfiniShopAPI.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly DataContext _dataContext;
        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        [HttpGet("GetAllBranchs")]
        public async Task<ActionResult<IEnumerable<BranchMechanics>>> GetAllBranches()
        {
            return await _dataContext.BranchMechanics.ToListAsync();

        }

        [AllowAnonymous]
        [HttpGet("GetAllBranchsFiltered/{serviceName}")]
        public async Task<ActionResult<IEnumerable<BranchMechanics>>> GetAllBranchsFiltered(string serviceName)
        {

            
            var filteredList = _dataContext.BranchMechanics.
                Where(x => x.MechanicServices.Any(y => y.MechanicServicesName.Contains(serviceName))).
                OrderByDescending(bm => bm.MechanicServices.Where(ms => ms.MechanicServicesName.Contains(serviceName)).Select(ms => ms.Price).FirstOrDefault()).ToList();

            return  filteredList;

        }



        [AllowAnonymous]
        [HttpGet("GetAllBranchCommentsById/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllBranchCommentsById(int id)
        {
            var commentList = _dataContext.Comments.Where(s => s.BranchId == id).ToListAsync();
            if (commentList == null)
            {
                return NotFound();
            }
            return await commentList;

        }

        [AllowAnonymous]
        [HttpGet("GetBranchAdressById/{id}")]
        public async Task<ActionResult<Adress>> GetBranchAdressById(int id)
        {
            var adress = await _dataContext.Adresses.Where(r => r.BranchMechanicsId == id).FirstOrDefaultAsync();
            if (adress == null)
            {
                return NotFound();
            }

            return adress;

        }

        [AllowAnonymous]
        [HttpGet("GetAllBranchsById/{id}")]
        public async Task<ActionResult<IEnumerable<BranchMechanics>>> GetAllBranchsById(int id)
        {
            var branchList = _dataContext.BranchMechanics.Where(s => s.AppUserId == id).ToListAsync();
            if (branchList == null)
            {
                return NotFound();
            }
            return await branchList;

        }

        [AllowAnonymous]
        [HttpGet("GetAllMechanicalServicesById/{id}")]
        public async Task<ActionResult<IEnumerable<MechanicServices>>> GetAllMechanicalServicesById(int id)
        {
            var serviceList = _dataContext.MechanicServices.Where(s => s.BranchMechanicsId == id).ToListAsync();
            if (serviceList == null)
            {
                return NotFound();
            }
            return await serviceList;
        }


        [AllowAnonymous]
        [HttpPost("CommentBranch")]
        public async Task<ActionResult<IEnumerable<Comment>>> CommentBranch(CommentDTO commentDTO)
        {
            var comment = new Comment
            {
                BranchId = commentDTO.BranchId,
                AppUserId = commentDTO.AppUserId,
                Commentary = commentDTO.Comment
            };

            await _dataContext.Comments.AddAsync(comment);
            await _dataContext.SaveChangesAsync();
            return Ok(comment);

        }

        //PETITION TO DELETE BY ID
        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {

            //find first the user
            var deleteItem = await _dataContext.Comments.FindAsync(id);
            if (deleteItem == null)
            {
                return NotFound();
            }

            //Remove from DB and save the changes
            _dataContext.Comments.Remove(deleteItem);
            await _dataContext.SaveChangesAsync();

            //Just return a not content after delete
            return Ok();

        }

        //PETITION TO DELETE BY ID
        [HttpDelete("DeleteBranch/{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {

            var branch = await _dataContext.BranchMechanics.Include(p => p.Adresses).SingleOrDefaultAsync(p => p.BranchMechanicsId == id);
            if (branch == null) {
                return NotFound();
            }
            _dataContext.BranchMechanics.Remove(branch);
            await _dataContext.SaveChangesAsync();

            return Ok();
            
        }

        //Do like
        [AllowAnonymous]
        [HttpPost("LikeBranch")]
        public async Task<ActionResult<Boolean>> LikeBranch(LikeDTO likeDTO)
        {

            var like = _dataContext.Likes.AnyAsync(x => x.AppUserId == likeDTO.AppUserId);
            
            //If exists delete like
            if (await like)
            {

                var likeItem = await _dataContext.Likes.Where(x => x.AppUserId == likeDTO.AppUserId).SingleOrDefaultAsync();
                if (likeItem == null)
                {
                    return NotFound();
                }

                //Remove from DB and save the changes
                _dataContext.Likes.Remove(likeItem);
                await _dataContext.SaveChangesAsync();

                return Ok("Already liked.");
            }

            var likeValue = new Like
            {
                BranchId = likeDTO.BranchId,
                AppUserId = likeDTO.AppUserId
            };

            await _dataContext.Likes.AddAsync(likeValue);
            await _dataContext.SaveChangesAsync();

            return Ok("Liked");
        }

        //Get num of likes by branch
        //Do like
        [AllowAnonymous]
        [HttpGet("GetBranchLikesByID/{id}")]
        public async Task<ActionResult<int>> GetBranchLikesByID(int id)
        {
            var count = _dataContext.Likes.Where(o => o.BranchId == id).Count();
            return count;
        }

        


        }
}
