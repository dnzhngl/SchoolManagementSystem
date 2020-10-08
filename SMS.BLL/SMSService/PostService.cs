using SMS.BLL.Abstract;
using SMS.Core.Data.Repositories;
using SMS.Core.Data.UnitOfWork;
using SMS.DTO;
using SMS.Mapping.ConfigProfile;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.BLL.SMSService
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Post> postRepo;
        public PostService(IUnitOfWork _uow)
        {
            uow = _uow;
            postRepo = uow.GetRepository<Post>();
        }
        public bool DeletePost(int postId)
        {
            try
            {
                var selectedPost = postRepo.Get(z => z.Id == postId);
                postRepo.Delete(selectedPost);
                uow.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<PostDTO> GetAll()
        {
            var postList = postRepo.GetIncludesList(null, z => z.User, z => z.PostCategory).ToList();
            return MapperFactory.CurrentMapper.Map<List<PostDTO>>(postList);
        }
        /// <summary>
        /// It gives list of posts based on a given post category
        /// </summary>
        /// <param name="postCategory"></param>
        /// <returns></returns>
        public List<PostDTO> GetAll(string postCategory)
        {
            //var postList = postRepo.GetAll().Where(z => z.PostCategory.CategoryName == postCategory).ToList();
            var postList = postRepo.GetIncludesList(z => z.PostCategory.CategoryName == postCategory, z => z.PostCategory, z => z.User).ToList();
            return MapperFactory.CurrentMapper.Map<List<PostDTO>>(postList);
        }

        public PostDTO GetPost(int postId)
        {
            var selectedPost = postRepo.GetIncludes(z => z.Id == postId, z => z.PostCategory, z => z.User);
            return MapperFactory.CurrentMapper.Map<PostDTO>(selectedPost);

        }

        public PostDTO NewPost(PostDTO post)
        {
            if (!postRepo.GetAll().Any(z => z.PostName.ToLower() == post.PostName.ToLower()))
            {
                var newPost = MapperFactory.CurrentMapper.Map<Post>(post);
                postRepo.Add(newPost);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<PostDTO>(newPost);
            }
            else
            {
                return null;
            }
        }

        public PostDTO UpdatePost(PostDTO post)
        {
            var selectedPost = postRepo.Get(z => z.Id == post.Id);
            selectedPost = MapperFactory.CurrentMapper.Map<Post>(post);
            postRepo.Update(selectedPost);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<PostDTO>(selectedPost);
        }
    }
}
