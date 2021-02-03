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
    public class ParentService : IParentService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Parent> parentRepo;
        private IRepository<Role> roleRepo;
        private IRepository<User> userRepo;
        private IRepository<Student> studentRepo;

        public ParentService(IUnitOfWork _uow)
        {
            uow = _uow;
            roleRepo = uow.GetRepository<Role>();
            parentRepo = uow.GetRepository<Parent>();
            userRepo = uow.GetRepository<User>();
            studentRepo = uow.GetRepository<Student>();
        }

        public bool DeleteParent(int id)
        {
            try
            {
                var selectedParent = parentRepo.Get(z => z.Id == id);
                parentRepo.Delete(selectedParent);
                uow.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ParentDTO> GetAll()
        {
            var parentIdList = studentRepo.GetIncludesList(z => z.StudentStatus == "Öğrenci" && z.SectionId != null).Select(z => z.ParentId);

            var parentList = parentRepo.GetIncludesList(null, z => z.User);
            var result = parentList.Where(z => parentIdList.Contains(z.Id));

            //var parentList = parentRepo.GetAll().ToList();
            //var parentList = parentRepo.GetIncludesList(null, z => z.User);
            return MapperFactory.CurrentMapper.Map<List<ParentDTO>>(result);
        }

        public ParentDTO GetParent(int id)
        {
            var selectedParent = parentRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<ParentDTO>(selectedParent);
        }

        public ParentDTO GetParentByUserId(int userId)
        {
            var parent = parentRepo.Get(z => z.UserId == userId);
            return MapperFactory.CurrentMapper.Map<ParentDTO>(parent);

        }

        public ParentDTO GetParentWithStudents(int parentId)
        {
            var parent = parentRepo.GetIncludes(z => z.Id == parentId, z => z.Students);
            return MapperFactory.CurrentMapper.Map<ParentDTO>(parent);
        }

        public ParentDTO NewParent(ParentDTO parent)
        {
            if (!parentRepo.GetAll().Any(z => z.FirstName.ToLower() == parent.FirstName.ToLower() && z.LastName.ToLower() == parent.LastName.ToLower()))
            {
                var newParent = MapperFactory.CurrentMapper.Map<Parent>(parent);
                //  newParent.RoleId = roleRepo.Get(z => z.RoleName.Contains("Veli")).Id;

                parentRepo.Add(newParent);
                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<ParentDTO>(newParent);
            }
            else
            {
                return null;
            }
        }

        public ParentDTO UpdateParent(ParentDTO parent)
        {
            var selectedParent = parentRepo.Get(z => z.Id == parent.Id);
            selectedParent = MapperFactory.CurrentMapper.Map<Parent>(parent);
            parentRepo.Update(selectedParent);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<ParentDTO>(selectedParent);
        }

        public List<ParentDTO> GetInstructorsParents(string instructorUsername)
        {
            var instructorUserId = userRepo.Get(z => z.UserName == instructorUsername).Id;
            var instructor = uow.GetRepository<Instructor>().Get(z => z.UserId == instructorUserId);
            var sectionIdList = uow.GetRepository<Timetable>().GetAll().Where(z => z.InstructorId == instructor.Id).GroupBy(z => z.SectionId).Select(z => z.Key);

            List<Parent> parentList = new List<Parent>();

            foreach (int sectionId in sectionIdList)
            {
                //var parents = uow.GetRepository<Student>().GetIncludesList(z => z.SectionId == sectionId, z => z.Parent).Select(z=> z.Parent);
                var parents = uow.GetRepository<Student>().GetIncludesList(z => z.SectionId == sectionId, z => z.Parent, z=>z.Parent.User).Select(z=> z.Parent);
                parentList.AddRange(parents);
            }

            return MapperFactory.CurrentMapper.Map<List<ParentDTO>>(parentList);

                //"Select Parents.FirstName + ' ' + Parents.LastName as [Ad-Soyad], Parents.Gender, Parents.CellPhone, Parents.HomePhone,  Parents.WorkPhone, Parents.Address, Instructors.FirstName, Instructors.LastName from Parents inner join Students on Parents.Id = Students.ParentId inner join Sections on Students.SectionId = Sections.Id inner join Timetables on Sections.Id = Timetables.SectionId inner join Instructors on Instructors.Id = Timetables.InstructorId Group by Parents.FirstName, Parents.LastName, Parents.Gender, Parents.CellPhone, Parents.HomePhone, Parents.WorkPhone, Parents.Address, Instructors.FirstName, Instructors.LastName"
        }

        public ParentDTO GetParent(string identityNo, string firstname, string lastname)
        {
            var parent = parentRepo.Get(z => z.IdentityNumber == identityNo && z.FirstName == firstname && z.LastName == lastname);
            return MapperFactory.CurrentMapper.Map<ParentDTO>(parent);
        }
    }
}
