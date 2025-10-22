﻿using ContosoUniversity.Models;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;


namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context, string xmlPath = @"C:\Users\tajsm\OneDrive\Documents\ASPProjects\ContosoUniversity\ContosoUniversity\Data\seed.xml")
        {

            if (context.Students.Any()) return;   // DB has been seeded

            SeedDto seed = LoadSeed(xmlPath);

            Dictionary<string, Instructor> instructorDict = seed.Instructors.ToDictionary(
                x => x.Id,
                x => new Instructor
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate
                });
            Dictionary<string, Student> studentDict = seed.Students.ToDictionary(
                x => x.Id,
                x => new Student
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EnrollmentDate = x.EnrollmentDate
                });
            Dictionary<string, Department> departmentDict = seed.Departments.ToDictionary(
                x => x.Id,
                x => new Department {
                        Name = x.Name,
                        Budget = x.Budget,
                        StartDate = x.StartDate,
                        Administrator = (!string.IsNullOrWhiteSpace(x.AdministratorRef) && instructorDict.ContainsKey(x.AdministratorRef)) ? instructorDict[x.AdministratorRef] : null
                }
            );
            Dictionary<int, Course> courseDict = seed.Courses.ToDictionary(
                x => x.CourseID,
                x => {
                    List<Instructor> instructorList = new List<Instructor>();

                    if (x.InstructorRefs != null)
                    {
                        foreach (string instructorRef in x.InstructorRefs)
                        {
                            instructorList.Add(instructorDict[instructorRef]);
                        }
                    }

                    return new Course
                    {
                        CourseID = x.CourseID,
                        Title = x.Title,
                        Credits = x.Credits,
                        Department = departmentDict.ContainsKey(x.DepartmentRef) ? departmentDict[x.DepartmentRef] : null,
                        Instructors = new List<Instructor>()
                    };
                }
            );
            context.AddRange(instructorDict.Values);
            context.AddRange(studentDict.Values);
            context.AddRange(departmentDict.Values);
            context.AddRange(courseDict.Values);

            foreach (EnrollmentDto e in seed.Enrollments)
            {
                Grade? grade = null;
                if (!string.IsNullOrWhiteSpace(e.Grade) && Enum.TryParse<Grade>(e.Grade, true, out Grade g)) 
                {
                    grade = g;
                }
               
                var enrollment = new Enrollment
                {
                    Student = studentDict[e.StudentRef],
                    Course = courseDict[e.CourseRef],
                    Grade = grade
                };
                context.Enrollments.Add(enrollment);
            }
            
            foreach (OfficeAssignmentDto o in seed.OfficeAssignments)
            {
                OfficeAssignment officeAssignment = new OfficeAssignment
                {
                    Instructor = instructorDict[o.InstructorRef],
                    Location = o.Location
                };
                context.OfficeAssignments.Add(officeAssignment);
            }
            context.SaveChanges();
        }


        public static SeedDto LoadSeed(string path) 
        {
            var ser = new XmlSerializer(typeof(SeedDto));
            var stg = new XmlReaderSettings{
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            };
            using var reader = XmlReader.Create(path, stg);

            try
            {
                return (SeedDto)ser.Deserialize(reader);
            }
            catch (InvalidOperationException ex)
            {
                string msg = $"Failed to deserialize '{path}'. {ex.InnerException?.Message ?? ex.Message}";
                throw new InvalidOperationException(msg, ex);
            }
        }
    }
}