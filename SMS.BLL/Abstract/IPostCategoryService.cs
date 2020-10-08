using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IPostCategoryService : IServiceBase
    {
        List<PostCategoryDTO> GetAll();
        PostCategoryDTO GetPostCategory(int postCategoryId);
        PostCategoryDTO GetPostCategory(string postCategoryName);
        PostCategoryDTO NewPostCategory(PostCategoryDTO postCategory);
        PostCategoryDTO UpdatePostCategory(PostCategoryDTO postCategory);
        bool DeletePostCategory(int postCategoryId);
    }
}
