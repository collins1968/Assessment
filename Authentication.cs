using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Assessment;
using Assessment.models;

namespace Assessment
{
    public class Authentication
    {
        public UserInfo loggedInUser;
        public void AuthenticatingPage()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "user.txt");


            while (true)
            {
                Console.WriteLine("Welcome to Jitu training System");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Register(path);
                        break;

                    case 2:
                        Login(path);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public void Register(string path)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Console.Write("Enter role (User/Admin): ");
            string roleInput = Console.ReadLine();
            if (!Enum.TryParse<UserRole>(roleInput, true, out UserRole role))
            {
                Console.WriteLine("Invalid role. Defaulting to User.");
                role = UserRole.user;
            }

            UserInfo newUser = new UserInfo
            {
                Username = username,
                Password = password,
                Role = role
            };

            string userData = $"{newUser.Username},{newUser.Password},{newUser.Role}";

            File.AppendAllText(path, userData + Environment.NewLine);

            Console.WriteLine("Registration successful.");
            Login(path);
        }
        public void Login(string path)
        {

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            string[] users = File.ReadAllLines(path);

            foreach (string user in users)
            {
                string[] userInfo = user.Split(',');
                string storedUsername = userInfo[0];
                string storedPassword = userInfo[1];
                string roleString = userInfo[2];

                if (username == storedUsername && password == storedPassword)
                {
                    LoggedUser.LoggedInUser = new UserInfo
                    {
                        Username = storedUsername,
                        Password = storedPassword,
                    };

                    //if (username == storedUsername && password == storedPassword)
                    //{
                    //    loggedInUser = new UserInfo
                    //    {
                    //        Username = storedUsername,
                    //        Password = storedPassword,

                    //    };

                    if (Enum.TryParse<UserRole>(roleString, true, out UserRole role))
                    {
                        if (role == UserRole.admin)
                        {
                            Console.WriteLine("Admin login successful.");
                            AdminDashboard dashboard = new AdminDashboard();
                            dashboard.AdminMenu();
                        }
                        else
                        {
                            Console.WriteLine("User login successful.");
                            Userdashboard dashboard = new Userdashboard();
                            dashboard.UserMenu();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid role for the user.");
                    }
                    break;
                    //return;
                }
            }

            Console.WriteLine("Login failed. Incorrect credentials.");
        }

        //private void PurchaseCourse(string courseData)
        //{
        //    string[] courseInfo = courseData.Split(',');
        //    if (int.TryParse(courseInfo[0], out int courseID) && courseInfo.Length >= 4)
        //    {
        //        int coursePrice = int.Parse(courseInfo[3]);

        //        if (loggedInUser != null && loggedInUser.Role == UserRole.user)
        //        {
        //            if (loggedInUser.Balance >= coursePrice)
        //            {
        //                // Simulate STK PUSH payment
        //                Console.WriteLine("Simulating STK PUSH payment...");

        //                // Deduct course price from user balance
        //                loggedInUser.Balance -= coursePrice;

        //                // Update analytics
        //                UpdateAnalytics(courseID, loggedInUser.Username);

        //                Console.WriteLine("Purchase successful!");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Insufficient balance to purchase the course.");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("User not logged in or not authorized.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid course data: " + courseData);
        //    }
        //}



    }
}


