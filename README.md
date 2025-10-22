# 🧪 Contoso University

## 📋 Objective
Verify that the Contoso University application correctly implements **sorting**, **filtering**, and **paging** functionalities for the **Students** Page ensuring proper data retrieval, display, and navigation.

---

## 🎯 Scope
This test plan focuses on the following pages and functionalities:
- **Students Index Page**
  - Sorting by Name and Enrollment Date  
  - Filtering by Name  
  - Paging (Next/Previous links)
    
---

## ⚙️ Test Environment
| Component | Description |
|------------|-------------|
| **Framework** | ASP.NET Core 8 / EF Core |
| **Database** | Local SQL Server (LocalDB) |
| **Browser** | Microsoft Edge / Chrome |
| **Test Data** | Seeded via `DbInitializer` |

---

## 🧩 Database Schema (Overview)

| **Table** | **Key Fields** | **Description** |
|------------|----------------|-----------------|
| **Student** | `ID` *(PK)*, `LastName`, `FirstName`, `EnrollmentDate` | Stores student records |
| **Enrollment** | `EnrollmentID` *(PK)*, `CourseID`, `StudentID`, `Grade` | Links students to courses (many-to-many relationship) |
| **Course** | `CourseID` *(PK)*, `Title`, `Credits`, `DepartmentID` | Represents courses offered by departments |
| **Department** | `DepartmentID` *(PK)*, `Name`, `Budget`, `StartDate`, `InstructorID` *(FK)* | Represents academic departments and their administrators |
| **Instructor** | `ID` *(PK)*, `LastName`, `FirstName`, `HireDate` | Represents faculty members who teach courses |
| **OfficeAssignment** | `InstructorID` *(PK, FK)*, `Location` | One-to-one table assigning offices to Instructors |

---

## 🧾 Test Scenarios

| **ID** | **Test Scenario** | **Steps** | **Expected Result** |
|:--:|:--|:--|:--|
| TS1 | Verify default student list load | Navigate to `/Students` | Student list displays sorted by **Last Name (ascending)** by default |
| TS2 | Sort by name descending | Click the "Last Name" column header | Student list toggles to **descending** order |
| TS3 | Sort by enrollment date | Click the "Enrollment Date" column header | List reorders correctly by date |
| TS4 | Filter by student name | Enter a partial name (e.g., "Carson") in the search box and submit | Only students with "Carson" in the name appear |
| TS5 | Verify paging | If more than 3 students exist, click **Next** link | Next page of results loads correctly |
| TS6 | Verify filter persistence across paging | Apply a filter, then click **Next** | Filter remains applied to subsequent pages |
| TS7 | Verify sort persistence across paging | Sort by name descending, click **Next** | Sorting order remains consistent |
| TS8 | Invalid filter test | Enter a non-existent name | No results appear, and the app doesn’t crash |

---

## ✅ Pass/Fail Criteria
- All pages load and display expected data without errors.  
- Sorting, filtering, and paging behave consistently and retain state between operations.  
