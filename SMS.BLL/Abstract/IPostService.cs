using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface IPostService : IServiceBase
    {
        List<PostDTO> GetAll();
        List<PostDTO> GetAll(string postCategory);
        PostDTO GetPost(int postId);
        PostDTO NewPost(PostDTO post);
        PostDTO UpdatePost(PostDTO post);
        bool DeletePost(int postId);
    }
}
