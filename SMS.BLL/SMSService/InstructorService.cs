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

        public InstructorService(IUnitOfWork _uow)
        {
            uow = _uow;
            instructorRepo = uow.GetRepository<Instructor>();
            branchRepo = uow.GetRepository<Branch>();
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
            var instructorList = instructorRepo.GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
        }

        public InstructorDTO GetInstructor(int id)
        {
            var selectedInstructor = instructorRepo.Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(selectedInstructor);
        }

        public List<InstructorDTO> GetAllInstructorsBasedOnBranch(int branchId)
        {
            var instructorList = instructorRepo.GetAll().Where(z => z.BranchId == branchId);
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

            var newInstructor = MapperFactory.CurrentMapper.Map<Instructor>(instructor);
            newInstructor.BranchId = instructor.BranchId; //
            newInstructor = instructorRepo.Add(newInstructor);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(newInstructor);

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
            var instructor = instructorRepo.Get(z => z.FirstName + ' ' + z.LastName == fullName);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(instructor);
        }

        public List<InstructorDTO> GetInstructorNameWithBranch()
        {
            var instructorList = instructorRepo.GetAll().Include( z => z.Branch);
            //foreach (var instructor in instructorList)
            //{
            //   instructor.Branch = branchRepo.Get(z => z.Id == instructor.BranchId);
            //}
            instructorList.Select(z => new { FullNameBranch = z.FirstName + ' ' + z.LastName + '/' + z.Branch.BranchName });
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
        }
    }
}
