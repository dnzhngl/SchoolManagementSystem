using SMS.Core.Services;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.BLL.Abstract
{
    public interface ILessonTimeService : IServiceBase
    {
        List<LessonTimeDTO> GetAll();
        LessonTimeDTO GetLessonTime(int id);
        LessonTimeDTO NewLessonTime(LessonTimeDTO lessonTime);
        LessonTimeDTO UpdateLessonTime(LessonTimeDTO lessonTime);
        bool DeleteLessonTime(int id);

    }
}
