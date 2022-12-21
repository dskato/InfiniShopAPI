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
        [HttpGet("GetAllBranchCommentsById/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllBranchCommentsById(int id)
        {
            var commentList = _dataContext.Comments.Where(s => s.CommentId == id).ToListAsync();
            if (commentList == null)
            {
                return NotFound();
            }
            return await commentList;

        }

        //Return all Likes, we can sum
        [AllowAnonymous]
        [HttpGet("GetAllBranchLikesById/{id}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetAllBranchLikesById(int id)
        {
            var likeList = _dataContext.Likes.Where(s => s.LikeId == id).ToListAsync();
            if (likeList == null)
            {
                return NotFound();
            }
            return await likeList;

        }


        //Do like
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
        [HttpPost("GetBranchLikesByID/{id}")]
        public async Task<ActionResult<int>> GetBranchLikesByID(int branchId)
        {
            var count = _dataContext.Likes.Where(o => o.BranchId == branchId).Count();
            return count;
        }

            /*
            //Like Controller
            [HttpPost("LikeBranch")]
            public async Task<ActionResult<Boolean>> LikeBranch(LikeDTO likeDTO)
            {

                var isLiked = true;
                var like = _dataContext.Likes.AnyAsync(x => x.AppUserId == likeDTO.AppUserId);
                Console.Write("Like data: "+ like);
                //Validation to check if user exists
                if (await like)
                {
                    isLiked = true;
                    await DeleteLike(likeDTO.BranchId);
                    return BadRequest("Already liked.");
                }
                isLiked = false;
                var likeValue = new Like {
                    numLike = 1
                };

                //load to db
                await _dataContext.Likes.AddAsync(likeValue);
                await _dataContext.SaveChangesAsync();

                return Ok(isLiked);

            }

            [HttpDelete("DeleteLike")]
            public async Task<IActionResult> DeleteLike(int id)
            {

                var likeItem = await _dataContext.Likes.Where(x => x.AppUserId == id).SingleOrDefaultAsync();
                if (likeItem == null)
                {
                    return NotFound();
                }

                //Remove from DB and save the changes
                _dataContext.Likes.Remove(likeItem);
                await _dataContext.SaveChangesAsync();

                //Just return a not content after delete
                return NoContent();

            }
            */


        }
}
