using Assessment.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment
{
    public class AdminDashboard
    {

        public void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("Admin Dashboard:");
                Console.WriteLine("1. Add a new Course");
                Console.WriteLine("2. View all Courses");
                Console.WriteLine("3. Delete a Course");
                Console.WriteLine("4. Update a Course");
                Console.WriteLine("5. View Analytics");
                Console.WriteLine("6. Log out");
                Console.Write("Enter your choice: ");
                int adminChoice = int.Parse(Console.ReadLine());

                switch (adminChoice)
                {
                    case 1:
                        AddNewCourse();
                        break;

                    case 2:
                        ViewAllCourses();
                        break;

                    case 3:
                        DeleteCourse();
                        break;

                    case 4:
                        UpdateCourse();
                        break;

                    case 5:
                        ViewAnalytics();
                        break;

                    case 6:
                        Console.WriteLine("Logging out...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

   
        private int GetNextCourseID(string coursesPath)
        {
            string[] courses = File.ReadAllLines(coursesPath);

            int maxCourseID = 0;

            foreach (string course in courses)
            {
                string[] courseInfo = course.Split(',');
                if (int.TryParse(courseInfo[0], out int courseID))
                {
                    if (courseID > maxCourseID)
                    {
                        maxCourseID = courseID;
                    }
                }
            }

            return maxCourseID + 1;
        }
        private void ViewAllCourses()
        {
            string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");
            string[] courses = File.ReadAllLines(coursesPath);

            Console.WriteLine("All Courses:");
            foreach (string course in courses)
            {
                string[] courseInfo = course.Split(',');

                if (courseInfo.Length >= 4 && int.TryParse(courseInfo[0], out int courseID))
                {
                    string courseName = courseInfo[1];
                    string courseDescription = courseInfo[2];
                    int coursePrice = int.Parse(courseInfo[3]);

                    Console.WriteLine($"ID: {courseID}, Name: {courseName}, Description: {courseDescription}, Price: {coursePrice}");
                }
                else
                {
                    Console.WriteLine("Invalid course data: " + course);
                }
            }
        }


        private void AddNewCourse()
        {
            Console.Write("Enter course name: ");
            string courseName = Console.ReadLine();

            Console.Write("Enter course description: ");
            string courseDescription = Console.ReadLine();

            Console.Write("Enter course price: ");
            int coursePrice = int.Parse(Console.ReadLine());

            string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");

            int newCourseID = GetNextCourseID(coursesPath);

            CoursesDto newCourse = new CoursesDto
            {
                CourseID = newCourseID,
                CourseName = courseName,
                CourseDescription = courseDescription,
                CoursePrice = coursePrice
            };

            string courseData = $"{newCourse.CourseID},{newCourse.CourseName},{newCourse.CourseDescription}, {newCourse.CoursePrice}";
            File.AppendAllText(coursesPath, courseData + Environment.NewLine);

            Console.WriteLine("New course added successfully.");
        }

        private void UpdateCourse()
        {
            Console.Write("Enter the course ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int courseIDToUpdate))
            {
                string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");
                string[] courses = File.ReadAllLines(coursesPath);

                bool courseFound = false;
                List<string> updatedCourses = new List<string>();

                foreach (string course in courses)
                {
                    string[] courseInfo = course.Split(',');
                    if (int.TryParse(courseInfo[0], out int courseID))
                    {
                        if (courseID == courseIDToUpdate)
                        {
                            courseFound = true;

                            Console.Write("Enter the new course name: ");
                            string newCourseName = Console.ReadLine();

                            Console.Write("Enter the new course description: ");
                            string newCourseDescription = Console.ReadLine();

                            Console.Write("Enter the new course price: ");
                            int newCoursePrice = int.Parse(Console.ReadLine());

                            courseInfo[1] = newCourseName;
                            courseInfo[2] = newCourseDescription;
                            courseInfo[3] = newCoursePrice.ToString();

                            updatedCourses.Add(string.Join(",", courseInfo));
                        }
                        else
                        {
                            updatedCourses.Add(course);
                        }
                    }
                }

                if (courseFound)
                {
                    File.WriteAllLines(coursesPath, updatedCourses);
                    Console.WriteLine("Course updated successfully.");
                }
                else
                {
                    Console.WriteLine("Course not found. Update failed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid course ID.");
            }
        }



        private void DeleteCourse()
        {
            Console.Write("Enter the course ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int courseIDToDelete))
            {
                string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");
                string[] courses = File.ReadAllLines(coursesPath);

                bool courseFound = false;
                List<string> updatedCourses = new List<string>();

                foreach (string course in courses)
                {
                    string[] courseInfo = course.Split(',');
                    if (int.TryParse(courseInfo[0], out int courseID))
                    {
                        if (courseID == courseIDToDelete)
                        {
                            courseFound = true;
                        }
                        else
                        {
                            updatedCourses.Add(course);
                        }
                    }
                }

                if (courseFound)
                {
                    File.WriteAllLines(coursesPath, updatedCourses);
                    Console.WriteLine("Course deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Course not found. Deletion failed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid course ID.");
            }
        }


        private void ViewAnalytics()
        {
            string analyticsPath = Path.Combine(Directory.GetCurrentDirectory(), "analytics.txt");
            string[] analyticsData = File.ReadAllLines(analyticsPath);

            string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");
            string[] coursesData = File.ReadAllLines(coursesPath);

            Dictionary<int, CoursesDto> coursesDictionary = new Dictionary<int, CoursesDto>();
            foreach (string course in coursesData)
            {
                string[] courseInfo = course.Split(',');
                if (int.TryParse(courseInfo[0], out int courseID) && courseInfo.Length >= 4)
                {
                    coursesDictionary[courseID] = new CoursesDto
                    {
                        CourseID = courseID,
                        CourseName = courseInfo[1],
                        CourseDescription = courseInfo[2],
                        CoursePrice = int.Parse(courseInfo[3])
                    };
                }
            }

            Console.WriteLine("Analytics Data:");
            foreach (string data in analyticsData)
            {
                string[] analyticsInfo = data.Split(',');

                if (analyticsInfo.Length >= 2 && int.TryParse(analyticsInfo[0], out int courseID) && int.TryParse(analyticsInfo[1], out int numberOfPurchases))
                {
                    if (coursesDictionary.TryGetValue(courseID, out CoursesDto course))
                    {
                        Console.WriteLine($"Course ID: {course.CourseID}, Name: {course.CourseName}, Purchases: {numberOfPurchases}, Price: {course.CoursePrice}");
                    }
                    else
                    {
                        Console.WriteLine($"Course ID: {courseID}, Purchases: {numberOfPurchases}, Course information not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid analytics data: " + data);
                }
            }
        }

    }
}
