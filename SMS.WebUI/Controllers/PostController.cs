using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.BLL.Abstract;
using SMS.DTO;
using SMS.WebUI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMS.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Öğretmen, Öğrenci, Veli")]
    public class PostController : BaseController
    {
        private readonly IPostService postService;
        private readonly IPostCategoryService postCategoryService;
        public PostController(IPostService _postService, IPostCategoryService _postCategoryService)
        {
            postService = _postService;
            postCategoryService = _postCategoryService;
        }
        public IActionResult PostList(string postCategoryName)
        {
            PostViewModel model = new PostViewModel();
            
            //var postList = new List<PostDTO>();
            if (postCategoryName != null)
            {
                ViewBag.postCategoryName = postCategoryName;
                model.PostDTOs = postService.GetAll(postCategoryName);
            }
            else
            {
                model.PostDTOs = postService.GetAll();
            }
            return View(model);
        }

        public IActionResult PostAdd(string postCategoryName)
        {
            ViewBag.postCategoryName = postCategoryName;
            return View();
        }
        [HttpPost]
        public IActionResult PostAdd(PostViewModel model, string postCategoryName)
        {
            string fileInfo = null;
            if (model.File != null)
            {
                if (model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/jpg" || model.File.ContentType == "image/png" || model.File.ContentType == "image/gif")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
                    var fileStream = new FileStream(Path.Combine(path, model.File.FileName), FileMode.Create);
                    model.File.CopyTo(fileStream);
                    fileInfo = "/Images/";
                }
                else if (model.File.ContentType == "application/doc" || model.File.ContentType == "application/docx" || model.File.ContentType == "application/pdf" || model.File.ContentType == "application/txt")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
                    var fileStream = new FileStream(Path.Combine(path, model.File.FileName), FileMode.Create);
                    model.File.CopyTo(fileStream);
                    fileInfo = "/Files/";
                }
                model.PostDTO.File = fileInfo + model.File.FileName;
            }
            else
            {
                model.PostDTO.File = null;
            }

            var postCategory = postCategoryService.GetPostCategory(postCategoryName);
            model.PostDTO.PostCategoryId = postCategory.Id;
            model.PostDTO.UserId = CurrentUser.Id;

            PostDTO newPost = model.PostDTO;
            postService.NewPost(newPost);
            return RedirectToAction("PostList", new { postCategoryName = postCategoryName });
        }
        public IActionResult PostDelete(int postId)
        {
            var selectedPostType = postService.GetPost(postId).PostCategory.CategoryName;
            postService.DeletePost(postId);
            return RedirectToAction("PostList", new { postCategoryName = selectedPostType });
        }
        public IActionResult PostDetails(int id)
        {
            var selectedPost = postService.GetPost(id);
            return PartialView(selectedPost);
        }
        public IActionResult PostUpdate(int postId)
        {
            PostViewModel model = new PostViewModel();
            model.PostDTO = postService.GetPost(postId);
            ViewBag.postCategoryName = model.PostDTO.PostCategory.CategoryName;

            return View(model);
        }
        [HttpPost]
        public IActionResult PostUpdate(PostViewModel model, string postCategoryName)
        {
            if (model.File != null)
            {
                if (model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/jpg" || model.File.ContentType == "image/png" || model.File.ContentType == "image/gif")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
                    var fileStream = new FileStream(Path.Combine(path, model.File.FileName), FileMode.Create);
                    model.File.CopyTo(fileStream);
                    model.PostDTO.File = "/Images/" + model.File.FileName;
                }
                else if (model.File.ContentType == "application/doc" || model.File.ContentType == "application/docx" || model.File.ContentType == "application/pdf" || model.File.ContentType == "text/plain")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
                    var fileStream = new FileStream(Path.Combine(path, model.File.FileName), FileMode.Create);
                    model.File.CopyTo(fileStream);
                    model.PostDTO.File = "/Files/" + model.File.FileName;
                }
            }
            model.PostDTO.PostCategoryId = postCategoryService.GetPostCategory(postCategoryName).Id;
            postService.UpdatePost(model.PostDTO);
            return RedirectToAction("PostList", new { postCategoryName = postCategoryName });
        }
    }
}
