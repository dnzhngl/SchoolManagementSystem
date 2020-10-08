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
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IUnitOfWork uow;
        private IRepository<PostCategory> postCategoryRepo;
        public PostCategoryService(IUnitOfWork _uow)
        {
            uow = _uow;
            postCategoryRepo = uow.GetRepository<PostCategory>();
        }
        public bool DeletePostCategory(int postCategoryId)
        {
            try
            {
                var selectedPostCategory = postCategoryRepo.Get(z => z.Id == postCategoryId);
                postCategoryRepo.Delete(selectedPostCategory);
                uow.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<PostCategoryDTO> GetAll()
        {
            var postCategoryList = postCategoryRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<PostCategoryDTO>>(postCategoryList);
        }

        public PostCategoryDTO GetPostCategory(int postCategoryId)
        {
            var selectedPostCategory = postCategoryRepo.Get(z => z.Id == postCategoryId);
            return MapperFactory.CurrentMapper.Map<PostCategoryDTO>(selectedPostCategory);
        }

        public PostCategoryDTO GetPostCategory(string postCategoryName)
        {
            var selectedPostCategory = postCategoryRepo.Get(z => z.CategoryName == postCategoryName);
            return MapperFactory.CurrentMapper.Map<PostCategoryDTO>(selectedPostCategory);
        }

        public PostCategoryDTO NewPostCategory(PostCategoryDTO postCategory)
        {
            if (!postCategoryRepo.GetAll().Any(z => z.CategoryName.ToLower() == postCategory.CategoryName.ToLower()))
            {
                var newPostCategory = MapperFactory.CurrentMapper.Map<PostCategory>(postCategory);
                postCategoryRepo.Add(newPostCategory);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<PostCategoryDTO>(newPostCategory);
            }
            else
            {
                return null;
            }
        }

        public PostCategoryDTO UpdatePostCategory(PostCategoryDTO postCategory)
        {
            var selectedPostCategory = postCategoryRepo.Get(z => z.Id == postCategory.Id);
            selectedPostCategory = MapperFactory.CurrentMapper.Map<PostCategory>(postCategory);
            postCategoryRepo.Update(selectedPostCategory);
            return MapperFactory.CurrentMapper.Map<PostCategoryDTO>(selectedPostCategory);
        }
    }
}
