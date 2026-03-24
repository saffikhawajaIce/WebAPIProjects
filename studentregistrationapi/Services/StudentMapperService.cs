using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace studentregistrationapi;

public class StudentMapperService
{
    //basically i want to use this class and its methods for manual mapping instead of having mapping logic live in the endpoints.
    //then i can call this class's methods in the endpoint instead of defining logic there

    //i have installed AutoMapper and i want to use it for mapping between my student objects and my student response DTOs, and between my create student request DTOs and my student objects, to simplify the mapping process and reduce boilerplate code in the endpoints.
    // I will configure AutoMapper profiles to define the mapping rules between these classes, and then inject the IMapper interface into this StudentMapperService class to perform the actual mapping operations in the methods that I will define here. 
    // This will help keep the mapping logic organized and maintainable, while also leveraging the benefits of AutoMapper for efficient and consistent object mapping throughout the application.

    private readonly AutoMapper.IMapper _mapper;

    public StudentMapperService(AutoMapper.IMapper mapper)
    {
        _mapper = mapper;
    }

    //basic mapping methods to convert between Student and StudentResponseDTO, and between CreateStudentRequestDTO and Student.
    //These methods will be used in the endpoints to map data between the different layers of the application,
    //ensuring a clear separation of concerns and keeping the mapping logic centralized in one place for easier maintenance and readability.


    //this method will take a student object as input and return a student response DTO object with the same properties mapped from the student object, using AutoMapper to perform the mapping based on the configured profiles.
    public StudentResponseDTO MapToStudentResponseDTO(Student student)
    {
        return _mapper.Map<StudentResponseDTO>(student);
    }

    //this method will take a create student request DTO object and a new ID as input, and return a student object with the properties mapped from the create student request DTO object, along with the new ID, the current date for enrollment date, and true for isEnrolled, using AutoMapper to perform the mapping based on the configured profiles.
    public Student MapToStudent(CreateStudentRequestDTO createStudentRequestDTO, int newId)
    {
        var student = _mapper.Map<Student>(createStudentRequestDTO);
        student.Id = newId;
        student.EnrollmentDate = DateTime.Now;
        student.IsEnrolled = true;
        return student;
    }

    //method to map updatestudentrequestDTO to student object, which will be used in the update endpoint to map the incoming data to the existing student object before updating it in the data manager service.
    public void MapToExistingStudent(Student existingStudent, UpdateStudentRequestDTO updateStudentRequestDTO)
    {
        _mapper.Map(updateStudentRequestDTO, existingStudent);
    }


}