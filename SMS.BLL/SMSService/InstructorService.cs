using Microsoft.EntityFrameworkCore;
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
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork uow;
        private IRepository<Instructor> instructorRepo;
        private IRepository<Branch> branchRepo;
        private IRepository<Role> roleRepo;
        private IRepository<User> userRepo;


        public InstructorService(IUnitOfWork _uow)
        {
            uow = _uow;
            instructorRepo = uow.GetRepository<Instructor>();
            branchRepo = uow.GetRepository<Branch>();
            roleRepo = uow.GetRepository<Role>();
            userRepo = uow.GetRepository<User>();
        }

        public bool DeleteInstructor(int id)
        {
            try
            {
                var deleteInstructor = instructorRepo.Get(z => z.Id == id);
                instructorRepo.Delete(deleteInstructor);
                uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<InstructorDTO> GetAll()
        {
            var instructorList = instructorRepo.GetIncludesList(null, z => z.User);
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
        }

        public InstructorDTO GetInstructor(int id)
        {
            var selectedInstructor = instructorRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(selectedInstructor);
        }

        public List<InstructorDTO> GetAllInstructorsBasedOnBranch(int branchId)
        {
            //var instructorList = instructorRepo.GetAll().Where(z => z.BranchId == branchId);
            var instructorList = instructorRepo.GetIncludesList(z => z.BranchId == branchId, z => z.User);
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
        }

        public List<InstructorDTO> GetInstructorName()
        {
            var instructorNameList = instructorRepo.GetAll();
            // instructorNameList = (IQueryable<Instructor>)uow.GetRepository<Instructor>().GetAll().Select(z => new { FullNames = z.FullName }).ToList();
            instructorNameList.Select(x => new { FullName = x.FirstName + x.LastName }).ToList();
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorNameList);
        }

        public InstructorDTO NewInstructor(InstructorDTO instructor)
        {
            if (!instructorRepo.GetAll().Any(z => z.IdentityNumber == instructor.IdentityNumber))
            {
                var newInstructor = MapperFactory.CurrentMapper.Map<Instructor>(instructor);
                //newInstructor.BranchId = instructor.BranchId; //
                                                              // newInstructor.RoleId = roleRepo.Get(z => z.RoleName.Contains("Öğretmen")).Id;
                newInstructor = instructorRepo.Add(newInstructor);

                uow.SaveChanges();
                return MapperFactory.CurrentMapper.Map<InstructorDTO>(newInstructor);
            }
            else
            {
                return null;
            }
        }


        public InstructorDTO UpdateInstructor(InstructorDTO instructor)
        {
            var selectedInstructor = instructorRepo.Get(z => z.Id == instructor.Id);
            selectedInstructor = MapperFactory.CurrentMapper.Map<Instructor>(instructor);
            instructorRepo.Update(selectedInstructor);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(selectedInstructor);
        }

        public InstructorDTO GetInstructorByName(string fullName)
        {
            Instructor instructor = new Instructor();
            instructor = instructorRepo.Get(z => z.FirstName + ' ' + z.LastName == fullName);
  
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(instructor);
        }

        public List<InstructorDTO> GetInstructorNameWithBranch()
        {
            var instructorList = instructorRepo.GetIncludesList(null, z => z.Branch);
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);

            //var instructorList = instructorRepo.GetAll().ToList();
            //var iList = MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
            //foreach (var instructor in iList)
            //{
            //    var branch = branchRepo.Get(z => z.Id == instructor.BranchId);
            //    instructor.Branch = MapperFactory.CurrentMapper.Map<BranchDTO>(branch);
            //}
            //return iList;
        }

        public List<InstructorDTO> GetInstructorsThatAreFree(int dayId, int lessontimeId)
        {
            var il = instructorRepo.GetIncludesList(null, z => z.Timetables, z=>z.Branch);
            //var il2 = il.Where(z => z.Timetables.Any(x => x.DayId == dayId && x.LessonTimeId == lessontimeId));
            //var final = il.Except(il2);
            var idList = il.Where(z => z.Timetables.Any(x => x.DayId == dayId && x.LessonTimeId == lessontimeId)).Select(x => x.Id);
            var result = il.Where(x => !idList.Contains(x.Id));

            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(result);
        }


        public InstructorDTO GetInstructoreByUserId(int id)
        {
            var instructor = instructorRepo.Get(z => z.UserId == id);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(instructor);
        }

        public InstructorDTO GetInstructorByUsername(string username)
        {
            var user = userRepo.Get(z => z.UserName == username);
            var instructor = instructorRepo.Get(z => z.UserId == user.Id);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(instructor);
        }

        public InstructorDTO GetInstructorByDuty(string duty)
        {
            var instructor = instructorRepo.Get(z => z.Duty == duty);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(instructor);
        }
    }
}
