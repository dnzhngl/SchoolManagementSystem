using SMS.BLL.Abstract;
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
        public InstructorService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public bool DeleteInstructor(int id)
        {
            try
            {
                var deleteInstructor = uow.GetRepository<Instructor>().Get(z => z.Id == id);
                uow.GetRepository<Instructor>().Delete(deleteInstructor);
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
            var instructorList = uow.GetRepository<Instructor>().GetAll().ToList();
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
        }

        public InstructorDTO GetInstructor(int id)
        {
            var selectedInstructor = uow.GetRepository<Instructor>().Get(z => z.Id == id);
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(selectedInstructor);
        }

        public List<InstructorDTO> GetAllInstructorsBasedOnBranch(int branchId)
        {
            var instructorList = uow.GetRepository<Instructor>().GetAll().Where(z => z.BranchId == branchId);
            return MapperFactory.CurrentMapper.Map<List<InstructorDTO>>(instructorList);
        }

        public List<InstructorDTO> GetInstructorName(string name)
        {
            throw new NotImplementedException();
        }

        public InstructorDTO NewInstructor(InstructorDTO instructor)
        {

            var newInstructor = MapperFactory.CurrentMapper.Map<Instructor>(instructor);
            newInstructor.BranchId = instructor.BranchId; //
            newInstructor = uow.GetRepository<Instructor>().Add(newInstructor);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(newInstructor);

        }

        public InstructorDTO UpdateInstructor(InstructorDTO instructor)
        {
            var selectedInstructor = uow.GetRepository<Instructor>().Get(z => z.Id == instructor.Id);
            selectedInstructor = MapperFactory.CurrentMapper.Map<Instructor>(instructor);
            uow.GetRepository<Instructor>().Update(selectedInstructor);
            uow.SaveChanges();
            return MapperFactory.CurrentMapper.Map<InstructorDTO>(selectedInstructor);
        }
    }
}
