using Assessment.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment
{
    public class  Userdashboard
    {
        public UserInfo loggedInUser;
        public void  UserMenu()
        {
            while (true)
            {
                Console.WriteLine("User Menu:");
                Console.WriteLine("1. View Courses");
                Console.WriteLine("2. Purchased Courses");
                Console.WriteLine("3. Check Balance");
                Console.WriteLine("4. Add Funds");
                Console.WriteLine("5. Log out");
                Console.Write("Enter your choice: ");
                int userChoice = int.Parse(Console.ReadLine());

                switch (userChoice)
                {
                    case 1:
                        ViewCourses();
                        break;

                    case 2:
                       PurchasedCourses();
                        break;
                    case 3: 
                        DisplayBalance();
                        break;
                    case 4:
                        AddFunds();
                        break;
                    case 5:
                        Console.WriteLine("Logging out...");
                        loggedInUser = null; // Clear logged-in user
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ViewCourses()
        {
            string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");
            string[] courses = File.ReadAllLines(coursesPath);

            Console.WriteLine("Available Courses:");
            for (int i = 0; i < courses.Length; i++)
            {
                string[] courseInfo = courses[i].Split(',');
                if (int.TryParse(courseInfo[0], out int courseID) && courseInfo.Length >= 4)
                {
                    string courseName = courseInfo[1];
                    string courseDescription = courseInfo[2];
                    int coursePrice = int.Parse(courseInfo[3]);

                    Console.WriteLine($"{i + 1}. ID: {courseID}, Name: {courseName}, Description: {courseDescription}, Price: {coursePrice}");
                }
                else
                {
                    Console.WriteLine("Invalid course data: " + courses[i]);
                }
            }

            Console.Write("Enter the course number to purchase (0 to go back): ");
            int selectedCourseNumber = int.Parse(Console.ReadLine());

            if (selectedCourseNumber >= 1 && selectedCourseNumber <= courses.Length)
            {
                PurchaseCourse(courses[selectedCourseNumber - 1]);
            }
        }
        private void PurchasedCourses()
        {
            string analyticsPath = Path.Combine(Directory.GetCurrentDirectory(), "analytics.txt");
            string[] analyticsData = File.ReadAllLines(analyticsPath);

            Console.WriteLine("Purchased Courses:");
            foreach (string data in analyticsData)
            {
                string[] analyticsInfo = data.Split(',');
                if (analyticsInfo.Length >= 2 && analyticsInfo[1] == LoggedUser.LoggedInUser.Username && int.TryParse(analyticsInfo[0], out int courseID))
                {
                    // Fetch course details from courses.txt
                    string coursesPath = Path.Combine(Directory.GetCurrentDirectory(), "courses.txt");
                    string[] coursesData = File.ReadAllLines(coursesPath);

                    foreach (string courseData in coursesData)
                    {
                        string[] courseInfo = courseData.Split(',');
                        if (int.TryParse(courseInfo[0], out int courseIdFromCourses) && courseIdFromCourses == courseID && courseInfo.Length >= 4)
                        {
                            string courseName = courseInfo[1];
                            string courseDescription = courseInfo[2];
                            int coursePrice = int.Parse(courseInfo[3]);

                            Console.WriteLine($"Course ID: {courseID}, Name: {courseName}, Description: {courseDescription}, Price: {coursePrice}");
                            break;
                        }
                    }
                }
            }
        }
        private void PurchaseCourse(string courseData)
        {
            string[] courseInfo = courseData.Split(',');

            if (int.TryParse(courseInfo[0], out int courseID) && courseInfo.Length >= 4)
            {
                int coursePrice = int.Parse(courseInfo[3]);

                if (LoggedUser.LoggedInUser != null && LoggedUser.LoggedInUser.Role == UserRole.user)
                {
                    if (LoggedUser.LoggedInUser.Balance >= coursePrice)
                    {
                        // Simulate STK PUSH payment
                        Console.WriteLine("Simulating STK PUSH payment...");

                        // Deduct course price from user balance
                        LoggedUser.LoggedInUser.Balance -= coursePrice;

                        // Update analytics
                        UpdateAnalytics(courseID, LoggedUser.LoggedInUser.Username);

                        Console.WriteLine("Purchase successful!");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance to purchase the course.");
                        AddFunds();
                    }
                }
                else
                {
                    Console.WriteLine("User not logged in or not authorized.");
                }
            }
            else
            {
                Console.WriteLine("Invalid course data: " + courseData);
            }
        }


        private void AddFunds()
        {
            Console.Write("Enter the amount to add to your balance: ");
            if (int.TryParse(Console.ReadLine(), out int amountToAdd))
            {
                LoggedUser.UpdateBalance(amountToAdd);
                Console.WriteLine("Balance updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        private void DisplayBalance()
        {
            int balance = LoggedUser.CheckBalance();
            if (balance > 0)
            {
                Console.WriteLine($"Your current balance: {balance}");
            }
            else
            {
                Console.WriteLine("Your balance is zero.");
            }
        }

        private void UpdateAnalytics(int courseID, string username)
        {
            string analyticsPath = Path.Combine(Directory.GetCurrentDirectory(), "analytics.txt");
            string analyticsData = $"{courseID},{username}";

            File.AppendAllText(analyticsPath, analyticsData + Environment.NewLine);
        }



    }
}
